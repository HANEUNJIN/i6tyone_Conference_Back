using AutoMapper;
using ClosedXML.Excel;
using eGhis_WebService_Core.Define;
using eGhis_WebService_Core.Infrastructure.Db;
using eGhis_WebService_Core.Infrastructure.Utils;
using eGhis_WebService_Core.Models.Common;
using eGhis_WebService_Core.Models.Dto.Auth;
using eGhis_WebService_Core.Repositories;
using i6tyone_Conference.Infrastructure.Utils;
using i6tyone_Conference.Models.Dto.Register;
using i6tyone_Conference.Models.Dto.Aligo;
using i6tyone_Conference.Models.Dto.Auth;
using i6tyone_Conference.Service.Aligo;
using eGhis_WebService_Core.Infrastructure.Utils.Crypto;

namespace eGhis_WebService_Core.Service.Auth
{
    public class RegisterService : IRegisterService
    {
        private readonly IDbConnectionFactory _connFactory;
        private readonly ISqlRepository _repo;
        private readonly IMapper _mapper;
        private readonly QRCodeUtil _qrCode;
        private readonly FtpUtil _ftp;
        private readonly IAligoMsgService _msgService;

        private readonly string ConferenceName = "2026 Solus CHRISTUS QRCode";
        private readonly string Today = DateTime.Now.ToString("yyyy-MM-dd");

        public RegisterService(IDbConnectionFactory connFactory, ISqlRepository repo, IMapper mapper, QRCodeUtil qrCode, FtpUtil ftp, IAligoMsgService msgService)
        {
            _connFactory = connFactory;
            _repo = repo;
            _mapper = mapper;
            _qrCode = qrCode;
            _ftp = ftp;
            _msgService = msgService;
        }

        public async Task<GenericResponse<RegisterAddResponseDto>> GenerateRegisterAsync(RegisterRequestDto req, CancellationToken cancellationToken = default)
        {
            var res = new GenericResponse<RegisterAddResponseDto>();

            await using var scope = await _connFactory.OpenSessionAsync(cancellationToken);
            var db = scope.Session;

            if (!string.IsNullOrWhiteSpace(req.phone))
            {
                req.phone = req.phone.Replace("-", "");
            }

            string uniqueId = Guid.NewGuid().ToString("N").Substring(0, 8);
            if(string.IsNullOrWhiteSpace(uniqueId))
            {
                res.SetResult(ErrorStatusCode.Invalid_Error);
                res.ResultMsg = "QR Code 발급 키 생성 오류";
                return res;
            }

            var data = await _repo.IC26DataDao.GenerateRegisterAsync(db, req, uniqueId);
            if (data < 0)
            {
                res.SetResult(ErrorStatusCode.Invalid_Error);
                res.ResultMsg = "컨퍼런스 참가 등록 실패";
                return res;
            }

            //Todo: 테스트로 인한 비활성화
            //DateTime today = DateTime.Now.Date;
            //DateTime[] validDates = { new DateTime(2026, 1, 27), new DateTime(2026, 1, 28), new DateTime(2026, 1, 29) };

            //if (validDates.Contains(today))
            //{
            //    var isCheckIn = await _repo.IC26DataDao.CheckAttendanceAsync(db, uniqueId);
            //    if (isCheckIn == 0)
            //    {
            //        res.SetResult(ErrorStatusCode.Invalid_Error);
            //        res.ResultMsg = "현장 입장 등록 실패";
            //        return res;
            //    }
            //}
            var result = new RegisterAddResponseDto() { successCount = data };

            res.SetResult(ErrorStatusCode.Success);
            res.Data = result;
            return res;
        }

        public async Task<GenericResponse<SuccessResponseDto>> UpdateRegisterAsync(RegisterUpdateRequestDto req, CancellationToken cancellationToken = default)
        {
            var res = new GenericResponse<SuccessResponseDto>();

            await using var scope = await _connFactory.OpenSessionAsync(cancellationToken);
            var db = scope.Session;

            if (string.IsNullOrWhiteSpace(req.uniqueIdKey))
            {
                res.SetResult(ErrorStatusCode.Invalid_Error);
                res.ResultMsg = "QR Code 발급 키가 누락되었습니다.";
                return res;
            }

            var uniqueId = CryptoUtil.CreateAESInstance(CTBizConstant.CryptoKey.I6TYONE, new byte[16])?.AESDecrypt(req.uniqueIdKey);

            if (!string.IsNullOrWhiteSpace(req.phone))
            {
                req.phone = req.phone.Replace("-", "");
            }

            var data = await _repo.IC26DataDao.UpdateRegisterAsync(db, req, uniqueId);
            if (!data)
            {
                res.SetResult(ErrorStatusCode.Invalid_Error);
                res.ResultMsg = "등록자 수정 실패";
                return res;
            }

            var result = new SuccessResponseDto() { success = data };

            res.SetResult(ErrorStatusCode.Success);
            res.Data = result;
            return res;
        }

        public async Task<GenericResponse<SuccessResponseDto>> DisposeRegisterAsync(string uniqueIdKey, CancellationToken cancellationToken = default)
        {
            var res = new GenericResponse<SuccessResponseDto>();

            await using var scope = await _connFactory.OpenSessionAsync(cancellationToken);
            var db = scope.Session;

            var uniqueId = CryptoUtil.CreateAESInstance(CTBizConstant.CryptoKey.I6TYONE, new byte[16])?.AESDecrypt(uniqueIdKey);
            var data = await _repo.IC26DataDao.DisposeRegisterAsync(db, uniqueId);
            if (!data)
            {
                res.SetResult(ErrorStatusCode.Authentication_Failed);
                return res;
            }

            var result = new SuccessResponseDto() { success = data };

            res.SetResult(ErrorStatusCode.Success);
            res.Data = result;
            return res;
        }

        public async Task<GenericResponse<SuccessResponseDto>> DeleteRegisterAsync(string uniqueIdKey, CancellationToken cancellationToken = default)
        {
            var res = new GenericResponse<SuccessResponseDto>();

            await using var scope = await _connFactory.OpenSessionAsync(cancellationToken);
            var db = scope.Session;

            var uniqueId = CryptoUtil.CreateAESInstance(CTBizConstant.CryptoKey.I6TYONE, new byte[16])?.AESDecrypt(uniqueIdKey);
            var data = await _repo.IC26DataDao.DeleteRegisterAsync(db, uniqueId);
            if (!data)
            {
                res.SetResult(ErrorStatusCode.Authentication_Failed);
                return res;
            }

            var result = new SuccessResponseDto() { success = data };

            res.SetResult(ErrorStatusCode.Success);
            res.Data = result;
            return res;
        }

        /// <summary>
        /// 전체 등록자 QR 코드 일괄 발급
        /// </summary>
        public async Task<GenericResponse<SendMassResponseDto>> GenerateRegisterQRCodeAsync(CancellationToken cancellationToken = default)
        {
            var res = new GenericResponse<SendMassResponseDto>();

            await using var scope = await _connFactory.OpenSessionAsync(cancellationToken);
            var db = scope.Session;

            var result = await _repo.IC26DataDao.GenerateRegisterQRCodeAsync(db);
            if (result is null || !result.Any())
            {
                res.SetResult(ErrorStatusCode.Invalid_Error);
                res.ResultMsg = "등록 대상자가 없습니다.";
                return res;
            }

            // QR 코드 생성이 필요한 항목만 필터링
            var itemsToProcess = result.Where(x => x.CreateQR != "Y").ToList();
            if (!itemsToProcess.Any())
            {
                res.SetResult(ErrorStatusCode.Invalid_Error);
                res.ResultMsg = "생성 대상 QR 코드가 없습니다.";
                return res;
            }

            int successCount = 0;
            int failCount = 0;
            var msgIds = new List<string>();

            foreach (var item in itemsToProcess)
            {
                try
                {
                    // QR 코드 생성
                    var uniqueIdKey = CryptoUtil.CreateAESInstance(CTBizConstant.CryptoKey.I6TYONE, new byte[16])?.AESEncrypt(item.UniqueId);
                    byte[] qrBytes = _qrCode.GenerateQRCodeBytes(uniqueIdKey);

                    var smsInfo = GetSmsInfo(item.Phone, qrBytes);

                    var qrSend = await _msgService.SendSmsAsync(smsInfo);
                    if (qrSend.ResultMsg != EnumUtil.GetDescription(ErrorStatusCode.Success))
                    {
                        res.SetResult(qrSend.ResultCd, qrSend.ResultMsg);
                        return res;
                    }

                    if (!string.IsNullOrEmpty(qrSend.Data?.msgId.ToString()))
                    {
                        msgIds.Add(qrSend.Data?.msgId.ToString());
                    }

                    await _repo.IC26DataDao.CheckCreateQRAsync(db, item.UniqueId);

                    successCount++;
                }
                catch
                {
                    failCount++;
                }
            }

            res.SetResult(ErrorStatusCode.Success);
            res.Data = new SendMassResponseDto()
            {
                msgId = string.Join(", ", msgIds),
                successCnt = successCount,
                errorCnt = failCount,
                msgType = "MMS"
            };
            return res;
        }

        /// <summary>
        /// 특정 등록자 QR 코드 발급/재발급
        /// </summary>
        public async Task<GenericResponse<SendMassResponseDto>> GenerateRegisterQRCodeSingleAsync(IssuanceRequestDto req, CancellationToken cancellationToken = default)
        {
            var res = new GenericResponse<SendMassResponseDto>();

            await using var scope = await _connFactory.OpenSessionAsync(cancellationToken);
            var db = scope.Session;

            if (string.IsNullOrWhiteSpace(req.buyer) && string.IsNullOrWhiteSpace(req.phone))
            {
                res.SetResult(ErrorStatusCode.Invalid_Error);
                res.ResultMsg = $"{req.buyer ?? "알 수 없는 사용자"}에 대한 정보를 찾을 수 없음.";
                return res;
            }

            req.phone = req.phone.Replace("-", "");

            var result = await _repo.IC26DataDao.GenerateRegisterQRCodeSingleAsync(db, req);
            if (string.IsNullOrWhiteSpace(result?.UniqueId))
            {
                res.SetResult(ErrorStatusCode.Invalid_Error);
                res.ResultMsg = $"{result?.Buyer}에 대한 정보를 찾을 수 없음.";
                return res;
            }

            //string qrCodeFileName = $"{result.No}_{result.Buyer}_{result.Phone.Replace("-", "")}.jpg";
            //string ftpFolderUrl = $"{_ftp.Url}/{ConferenceName}";

            //await _ftp.CreateFtpDirectoryRecursiveAsync(ftpFolderUrl);


            var uniqueIdKey = CryptoUtil.CreateAESInstance(CTBizConstant.CryptoKey.I6TYONE, new byte[16]).AESEncrypt(result.UniqueId);
            byte[] qrBytes = _qrCode.GenerateQRCodeBytes(uniqueIdKey);

            #region FTP 업로드
            //bool uploadSuccess = await _ftp.UploadFileToFtpAsync(qrBytes, ftpFolderUrl, qrCodeFileName);
            //if (!uploadSuccess)
            //{
            //    res.SetResult(ErrorStatusCode.File_Upload_Fail);
            //    res.ResultMsg = "FTP 업로드 실패!";
            //    return res;
            //}
            #endregion

            var smsInfo = GetSmsInfo(result.Phone, qrBytes);

            var qrSend = await _msgService.SendSmsAsync(smsInfo);
            if(qrSend.ResultMsg != EnumUtil.GetDescription(ErrorStatusCode.Success))
            {
                res.SetResult(qrSend.ResultCd, qrSend.ResultMsg);
                return res;
            }

            // QR 생성 여부 업데이트
            await _repo.IC26DataDao.CheckCreateQRAsync(db, result.UniqueId);

            res.SetResult(ErrorStatusCode.Success);
            res.Data = qrSend.Data;
            //res.Data = new QRCodeResponseDto {
            //    successMsg = $"{result.Buyer} QRCode 생성 완료!",
            //    folderPath = ftpFolderUrl,
            //};
            return res;
        }

        public async Task<GenericResponse<RegisterResponseDto>> GetRegisterInfoAsync(RegisterInfoRequestDto req, CancellationToken cancellationToken = default)
        {
            var res = new GenericResponse<RegisterResponseDto>();

            await using var scope = await _connFactory.OpenSessionAsync(cancellationToken);
            var db = scope.Session;

            if (!string.IsNullOrWhiteSpace(req.keyword))
            {
                req.keyword = req.keyword.Replace("-", "");
            }

            var data = await _repo.IC26DataDao.GetRegisterInfoAsync(db, req);
            if (data is null)
            {
                res.SetResult(ErrorStatusCode.Authentication_Failed);
                return res;
            }

            var aes = CryptoUtil.CreateAESInstance(CTBizConstant.CryptoKey.I6TYONE, new byte[16]);

            foreach (var item in data)
            {
                item.UniqueId = aes.AESEncrypt(item.UniqueId);
            }

            var mappedList = _mapper.Map<List<RegisterInfo>>(data);
            var result = new RegisterResponseDto() { list = mappedList };

            res.SetResult(ErrorStatusCode.Success);
            res.Data = result;
            return res;
        }

        public async Task<GenericResponse<ExcelResponseDto>> GetRegisterExcelAsync(CancellationToken cancellationToken = default)
        {
            var res = new GenericResponse<ExcelResponseDto>();

            await using var scope = await _connFactory.OpenSessionAsync(cancellationToken);
            var db = scope.Session;

            var list = await _repo.IC26DataDao.GetRegisterListAsync(db);
            if (list == null || !list.Any())
            {
                res.SetResult(ErrorStatusCode.DB_Error);
                return res;
            }

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Register");

            var headers = new[]
            {
                "순번","티켓구분","신청일","구매자","참석자","전화번호","성별","나이","교회","거주지역",
                "교단","새신자여부","구매수량","좌석구역","메모", "출석여부","QR 생성여부",
                "SMS 전송여부","Notion 링크 발송"
            };

            // 1. 제목 추가 (1행)
            var titleCell = worksheet.Cell(1, 1);
            titleCell.Value = $"2026 Solus CHRISTUS CONFERENCE 등록자 명단 ({DateTime.Now:yyyy-MM-dd HH:mm:ss})";
            titleCell.Style.Font.Bold = true;
            titleCell.Style.Font.FontSize = 16;  // 원하는 크기 조절 가능
            titleCell.Style.Font.FontColor = XLColor.White;
            titleCell.Style.Fill.BackgroundColor = XLColor.FromHtml("#073763");
            titleCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            titleCell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            // 제목을 헤더 열 수만큼 합치기
            worksheet.Range(1, 1, 1, headers.Length).Merge();

            // 제목 행 높이 설정
            worksheet.Row(1).Height = 40;

            // 2. 헤더 추가 (2행)
            for (int i = 0; i < headers.Length; i++)
            {
                var cell = worksheet.Cell(2, i + 1);
                cell.Value = headers[i];

                // 헤더 스타일 적용
                cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#0B5394");
                cell.Style.Font.Bold = true;
                cell.Style.Font.FontColor = XLColor.White;
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                // 테두리 적용
                cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                cell.Style.Border.TopBorderColor = XLColor.Black;
                cell.Style.Border.BottomBorderColor = XLColor.Black;
                cell.Style.Border.LeftBorderColor = XLColor.Black;
                cell.Style.Border.RightBorderColor = XLColor.Black;
            }

            // 데이터
            var row = 3;
            foreach (var item in list)
            {
                var values = new object[]
                {
                    item.No, ConvertOption(item.Option), ConvertDay(item.Day), item.Buyer, item.Attender, item.Phone,
                    item.Gender, item.Age, item.Church, item.Local, item.Denom, item.newBelieverYn,
                    item.Count, item.Area, item.Memo, item.Attend, item.CreateQR,
                    item.SMS, item.NotionSmsYn
                };

                for (int col = 0; col < values.Length; col++)
                {
                    var cell = worksheet.Cell(row, col + 1);
                    cell.Value = values[col]?.ToString() ?? "";

                    if (values[col] == item.Church || values[col] == item.Local || values[col] == item.Denom || values[col] == item.Memo)
                        cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                    else
                        cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                }

                row++;
            }

            worksheet.Row(2).Height = 20;
            worksheet.SheetView.FreezeRows(1);
            worksheet.SheetView.FreezeRows(2);
            worksheet.Columns().AdjustToContents();

            foreach (var column in worksheet.Columns())
            {
                if (column.Width < 10)
                    column.Width = 10;
            }

            // 메모리 스트림으로 변환
            using var ms = new MemoryStream();
            workbook.SaveAs(ms);

            res.Data = new ExcelResponseDto
            {
                FileBytes = ms.ToArray(),
                FileName = $"2026 Solus CHRISTUS_{DateTime.Now:yyyyMMddHHmmss}.xlsx",
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            };

            res.SetResult(ErrorStatusCode.Success);
            return res;
        }

        public async Task<GenericResponse<RegisterInfoResponseDto>> GetRegisterDetailInfoAsync(string uniqueIdKey, CancellationToken cancellationToken = default)
        {
            var res = new GenericResponse<RegisterInfoResponseDto>();

            await using var scope = await _connFactory.OpenSessionAsync(cancellationToken);
            var db = scope.Session;

            if (string.IsNullOrWhiteSpace(uniqueIdKey))
            {
                res.SetResult(ErrorStatusCode.Invalid_Error);
                res.ResultMsg = "QR Code 발급 키 누락";
                return res;
            }

            var aes = CryptoUtil.CreateAESInstance(CTBizConstant.CryptoKey.I6TYONE, new byte[16]);
            var uniqueId = aes?.AESDecrypt(uniqueIdKey);

            var data = await _repo.IC26DataDao.GetRegisterDetailInfoAsync(db, uniqueId);
            if (data is null)
            {
                res.SetResult(ErrorStatusCode.Authentication_Failed);
                return res;
            }

            var result = _mapper.Map<RegisterInfoResponseDto>(data);

            res.SetResult(ErrorStatusCode.Success);
            res.Data = result;
            return res;
        }

        public async Task<GenericResponse<CheckInResponseDto>> CheckAttendanceAsync(string uniqueIdKey, CancellationToken cancellationToken = default)
        {
            var res = new GenericResponse<CheckInResponseDto>();

            await using var scope = await _connFactory.OpenSessionAsync(cancellationToken);
            var db = scope.Session;

            //Todo: 테스트로 인한 비활성화
            //DateTime today = DateTime.Now.Date;
            //DateTime[] validDates = { new DateTime(2026, 1, 27), new DateTime(2026, 1, 28), new DateTime(2026, 1, 29) };

            //if (!validDates.Contains(today))
            //{
            //    res.SetResult(ErrorStatusCode.Invalid_Error);
            //    res.ResultMsg = "현장 입장 등록 기간이 아닙니다.";
            //    return res;
            //}

            if (string.IsNullOrWhiteSpace(uniqueIdKey))
            {
                res.SetResult(ErrorStatusCode.Invalid_Error);
                res.ResultMsg = "QR Code 발급 키 누락";
                return res;
            }

            string? uniqueId = null;

            try
            {
                var aesInstance = CryptoUtil.CreateAESInstance(CTBizConstant.CryptoKey.I6TYONE, new byte[16]);
                uniqueId = aesInstance?.AESDecrypt(uniqueIdKey);
            }
            catch (Exception ex)
            {
            }

            if (uniqueId is null)
            {
                res.SetResult(ErrorStatusCode.Decrypt_Error);
                res.ResultMsg = "복호화 오류";
                return res;
            }

            var isCheckIn = await _repo.IC26DataDao.CheckAttendanceAsync(db, uniqueId);

            var registerInfo = await _repo.IC26DataDao.GetRegisterDetailAsync(db, uniqueId);
            if (registerInfo is null)
            {
                res.SetResult(ErrorStatusCode.Invalid_Error);
                res.ResultMsg = "정보를 찾을 수 없음.";
                return res;
            }

            var mappedInfo = _mapper.Map<CheckInResponseDto>(registerInfo);
            res.Data = mappedInfo;

            if (isCheckIn == 0)
            {
                res.SetResult(ErrorStatusCode.Invalid_Error);
                res.ResultMsg = "이미 발급된 티켓";
                return res;
            }

            res.SetResult(ErrorStatusCode.Success);
            return res;
        }

        public async Task<GenericResponse<FtpInfoResponseDto>> GetFtpInfoAsync()
        {
            var res = new GenericResponse<FtpInfoResponseDto>();

            var result = new FtpInfoResponseDto() { 
                host = _ftp.Url,
                user = _ftp.User,
                passWord = _ftp.Password,
            };

            res.SetResult(ErrorStatusCode.Success);
            res.Data = result;
            return res;
        }

        private string ConvertOption(int option)
        {
            switch (option)
            {
                case 1:
                    return "슈퍼얼리";
                case 2:
                    return "얼리1차";
                case 3:
                    return "얼리2차";
                case 4:
                    return "공식";
                case 5:
                    return "이벤트";
                case 6:
                    return "현장구매";
                case 7:
                    return "VIP";
            }
            return string.Empty;
        }

        private string ConvertDay(int day)
        {
            switch (day)
            {
                case 1:
                    return "[화]";
                case 2:
                    return "[수]";
                case 3:
                    return "[목]";
                case 4:
                    return "3-day";
            }
            return string.Empty;
        }

        private SendMessageApiRequestDto GetSmsInfo(string phone, byte[] qrBytes)
        {
            return new SendMessageApiRequestDto()
            {
                receiver = phone.Replace("-", ""),
                msg = "[테스트용] 2026 Isaiah6tyOne Conference\nSolus CHRISTUS : 예수 그리스도\n\n다가올 2026년, 모든 것의 중심이 되시는\n예수 그리스도를 높이는 아이자야 컨퍼런스에\n예배자 여러분을 초대합니다. ♥\n\n본 QR 코드는 컨퍼런스 입장 전용 티켓 교환권입니다.\n행사 당일 등록 부스에서 QR 코드 스캔 후\n티켓을 수령해 주세요.\n\n※ 본인 전용 코드로 캡처·전달·외부 공유 시 입장이 제한될 수 있습니다.\n\n* 2026.01.27(화) – 01.29(목)\n* 인천삼산월드체육관 (인천 부평구 체육관로 60)\n\n☎ 문의\n* 인스타그램 @isaiah6tyone\n* 카카오채널 @아이자야씩스티원컨퍼런스\n* 본 문자는 회신되지 않습니다.",
                msgType = "MMS",
                title = "아이자야씩스티원",
                destination = "",
                rDate = "20261231",
                rTime = "1200",
                image1 = qrBytes,
                testModeYn = "Y"
            };
        }
    }
}

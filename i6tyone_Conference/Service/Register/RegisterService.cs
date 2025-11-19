using AutoMapper;
using eGhis_WebService_Core.Define;
using eGhis_WebService_Core.Infrastructure.Db;
using eGhis_WebService_Core.Models.Common;
using eGhis_WebService_Core.Models.Dto.Auth;
using eGhis_WebService_Core.Repositories;
using i6tyone_Conference.Infrastructure.Utils;
using i6tyone_Conference.Models.Dto.Register;
using i6tyone_Conference.Models.Dto.Auth;
using System.IO.Compression;
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

        private readonly string ConferenceName = "2026 Solus CHRISTUS QRCode";
        private readonly string Today = DateTime.Now.ToString("yyyy-MM-dd");

        public RegisterService(IDbConnectionFactory connFactory, ISqlRepository repo, IMapper mapper, QRCodeUtil qrCode, FtpUtil ftp)
        {
            _connFactory = connFactory;
            _repo = repo;
            _mapper = mapper;
            _qrCode = qrCode;
            _ftp = ftp;
        }

        public async Task<GenericResponse<RegisterAddResponseDto>> GenerateRegisterAsync(RegisterRequestDto req, CancellationToken cancellationToken = default)
        {
            var res = new GenericResponse<RegisterAddResponseDto>();

            await using var scope = await _connFactory.OpenSessionAsync(cancellationToken);
            var db = scope.Session;

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
            //DateTime[] validDates = { new DateTime(2025, 1, 27), new DateTime(2025, 1, 28), new DateTime(2025, 1, 29) };

            //if (validDates.Contains(today))
            //{
            //    var isCheckIn = await _repo.IC26DataDao.CheckAttendanceAsync(db, uniqueId);
            //    if (!isCheckIn)
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
        public async Task<GenericResponse<QRCodeResponseDto>> GenerateRegisterQRCodeAsync(CancellationToken cancellationToken = default)
        {
            var res = new GenericResponse<QRCodeResponseDto>();

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
            int skipCount = result.Count - itemsToProcess.Count;

            if (!itemsToProcess.Any())
            {
                res.SetResult(ErrorStatusCode.Invalid_Error);
                res.ResultMsg = "생성 대상 QR 코드가 없습니다.";
                return res;
            }

            string ftpFolderUrl = $"{_ftp.Url}/{ConferenceName}";
            await _ftp.CreateFtpDirectoryRecursiveAsync(ftpFolderUrl);

            int successCount = 0;
            int failCount = 0;

            // 메모리 스트림을 이용한 ZIP 생성
            using var zipStream = new MemoryStream();
            using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
            {
                foreach (var item in itemsToProcess)
                {
                    try
                    {
                        // QR 코드 생성
                        var uniqueIdKey = CryptoUtil.CreateAESInstance(CTBizConstant.CryptoKey.I6TYONE, new byte[16])?.AESEncrypt(item.UniqueId);
                        byte[] qrBytes = _qrCode.GenerateQRCodeBytes(uniqueIdKey);
                        string qrFileName = $"{item.No}_{item.Buyer}_{item.Phone.Replace("-", "")}.jpg";

                        // ZIP에 추가
                        var zipEntry = archive.CreateEntry(qrFileName);
                        using var entryStream = zipEntry.Open();
                        await entryStream.WriteAsync(qrBytes, cancellationToken);

                        // DB 업데이트
                        await using var localScope = await _connFactory.OpenSessionAsync(cancellationToken);
                        var localDb = localScope.Session;
                        await _repo.IC26DataDao.CheckCreateQRAsync(localDb, item.UniqueId);

                        successCount++;
                    }
                    catch
                    {
                        failCount++;
                    }
                }
            }

            // ZIP 스트림을 FTP로 업로드
            zipStream.Position = 0;
            string zipFileName = $"{Today}.zip";
            bool uploadSuccess = await _ftp.UploadFileToFtpAsync(zipStream.ToArray(), ftpFolderUrl, zipFileName);

            if (!uploadSuccess)
            {
                res.SetResult(ErrorStatusCode.Invalid_Error);
                res.ResultMsg = "ZIP 업로드 실패";
                return res;
            }

            res.SetResult(ErrorStatusCode.Success);
            res.Data = new QRCodeResponseDto
            {
                successMsg = $"QR {successCount}건 QR 코드 생성 완료! 실패 {failCount}건",
                folderPath = ftpFolderUrl,
            };
            return res;
        }

        /// <summary>
        /// 특정 등록자 QR 코드 발급/재발급
        /// </summary>
        public async Task<GenericResponse<QRCodeResponseDto>> GenerateRegisterQRCodeSingleAsync(IssuanceRequestDto req, CancellationToken cancellationToken = default)
        {
            var res = new GenericResponse<QRCodeResponseDto>();

            await using var scope = await _connFactory.OpenSessionAsync(cancellationToken);
            var db = scope.Session;

            if (string.IsNullOrWhiteSpace(req.buyer) && string.IsNullOrWhiteSpace(req.phone))
            {
                res.SetResult(ErrorStatusCode.Invalid_Error);
                res.ResultMsg = $"{req.buyer ?? "알 수 없는 사용자"}에 대한 정보를 찾을 수 없음.";
                return res;
            }

            var result = await _repo.IC26DataDao.GenerateRegisterQRCodeSingleAsync(db, req);
            if (string.IsNullOrWhiteSpace(result?.UniqueId))
            {
                res.SetResult(ErrorStatusCode.Invalid_Error);
                res.ResultMsg = $"{result?.Buyer}에 대한 정보를 찾을 수 없음.";
                return res;
            }

            string qrCodeFileName = $"{result.No}_{result.Buyer}_{result.Phone.Replace("-", "")}.jpg";
            string ftpFolderUrl = $"{_ftp.Url}/{ConferenceName}";

            await _ftp.CreateFtpDirectoryRecursiveAsync(ftpFolderUrl);


            var uniqueIdKey = CryptoUtil.CreateAESInstance(CTBizConstant.CryptoKey.I6TYONE, new byte[16]).AESEncrypt(result.UniqueId);
            byte[] qrBytes = _qrCode.GenerateQRCodeBytes(uniqueIdKey);

            // FTP 업로드
            bool uploadSuccess = await _ftp.UploadFileToFtpAsync(qrBytes, ftpFolderUrl, qrCodeFileName);
            if (!uploadSuccess)
            {
                res.SetResult(ErrorStatusCode.File_Upload_Fail);
                res.ResultMsg = "FTP 업로드 실패!";
                return res;
            }

            // QR 생성 여부 업데이트
            await _repo.IC26DataDao.CheckCreateQRAsync(db, result.UniqueId);

            res.SetResult(ErrorStatusCode.Success);
            res.Data = new QRCodeResponseDto {
                successMsg = $"{result.Buyer} QRCode 생성 완료!",
                folderPath = ftpFolderUrl,
            };
            return res;
        }

        public async Task<GenericResponse<RegisterResponseDto>> GetRegisterInfoAsync(RegisterInfoRequestDto req, CancellationToken cancellationToken = default)
        {
            var res = new GenericResponse<RegisterResponseDto>();

            await using var scope = await _connFactory.OpenSessionAsync(cancellationToken);
            var db = scope.Session;

            var data = await _repo.IC26DataDao.GetRegisterInfoAsync(db, req);
            if (data is null)
            {
                res.SetResult(ErrorStatusCode.Authentication_Failed);
                return res;
            }

            var mappedList = _mapper.Map<List<RegisterInfo>>(data);
            var result = new RegisterResponseDto() { list = mappedList };

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
            //DateTime[] validDates = { new DateTime(2025, 1, 27), new DateTime(2025, 1, 28), new DateTime(2025, 1, 29) };

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

            var uniqueId = CryptoUtil.CreateAESInstance(CTBizConstant.CryptoKey.I6TYONE, new byte[16])?.AESDecrypt(uniqueIdKey);

            var isCheckIn = await _repo.IC26DataDao.CheckAttendanceAsync(db, uniqueId);
            if (!isCheckIn)
            {
                res.SetResult(ErrorStatusCode.Invalid_Error);
                res.ResultMsg = "현장 입장 등록 실패";
                return res;
            }

            var registerInfo = await _repo.IC26DataDao.GetRegisterDetailAsync(db, uniqueId);
            if (registerInfo is null)
            {
                res.SetResult(ErrorStatusCode.Invalid_Error);
                res.ResultMsg = "정보를 찾을 수 없음.";
                return res;
            }
            
            var mappedInfo = _mapper.Map<CheckInResponseDto>(registerInfo);

            res.SetResult(ErrorStatusCode.Success);
            res.Data = mappedInfo;
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
    }
}

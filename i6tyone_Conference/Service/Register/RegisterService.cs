using AutoMapper;
using eGhis_WebService_Core.Define;
using eGhis_WebService_Core.Infrastructure.Db;
using eGhis_WebService_Core.Models.Common;
using eGhis_WebService_Core.Models.Dto.Auth;
using eGhis_WebService_Core.Repositories;
using i6tyone_Conference.Infrastructure.Utils;
using i6tyone_Conference.Models.Dto.Register;
using i6tyone_Conference.Models.Dto.Auth;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace eGhis_WebService_Core.Service.Auth
{
    public class RegisterService : IRegisterService
    {
        private readonly IDbConnectionFactory _connFactory;
        private readonly ISqlRepository _repo;
        private readonly IMapper _mapper;
        private readonly QRCodeUtil _qrCode;

        private readonly string ConferenceName = "2026 Solus CHRISTUS QRCode";
        private readonly string Today = DateTime.Now.ToString("yyyy-MM-dd");

        public RegisterService(IDbConnectionFactory connFactory, ISqlRepository repo, IMapper mapper, QRCodeUtil qrCode)
        {
            _connFactory = connFactory;
            _repo = repo;
            _mapper = mapper;
            _qrCode = qrCode;
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

            int seq = await _repo.IC26DataDao.GetSeqAsync(db);
            var data = await _repo.IC26DataDao.GenerateRegisterAsync(db, req, seq, uniqueId);
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

        public async Task<GenericResponse<QRCodeResponseDto>> GenerateRegisterQRCodeAsync(CancellationToken cancellationToken = default)
        {
            var res = new GenericResponse<QRCodeResponseDto>();

            await using var scope = await _connFactory.OpenSessionAsync(cancellationToken);
            var db = scope.Session;

            var result = await _repo.IC26DataDao.GenerateRegisterQRCodeAsync(db);
            if (result is null || !result.Any())
            {
                res.SetResult(ErrorStatusCode.Authentication_Failed);
                return res;
            }

            string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string downloadPath = Path.Combine(userProfile, "Downloads");
            string savePath = Path.Combine(downloadPath, ConferenceName, Today);

            Directory.CreateDirectory(savePath);

            var stopwatch = Stopwatch.StartNew();
            int successCount = 0;

            var tasks = result.Select(async item =>
            {
                try
                {
                    if (item.CreateQR)
                        return;

                    if (string.IsNullOrWhiteSpace(item.UniqueId))
                    {
                        File.AppendAllText(Path.Combine(savePath, "error.log"), $"{item.Buyer}: UniqueId 없음\n");
                        return;
                    }

                    var safeBuyer = string.Join("_", item.Buyer.Split(Path.GetInvalidFileNameChars()));
                    var safePhone = string.Join("_", item.Phone.Split(Path.GetInvalidFileNameChars()));
                    var fileName = $"{item.No}_{safeBuyer}_{safePhone}.jpg";
                    var fullPath = Path.Combine(savePath, fileName);

                    // QR 생성
                    _qrCode.GenerateQRCodePngFile(item.UniqueId, fullPath);

                    Interlocked.Increment(ref successCount);

                    // QR 생성 여부 업데이트
                    await _repo.IC26DataDao.CheckCreateQRAsync(db, item.UniqueId);
                }
                catch (Exception ex)
                {
                    File.AppendAllText(Path.Combine(savePath, "error.log"), $"{item.Buyer}: {ex}\n");
                }
            });

            await Task.WhenAll(tasks);

            stopwatch.Stop();

            res.SetResult(ErrorStatusCode.Success);
            res.Data = new QRCodeResponseDto
            {
                successMsg = $"QR {successCount}건 생성 완료!",
                qrGenTime = (int)stopwatch.Elapsed.TotalSeconds,
                folderPath = savePath,
            };
            return res;
        }

        public async Task<GenericResponse<QRCodeResponseDto>> GenerateRegisterQRCodeSingleAsync(IssuanceRequestDto req, CancellationToken cancellationToken = default)
        {
            var res = new GenericResponse<QRCodeResponseDto>();

            await using var scope = await _connFactory.OpenSessionAsync(cancellationToken);
            var db = scope.Session;

            if(string.IsNullOrWhiteSpace(req.buyer) && string.IsNullOrWhiteSpace(req.buyer))
            {
                res.SetResult(ErrorStatusCode.Invalid_Error);
                res.ResultMsg = $"{req.buyer}에 대한 정보를 찾을 수 없음.";
                return res;
            }

            var result = await _repo.IC26DataDao.GenerateRegisterQRCodeSingleAsync(db, req);
            if (result is null && string.IsNullOrWhiteSpace(result?.UniqueId))
            {
                res.SetResult(ErrorStatusCode.Invalid_Error);
                res.ResultMsg = $"{result?.Buyer}에 대한 정보를 찾을 수 없음.";
                return res;
            }

            string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string downloadPath = Path.Combine(userProfile, "Downloads");
            string savePath = Path.Combine(downloadPath, ConferenceName, Today);

            Directory.CreateDirectory(savePath);

            var safeBuyer = string.Join("_", result.Buyer.Split(Path.GetInvalidFileNameChars()));
            var safePhone = string.Join("_", result.Phone.Split(Path.GetInvalidFileNameChars()));
            var fileName = $"{result.No}_{safeBuyer}_{safePhone}.jpg";
            var fullPath = Path.Combine(savePath, fileName);

            // QR 생성
            _qrCode.GenerateQRCodePngFile(result.UniqueId, fullPath);

            // QR 생성 여부 업데이트
            await _repo.IC26DataDao.CheckCreateQRAsync(db, result.UniqueId);

            res.SetResult(ErrorStatusCode.Success);
            res.Data = new QRCodeResponseDto {
                successMsg = $"{result.Buyer} QRCode 생성 완료!",
                folderPath = savePath,
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

        public async Task<GenericResponse<CheckInResponseDto>> CheckAttendanceAsync(string uniqueId, CancellationToken cancellationToken = default)
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

            if (string.IsNullOrWhiteSpace(uniqueId))
            {
                res.SetResult(ErrorStatusCode.Invalid_Error);
                res.ResultMsg = "QR Code 발급 키 누락";
                return res;
            }

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
    }
}

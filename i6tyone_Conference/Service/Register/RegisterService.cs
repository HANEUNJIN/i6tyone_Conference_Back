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

            var data = await _repo.RegisterDao.GenerateRegisterAsync(db, req);
            if (data < 0)
            {
                res.SetResult(ErrorStatusCode.Invalid_Error);
                res.ResultMsg = "컨퍼런스 등록 실패";
                return res;
            }
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

            //QRCode 발급여부에 대해 발급하지 않은 등록자들만 result 되도록 수정하기.
            var result = await _repo.RegisterDao.GenerateRegisterQRCodeAsync(db);
            if (result is null)
            {
                res.SetResult(ErrorStatusCode.Authentication_Failed);
                return res;
            }

            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string baseFolder = "2026 Solus CHRISTUS";
            string today = DateTime.Now.ToString("yyyy-MM-dd");
            string savePath = Path.Combine(desktopPath, baseFolder, today);

            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);

            var stopwatch = Stopwatch.StartNew();
            var successBag = new ConcurrentBag<bool>();

            Parallel.ForEach(result, item =>
            {
                try
                {
                    var fileName = $"{item.name}_{item.phoneNumber}";
                    var fullPath = Path.Combine(savePath, fileName + ".jpg");
                    _qrCode.GenerateQRCodePngFile(item.id, fullPath);

                    successBag.Add(true);
                }
                catch (Exception)
                {
                }
            });

            stopwatch.Stop();
            string Message = $"QR {successBag.Count}건 생성 완료!";

            res.SetResult(ErrorStatusCode.Success);
            res.Data = new QRCodeResponseDto
            {
                successMsg = Message,
                qrGenTime = (int)stopwatch.Elapsed.TotalSeconds
            };
            return res;
        }

        public async Task<GenericResponse<RegisterResponseDto>> GetRegisterInfoAsync(RegisterInfoRequestDto req, CancellationToken cancellationToken = default)
        {
            var res = new GenericResponse<RegisterResponseDto>();

            if (string.IsNullOrWhiteSpace(req.name) || string.IsNullOrWhiteSpace(req.phoneNumber))
            {
                res.SetResult(ErrorStatusCode.Invalid_Error);
                res.ResultMsg = "이름과 연락처를 기입해주세요.";
                return res;
            }

            await using var scope = await _connFactory.OpenSessionAsync(cancellationToken);
            var db = scope.Session;

            var data = await _repo.RegisterDao.GetRegisterInfoAsync(db, req);
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

        public async Task<GenericResponse<CheckInResponseDto>> CheckAttendanceAsync(string qrCodeKey, CancellationToken cancellationToken = default)
        {
            var res = new GenericResponse<CheckInResponseDto>();

            await using var scope = await _connFactory.OpenSessionAsync(cancellationToken);
            var db = scope.Session;

            if (string.IsNullOrWhiteSpace(qrCodeKey))
            {
                res.SetResult(ErrorStatusCode.Invalid_Error);
                res.ResultMsg = "QR Code 누락";
                return res;
            }

            var isCheckIn = await _repo.RegisterDao.CheckAttendanceAsync(db, qrCodeKey);
            if (!isCheckIn)
            {
                res.SetResult(ErrorStatusCode.Invalid_Error);
                res.ResultMsg = "컨퍼런스 체크인 실패";
                return res;
            }

            var registerInfo = await _repo.RegisterDao.GetRegisterDetailAsync(db, qrCodeKey);
            if (registerInfo is null || registerInfo.consentPrivacy == "N")
            {
                res.SetResult(ErrorStatusCode.Invalid_Error);
                res.ResultMsg = "등록자를 찾을 수 없음.";
                return res;
            }
            else
            {
                registerInfo.message = "2026 Solus CHRISTUS에 오신 것을 환영합니다!";

                res.SetResult(ErrorStatusCode.Success);
                res.Data = registerInfo;
                return res;
            }
        }
    }
}

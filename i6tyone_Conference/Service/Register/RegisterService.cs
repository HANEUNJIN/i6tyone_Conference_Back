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

        private readonly string ConferenceName = "2026 Solus CHRISTUS";
        private readonly string Today = DateTime.Now.ToString("yyyy-MM-dd");
        private readonly string SuccessMessage = "2026 Solus CHRISTUS에 오신 것을 환영합니다!";

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
                res.ResultMsg = "컨퍼런스 참가 등록 실패";
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

            var result = await _repo.RegisterDao.GenerateRegisterQRCodeAsync(db);
            if (result is null)
            {
                res.SetResult(ErrorStatusCode.Authentication_Failed);
                return res;
            }

            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string savePath = Path.Combine(desktopPath, ConferenceName, Today);

            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);

            var stopwatch = Stopwatch.StartNew();
            var successBag = new ConcurrentBag<bool>();

            Parallel.ForEach(result, item =>
            {
                try
                {
                    if(string.IsNullOrWhiteSpace(item.IC26UniqueId))
                    {
                        res.SetResult(ErrorStatusCode.Invalid_Error);
                        res.ResultMsg = $"{item.IC26Buyer}에 대한 정보를 찾을 수 없음.";
                        return;
                    }

                    var fileName = $"{item.IC26No}_{item.IC26Buyer}_{item.IC26Phone}";
                    var fullPath = Path.Combine(savePath, fileName + ".jpg");
                    _qrCode.GenerateQRCodePngFile(item.IC26UniqueId, fullPath);

                    successBag.Add(true);
                }
                catch (Exception ex)
                {
                    File.AppendAllText(Path.Combine(savePath, "error.log"), $"{item.IC26Buyer}: {ex}\n");
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

            if (string.IsNullOrWhiteSpace(req.iC26Buyer))
            {
                res.SetResult(ErrorStatusCode.Invalid_Error);
                res.ResultMsg = "구매자명 누락";
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

        public async Task<GenericResponse<CheckInResponseDto>> CheckAttendanceAsync(string iC26UniqueId, CancellationToken cancellationToken = default)
        {
            var res = new GenericResponse<CheckInResponseDto>();

            await using var scope = await _connFactory.OpenSessionAsync(cancellationToken);
            var db = scope.Session;

            if (string.IsNullOrWhiteSpace(iC26UniqueId))
            {
                res.SetResult(ErrorStatusCode.Invalid_Error);
                res.ResultMsg = "QR Code 발급 키 누락";
                return res;
            }

            var isCheckIn = await _repo.RegisterDao.CheckAttendanceAsync(db, iC26UniqueId);
            if (!isCheckIn)
            {
                res.SetResult(ErrorStatusCode.Invalid_Error);
                res.ResultMsg = "현장 입장 등록 실패";
                return res;
            }

            var registerInfo = await _repo.RegisterDao.GetRegisterDetailAsync(db, iC26UniqueId);
            if (registerInfo is null)
            {
                res.SetResult(ErrorStatusCode.Invalid_Error);
                res.ResultMsg = "정보를 찾을 수 없음.";
                return res;
            }
            
            var mappedInfo = _mapper.Map<CheckInResponseDto>(registerInfo);
            mappedInfo.message = SuccessMessage;

            res.SetResult(ErrorStatusCode.Success);
            res.Data = mappedInfo;
            return res;
        }
    }
}

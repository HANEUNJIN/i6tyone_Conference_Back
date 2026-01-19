using AutoMapper;
using eGhis_WebService_Core.Define;
using eGhis_WebService_Core.Infrastructure.Db;
using eGhis_WebService_Core.Infrastructure.Utils.Crypto;
using eGhis_WebService_Core.Models.Common;
using eGhis_WebService_Core.Models.Dto.Auth;
using eGhis_WebService_Core.Repositories;
using i6tyone_Conference.Models.Dto.Auth;
using i6tyone_Conference.Models.Dto.OnSite;
using i6tyone_Conference.Models.Dto.Register;

namespace i6tyone_Conference.Service.OnSiteRegister
{
    public class OnSiteRegisterService : IOnSiteRegisterService
    {
        private readonly IDbConnectionFactory _connFactory;
        private readonly ISqlRepository _repo;
        private readonly IMapper _mapper;

        public OnSiteRegisterService(IDbConnectionFactory connFactory, ISqlRepository repo, IMapper mapper)
        {
            _connFactory = connFactory;
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<GenericResponse<RegisterResponseDto>> GetOnSiteRegisterInfoAsync(RegisterInfoRequestDto req, CancellationToken cancellationToken = default)
        {
            var res = new GenericResponse<RegisterResponseDto>();

            await using var scope = await _connFactory.OpenSessionAsync(cancellationToken);
            var db = scope.Session;

            if (!string.IsNullOrWhiteSpace(req.keyword))
            {
                req.keyword = req.keyword.Replace("-", "");
            }

            var data = await _repo.IC26DataDao.GetOnSiteRegisterInfoAsync(db, req);
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

        public async Task<GenericResponse<RegisterInfoResponseDto>> GetOnSiteRegisterDetailInfoAsync(string uniqueId, CancellationToken cancellationToken = default)
        {
            var res = new GenericResponse<RegisterInfoResponseDto>();

            await using var scope = await _connFactory.OpenSessionAsync(cancellationToken);
            var db = scope.Session;

            if (string.IsNullOrWhiteSpace(uniqueId))
            {
                res.SetResult(ErrorStatusCode.Invalid_Error);
                res.ResultMsg = "QR Code 발급 키 누락";
                return res;
            }

            var data = await _repo.IC26DataDao.GetOnSiteRegisterDetailInfoAsync(db, uniqueId);
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

        public async Task<GenericResponse<PayResponseDto>> CompletePaymentAsync(string uniqueId, CancellationToken cancellationToken = default)
        {
            var res = new GenericResponse<PayResponseDto>();

            await using var scope = await _connFactory.OpenSessionAsync(cancellationToken);
            var db = scope.Session;

            if (string.IsNullOrWhiteSpace(uniqueId))
            {
                res.SetResult(ErrorStatusCode.Invalid_Error);
                res.ResultMsg = "QR Code 발급 키 누락";
                return res;
            }

            var isSuccess = await _repo.IC26DataDao.CompletePaymentAsync(uniqueId, db);
            if (!isSuccess)
            {
                res.SetResult(ErrorStatusCode.DB_Update_Error);
                return res;
            }

            var info = await _repo.IC26DataDao.GetOnSiteRegisterDetailInfoAsync(db, uniqueId);
            if (info is null)
            {
                res.SetResult(ErrorStatusCode.Error);
                res.ResultMsg = "해당 등록자가 조회되지 않습니다.";
                return res;
            }

            var req = new RegisterRequestDto()
            {
                option = info.Option,
                day = info.Day,
                buyer = info.Buyer,
                attender = info.Attender,
                phone = info.Phone,
                gender = info.Gender,
                age = info.Age,
                church = info.Church,
                local = info.Local,
                denom = info.Denom,
                count = info.Count,
                applyYmd = info.ApplyYmd,
                newBelieverYn = "",
                notionSmsYn = "N",
            };

            var data = await _repo.IC26DataDao.GenerateRegisterAsync(db, req, uniqueId);
            bool success = data >= 0;
            if (!success)
            {
                res.SetResult(ErrorStatusCode.Invalid_Error);
                res.ResultMsg = "컨퍼런스 참가 등록 실패";
                return res;
            }

            var result = new PayResponseDto() { success = success };

            res.SetResult(ErrorStatusCode.Success);
            res.Data = result;
            return res;
        }
    }
}

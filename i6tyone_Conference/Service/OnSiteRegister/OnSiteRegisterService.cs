using AutoMapper;
using eGhis_WebService_Core.Define;
using eGhis_WebService_Core.Infrastructure.Db;
using eGhis_WebService_Core.Infrastructure.Utils.Crypto;
using eGhis_WebService_Core.Models.Common;
using eGhis_WebService_Core.Repositories;
using i6tyone_Conference.Models.Dto.Auth;
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
    }
}

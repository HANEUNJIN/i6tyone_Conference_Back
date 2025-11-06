using AutoMapper;
using eGhis_WebService_Core.Define;
using eGhis_WebService_Core.Infrastructure.Db;
using eGhis_WebService_Core.Models.Common;
using eGhis_WebService_Core.Repositories;
using i6tyone_Conference.Infrastructure.Utils;
using i6tyone_Conference.Models.Dto.Auth;
using i6tyone_Conference.Models.Dto.Register;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace i6tyone_Conference.Service.Management
{
    public class ManagementService : IManagementService
    {
        private readonly IDbConnectionFactory _connFactory;
        private readonly ISqlRepository _repo;
        private readonly IMapper _mapper;

        private readonly string ConferenceName = "2026 Solus CHRISTUS QRCode";
        private readonly string Today = DateTime.Now.ToString("yyyy-MM-dd");

        public ManagementService(IDbConnectionFactory connFactory, ISqlRepository repo, IMapper mapper)
        {
            _connFactory = connFactory;
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<GenericResponse<SuccessResponseDto>> ClearAttendAsync(CancellationToken cancellationToken = default)
        {
            var res = new GenericResponse<SuccessResponseDto>();

            await using var scope = await _connFactory.OpenSessionAsync(cancellationToken);
            var db = scope.Session;

            var isClear = await _repo.IC26DataDao.ClearAttendAsync(db);
            if (!isClear)
            {
                res.SetResult(ErrorStatusCode.Invalid_Error);
                return res;
            }

            var result = new SuccessResponseDto() { success = isClear };

            res.SetResult(ErrorStatusCode.Success);
            res.Data = result;
            return res;
        }

        public async Task<GenericResponse<SuccessResponseDto>> ClearCreateQRAsync(CancellationToken cancellationToken = default)
        {
            var res = new GenericResponse<SuccessResponseDto>();

            await using var scope = await _connFactory.OpenSessionAsync(cancellationToken);
            var db = scope.Session;

            var isClear = await _repo.IC26DataDao.ClearCreateQRAsync(db);
            if (!isClear)
            {
                res.SetResult(ErrorStatusCode.Invalid_Error);
                return res;
            }

            var result = new SuccessResponseDto() { success = isClear };

            res.SetResult(ErrorStatusCode.Success);
            res.Data = result;
            return res;
        }
    }
}

using eGhis_WebService_Core.Define;
using eGhis_WebService_Core.Infrastructure.Db;
using eGhis_WebService_Core.Models.Common;
using eGhis_WebService_Core.Repositories;
using i6tyone_Conference.Models.Dto.Register;

namespace i6tyone_Conference.Service.Management
{
    public class ManagementService : IManagementService
    {
        private readonly IDbConnectionFactory _connFactory;
        private readonly ISqlRepository _repo;

        public ManagementService(IDbConnectionFactory connFactory, ISqlRepository repo)
        {
            _connFactory = connFactory;
            _repo = repo;
        }

        public async Task<GenericResponse<SuccessResponseDto>> ClearAttendAsync(CancellationToken cancellationToken = default)
        {
            var res = new GenericResponse<SuccessResponseDto>();

            await using var scope = await _connFactory.OpenSessionAsync(cancellationToken);
            var db = scope.Session;

            var isSuccess = await _repo.IC26DataDao.ClearAttendAsync(db);
            if (!isSuccess)
            {
                res.SetResult(ErrorStatusCode.DB_Update_Error);
                return res;
            }

            var result = new SuccessResponseDto() { success = isSuccess };

            res.SetResult(ErrorStatusCode.Success);
            res.Data = result;
            return res;
        }

        public async Task<GenericResponse<SuccessResponseDto>> ClearCreateQRAsync(CancellationToken cancellationToken = default)
        {
            var res = new GenericResponse<SuccessResponseDto>();

            await using var scope = await _connFactory.OpenSessionAsync(cancellationToken);
            var db = scope.Session;

            var isSuccess = await _repo.IC26DataDao.ClearCreateQRAsync(db);
            if (!isSuccess)
            {
                res.SetResult(ErrorStatusCode.DB_Update_Error);
                return res;
            }

            var result = new SuccessResponseDto() { success = isSuccess };

            res.SetResult(ErrorStatusCode.Success);
            res.Data = result;
            return res;
        }

        public async Task<GenericResponse<SuccessResponseDto>> ClearSmsAsync(CancellationToken cancellationToken = default)
        {
            var res = new GenericResponse<SuccessResponseDto>();

            await using var scope = await _connFactory.OpenSessionAsync(cancellationToken);
            var db = scope.Session;

            var isSuccess = await _repo.IC26DataDao.ClearSmsAsync(db);
            if (!isSuccess)
            {
                res.SetResult(ErrorStatusCode.DB_Update_Error);
                return res;
            }

            var result = new SuccessResponseDto() { success = isSuccess };

            res.SetResult(ErrorStatusCode.Success);
            res.Data = result;
            return res;
        }
    }
}

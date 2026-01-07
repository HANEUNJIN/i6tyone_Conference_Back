using AutoMapper;
using eGhis_WebService_Core.Define;
using eGhis_WebService_Core.Infrastructure.Db;
using eGhis_WebService_Core.Models.Common;
using eGhis_WebService_Core.Repositories;
using i6tyone_Conference.Models.Dto.Statistics;

namespace i6tyone_Conference.Service.Statistics
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IDbConnectionFactory _connFactory;
        private readonly ISqlRepository _repo;
        private readonly IMapper _mapper;

        public StatisticsService(IDbConnectionFactory connFactory, ISqlRepository repo, IMapper mapper)
        {
            _connFactory = connFactory;
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<GenericResponse<TicketRequestDto>> GetTicketAsync(CancellationToken cancellationToken = default)
        {
            var res = new GenericResponse<TicketRequestDto>();

            await using var scope = await _connFactory.OpenSessionAsync(cancellationToken);
            var db = scope.Session;

            var result = await _repo.IC26DataDao.GetTicketAsync(db);

            res.SetResult(ErrorStatusCode.Success);
            res.Data = result;
            return res;
        }

        public async Task<GenericResponse<DateRequestDto>> GetDataAsync(CancellationToken cancellationToken = default)
        {
            var res = new GenericResponse<DateRequestDto>();

            await using var scope = await _connFactory.OpenSessionAsync(cancellationToken);
            var db = scope.Session;

            var result = await _repo.IC26DataDao.GetDataAsync(db);

            res.SetResult(ErrorStatusCode.Success);
            res.Data = new DateRequestDto() { list = result };
            return res;
        }

        public async Task<GenericResponse<TicketOptionRequestDto>> GetTicketOptionAsync(CancellationToken cancellationToken = default)
        {
            var res = new GenericResponse<TicketOptionRequestDto>();

            await using var scope = await _connFactory.OpenSessionAsync(cancellationToken);
            var db = scope.Session;

            var data = await _repo.IC26DataDao.GetTicketOptionAsync(db);
            var result = new TicketOptionRequestDto() { list = data };

            res.SetResult(ErrorStatusCode.Success);
            res.Data = result;
            return res;
        }

        public async Task<GenericResponse<AreaRequestDto>> GetAreaAsync(CancellationToken cancellationToken = default)
        {
            var res = new GenericResponse<AreaRequestDto>();

            await using var scope = await _connFactory.OpenSessionAsync(cancellationToken);
            var db = scope.Session;

            var result = await _repo.IC26DataDao.GetAreaAsync(db);

            res.SetResult(ErrorStatusCode.Success);
            res.Data = result;
            return res;
        }

        public async Task<GenericResponse<RegistrationRequestDto>> GetRegistrationAsync(CancellationToken cancellationToken = default)
        {
            var res = new GenericResponse<RegistrationRequestDto>();

            await using var scope = await _connFactory.OpenSessionAsync(cancellationToken);
            var db = scope.Session;

            var result = await _repo.IC26DataDao.GetRegistrationAsync(db);

            res.SetResult(ErrorStatusCode.Success);
            res.Data = result;
            return res;
        }

        public async Task<GenericResponse<SendRequestDto>> GetSendAsync(CancellationToken cancellationToken = default)
        {
            var res = new GenericResponse<SendRequestDto>();

            await using var scope = await _connFactory.OpenSessionAsync(cancellationToken);
            var db = scope.Session;

            var result = await _repo.IC26DataDao.GetSendAsync(db);

            res.SetResult(ErrorStatusCode.Success);
            res.Data = new SendRequestDto() { list = result };
            return res;
        }
    }
}
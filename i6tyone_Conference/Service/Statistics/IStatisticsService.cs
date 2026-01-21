using eGhis_WebService_Core.Infrastructure.Common.Interfaces;
using eGhis_WebService_Core.Models.Common;
using i6tyone_Conference.Models.Dto.Statistics;

namespace i6tyone_Conference.Service.Statistics
{
    public interface IStatisticsService : IServiceMarker
    {
        public Task<GenericResponse<RegistrationRequestDto>> GetRegistrationAsync(CancellationToken cancellationToken = default);
        public Task<GenericResponse<TicketRequestDto>> GetTicketAsync(CancellationToken cancellationToken = default);
        public Task<GenericResponse<DateRequestDto>> GetDataAsync(CancellationToken cancellationToken = default);
        public Task<GenericResponse<TicketOptionRequestDto>> GetTicketOptionAsync(CancellationToken cancellationToken = default);
        public Task<GenericResponse<AreaRequestDto>> GetAreaAsync(CancellationToken cancellationToken = default);
        public Task<GenericResponse<AreaSeatStatsRequestDto>> GetAreaSeatStatsAsync(CancellationToken cancellationToken = default);
        public Task<GenericResponse<SendRequestDto>> GetSendAsync(CancellationToken cancellationToken = default);
        public Task<GenericResponse<DayRequestDto>> GetDayAsync(CancellationToken cancellationToken = default);
    }
}

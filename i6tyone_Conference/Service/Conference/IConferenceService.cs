using eGhis_WebService_Core.Infrastructure.Common.Interfaces;
using eGhis_WebService_Core.Models.Common;
using i6tyone_Conference.Models.Dto.Conference;

namespace i6tyone_Conference.Service.Conference
{
    public interface IConferenceService : IServiceMarker
    {
        public Task<GenericResponse<KeyValueResponseDto>> GetDayListAsync();
        public Task<GenericResponse<KeyValueResponseDto>> GetOptionListAsync();
        public Task<GenericResponse<KeyValueResponseDto>> GetAreaListAsync();
    }
}

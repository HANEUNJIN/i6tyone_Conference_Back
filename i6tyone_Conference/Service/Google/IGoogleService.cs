using eGhis_WebService_Core.Infrastructure.Common.Interfaces;
using eGhis_WebService_Core.Models.Common;
using i6tyone_Conference.Models.Dto.Auth;

namespace i6tyone_Conference.Service.Google
{
    public interface IGoogleService : IServiceMarker
    {
        public Task<GenericResponse<RegisterAddResponseDto>> GetGoogleSheetAsync(CancellationToken cancellationToken = default);
    }
}

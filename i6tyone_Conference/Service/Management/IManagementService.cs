using eGhis_WebService_Core.Infrastructure.Common.Interfaces;
using eGhis_WebService_Core.Models.Common;
using i6tyone_Conference.Models.Dto.Management;
using i6tyone_Conference.Models.Dto.Register;

namespace i6tyone_Conference.Service.Management
{
    public interface IManagementService : IServiceMarker
    {
        public Task<GenericResponse<attendResponseDto>> ClearAttendAsync(CancellationToken cancellationToken = default);
        public Task<GenericResponse<SuccessResponseDto>> ClearCreateQRAsync(CancellationToken cancellationToken = default);
        public Task<GenericResponse<SuccessResponseDto>> ClearSmsAsync(CancellationToken cancellationToken = default);
    }
}

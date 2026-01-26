using eGhis_WebService_Core.Infrastructure.Common.Interfaces;
using eGhis_WebService_Core.Models.Common;
using i6tyone_Conference.Models.Dto.Auth;
using i6tyone_Conference.Models.Dto.OnSite;
using i6tyone_Conference.Models.Dto.Register;

namespace i6tyone_Conference.Service.OnSiteRegister
{
    public interface IOnSiteRegisterService : IServiceMarker
    {
        public Task<GenericResponse<RegisterResponseDto>> GetOnSiteRegisterInfoAsync(RegisterInfoRequestDto req, CancellationToken cancellationToken = default);
        public Task<GenericResponse<RegisterInfoResponseDto>> GetOnSiteRegisterDetailInfoAsync(string uniqueIdKey, CancellationToken cancellationToken = default);
        public Task<GenericResponse<PayResponseDto>> CompletePaymentAsync(string uniqueIdKey, CancellationToken cancellationToken = default);
        public Task<GenericResponse<SuccessResponseDto>> DisposeOnSiteRegisterAsync(string uniqueIdKey, CancellationToken cancellationToken = default);
    }
}

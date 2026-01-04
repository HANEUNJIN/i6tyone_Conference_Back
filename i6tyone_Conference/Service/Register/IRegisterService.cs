using eGhis_WebService_Core.Infrastructure.Common.Interfaces;
using eGhis_WebService_Core.Models.Common;
using eGhis_WebService_Core.Models.Dto.Auth;
using i6tyone_Conference.Models.Dto.Aligo;
using i6tyone_Conference.Models.Dto.Register;
using i6tyone_Conference.Models.Dto.Auth;

namespace eGhis_WebService_Core.Service.Auth
{
    public interface IRegisterService : IServiceMarker
    {
        public Task<GenericResponse<RegisterAddResponseDto>> GenerateRegisterAsync(RegisterRequestDto req, CancellationToken cancellationToken = default);
        public Task<GenericResponse<SuccessResponseDto>> UpdateRegisterAsync(RegisterUpdateRequestDto req, CancellationToken cancellationToken = default);
        public Task<GenericResponse<SuccessResponseDto>> DisposeRegisterAsync(string uniqueIdKey, CancellationToken cancellationToken = default);
        public Task<GenericResponse<SuccessResponseDto>> DeleteRegisterAsync(string uniqueIdKey, CancellationToken cancellationToken = default);
        public Task<GenericResponse<SendMassResponseDto>> GenerateRegisterQRCodeAsync(CancellationToken cancellationToken = default);
        public Task<GenericResponse<SendMassResponseDto>> GenerateRegisterQRCodeSingleAsync(IssuanceRequestDto req, CancellationToken cancellationToken = default);
        public Task<GenericResponse<RegisterResponseDto>> GetRegisterInfoAsync(RegisterInfoRequestDto req, CancellationToken cancellationToken = default);
        public Task<GenericResponse<ExcelResponseDto>> GetRegisterExcelAsync(CancellationToken cancellationToken = default);
        public Task<GenericResponse<RegisterInfoResponseDto>> GetRegisterDetailInfoAsync(string uniqueIdKey, CancellationToken cancellationToken = default);
        public Task<GenericResponse<CheckInResponseDto>> CheckAttendanceAsync(string iC26UniqueId, CancellationToken cancellationToken = default);
        public Task<GenericResponse<FtpInfoResponseDto>> GetFtpInfoAsync();
    }
}

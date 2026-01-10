using eGhis_WebService_Core.Infrastructure.Common.Interfaces;
using eGhis_WebService_Core.Models.Common;
using i6tyone_Conference.Models.Dto.Auth;
using i6tyone_Conference.Models.Dto.Excel;

namespace i6tyone_Conference.Service.Excel
{
    public interface IExcelService : IServiceMarker
    {
        public Task<GenericResponse<RegisterAddResponseDto>> GetGoogleSheetAsync(CancellationToken cancellationToken = default);
        public Task<GenericResponse<RegisterAddResponseDto>> GetEventUsSheetAsync(CsvRequestDto req, CancellationToken cancellationToken = default);
    }
}

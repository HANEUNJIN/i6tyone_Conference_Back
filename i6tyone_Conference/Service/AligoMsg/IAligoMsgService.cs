using eGhis_WebService_Core.Infrastructure.Common.Interfaces;
using eGhis_WebService_Core.Models.Common;
using i6tyone_Conference.Models.Dto.Aligo;
using i6tyone_Conference.Models.Dto.AligoMsg;

namespace i6tyone_Conference.Service.Aligo
{
    public interface IAligoMsgService : IServiceMarker
    {
        public Task<GenericResponse<SendMassResponseDto>> SendSmsAsync(SendMessageApiRequestDto req);
        public Task<GenericResponse<SendMassResponseDto>> SendMultiSmsAsync(SendMassApiRequestDto req);
        public Task<GenericResponse<SendResponseDto>> GetSendHistoryAsync(SendApiRequestDto req);
        public Task<GenericResponse<SmsResponseDto>> GetSendResultDetailAsync(SmsApiRequestDto req);
        public Task<GenericResponse<RemainResponseDto>> GetSmsBalanceAsync();
        public Task<GenericResponse<CancelResponseDto>> CancelSmsAsync(long mid);
    }
}

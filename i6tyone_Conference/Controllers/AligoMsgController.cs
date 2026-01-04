using eGhis_WebService_Core.Infrastructure.Attributes;
using eGhis_WebService_Core.Models.Common;
using i6tyone_Conference.Models.Dto.Aligo;
using i6tyone_Conference.Models.Dto.AligoMsg;
using i6tyone_Conference.Service.Aligo;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using eGhis_WebService_Core.Controllers.Base;

namespace i6tyone_Conference.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AligoMsgController : BaseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAligoMsgService _aligoMsgService;


        public AligoMsgController(IHttpContextAccessor httpContextAccessor, IAligoMsgService aligoMsgService)
        {
            _httpContextAccessor = httpContextAccessor;
            _aligoMsgService = aligoMsgService;
        }

        /// <summary>
        /// 문자전송
        /// </summary>
        /// <returns>동일내용을 1천명에게 동시전송</returns>
        /// <returns>요청 정보<see cref="SendMessageApiRequestDto"/></returns>
        /// <returns>응답 정보<see cref="SendMassResponseDto"/></returns>
        [HttpPost("send")]
        [AllowAnonymousToken]
        [SwaggerResponse(200, typeof(GenericResponse<SendMassResponseDto>), Description = "정상 처리되었습니다.")]
        public async Task<ActionResult<GenericResponse<SendMassResponseDto>>> SendSms(SendMessageApiRequestDto req)
        {
            var res = await _aligoMsgService.SendSmsAsync(req);
            return Ok(res);
        }

        /// <summary>
        /// 문자전송(대량)
        /// </summary>
        /// <returns>각기 다른내용을 5백명에게 동시 전송</returns>
        /// <returns>요청 정보<see cref="SendMassApiRequestDto"/></returns>
        /// <returns>응답 정보<see cref="SendMassResponseDto"/></returns>
        [HttpPost("send-mult")]
        [AllowAnonymousToken]
        [SwaggerResponse(200, typeof(GenericResponse<SendMassResponseDto>), Description = "정상 처리되었습니다.")]
        public async Task<ActionResult<GenericResponse<SendMassResponseDto>>> SendMultiSms(SendMassApiRequestDto req)
        {
            var res = await _aligoMsgService.SendMultiSmsAsync(req);
            return Ok(res);
        }

        /// <summary>
        /// 전송내역조회
        /// </summary>
        /// <returns>최근발송된 전송내역조회</returns>
        /// <returns>요청 정보<see cref="SendApiRequestDto"/></returns>
        /// <returns>응답 정보<see cref="SendResponseDto"/></returns>
        [HttpPost("history")]
        [AllowAnonymousToken]
        [SwaggerResponse(200, typeof(GenericResponse<SendResponseDto>), Description = "정상 처리되었습니다.")]
        public async Task<ActionResult<GenericResponse<SendResponseDto>>> GetSendHistory(SendApiRequestDto req)
        {
            var res = await _aligoMsgService.GetSendHistoryAsync(req);
            return Ok(res);
        }

        /// <summary>
        /// 전송결과조회(상세)
        /// </summary>
        /// <returns>전화번호별 성공유무 조회</returns>
        /// <returns>요청 정보<see cref="SmsApiRequestDto"/></returns>
        /// <returns>응답 정보<see cref="SmsResponseDto"/></returns>
        [HttpPost("result-detail")]
        [AllowAnonymousToken]
        [SwaggerResponse(200, typeof(GenericResponse<SmsResponseDto>), Description = "정상 처리되었습니다.")]
        public async Task<ActionResult<GenericResponse<SmsResponseDto>>> GetSendResultDetail(SmsApiRequestDto req)
        {
            var res = await _aligoMsgService.GetSendResultDetailAsync(req);
            return Ok(res);
        }

        /// <summary>
        /// 발송가능건수
        /// </summary>
        /// <returns>잔여건수 조회</returns>
        /// <returns>응답 정보<see cref="RemainResponseDto"/></returns>
        [HttpPost("balance")]
        [AllowAnonymousToken]
        [SwaggerResponse(200, typeof(GenericResponse<RemainResponseDto>), Description = "정상 처리되었습니다.")]
        public async Task<ActionResult<GenericResponse<RemainResponseDto>>> GetSmsBalance()
        {
            var res = await _aligoMsgService.GetSmsBalanceAsync();
            return Ok(res);
        }

        /// <summary>
        /// 예약문자 취소
        /// </summary>
        /// <returns>예약대기중 문자 취소요청</returns>
        /// <param name="mid">메시지ID</param>
        /// <returns>응답 정보<see cref="CancelResponseDto"/></returns>
        [HttpPost("cancel")]
        [AllowAnonymousToken]
        [SwaggerResponse(200, typeof(GenericResponse<CancelResponseDto>), Description = "정상 처리되었습니다.")]
        public async Task<ActionResult<GenericResponse<CancelResponseDto>>> CancelSms(long mid)
        {
            var res = await _aligoMsgService.CancelSmsAsync(mid);
            return Ok(res);
        }
    }
}

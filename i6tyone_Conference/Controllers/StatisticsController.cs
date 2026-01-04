using eGhis_WebService_Core.Controllers.Base;
using eGhis_WebService_Core.Infrastructure.Attributes;
using eGhis_WebService_Core.Models.Common;
using i6tyone_Conference.Models.Dto.Statistics;
using i6tyone_Conference.Service.Statistics;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace i6tyone_Conference.Controllers
{
    [Route("api/[controller]")]
    public class StatisticsController : BaseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStatisticsService _statisticsService;

        public StatisticsController(IHttpContextAccessor httpContextAccessor, IStatisticsService statisticsService)
        {
            _httpContextAccessor = httpContextAccessor;
            _statisticsService = statisticsService;
        }

        /// <summary>
        /// 등록자별 통계
        /// </summary>
        /// <returns>응답 정보<see cref="RegistrationRequestDto"/></returns>
        /// <returns></returns>
        [HttpGet("registration")]
        [AllowAnonymousToken]
        [SwaggerResponse(200, typeof(GenericResponse<RegistrationRequestDto>), Description = "정상 처리되었습니다.")]
        public async Task<ActionResult<GenericResponse<RegistrationRequestDto>>> GetRegistration()
        {
            var res = await _statisticsService.GetRegistrationAsync();
            return Ok(res);
        }

        /// <summary>
        /// 티켓 구매 수량 통계
        /// </summary>
        /// <returns>응답 정보<see cref="TicketRequestDto"/></returns>
        /// <returns></returns>
        [HttpGet("ticket")]
        [AllowAnonymousToken]
        [SwaggerResponse(200, typeof(GenericResponse<TicketRequestDto>), Description = "정상 처리되었습니다.")]
        public async Task<ActionResult<GenericResponse<TicketRequestDto>>> GetTicket()
        {
            var res = await _statisticsService.GetTicketAsync();
            return Ok(res);
        }

        /// <summary>
        /// 날짜별 통계
        /// </summary>
        /// <returns>응답 정보<see cref="DateRequestDto"/></returns>
        /// <returns></returns>
        [HttpGet("daily")]
        [AllowAnonymousToken]
        [SwaggerResponse(200, typeof(GenericResponse<DateRequestDto>), Description = "정상 처리되었습니다.")]
        public async Task<ActionResult<GenericResponse<DateRequestDto>>> GetData()
        {
            var res = await _statisticsService.GetDataAsync();
            return Ok(res);
        }

        /// <summary>
        /// 티켓구분별·신청일자별 구매 수량 현황
        /// </summary>
        /// <param name="adminPassword">관리자 비밀번호</param>
        /// <returns>응답 정보<see cref="TicketOptionRequestDto"/></returns>
        [HttpGet("ticket-option-summary")]
        [AllowAnonymousToken]
        [SwaggerResponse(200, typeof(GenericResponse<TicketOptionRequestDto>), Description = "정상 처리되었습니다.")]
        public async Task<ActionResult<GenericResponse<TicketOptionRequestDto>>> GetTicketOption(string adminPassword)
        {
            var res = await _statisticsService.GetTicketOptionAsync(adminPassword);
            return Ok(res);
        }

        /// <summary>
        /// 좌석별 통계
        /// </summary>
        /// <returns>응답 정보<see cref="AreaRequestDto"/></returns>
        /// <returns></returns>
        [HttpGet("area")]
        [AllowAnonymousToken]
        [SwaggerResponse(200, typeof(GenericResponse<AreaRequestDto>), Description = "정상 처리되었습니다.")]
        public async Task<ActionResult<GenericResponse<AreaRequestDto>>> GetArea()
        {
            var res = await _statisticsService.GetAreaAsync();
            return Ok(res);
        }

        /// <summary>
        /// 전송별 통계
        /// </summary>
        /// <returns>응답 정보<see cref="SendRequestDto"/></returns>
        /// <returns></returns>
        [HttpGet("send")]
        [AllowAnonymousToken]
        [SwaggerResponse(200, typeof(GenericResponse<SendRequestDto>), Description = "정상 처리되었습니다.")]
        public async Task<ActionResult<GenericResponse<SendRequestDto>>> GetSend()
        {
            var res = await _statisticsService.GetSendAsync();
            return Ok(res);
        }
    }
}
using eGhis_WebService_Core.Controllers.Base;
using eGhis_WebService_Core.Infrastructure.Attributes;
using eGhis_WebService_Core.Models.Common;
using i6tyone_Conference.Infrastructure.Utils;
using i6tyone_Conference.Models.Dto.Conference;
using i6tyone_Conference.Service.Conference;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace i6tyone_Conference.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConferenceController : BaseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConferenceService _conferenceService;

        public ConferenceController(IHttpContextAccessor httpContextAccessor, IConferenceService conferenceService)
        {
            _httpContextAccessor = httpContextAccessor;
            _conferenceService = conferenceService;
        }

        /// <summary>
        /// 신청일(Day) 목록 조회
        /// </summary>
        /// <returns></returns>
        /// <returns>응답 정보<see cref="StringKeyValue"/></returns>
        [HttpGet("days")]
        [AllowAnonymousToken]
        [SwaggerResponse(200, typeof(GenericResponse<KeyValueResponseDto>), Description = "정상 처리되었습니다.")]
        public async Task<ActionResult<GenericResponse<KeyValueResponseDto>>> GetDayList()
        {
            var res = await _conferenceService.GetDayListAsync();
            return Ok(res);
        }

        /// <summary>
        /// 티켓구분(Option) 목록 조회
        /// </summary>
        /// <returns></returns>
        /// <returns>응답 정보<see cref="StringKeyValue"/></returns>
        [HttpGet("options")]
        [AllowAnonymousToken]
        [SwaggerResponse(200, typeof(GenericResponse<KeyValueResponseDto>), Description = "정상 처리되었습니다.")]
        public async Task<ActionResult<GenericResponse<KeyValueResponseDto>>> GetOptionList()
        {
            var res = await _conferenceService.GetOptionListAsync();
            return Ok(res);
        }
    }
}

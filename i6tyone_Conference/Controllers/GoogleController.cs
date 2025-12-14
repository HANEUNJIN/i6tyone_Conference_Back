using eGhis_WebService_Core.Controllers.Base;
using eGhis_WebService_Core.Infrastructure.Attributes;
using eGhis_WebService_Core.Models.Common;
using i6tyone_Conference.Models.Dto.Auth;
using i6tyone_Conference.Service.Google;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace i6tyone_Conference.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoogleController : BaseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGoogleService _googleService;

        public GoogleController(IHttpContextAccessor httpContextAccessor, IGoogleService googleService)
        {
            _httpContextAccessor = httpContextAccessor;
            _googleService = googleService;
        }

        /// <summary>
        /// Google Sheet 동기화
        /// </summary>
        /// <returns>응답 정보<see cref="RegisterAddResponseDto"/></returns>
        [HttpGet("google-sheet")]
        [AllowAnonymousToken]
        [NonAction]
        [SwaggerResponse(200, typeof(GenericResponse<RegisterAddResponseDto>), Description = "정상 처리되었습니다.")]
        public async Task<ActionResult<GenericResponse<RegisterAddResponseDto>>> GetGoogleSheet()
        {
            var res = await _googleService.GetGoogleSheetAsync();
            return Ok(res);
        }
    }
}

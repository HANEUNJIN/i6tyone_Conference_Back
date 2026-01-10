using eGhis_WebService_Core.Controllers.Base;
using eGhis_WebService_Core.Infrastructure.Attributes;
using eGhis_WebService_Core.Models.Common;
using i6tyone_Conference.Models.Dto.Auth;
using i6tyone_Conference.Models.Dto.Excel;
using i6tyone_Conference.Service.Excel;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace i6tyone_Conference.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExcelController : BaseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IExcelService _excelService;

        public ExcelController(IHttpContextAccessor httpContextAccessor, IExcelService excelService)
        {
            _httpContextAccessor = httpContextAccessor;
            _excelService = excelService;
        }

        /// <summary>
        /// Google Sheet 동기화
        /// </summary>
        /// <returns>응답 정보<see cref="RegisterAddResponseDto"/></returns>
        [HttpGet("google-sheet")]
        [AllowAnonymousToken]
        //[NonAction]
        [SwaggerResponse(200, typeof(GenericResponse<RegisterAddResponseDto>), Description = "정상 처리되었습니다.")]
        public async Task<ActionResult<GenericResponse<RegisterAddResponseDto>>> GetGoogleSheet()
        {
            var res = await _excelService.GetGoogleSheetAsync();
            return Ok(res);
        }

        /// <summary>
        /// 이벤터스 CSB 파일 동기화
        /// </summary>
        /// <returns>요청 정보<see cref="CsvRequestDto"/></returns>
        /// <returns>응답 정보<see cref="RegisterAddResponseDto"/></returns>
        [HttpPost("eventus-sheet")]
        [AllowAnonymousToken]
        //[NonAction]
        [SwaggerResponse(200, typeof(GenericResponse<RegisterAddResponseDto>), Description = "정상 처리되었습니다.")]
        public async Task<ActionResult<GenericResponse<RegisterAddResponseDto>>> GetEventUsSheet(CsvRequestDto req)
        {
            var res = await _excelService.GetEventUsSheetAsync(req);
            return Ok(res);
        }
    }
}

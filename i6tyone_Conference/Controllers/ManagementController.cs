using eGhis_WebService_Core.Controllers.Base;
using eGhis_WebService_Core.Infrastructure.Attributes;
using eGhis_WebService_Core.Models.Common;
using i6tyone_Conference.Models.Dto.Register;
using i6tyone_Conference.Service.Management;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace i6tyone_Conference.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagementController : BaseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IManagementService _managementService;

        public ManagementController(IHttpContextAccessor httpContextAccessor, IManagementService managementService)
        {
            _httpContextAccessor = httpContextAccessor;
            _managementService = managementService;
        }

        /// <summary>
        /// 출석여부 일괄 초기화
        /// </summary>
        /// <returns>응답 정보<see cref="SuccessResponseDto"/></returns>
        /// <returns>출석여부 '0'으로 일괄 초기화합니다.</returns>
        [HttpPost("attend-clear")]
        [AllowAnonymousToken]
        [SwaggerResponse(200, typeof(GenericResponse<SuccessResponseDto>), Description = "정상 처리되었습니다.")]
        public async Task<ActionResult<GenericResponse<SuccessResponseDto>>> ClearAttend()
        {
            var res = await _managementService.ClearAttendAsync();
            return Ok(res);
        }

        /// <summary>
        /// QR 생성여부 일괄 초기화
        /// </summary>
        /// <returns>응답 정보<see cref="SuccessResponseDto"/></returns>
        /// <returns>QR 생성여부 '0'으로 일괄 초기화합니다.</returns>
        [HttpPost("createqr-clear")]
        [AllowAnonymousToken]
        [SwaggerResponse(200, typeof(GenericResponse<SuccessResponseDto>), Description = "정상 처리되었습니다.")]
        public async Task<ActionResult<GenericResponse<SuccessResponseDto>>> ClearCreateQR()
        {
            var res = await _managementService.ClearCreateQRAsync();
            return Ok(res);
        }

        /// <summary>
        /// SMS 전송여부 일괄 초기화
        /// </summary>
        /// <returns>응답 정보<see cref="SuccessResponseDto"/></returns>
        /// <returns>QR 생성여부 '0'으로 일괄 초기화합니다.</returns>
        [HttpPost("sms-clear")]
        [AllowAnonymousToken]
        [SwaggerResponse(200, typeof(GenericResponse<SuccessResponseDto>), Description = "정상 처리되었습니다.")]
        public async Task<ActionResult<GenericResponse<SuccessResponseDto>>> ClearSms()
        {
            var res = await _managementService.ClearSmsAsync();
            return Ok(res);
        }
    }
}

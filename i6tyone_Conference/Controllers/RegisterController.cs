using eGhis_WebService_Core.Controllers.Base;
using eGhis_WebService_Core.Infrastructure.Attributes;
using eGhis_WebService_Core.Models.Common;
using eGhis_WebService_Core.Models.Dto.Auth;
using eGhis_WebService_Core.Service.Auth;
using i6tyone_Conference.Models.Dto.Register;
using i6tyone_Conference.Models.Dto.Auth;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace eGhis_WebService_Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : BaseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRegisterService _registerService;

        public RegisterController(IHttpContextAccessor httpContextAccessor, IRegisterService registerService)
        {
            _httpContextAccessor = httpContextAccessor;
            _registerService = registerService;
        }

        /// <summary>
        /// 컨퍼런스 참가 등록
        /// </summary>
        /// <returns>요청 정보<see cref="RegisterRequestDto"/></returns>
        /// <returns>응답 정보<see cref="RegisterAddResponseDto"/></returns>
        /// <returns>컨퍼런스 참가 신청 정보를 등록합니다.</returns>
        [HttpPost("sign-up")]
        [AllowAnonymousToken]
        [SwaggerResponse(200, typeof(GenericResponse<RegisterAddResponseDto>), Description = "정상 처리되었습니다.")]
        public async Task<ActionResult<GenericResponse<RegisterAddResponseDto>>> GenerateRegister(RegisterRequestDto req)
        {
            var res = await _registerService.GenerateRegisterAsync(req);
            return Ok(res);
        }

        /// <summary>
        /// 전체 컨퍼런스 등록자 QR Code 일괄발급
        /// </summary>
        /// <returns>응답 정보<see cref="QRCodeResponseDto"/></returns>
        /// <returns>등록자 목록 정보를 기반으로 각 참가자의 QR 코드를 생성하여 PNG 파일 형태로 일괄 생성 및 다운로드합니다.</returns>
        [HttpPost("qrcode")]
        [AllowAnonymousToken]
        [SwaggerResponse(200, typeof(GenericResponse<QRCodeResponseDto>), Description = "정상 처리되었습니다.")]
        public async Task<ActionResult<GenericResponse<QRCodeResponseDto>>> GenerateRegisterQRCode()
        {
            var res = await _registerService.GenerateRegisterQRCodeAsync();
            return Ok(res);
        }

        /// <summary>
        /// 컨퍼런스 특정 등록자 QR Code 발급/재발급
        /// </summary>
        /// <returns>응답 정보<see cref="QRCodeResponseDto"/></returns>
        /// <returns></returns>
        [HttpPost("qrcode-single")]
        [AllowAnonymousToken]
        [SwaggerResponse(200, typeof(GenericResponse<QRCodeResponseDto>), Description = "정상 처리되었습니다.")]
        public async Task<ActionResult<GenericResponse<QRCodeResponseDto>>> GenerateRegisterSingleQRCode(IssuanceRequestDto req)
        {
            var res = await _registerService.GenerateRegisterQRCodeSingleAsync(req);
            return Ok(res);
        }

        /// <summary>
        /// 컨퍼런스 등록자 조회
        /// </summary>
        /// <returns>응답 정보<see cref="RegisterResponseDto"/></returns>
        /// <returns>등록자의 정보를 조회합니다.</returns>
        [HttpPost("list")]
        [AllowAnonymousToken]
        [SwaggerResponse(200, typeof(GenericResponse<RegisterResponseDto>), Description = "정상 처리되었습니다.")]
        public async Task<ActionResult<GenericResponse<RegisterResponseDto>>> GetRegisterInfo(RegisterInfoRequestDto req)
        {
            var res = await _registerService.GetRegisterInfoAsync(req);
            return Ok(res);
        }

        /// <summary>
        /// 컨퍼런스 현장 입장 등록
        /// </summary>
        /// <param name="uniqueId">QR Code Key</param>
        /// <returns>응답 정보<see cref="CheckInResponseDto"/></returns>
        /// <returns>현장에서 QR Code를 스캔하면 자동으로 출석이 처리됩니다.</returns>
        [HttpPost("check-in")]
        [AllowAnonymousToken]
        [SwaggerResponse(200, typeof(GenericResponse<CheckInResponseDto>), Description = "정상 처리되었습니다.")]
        public async Task<ActionResult<GenericResponse<CheckInResponseDto>>> CheckAttendance(string uniqueId)
        {
            var res = await _registerService.CheckAttendanceAsync(uniqueId);
            return Ok(res);
        }
    }
}
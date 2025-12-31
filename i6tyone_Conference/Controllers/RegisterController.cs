using eGhis_WebService_Core.Controllers.Base;
using eGhis_WebService_Core.Infrastructure.Attributes;
using eGhis_WebService_Core.Models.Common;
using eGhis_WebService_Core.Models.Dto.Auth;
using eGhis_WebService_Core.Service.Auth;
using i6tyone_Conference.Models.Dto.Auth;
using i6tyone_Conference.Models.Dto.Register;
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
        [HttpPost("sign-up")]
        [AllowAnonymousToken]
        [SwaggerResponse(200, typeof(GenericResponse<RegisterAddResponseDto>), Description = "정상 처리되었습니다.")]
        public async Task<ActionResult<GenericResponse<RegisterAddResponseDto>>> GenerateRegister(RegisterRequestDto req)
        {
            var res = await _registerService.GenerateRegisterAsync(req);
            return Ok(res);
        }

        /// <summary>
        /// 컨퍼런스 등록 수정
        /// </summary>
        /// <returns>요청 정보<see cref="RegisterUpdateRequestDto"/></returns>
        /// <returns>응답 정보<see cref="SuccessResponseDto"/></returns>
        [HttpPost("update")]
        [AllowAnonymousToken]
        [SwaggerResponse(200, typeof(GenericResponse<SuccessResponseDto>), Description = "정상 처리되었습니다.")]
        public async Task<ActionResult<GenericResponse<SuccessResponseDto>>> UpdateRegister(RegisterUpdateRequestDto req)
        {
            var res = await _registerService.UpdateRegisterAsync(req);
            return Ok(res);
        }

        /// <summary>
        /// 컨퍼런스 등록 폐기
        /// </summary>
        /// <param name="uniqueIdKey">QR Code 발급 키</param>
        /// <returns>응답 정보<see cref="SuccessResponseDto"/></returns>
        [HttpPost("dispose")]
        [AllowAnonymousToken]
        [SwaggerResponse(200, typeof(GenericResponse<SuccessResponseDto>), Description = "정상 처리되었습니다.")]
        public async Task<ActionResult<GenericResponse<SuccessResponseDto>>> DisposeRegister(string uniqueIdKey)
        {
            var res = await _registerService.DisposeRegisterAsync(uniqueIdKey);
            return Ok(res);
        }

        /// <summary>
        /// 컨퍼런스 등록 삭제
        /// </summary>
        /// <param name="uniqueIdKey">QR Code 발급 키</param>
        /// <returns>응답 정보<see cref="SuccessResponseDto"/></returns>
        [HttpPost("delete")]
        [AllowAnonymousToken]
        [SwaggerResponse(200, typeof(GenericResponse<SuccessResponseDto>), Description = "정상 처리되었습니다.")]
        public async Task<ActionResult<GenericResponse<SuccessResponseDto>>> DeleteRegister(string uniqueIdKey)
        {
            var res = await _registerService.DeleteRegisterAsync(uniqueIdKey);
            return Ok(res);
        }

        /// <summary>
        /// 전체 등록자 QR 코드 일괄 발급
        /// </summary>
        /// <returns>응답 정보<see cref="QRCodeResponseDto"/></returns>
        [HttpPost("qrcode")]
        [AllowAnonymousToken]
        [SwaggerResponse(200, typeof(GenericResponse<QRCodeResponseDto>), Description = "정상 처리되었습니다.")]
        public async Task<ActionResult<GenericResponse<QRCodeResponseDto>>> GenerateRegisterQRCode()
        {
            var res = await _registerService.GenerateRegisterQRCodeAsync();
            return Ok(res);
        }

        /// <summary>
        /// 특정 등록자 QR 코드 발급/재발급
        /// </summary>
        /// <returns>응답 정보<see cref="QRCodeResponseDto"/></returns>
        [HttpPost("qrcode-single")]
        [AllowAnonymousToken]
        [SwaggerResponse(200, typeof(GenericResponse<QRCodeResponseDto>), Description = "정상 처리되었습니다.")]
        public async Task<ActionResult<GenericResponse<QRCodeResponseDto>>> GenerateRegisterSingleQRCode(IssuanceRequestDto req)
        {
            var res = await _registerService.GenerateRegisterQRCodeSingleAsync(req);
            return Ok(res);
        }

        /// <summary>
        /// 등록자 조회
        /// </summary>
        /// <returns>응답 정보<see cref="RegisterResponseDto"/></returns>
        [HttpPost("list")]
        [AllowAnonymousToken]
        [SwaggerResponse(200, typeof(GenericResponse<RegisterResponseDto>), Description = "정상 처리되었습니다.")]
        public async Task<ActionResult<GenericResponse<RegisterResponseDto>>> GetRegisterInfo(RegisterInfoRequestDto req)
        {
            var res = await _registerService.GetRegisterInfoAsync(req);
            return Ok(res);
        }

        /// <summary>
        /// 등록자 엑셀 다운로드
        /// </summary>
        [HttpGet("excel")]
        [AllowAnonymousToken]
        public async Task<ActionResult> GetRegisterExcel()
        {
            var res = await _registerService.GetRegisterExcelAsync();
            return File(res.Data.FileBytes, res.Data.ContentType, res.Data.FileName);
        }

        /// <summary>
        /// 등록자 상세조회
        /// </summary>
        /// <param name="uniqueIdKey">QR Code Key</param>
        /// <returns>응답 정보<see cref="RegisterInfoResponseDto"/></returns>
        [HttpPost("detail")]
        [AllowAnonymousToken]
        [SwaggerResponse(200, typeof(GenericResponse<RegisterInfoResponseDto>), Description = "정상 처리되었습니다.")]
        public async Task<ActionResult<GenericResponse<RegisterInfoResponseDto>>> GetRegisterDetailInfo(string uniqueIdKey)
        {
            var res = await _registerService.GetRegisterDetailInfoAsync(uniqueIdKey);
            return Ok(res);
        }

        /// <summary>
        /// 현장 입장 등록
        /// </summary>
        /// <param name="uniqueIdKey">QR Code Key</param>
        /// <returns>응답 정보<see cref="CheckInResponseDto"/></returns>
        [HttpPost("check-in")]
        [AllowAnonymousToken]
        [SwaggerResponse(200, typeof(GenericResponse<CheckInResponseDto>), Description = "정상 처리되었습니다.")]
        public async Task<ActionResult<GenericResponse<CheckInResponseDto>>> CheckAttendance(string uniqueIdKey)
        {
            var res = await _registerService.CheckAttendanceAsync(uniqueIdKey);
            return Ok(res);
        }

        /// <summary>
        /// FTP 정보 조회
        /// </summary>
        /// <returns>응답 정보<see cref="FtpInfoResponseDto"/></returns>
        [HttpGet("ftp-info")]
        [AllowAnonymousToken]
        [SwaggerResponse(200, typeof(GenericResponse<FtpInfoResponseDto>), Description = "정상 처리되었습니다.")]
        public async Task<ActionResult<GenericResponse<FtpInfoResponseDto>>> GetFtpInfo()
        {
            var res = await _registerService.GetFtpInfoAsync();
            return Ok(res);
        }
    }
}
using eGhis_WebService_Core.Controllers.Base;
using eGhis_WebService_Core.Infrastructure.Attributes;
using eGhis_WebService_Core.Models.Common;
using eGhis_WebService_Core.Service.Auth;
using i6tyone_Conference.Models.Dto.Auth;
using i6tyone_Conference.Models.Dto.OnSite;
using i6tyone_Conference.Models.Dto.Register;
using i6tyone_Conference.Service.OnSiteRegister;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace i6tyone_Conference.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OnSiteRegisterController : BaseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOnSiteRegisterService _onSiteRegisterService;

        public OnSiteRegisterController(IHttpContextAccessor httpContextAccessor, IOnSiteRegisterService onSiteRegisterService)
        {
            _httpContextAccessor = httpContextAccessor;
            _onSiteRegisterService = onSiteRegisterService;
        }

        /// <summary>
        /// 현장등록자 조회
        /// </summary>
        /// <returns>응답 정보<see cref="RegisterResponseDto"/></returns>
        [HttpPost("onsite-list")]
        [AllowAnonymousToken]
        [SwaggerResponse(200, typeof(GenericResponse<RegisterResponseDto>), Description = "정상 처리되었습니다.")]
        public async Task<ActionResult<GenericResponse<RegisterResponseDto>>> GetOnSiteRegisterInfo(RegisterInfoRequestDto req)
        {
            var res = await _onSiteRegisterService.GetOnSiteRegisterInfoAsync(req);
            return Ok(res);
        }

        /// <summary>
        /// 현장등록자 상세조회
        /// </summary>
        /// <param name="uniqueIdKey">QR Code Key</param>
        /// <returns>응답 정보<see cref="RegisterInfoResponseDto"/></returns>
        [HttpPost("onsite-detail")]
        [AllowAnonymousToken]
        [SwaggerResponse(200, typeof(GenericResponse<RegisterInfoResponseDto>), Description = "정상 처리되었습니다.")]
        public async Task<ActionResult<GenericResponse<RegisterInfoResponseDto>>> GetOnSiteRegisterDetailInfo(string uniqueIdKey)
        {
            var res = await _onSiteRegisterService.GetOnSiteRegisterDetailInfoAsync(uniqueIdKey);
            return Ok(res);
        }

        /// <summary>
        /// 현장등록자 결제완료 및 연동
        /// </summary>
        /// <param name="uniqueIdKey">QR Code 발급 키</param>
        /// <returns>응답 정보<see cref="PayResponseDto"/></returns>
        [HttpPost("payments/complete")]
        [AllowAnonymousToken]
        [SwaggerResponse(200, typeof(GenericResponse<PayResponseDto>), Description = "정상 처리되었습니다.")]
        public async Task<ActionResult<GenericResponse<PayResponseDto>>> CompletePayment(string uniqueIdKey)
        {
            var res = await _onSiteRegisterService.CompletePaymentAsync(uniqueIdKey);
            return Ok(res);
        }

        /// <summary>
        /// 현장등록자 컨퍼런스 등록 폐기
        /// </summary>
        /// <param name="uniqueIdKey">QR Code 발급 키</param>
        /// <returns>응답 정보<see cref="SuccessResponseDto"/></returns>
        [HttpPost("dispose")]
        [AllowAnonymousToken]
        [SwaggerResponse(200, typeof(GenericResponse<SuccessResponseDto>), Description = "정상 처리되었습니다.")]
        public async Task<ActionResult<GenericResponse<SuccessResponseDto>>> DisposeOnSiteRegister(string uniqueIdKey)
        {
            var res = await _onSiteRegisterService.DisposeOnSiteRegisterAsync(uniqueIdKey);
            return Ok(res);
        }
    }
}

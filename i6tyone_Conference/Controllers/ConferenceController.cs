using eGhis_WebService_Core.Controllers.Base;
using eGhis_WebService_Core.Service.Auth;
using Microsoft.AspNetCore.Mvc;

namespace i6tyone_Conference.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConferenceController : BaseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRegisterService _registerService;

        public ConferenceController(IRegisterService authService, IHttpContextAccessor httpContextAccessor)
        {
            _registerService = authService;
            _httpContextAccessor = httpContextAccessor;
        }
    }
}

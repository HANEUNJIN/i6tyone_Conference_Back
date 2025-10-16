using eGhis_WebService_Core.Controllers.Base;
using i6tyone_Conference.Service.Management;
using Microsoft.AspNetCore.Mvc;

namespace i6tyone_Conference.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagementController : BaseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IManagementService _managementService;

        public ManagementController(IManagementService managementService, IHttpContextAccessor httpContextAccessor)
        {
            _managementService = managementService;
            _httpContextAccessor = httpContextAccessor;
        }
    }
}

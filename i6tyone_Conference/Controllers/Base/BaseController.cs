using eGhis_WebService_Core.Models.Common;
using Microsoft.AspNetCore.Mvc;

namespace eGhis_WebService_Core.Controllers.Base
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected JwtPayloadModel CurrentUser => new JwtPayloadModel
        {
            id = User.FindFirst("Id")?.Value ?? "",
            name = User.FindFirst("Name")?.Value ?? "",
            phoneNumber = User.FindFirst("PhoneNumber")?.Value ?? "",
        };
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace eGhis_WebService_Core.Filter
{
    public class HttpMethodFilter : IAsyncActionFilter
    {
        private static readonly HashSet<string> AllowedMethods = new(StringComparer.OrdinalIgnoreCase)
        {
            "GET", "POST", "PUT", "PATCH", "DELETE"
        };

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var requestMethod = context.HttpContext.Request.Method;

            if (!AllowedMethods.Contains(requestMethod))
            {
                context.Result = new JsonResult(new
                {
                    resultCd = "403",
                    resultMsg = $"HTTP method '{requestMethod}' is not allowed."
                })
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };
                return;
            }

            await next();
        }
    }
}

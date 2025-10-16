//using FluentValidation; // 나중에 적용할 경우를 위한 참조
using eGhis_WebService_Core.Define;
using eGhis_WebService_Core.Exceptions;
using eGhis_WebService_Core.Infrastructure.Utils;
using eGhis_WebService_Core.Models.Common;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace eGhis_WebService_Core.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException vex) // 향후 FluentValidation 적용 시 유효성 오류 처리
            {
                _logger.LogWarning(vex, "Validation exception caught");

                // ⚠️ 나중에 적용 시 상세 오류 메시지 구성 예정
                await HandleExceptionAsync(
                    context,
                    EnumUtil.GetDisplayName(ErrorStatusCode.Invalid_Error),
                    EnumUtil.GetDescription(ErrorStatusCode.Invalid_Error),
                    StatusCodes.Status400BadRequest
                );
            }
            catch (BusinessException bex)
            {
                _logger.LogWarning(bex, "Business exception caught");

                await HandleExceptionAsync(
                    context,
                    bex.Code,
                    bex.Message,
                    StatusCodes.Status400BadRequest
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");

                await HandleExceptionAsync(
                    context,
                    "500",
                    "서버 내부 오류가 발생했습니다.",
                    StatusCodes.Status500InternalServerError
                );
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, string code, string message, int statusCode)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json; charset=utf-8";

            var response = new ResponseBaseModel
            {
                ResultCd = code,
                ResultMsg = message,
                Data = null
            };

            var json = JsonConvert.SerializeObject(response);
            await context.Response.WriteAsync(json);
        }
    }
}

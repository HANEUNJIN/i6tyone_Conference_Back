using eGhis_WebService_Core.Models.Common;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace eGhis_WebService_Core.Filter
{
    public sealed class FluentValidationFilter : IAsyncActionFilter
    {
        private readonly IServiceProvider _sp;
        public FluentValidationFilter(IServiceProvider sp) => _sp = sp;

        public async Task OnActionExecutionAsync(ActionExecutingContext ctx, ActionExecutionDelegate next)
        {
            if (!ctx.ModelState.IsValid)
            {
                var bindingErrors = ctx.ModelState
                    .Where(kv => kv.Value?.Errors.Count > 0)
                    .Select(kv => new {
                        Field = kv.Key,
                        Errors = kv.Value!.Errors.Select(e => e.ErrorMessage)
                    });

                ctx.Result = new BadRequestObjectResult(
                    ResponseBaseModel.Fail("400", "유효성 검사 실패", bindingErrors));
                return;
            }

            var failures = new List<ValidationFailure>();

            foreach (var arg in ctx.ActionArguments.Values.Where(v => v is not null))
            {
                var argType = arg!.GetType();
                var validatorType = typeof(IValidator<>).MakeGenericType(argType);

                if (_sp.GetService(validatorType) is not IValidator validator)
                {
                    continue;
                }
                    
                var vctx = new ValidationContext<object>(arg);
                var result = await validator.ValidateAsync(vctx, ctx.HttpContext.RequestAborted);
                if (!result.IsValid) failures.AddRange(result.Errors);
            }

            if (failures.Count > 0)
            {
                var body = ResponseBaseModel.Fail("400", "유효성 검사 실패",
                    failures.GroupBy(f => f.PropertyName)
                            .Select(g => new { Field = g.Key, Errors = g.Select(e => e.ErrorMessage) }));

                ctx.Result = new BadRequestObjectResult(body);
                return;
            }

            await next();
        }
    }
}

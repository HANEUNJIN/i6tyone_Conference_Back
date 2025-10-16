using eGhis_WebService_Core.Models.Common;
using NSwag;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;
using NJsonSchema;

namespace eGhis_WebService_Core.Filter
{
    /// <summary>
    /// 모든 API 엔드포인트에 공통 응답 스펙(200, 400, 401, 403, 500)을 자동 추가합니다.
    /// 컨트롤러에 SwaggerResponse 어노테이션이 없어도 기본 응답 스펙이 문서에 반영됩니다.
    /// </summary>
    public class NSwagResponseFilter : IOperationProcessor
    {
        public bool Process(OperationProcessorContext context)
        {
            AddStandardResponse(context, "400", "요청 값이 잘못되었거나 누락되었습니다.");
            AddStandardResponse(context, "401", "엑세스 토큰이 유효하지 않습니다.");
            AddStandardResponse(context, "403", "접근 권한이 없습니다.");
            AddStandardResponse(context, "500", "내부 서버 오류가 발생했습니다.");

            return true;
        }

        private void AddStandardResponse(OperationProcessorContext context, string statusCode, string description)
        {
            if (!context.OperationDescription.Operation.Responses.ContainsKey(statusCode))
            {
                var schema = context.SchemaGenerator.Generate(
                    typeof(ResponseBaseModel),
                    context.SchemaResolver
                );

                var response = new OpenApiResponse
                {
                    Description = description,
                    Content =
                    {
                        ["application/json"] = new OpenApiMediaType { Schema = schema },
                        ["application/xml"] = new OpenApiMediaType { Schema = schema }
                    }
                };

                context.OperationDescription.Operation.Responses.Add(statusCode, response);
            }
        }
    }
}

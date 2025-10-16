using NJsonSchema;
using NSwag;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace eGhis_WebService_Core.Swagger
{
    public class FileUploadOperationProcessor : IOperationProcessor
    {
        public bool Process(OperationProcessorContext context)
        {
            var fileParam = context.MethodInfo.GetParameters()
                .FirstOrDefault(p => p.ParameterType == typeof(IFormFile));

            if (fileParam == null)
                return true;

            context.OperationDescription.Operation.RequestBody = new OpenApiRequestBody
            {
                IsRequired = true,
                Content =
                {
                    ["multipart/form-data"] = new OpenApiMediaType
                    {
                        Schema = new JsonSchema
                        {
                            Type = JsonObjectType.Object,
                            Properties =
                            {
                                [fileParam.Name] = new JsonSchemaProperty
                                {
                                    Type = JsonObjectType.String,
                                    Format = JsonFormatStrings.Binary,
                                    Description = "업로드할 파일"
                                }
                            },
                            RequiredProperties = { fileParam.Name }
                        }
                    }
                }
            };

            return true;
        }
    }
}

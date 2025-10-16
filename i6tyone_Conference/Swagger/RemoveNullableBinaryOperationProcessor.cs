using NJsonSchema;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace eGhis_WebService_Core.Swagger
{
    public class RemoveNullableBinaryOperationProcessor : IOperationProcessor
    {
        public bool Process(OperationProcessorContext context)
        {
            var parameters = context.OperationDescription.Operation.RequestBody?.Content;

            if (parameters != null && parameters.ContainsKey("multipart/form-data"))
            {
                var schema = parameters["multipart/form-data"].Schema;

                if (schema.Type == JsonObjectType.Object && schema.Properties != null)
                    this.RemoveBinaryNullability(schema);
            }

            return true;
        }

        private void RemoveBinaryNullability(JsonSchema schema)
        {
            if (schema != null)
            {
                if (schema.Type == JsonObjectType.String && schema.Format == "binary")
                    schema.IsNullableRaw = false;

                if (schema.Type == JsonObjectType.Object && schema.Properties != null)
                {
                    foreach (var prop in schema.Properties)
                        RemoveBinaryNullability(prop.Value);
                }

                if (schema.Type == JsonObjectType.Array && schema.Item != null)
                    RemoveBinaryNullability(schema.Item);

                if (schema.AllOf != null)
                {
                    foreach (var s in schema.AllOf)
                        RemoveBinaryNullability(s);
                }
            }
        }
    }
}

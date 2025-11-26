using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Any;

// IOperationFilter tells Swagger to modify the documentation (OpenAPI Specification)
public class AddDatabaseHeaderParameter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Ensure the parameters collection exists
        if (operation.Parameters == null)
        {
            operation.Parameters = new List<OpenApiParameter>();
        }

        // Define the custom header parameter for database selection
        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "X-Database-Type",
            In = ParameterLocation.Header,
            Description = "Optional: Specifies the target database (mysql, mongodb, neo4j). Defaults to MySQL if omitted.",
            Required = false, 
            Schema = new OpenApiSchema
            {
                Type = "string",
                // Provide the allowed values for easy selection in Swagger UI
                Enum = new List<IOpenApiAny>
                {
                    new OpenApiString("mysql"),
                    new OpenApiString("mongodb"),
                    new OpenApiString("neo4j")
                }
            }
        });
    }
}
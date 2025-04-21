using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

public class AddFileUploadOperation : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var hasFileParam = context.MethodInfo.GetParameters()
            .Any(p => p.ParameterType == typeof(IFormFile));

        if (!hasFileParam)
            return;

        operation.RequestBody = new OpenApiRequestBody
        {
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["multipart/form-data"] = new OpenApiMediaType
                {
                    Schema = new OpenApiSchema
                    {
                        Type = "object",
                        Properties =
                        {
                            ["file"] = new OpenApiSchema
                            {
                                Type = "string",
                                Format = "binary",
                                Description = "File upload"
                            }
                        }
                    }
                }
            }
        };

        // Thêm các parameter khác (nếu có)
        if (operation.Parameters.Any())
        {
            foreach (var param in operation.Parameters)
            {
                operation.RequestBody.Content["multipart/form-data"].Schema.Properties.Add(
                    param.Name,
                    new OpenApiSchema
                    {
                        Type = param.Schema?.Type,
                        Description = param.Description
                    });
            }

            operation.Parameters.Clear(); // Xóa parameters khỏi URL query nếu đã đưa vào body
        }

        // Thêm Authorization vào Swagger UI
        operation.Security = new List<OpenApiSecurityRequirement>
        {
            new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            }
        };

        // Thêm parameter nhập token vào Swagger UI
        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "Authorization",
            In = ParameterLocation.Header,
            Required = false,
            Description = "Nhập token dưới dạng 'Bearer {token}'",
            Schema = new OpenApiSchema
            {
                Type = "string"
            }
        });
    }
}

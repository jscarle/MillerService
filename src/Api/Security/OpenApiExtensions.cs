using Microsoft.OpenApi.Models;

namespace MillerDemo.Api.Security;

public static class OpenApiExtensions
{
    public static TBuilder WithSecurityRequirement<TBuilder>(this TBuilder builder)
        where TBuilder : IEndpointConventionBuilder
    {
        return builder.WithOpenApi(operation =>
        {
            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Azure"
                            }
                        },
                        Array.Empty<string>()
                    }
                }
            };
            return operation;
        });
    }
}
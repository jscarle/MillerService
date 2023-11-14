using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using MillerDemo.Api.Security;

namespace MillerDemo.Api;

/// <summary>
/// Contains extensions methods for configuring API services.
/// </summary>
public static class ConfigureServices
{
    /// <summary>
    /// Adds API services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors();
        services.AddAuthentication(configuration);
        services.AddOpenApi(configuration);

        return services;
    }

    private static void AddCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(
                policy =>
                {
                    policy.WithOrigins("http://localhost:3000",
                            "https://agreeable-smoke-0c65d9210.4.azurestaticapps.net")
                        .AllowCredentials()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });
    }

    private static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(configuration.GetSection("AzureAdB2C"));
        services.AddAuthorization();
        services.AddSingleton<IAuthorizationService, AuthorizationService>();
    }

    private static void AddOpenApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.AddServer(new OpenApiServer
            {
                Url = configuration.GetValue<string>("API_URL") ??
                      throw new InvalidOperationException("Could not get value for API_URL.")
            });
            options.AddSecurityDefinition("Azure",
                new OpenApiSecurityScheme
                {
                    Description = "Azure AD B2C Access Token",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
        });
    }
}
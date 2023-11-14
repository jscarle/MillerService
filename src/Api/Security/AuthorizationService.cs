using Microsoft.Identity.Web.Resource;

namespace MillerDemo.Api.Security;

/// <summary>
/// Represents an authorization service.
/// </summary>
/// <param name="configuration">The configuration.</param>
/// <param name="httpContextAccessor">The HTTP context accessor.</param>
public sealed class AuthorizationService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : IAuthorizationService
{
    private readonly string _requiredScope = configuration["AzureAdB2C:Scopes"] ?? "";

    /// <inheritdoc/>
    /// <exception cref="InvalidOperationException">Thrown when the HTTP context cannot be retrieved.</exception>
    public bool IsAuthorized()
    {
        var httpContext = httpContextAccessor.HttpContext ?? throw new InvalidOperationException("Could not get HttpContext.");

        try
        {
            httpContext.VerifyUserHasAnyAcceptedScope(_requiredScope);
        }
        catch
        {
            return false;
        }

        return true;
    }
}
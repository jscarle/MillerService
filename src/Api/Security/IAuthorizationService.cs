namespace MillerDemo.Api.Security;

/// <summary>
/// Defines an authorization service.
/// </summary>
public interface IAuthorizationService
{
    /// <summary>
    /// Verifies if the current user is authorized.
    /// </summary>
    /// <returns>Returns <see langword="true"/> when the user is authorized, <see langword="false"/> otherwise.</returns>
    bool IsAuthorized();
}
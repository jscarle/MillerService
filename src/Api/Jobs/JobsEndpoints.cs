using MillerDemo.Api.Jobs.Dns;

namespace MillerDemo.Api.Jobs;

/// <summary>
/// Groups the Jobs endpoints.
/// </summary>
public static class JobsEndpoints
{
    /// <summary>
    /// Maps the Jobs endpoints group.
    /// </summary>
    /// <param name="app">The web application.</param>
    public static void MapJobsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/jobs");

        group.MapGetJobsEndpoint();
        group.MapDnsEndpoints();
    }
}
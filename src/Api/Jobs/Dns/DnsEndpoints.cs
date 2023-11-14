namespace MillerDemo.Api.Jobs.Dns;

/// <summary>
/// Groups the Dns endpoints.
/// </summary>
public static class DnsEndpoints
{
    /// <summary>
    /// Maps the Dns endpoints group.
    /// </summary>
    /// <param name="parent">The parent endpoint group.</param>
    public static void MapDnsEndpoints(this RouteGroupBuilder parent)
    {
        var group = parent.MapGroup("dns");

        group.MapGetDnsRecordsEndpoint();
        group.MapCreateDnsRecordEndpoint();
        group.MapUpdateDnsRecordEndpoint();
        group.MapDeleteDnsRecordEndpoint();
    }
}
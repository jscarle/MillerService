using System.Collections.Immutable;
using MediatR;
using MillerDemo.Api.Security;
using MillerDemo.Api.Validation;
using MillerDemo.Application.Jobs.Dns;

namespace MillerDemo.Api.Jobs.Dns;

/// <summary>
/// Contains the GetDnsRecords endpoint.
/// </summary>
public static class GetDnsRecordsEndpoint
{
    /// <summary>
    /// Maps the GetDnsRecords endpoint.
    /// </summary>
    /// <param name="group">The endpoint group.</param>
    public static void MapGetDnsRecordsEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("", Handler)
            .WithName("GetDnsRecords")
            .WithTags("Jobs/Dns")
            .WithSecurityRequirement()
            .RequireAuthorization()
            .Produces<ImmutableList<GetDnsRecordsResponseItem>>()
            .ProducesProblem(500);
    }

    private static async Task<IResult> Handler(IMediator mediator, CancellationToken cancellationToken)
    {
        var getDnsRecordsQuery = new GetDnsRecordsQuery();

        var result = await mediator.Send(getDnsRecordsQuery, cancellationToken);
        if (result.IsFailed)
            return result.ProblemDetails();

        var getDnsRecordsResponse = result.Value.Select(x => new GetDnsRecordsResponseItem(x.Name, x.IpAddress))
            .ToImmutableList();

        return TypedResults.Ok(getDnsRecordsResponse);
    }
}

/// <summary>
/// Represents a GetDnsRecords response item.
/// </summary>
/// <param name="Name">The DNS name.</param>
/// <param name="IpAddress">The IP address.</param>
public record GetDnsRecordsResponseItem(string Name, string IpAddress);
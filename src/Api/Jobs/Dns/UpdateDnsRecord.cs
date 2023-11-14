using MediatR;
using Microsoft.AspNetCore.Mvc;
using MillerDemo.Api.Security;
using MillerDemo.Api.Validation;
using MillerDemo.Application.Jobs.Dns;

namespace MillerDemo.Api.Jobs.Dns;

/// <summary>
/// Represents a UpdateDnsRecord request body.
/// </summary>
/// <param name="IpAddress">The IP address.</param>
public record UpdateDnsRecordRequestBody(string IpAddress);

/// <summary>
/// Contains the UpdateDnsRecord endpoint.
/// </summary>
public static class UpdateDnsRecordEndpoint
{
    /// <summary>
    /// Maps the UpdateDnsRecord endpoint.
    /// </summary>
    /// <param name="group">The endpoint group.</param>
    public static void MapUpdateDnsRecordEndpoint(this RouteGroupBuilder group)
    {
        group.MapPut("{name}", Handler)
            .WithName("UpdateDnsRecord")
            .WithTags("Jobs/Dns")
            .WithSecurityRequirement()
            .RequireAuthorization()
            .Produces(200)
            .ProducesProblem(500)
            .ProducesValidationProblem();
    }

    private static async Task<IResult> Handler([FromRoute] string name, [FromBody] UpdateDnsRecordRequestBody body, IMediator mediator, CancellationToken cancellationToken)
    {
        var updateDnsRecordsCommand = new UpdateDnsRecordCommand(name, body.IpAddress);

        var result = await mediator.Send(updateDnsRecordsCommand, cancellationToken);
        if (result.IsFailed)
            return result.ProblemDetails();

        return TypedResults.Ok();
    }
}
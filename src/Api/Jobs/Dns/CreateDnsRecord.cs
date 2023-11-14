using MediatR;
using Microsoft.AspNetCore.Mvc;
using MillerDemo.Api.Security;
using MillerDemo.Api.Validation;
using MillerDemo.Application.Jobs.Dns;

namespace MillerDemo.Api.Jobs.Dns;

/// <summary>
/// Represents a CreateDnsRecord request body.
/// </summary>
/// <param name="Name">The DNS name.</param>
/// <param name="IpAddress">The IP address.</param>
public record CreateDnsRecordRequestBody(string Name, string IpAddress);

/// <summary>
/// Contains the CreateDnsRecord endpoint.
/// </summary>
public static class CreateDnsRecordEndpoint
{
    /// <summary>
    /// Maps the CreateDnsRecord endpoint.
    /// </summary>
    /// <param name="group">The endpoint group.</param>
    public static void MapCreateDnsRecordEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("", Handler)
            .WithName("CreateDnsRecord")
            .WithTags("Jobs/Dns")
            .WithSecurityRequirement()
            .RequireAuthorization()
            .Produces(200)
            .ProducesProblem(500)
            .ProducesValidationProblem();
    }

    private static async Task<IResult> Handler([FromBody] CreateDnsRecordRequestBody body, IMediator mediator, CancellationToken cancellationToken)
    {
        var createDnsRecordsCommand = new CreateDnsRecordCommand(body.Name, body.IpAddress);

        var result = await mediator.Send(createDnsRecordsCommand, cancellationToken);
        if (result.IsFailed)
            return result.ProblemDetails();

        return TypedResults.Ok();
    }
}
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MillerDemo.Api.Security;
using MillerDemo.Api.Validation;
using MillerDemo.Application.Jobs.Dns;

namespace MillerDemo.Api.Jobs.Dns;

/// <summary>
/// Contains the DeleteDnsRecord endpoint.
/// </summary>
public static class DeleteDnsRecordEndpoint
{
    /// <summary>
    /// Maps the DeleteDnsRecord endpoint.
    /// </summary>
    /// <param name="group">The endpoint group.</param>
    public static void MapDeleteDnsRecordEndpoint(this RouteGroupBuilder group)
    {
        group.MapDelete("{name}", Handler)
            .WithName("DeleteDnsRecord")
            .WithTags("Jobs/Dns")
            .WithSecurityRequirement()
            .RequireAuthorization()
            .Produces(200)
            .ProducesProblem(500)
            .ProducesValidationProblem();
    }

    private static async Task<IResult> Handler([FromRoute] string name, IMediator mediator, CancellationToken cancellationToken)
    {
        var deleteDnsRecordsCommand = new DeleteDnsRecordCommand(name);

        var result = await mediator.Send(deleteDnsRecordsCommand, cancellationToken);
        if (result.IsFailed)
            return result.ProblemDetails();

        return TypedResults.Ok();
    }
}
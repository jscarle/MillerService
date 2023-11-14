using System.Collections.Immutable;
using MediatR;
using MillerDemo.Api.Security;
using MillerDemo.Api.Validation;
using MillerDemo.Application.Jobs;

namespace MillerDemo.Api.Jobs;

/// <summary>
/// Contains the GetJobs endpoint.
/// </summary>
public static class GetJobsEndpoint
{
    /// <summary>
    /// Maps the GetJobs endpoint.
    /// </summary>
    /// <param name="group">The endpoint group.</param>
    public static void MapGetJobsEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("", Handler)
            .WithName("GetJobs")
            .WithTags("Jobs")
            .WithSecurityRequirement()
            .RequireAuthorization()
            .Produces<ImmutableList<GetJobsResponseItem>>()
            .ProducesProblem(500);
    }

    private static async Task<IResult> Handler(IMediator mediator, CancellationToken cancellationToken)
    {
        var getJobsQuery = new GetJobsQuery();

        var result = await mediator.Send(getJobsQuery, cancellationToken);
        if (result.IsFailed)
            return result.ProblemDetails();

        var getJobsResponse = result.Value.Select(x => new GetJobsResultItem(x.Id, x.Description, x.State))
            .ToImmutableList();

        return TypedResults.Ok(getJobsResponse);
    }
}

/// <summary>
/// Represents a GetJobs response item.
/// </summary>
/// <param name="Id">The job identifier.</param>
/// <param name="Description">The job description.</param>
/// <param name="State">The job state.</param>
public record GetJobsResponseItem(Guid Id, string Description, string State);
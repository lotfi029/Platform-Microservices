using MediatR;
using PlatformService.DataServices;
using PlatformService.Features.Platforms.Commands;
using PlatformService.Features.Platforms.Queries;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Endpoints;

public class PlatformEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/platforms")
            .WithTags("Platforms");

        group.MapPost("", CreatePlatform)
            .WithName("CreatePlatform")
            .Produces<PlatformResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi();

        group.MapGet("", GetPlatfroms)
            .WithName("GetAllPlatforms")
            .Produces<IEnumerable<PlatformResponse>>(StatusCodes.Status200OK)
            .WithOpenApi();

        group.MapGet("{id:guid}", GetPlatformById)
            .WithName("GetPlatformById")
            .Produces<PlatformResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi();

    }

    private async Task<IResult> CreatePlatform(
        [FromServices] ISender _sender,
        [FromBody] CreatePlatformRequest request,
        [FromServices] IValidator<CreatePlatformRequest> validator,
        [FromServices] ICommandDataClient _commandDataClient,
        [FromServices] IMessageBusClient _messageBusClient,
        CancellationToken ct = default
        )
    {
        var validationResult = await validator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
        {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }
        var command = new CreatePlatformCommand(request);
        var result = await _sender.Send(command, ct);

        if (result.IsSuccess)
        {
            await _commandDataClient.SendPlatformToCommand(result.Value);
            var platformPublish = new PlatformPublishDto(
                result.Value.Id,
                result.Value.Name,
                "Platform_Published"
            );
            await _messageBusClient.PublishNewPlatformAsync(platformPublish, ct);
            return TypedResults.CreatedAtRoute("GetPlatformById", new {id = result.Value.Id});
        }

        return TypedResults.BadRequest(result.Error);
    }
    private async Task<IResult> GetPlatfroms(
        [FromServices] ISender _sender,
        CancellationToken ct = default
        )
    {
        var query = new GetAllPlatformQuery();
        var result = await _sender.Send(query, ct);

        return TypedResults.Ok(result.Value);
    }

    private async Task<IResult> GetPlatformById(
        [FromServices] ISender _sender,
        [FromRoute] Guid id,
        CancellationToken ct = default
        )
    {
        var query = new GetPlatformByIdQuery(id);
        var result = await _sender.Send(query, ct);
        return result.IsSuccess 
            ? TypedResults.Ok(result.Value) 
            : TypedResults.NotFound();
    }
}

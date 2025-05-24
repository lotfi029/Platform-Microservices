
namespace PlatformService.Features.Platforms.Commands;

public record CreatePlatformCommand(CreatePlatformRequest Request) : ICommand<PlatformResponse>;

public class CreatePlatformCommandHandler(IPlatformRepo platformRepo) : ICommandHandler<CreatePlatformCommand, PlatformResponse>
{
    public async Task<Result<PlatformResponse>> Handle(CreatePlatformCommand request, CancellationToken cancellationToken)
    {
        var platform = request.Request.Adapt<Platform>();

        var result = await platformRepo.CreatePlatformAsync(platform, cancellationToken);

        return result;
    }
}

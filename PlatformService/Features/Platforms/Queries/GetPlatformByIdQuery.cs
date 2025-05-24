namespace PlatformService.Features.Platforms.Queries;

public record GetPlatformByIdQuery (Guid Id) : IQuery<PlatformResponse>;

public class GetPlatformByIdQueryHandler(IPlatformRepo _platformRepo) : IQueryHandler<GetPlatformByIdQuery, PlatformResponse>
{
    public async Task<Result<PlatformResponse>> Handle(GetPlatformByIdQuery request, CancellationToken cancellationToken)
    {
        var platform = await _platformRepo.GetPlatformByIdAsync(request.Id, cancellationToken);

        if (platform.IsSuccess)
            return platform.Value.Adapt<PlatformResponse>();

        return platform.Error;
    }
}

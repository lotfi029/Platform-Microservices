
namespace PlatformService.Features.Platforms.Queries;

public record GetAllPlatformQuery : IQuery<IEnumerable<PlatformResponse>>;

public class GetAllPlatformQueryHandler(IPlatformRepo _platformRepo) : IQueryHandler<GetAllPlatformQuery, IEnumerable<PlatformResponse>>
{
    public async Task<Result<IEnumerable<PlatformResponse>>> Handle(GetAllPlatformQuery request, CancellationToken cancellationToken)
    {
        var platforms = await _platformRepo.GetAllPlatfromAsync(cancellationToken);

        return Result.Success(platforms.Adapt<IEnumerable<PlatformResponse>>());
    }
}
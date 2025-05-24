namespace PlatformService.Persistence.Repositories;

public interface IPlatformRepo
{
    Task<Result<PlatformResponse>> CreatePlatformAsync(Platform request, CancellationToken ct = default);
    Task<IEnumerable<Platform>> GetAllPlatfromAsync(CancellationToken ct = default);
    Task<Result<Platform>> GetPlatformByIdAsync(Guid id, CancellationToken ct = default);
}

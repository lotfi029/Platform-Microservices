namespace PlatformService.Persistence.Repositories;

public class PlatformRepo(ApplicationDbContext _context) : IPlatformRepo
{
    public async Task<Result<PlatformResponse>> CreatePlatformAsync(Platform request, CancellationToken ct = default)
    {
        
        await _context.Platforms.AddAsync(request, ct);
        await _context.SaveChangesAsync(ct);

        return request.Adapt<PlatformResponse>();
    }

    public async Task<IEnumerable<Platform>> GetAllPlatfromAsync(CancellationToken ct = default)
    {
        var platforms = await _context.Platforms
            .AsNoTracking()
            .ToListAsync(ct);

        return platforms ?? [];
    }

    public async Task<Result<Platform>> GetPlatformByIdAsync(Guid id, CancellationToken ct = default)
    {
        if (await _context.Platforms.FindAsync([id], ct) is not { } platform)
            return Error.NotFound("Platform.NotFound", "this platfrom with this id not exist");
        
        return platform;
    }
}

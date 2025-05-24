using PlatformService.Abstractions;

namespace CommandsService.Persistence.Repositories;

public class CommandRespository(ApplicationDbContext context) : ICommandRespository
{

    public async Task<IEnumerable<Platform>> GetAllPlatformAsync()
    {
        return await context.Platforms
            .Include(p => p.Commands)
            .ToListAsync();
    }

    public async Task<Result<Guid>> CreatePlatformAsync(Platform platform)
    {
        context.Platforms.Add(platform);
        await context.SaveChangesAsync();
        return platform.Id;
    }

    public async Task<Result> PlatformExist(Guid platformId)
    {
        var platform = await context.Platforms
            .FirstOrDefaultAsync(p => p.Id == platformId);
        if (platform is null)
        {
            return Error.NotFound("Platform.NotFound", "this platform not exist.");
        }
        return Result.Success();
    }


    public async Task<IEnumerable<Command>> GetAllCommandsByPlatfromIdAsync(Guid platformId)
    {
        return await context.Commands
            .Where(c => c.PlatformId == platformId)
            .ToListAsync();
    }
    public async Task<Result<Command>> GetCommand(Guid platformId, Guid commandId)
    {
        if (await context.Commands.SingleOrDefaultAsync(c => c.PlatformId == platformId && c.Id == commandId) is not { } command)
            return Error.NotFound("Command.NotFound", "Command is not found");

        return command;
    }
    public async Task<Result<Guid>> CreateCommand(Guid platformId, Command command)
    {
        command.PlatformId = platformId;
        context.Commands.Add(command);
        await context.SaveChangesAsync();

        return command.Id;
    }
    public async Task<Result> ExternalPlatformExist(Guid platformId)
    {
        if(! await context.Platforms.AnyAsync(e => e.ExternalId == platformId))
            return Error.NotFound("ExternalPlatform.NotFound", "External platform is not found");

        return Result.Success();
    }
}
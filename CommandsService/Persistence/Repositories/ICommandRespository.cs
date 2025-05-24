using PlatformService.Abstractions;

namespace CommandsService.Persistence.Repositories;
public interface ICommandRespository
{
    Task<Result<Guid>> CreateCommand(Guid platformId, Command command);
    Task<Result<Guid>> CreatePlatformAsync(Platform platform);
    Task<IEnumerable<Command>> GetAllCommandsByPlatfromIdAsync(Guid platformId);
    Task<IEnumerable<Platform>> GetAllPlatformAsync();
    Task<Result<Command>> GetCommand(Guid platformId, Guid commandId);
    Task<Result> PlatformExist(Guid platformId);
    Task<Result> ExternalPlatformExist(Guid platformId);
}
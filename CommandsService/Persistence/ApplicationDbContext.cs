using CommandsService.Models;

namespace CommandsService.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Platform> Platforms { get; set; }
    public DbSet<Command> Commands { get; set; }

}

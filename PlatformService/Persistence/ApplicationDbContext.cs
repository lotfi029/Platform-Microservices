namespace PlatformService.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Platform> Platforms { get; set; }

}

using FluentValidation.AspNetCore;
using MapsterMapper;
using PlatformService.DataServices;
using PlatformService.HostedServices;
using PlatformService.Proflies;
using PlatformService.SyncDataServices.Http;

namespace PlatformService;

public static class DependancyInjection
{
    public static IServiceCollection AddPlatformServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
    {
        services.AddEndpointsApiExplorer();

        if (webHostEnvironment.IsDevelopment())
        {
            Console.WriteLine("--> Using InMemory DB");
            services.AddDbContext<ApplicationDbContext>(opt =>
                opt.UseInMemoryDatabase("inMem")
            
            );
        }
        else
        {
            Console.WriteLine("--> Using SQL Server DB");
            services.AddDbContext<ApplicationDbContext>(opt =>
                opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
            );
            Console.WriteLine("--> Attempting to apply migratino to Sql Server DB.... ");
            services.AddHostedService<MigrationService>();
        }   


        services.RegisterServices(configuration);

        var commandService = configuration.GetValue<string>("ApiSettings:CommandService");

        Console.WriteLine($"--> CommandService Endpoint: {commandService}");


        return services;
    }
    private static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddFluentValidationAutoValidation()
            .AddValidatorsFromAssembly(typeof(CreatePlatformRequestValidator).Assembly);

        services.AddScoped<IPlatformRepo, PlatformRepo>();

        services.AddOptions<ApiSettings>()
            .Bind(configuration.GetSection("ApiSettings"))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<RabbitMQSettings>()
            .Bind(configuration.GetSection(nameof(RabbitMQSettings)))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
        services.AddSingleton<IMessageBusClient, MessageBusClient>();

        var mappingConfig = TypeAdapterConfig.GlobalSettings;
        mappingConfig.Scan(typeof(MappingConfiguration).Assembly);
        services.AddSingleton<IMapper>(new Mapper(mappingConfig));

        services.AddCarter();

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(DependancyInjection).Assembly);
        });


        return services;
    }
}

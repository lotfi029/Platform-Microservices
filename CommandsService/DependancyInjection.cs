using CommandsService.DataService;
using CommandsService.IEventProcessing;
using CommandsService.Proflies;
using FluentValidation.AspNetCore;
using MapsterMapper;

namespace CommandsService;
public static class DependancyInjection
{
    public static IServiceCollection AddCommandsServices(this IServiceCollection services, IConfiguration configuration) 
    {
        services.AddEndpointsApiExplorer();
        services.AddControllers();

        services.AddDbContext<ApplicationDbContext>(opt =>
            opt.UseInMemoryDatabase("inMem")
        );

        services.RegisterServices(configuration);

        return services;
    }
    private static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddFluentValidationAutoValidation()
            .AddValidatorsFromAssembly(typeof(CreateCommandRequestValidator).Assembly);

        var mappingConfig = TypeAdapterConfig.GlobalSettings;
        mappingConfig.Scan(typeof(MappingConfiguration).Assembly);
        services.AddSingleton<IMapper>(new Mapper(mappingConfig));

        services.AddScoped<ICommandRespository, CommandRespository>();
        services.AddSingleton<IEventProcessor, EventProcessor>();
        services.AddHostedService<MessageBusSubscriber>();

        services.AddOptions<RabbitMQSettings>()
            .Bind(configuration.GetSection(nameof(RabbitMQSettings)))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services;
    }
}

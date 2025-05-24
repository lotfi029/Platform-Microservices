using CommandsService.Contracts.Platforms;

namespace CommandsService.Proflies;

public class MappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<PlatformPublishDto, Platform>()
            .Map(dest => dest.ExternalId, src => src.Id);
    }
}

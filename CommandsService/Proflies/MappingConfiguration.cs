using CommandsService.Contracts.Platforms;
using PlatformService;

namespace CommandsService.Proflies;

public class MappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<PlatformPublishDto, Platform>()
            .Map(dest => dest.ExternalId, src => src.Id);

        config.NewConfig<GrpcPlatformModel, Platform>()
            .Map(dest => dest.ExternalId, src => Guid.Parse(src.Id))
            .Map(dest => dest.Name, src => src.Name)
            .Ignore(dest => dest.Commands);
    }
}

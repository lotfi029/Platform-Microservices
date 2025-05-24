namespace PlatformService.Proflies;

public class MappingConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        //config.NewConfig<CreatePlatformRequest, >()
        //    .Map(dest => dest.Name, src => src.Name)
        //    .Map(dest => dest.Publisher, src => src.Publisher)
        //    .Map(dest => dest.LicenseUrl, src => src.LicenseUrl)
        //    .Map(dest => dest.Platforms, src => src.Platforms);
    }
}

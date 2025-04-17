using AutoMapper;
using PANDA.Api.Mapping;

namespace PANDA.Api.Tests.TestUtilities;

public static class MapperFactory
{
    public static IMapper Create()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>(); // Your actual AutoMapper profile
        });
        return config.CreateMapper();
    }
}
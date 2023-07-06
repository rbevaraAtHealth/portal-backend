using AutoMapper;
using CodeMatcherV2Api.Dtos;
using CodeMatcherV2Api.Models;

namespace CodeMatcherV2Api
{
    public class MapperConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mapperConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<UserModel, UserDto>().ReverseMap();
                //config.CreateMap<LookUpModel, LookUpDto>().ReverseMap();
            });
            return mapperConfig;
        }
    }
}

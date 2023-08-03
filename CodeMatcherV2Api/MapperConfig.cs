using AutoMapper;
using CodeMappingEfCore.DatabaseModels;
using CodeMatcher.Api.V2.Models.SummaryModel;
using CodeMatcher.EntityFrameworkCore.DatabaseModels.SummaryTables;
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
                config.CreateMap<LookupModel, LookupDto>().ReverseMap();
                config.CreateMap<LookupTypeDto, LookupTypeModel>().ReverseMap();
                config.CreateMap<CodeGenerationSummaryModel, CodeGenerationSummaryDto>().ReverseMap();
                config.CreateMap<MonthlyEmbeddingsSummaryDto, MonthlyEmbedSummaryModel>().ReverseMap();
                config.CreateMap<WeeklyEmbeddingsSummaryDto, WeeklyEmbedSummaryModel>().ReverseMap();
            });
            return mapperConfig;
        }
    }
}

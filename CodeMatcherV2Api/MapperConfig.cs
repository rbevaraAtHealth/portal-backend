using AutoMapper;
using CodeMappingEfCore.DatabaseModels;
using CodeMatcher.Api.V2.Models.SummaryModel;
using CodeMatcher.EntityFrameworkCore.DatabaseModels.SummaryTables;
using CodeMatcherV2Api.ApiRequestModels;
using CodeMatcherV2Api.Models;
using System.Globalization;
using System;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using CodeMatcher.Api.V2.Models;
using CodeMatcher.EntityFrameworkCore.DatabaseModels;

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
                config.CreateMap<CodeGenerationSummaryModel, CodeGenerationSummaryDto>()
                 .ForMember(x => x.Date, y => y.MapFrom(z => DateTime.ParseExact(z.Date, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture)))
                 .ReverseMap();
                config.CreateMap<MonthlyEmbedSummaryModel, MonthlyEmbeddingsSummaryDto>()
                .ForMember(x => x.Date, y => y.MapFrom(z => DateTime.ParseExact(z.Date, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture)))
                 .ReverseMap();
                config.CreateMap< WeeklyEmbedSummaryModel, WeeklyEmbeddingsSummaryDto>()
                .ForMember(x => x.Date, y => y.MapFrom(z => DateTime.ParseExact(z.Date, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture)))
                 .ReverseMap();
                config.CreateMap<CodeMappingRequestDto, CgTriggeredRunReqModel>().ReverseMap();
                config.CreateMap<LogTableModel, LogTableDto>().ReverseMap();
                config.CreateMap<APIKeyModel, APIKeyDto>().ReverseMap();
            });
            return mapperConfig;
        }
    }
}

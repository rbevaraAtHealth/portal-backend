using CodeMatcherV2Api.ApiRequestModels;
using CodeMatcherV2Api.ApiResponeModel;
using CodeMatcherV2Api.BusinessLayer.Enums;
using CodeMappingEfCore.DatabaseModels;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using CodeMatcherV2Api.EntityFrameworkCore;
using CodeMatcher.Api.V2.BusinessLayer.Enums;

namespace CodeMatcherV2Api.RepoModelAdapter
{
    public class WeeklyEmbedTriggeredDbModelAdapter : IRepositoryModel<CodeMappingRequestDto, WeeklyEmbedTriggeredRunReqModel>
    {
        public CodeMappingRequestDto RequestModel_Get(WeeklyEmbedTriggeredRunReqModel pyAPIModel, RequestType type, CodeMappingType codeMappingType, CodeMatcherDbContext context)
        {
            CodeMappingRequestDto codeMappingRequestDto = new CodeMappingRequestDto();
            var lookup = context.Lookups.FirstOrDefaultAsync(x => x.Id == (int)type).Result;
            var runTypeId = lookup.Id;
            var segment = context.Lookups.FirstOrDefaultAsync(x => x.Name == pyAPIModel.Segment).Result;
            var segmentId = segment.Id;

            var codeMapping = context.Lookups.FirstOrDefaultAsync(x => x.Id == (int)codeMappingType).Result;
            codeMappingRequestDto.CodeMappingId = codeMapping.Id;
            codeMappingRequestDto.RunTypeId = runTypeId;
            codeMappingRequestDto.SegmentTypeId = segmentId;
            codeMappingRequestDto.ClientId = "All";
            return codeMappingRequestDto;
        }
    }
}

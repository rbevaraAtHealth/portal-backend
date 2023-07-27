using CodeMatcherV2Api.ApiRequestModels;
using CodeMatcherV2Api.BusinessLayer.Enums;
using CodeMatcherV2Api.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CodeMappingEfCore.DatabaseModels;
using CodeMatcher.Api.V2.BusinessLayer.Enums;

namespace CodeMatcherV2Api.RepoModelAdapter
{
    public class MonthlyEmbedTriggeredDbModelAdapter : IRepositoryModel<CodeMappingRequestDto, MonthlyEmbedTriggeredRunReqModel>
    {
        public CodeMappingRequestDto RequestModel_Get(MonthlyEmbedTriggeredRunReqModel pyAPIModel, RequestType type, CodeMappingType codeMappingType, CodeMatcherDbContext context)
        {
            CodeMappingRequestDto codeMappingRequestDto = new CodeMappingRequestDto();
            var lookup = context.Lookups.FirstOrDefaultAsync(x => x.Id == (int)type).Result;
            var runTypeId = lookup.Id;
            var segment = context.Lookups.FirstOrDefaultAsync(x => x.Name == pyAPIModel.Segment).Result;
            var segmentId = segment.Id;

            var codeMapping = context.Lookups.FirstOrDefaultAsync(x => x.Id == (int)codeMappingType).Result;
            codeMappingRequestDto.CodeMappingId=codeMapping.Id;
            codeMappingRequestDto.RunTypeId = runTypeId;
            codeMappingRequestDto.SegmentTypeId = segmentId;
            return codeMappingRequestDto;
        }
    }
}

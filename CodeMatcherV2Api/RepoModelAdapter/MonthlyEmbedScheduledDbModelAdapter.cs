using CodeMatcherV2Api.ApiRequestModels;
using CodeMatcherV2Api.ApiResponeModel;
using CodeMatcherV2Api.BusinessLayer.Enums;
using CodeMappingEfCore.DatabaseModels;
using CodeMatcherV2Api.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CodeMatcher.Api.V2.BusinessLayer.Enums;

namespace CodeMatcherV2Api.RepoModelAdapter
{
    public class MonthlyEmbedScheduledDbModelAdapter : IRepositoryModel<CodeMappingRequestDto, MonthlyEmbedScheduledRunReqModel>
    {
        public CodeMappingRequestDto RequestModel_Get(MonthlyEmbedScheduledRunReqModel pyAPIModel, RequestType type, CodeMappingType codeMappingType, CodeMatcherDbContext context)
        {
            CodeMappingRequestDto embeddingsDto = new CodeMappingRequestDto();
            var lookup = context.Lookups.FirstOrDefaultAsync(x => x.Id == (int)type).Result;
            var runTypeId = lookup.Id;
            var segment = context.Lookups.FirstOrDefaultAsync(x => x.Name == pyAPIModel.Segment).Result;
            var segmentId = segment.Id;
            var frequencyId = context.Lookups.FirstOrDefaultAsync(x => x.Id == (int)codeMappingType).Result;
            embeddingsDto.RunTypeId = runTypeId;
            embeddingsDto.SegmentTypeId = segmentId;
            embeddingsDto.RunSchedule = pyAPIModel.Runschedule;
            return embeddingsDto;
        }
    }
}

using CodeMatcherV2Api.ApiRequestModels;
using CodeMatcherV2Api.BusinessLayer.Enums;
using CodeMappingEfCore.DatabaseModels;
using CodeMatcherV2Api.EntityFrameworkCore;
using CodeMatcher.Api.V2.BusinessLayer.Enums;
using Microsoft.EntityFrameworkCore;

namespace CodeMatcherV2Api.RepoModelAdapter
{
    public class WeeklyEmbedScheduledDbModelAdapter : IRepositoryModel<CodeMappingRequestDto, WeeklyEmbedScheduledRunReqModel>//, EmbeddingsResponseDto, WeeklyEmbedScheduledRunResModel>
    {
        public CodeMappingRequestDto RequestModel_Get(WeeklyEmbedScheduledRunReqModel pyAPIModel, RequestType type, CodeMappingType codeMappingType, CodeMatcherDbContext context)
        {
            CodeMappingRequestDto codeMappingRequest = new CodeMappingRequestDto();
            var lookup = context.Lookups.FirstOrDefaultAsync(x => x.Id == (int)type).Result;
            var runTypeId = lookup.Id;
            var segment = context.Lookups.FirstOrDefaultAsync(x => x.Name == pyAPIModel.Segment).Result;
            var segmentId = segment.Id;

            var frequencyId = context.Lookups.FirstOrDefaultAsync(x => x.Id == (int)codeMappingType).Result;
            //embeddingsDto.RunTypeId = runTypeId;
            //embeddingsDto.SegmentTypeId = segmentId;

            //embeddingsDto.EmbeddingFrequencyId = frequencyId.Id;
            //embeddingsDto.CreatedBy = createdBy;

            return codeMappingRequest;
        }
    }
}

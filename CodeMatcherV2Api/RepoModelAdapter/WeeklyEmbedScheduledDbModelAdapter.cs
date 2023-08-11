using CodeMatcherV2Api.ApiRequestModels;
using CodeMappingEfCore.DatabaseModels;
using CodeMatcherV2Api.EntityFrameworkCore;
using CodeMatcherV2Api.Middlewares.SqlHelper;

namespace CodeMatcherV2Api.RepoModelAdapter
{
    public class WeeklyEmbedScheduledDbModelAdapter : IRepositoryModel<CodeMappingRequestDto, WeeklyEmbedScheduledRunReqModel>//, EmbeddingsResponseDto, WeeklyEmbedScheduledRunResModel>
    {
        public CodeMappingRequestDto RequestModel_Get(WeeklyEmbedScheduledRunReqModel pyAPIModel, string runType, string codeMappingType, CodeMatcherDbContext context)
        {
            CodeMappingRequestDto codeMappingRequest = new CodeMappingRequestDto();
            codeMappingRequest.RunTypeId = SqlHelper.GetLookupType(runType, context);
            codeMappingRequest.SegmentTypeId = SqlHelper.GetLookupType(pyAPIModel.Segment, context);
            codeMappingRequest.CodeMappingId=SqlHelper.GetLookupType(codeMappingType, context);
            codeMappingRequest.ClientId = "All";
            return codeMappingRequest;
        }
    }
}

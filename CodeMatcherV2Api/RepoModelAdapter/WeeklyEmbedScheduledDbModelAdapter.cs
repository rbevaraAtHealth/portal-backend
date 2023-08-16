using CodeMatcherV2Api.ApiRequestModels;
using CodeMappingEfCore.DatabaseModels;
using CodeMatcherV2Api.EntityFrameworkCore;
using CodeMatcherV2Api.Middlewares.SqlHelper;

namespace CodeMatcherV2Api.RepoModelAdapter
{
    public class WeeklyEmbedScheduledDbModelAdapter : IRepositoryModel<CodeMappingRequestDto, WeeklyEmbedScheduledRunReqModel>//, EmbeddingsResponseDto, WeeklyEmbedScheduledRunResModel>
    {
        private readonly SqlHelper _sqlHelper;
        public WeeklyEmbedScheduledDbModelAdapter(SqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }
        public CodeMappingRequestDto RequestModel_Get(WeeklyEmbedScheduledRunReqModel pyAPIModel, string runType, string codeMappingType)
        {
            CodeMappingRequestDto codeMappingRequest = new CodeMappingRequestDto();
            codeMappingRequest.RunTypeId = _sqlHelper.GetLookupIdOnName(runType);
            codeMappingRequest.SegmentTypeId = _sqlHelper.GetLookupIdOnName(pyAPIModel.Segment);
            codeMappingRequest.CodeMappingId=_sqlHelper.GetLookupIdOnName(codeMappingType);
            codeMappingRequest.ClientId = "All";
            return codeMappingRequest;
        }
    }
}

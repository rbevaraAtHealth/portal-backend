using CodeMatcherV2Api.ApiRequestModels;
using CodeMappingEfCore.DatabaseModels;
using CodeMatcherV2Api.Middlewares.SqlHelper;

namespace CodeMatcherV2Api.RepoModelAdapter
{
    public class MonthlyEmbedTriggeredDbModelAdapter : IRepositoryModel<CodeMappingRequestDto, MonthlyEmbedTriggeredRunReqModel>
    {
        private readonly SqlHelper _sqlHelper;
        public MonthlyEmbedTriggeredDbModelAdapter(SqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }
        public CodeMappingRequestDto RequestModel_Get(MonthlyEmbedTriggeredRunReqModel pyAPIModel, string runType, string codeMappingType)
        {
            CodeMappingRequestDto codeMappingRequestDto = new CodeMappingRequestDto();
            codeMappingRequestDto.RunTypeId = _sqlHelper.GetLookupIdOnName(runType);
            codeMappingRequestDto.SegmentTypeId = _sqlHelper.GetLookupIdOnName(pyAPIModel.Segment);
            codeMappingRequestDto.CodeMappingId = _sqlHelper.GetLookupIdOnName(codeMappingType);
            codeMappingRequestDto.ClientId = "All";
            return codeMappingRequestDto;
        }
    }
}

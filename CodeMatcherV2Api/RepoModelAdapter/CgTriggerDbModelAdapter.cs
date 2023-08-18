using CodeMatcherV2Api.ApiRequestModels;
using CodeMappingEfCore.DatabaseModels;
using CodeMatcherV2Api.EntityFrameworkCore;
using CodeMatcherV2Api.Middlewares.SqlHelper;


namespace CodeMatcherV2Api.RepoModelAdapter
{
    public class CgTriggerDbModelAdapter : IRepositoryModel<CodeMappingRequestDto, CgTriggeredRunReqModel>
    {
        private readonly SqlHelper _sqlHelper;
        public CgTriggerDbModelAdapter(SqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }
        public CodeMappingRequestDto RequestModel_Get(CgTriggeredRunReqModel pyAPIModel, string runType, string codeMappingType)
        {
            CodeMappingRequestDto codeMappingRequestDto = new CodeMappingRequestDto();
            codeMappingRequestDto.RunTypeId = _sqlHelper.GetLookupIdOnName(runType);
            codeMappingRequestDto.SegmentTypeId = _sqlHelper.GetLookupIdOnName(pyAPIModel.Segment);
            codeMappingRequestDto.CodeMappingId = _sqlHelper.GetLookupIdOnName(codeMappingType);
            codeMappingRequestDto.Threshold = pyAPIModel.Threshold;
            codeMappingRequestDto.LatestLink = pyAPIModel.LatestLink;
            codeMappingRequestDto.ClientId = pyAPIModel.ClientId;
            return codeMappingRequestDto;
        }
    }
}

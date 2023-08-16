using CodeMatcherV2Api.ApiRequestModels;
using CodeMappingEfCore.DatabaseModels;
using CodeMatcherV2Api.EntityFrameworkCore;
using CodeMatcherV2Api.Middlewares.SqlHelper;

namespace CodeMatcherV2Api.RepoModelAdapter
{
    public class CgScheduleDbModelAdapter : IRepositoryModel<CodeMappingRequestDto, CgScheduledRunReqModel>
    {
        private readonly SqlHelper _sqlHelper;
        public CgScheduleDbModelAdapter(SqlHelper sqlHelper)
        {
                _sqlHelper= sqlHelper;
        }
        public CodeMappingRequestDto RequestModel_Get(CgScheduledRunReqModel pyAPIModel, string runType, string codeMappingType)
        {
            CodeMappingRequestDto cgDBRequestModel = new CodeMappingRequestDto();
            cgDBRequestModel.RunTypeId = _sqlHelper.GetLookupIdOnName(runType);
            cgDBRequestModel.SegmentTypeId = _sqlHelper.GetLookupIdOnName(pyAPIModel.Segment);
            cgDBRequestModel.CodeMappingId = _sqlHelper.GetLookupIdOnName(codeMappingType);
            cgDBRequestModel.Threshold = pyAPIModel.Threshold;
            cgDBRequestModel.LatestLink = pyAPIModel.LatestLink;
            cgDBRequestModel.RunSchedule = pyAPIModel.RunSchedule;
            cgDBRequestModel.ClientId = pyAPIModel.ClientId;
            return cgDBRequestModel;
        }
    }
}

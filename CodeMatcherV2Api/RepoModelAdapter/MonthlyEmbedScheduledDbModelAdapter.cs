using CodeMatcherV2Api.ApiRequestModels;
using CodeMappingEfCore.DatabaseModels;
using CodeMatcherV2Api.EntityFrameworkCore;
using CodeMatcherV2Api.Middlewares.SqlHelper;

namespace CodeMatcherV2Api.RepoModelAdapter
{
    public class MonthlyEmbedScheduledDbModelAdapter : IRepositoryModel<CodeMappingRequestDto, MonthlyEmbedScheduledRunReqModel>
    {
        private readonly SqlHelper _sqlHelper;
        public MonthlyEmbedScheduledDbModelAdapter(SqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper; 
        }
        public CodeMappingRequestDto RequestModel_Get(MonthlyEmbedScheduledRunReqModel pyAPIModel, string runType, string codeMappingType)
        {
            CodeMappingRequestDto codeMappingRequestDto = new CodeMappingRequestDto();
            codeMappingRequestDto.RunTypeId = _sqlHelper.GetLookupIdOnName(runType);
            codeMappingRequestDto.SegmentTypeId = _sqlHelper.GetLookupIdOnName(pyAPIModel.Segment);
            codeMappingRequestDto.CodeMappingId = _sqlHelper.GetLookupIdOnName(codeMappingType);
            codeMappingRequestDto.RunSchedule = pyAPIModel.Runschedule;
            codeMappingRequestDto.ClientId = "All";
            return codeMappingRequestDto;
        }
    }
}

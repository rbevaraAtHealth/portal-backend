using CodeMatcherV2Api.ApiRequestModels;
using CodeMappingEfCore.DatabaseModels;
using CodeMatcherV2Api.EntityFrameworkCore;
using CodeMatcherV2Api.Middlewares.SqlHelper;

namespace CodeMatcherV2Api.RepoModelAdapter
{
    public class MonthlyEmbedScheduledDbModelAdapter : IRepositoryModel<CodeMappingRequestDto, MonthlyEmbedScheduledRunReqModel>
    {
        public CodeMappingRequestDto RequestModel_Get(MonthlyEmbedScheduledRunReqModel pyAPIModel, string runType, string codeMappingType, CodeMatcherDbContext context)
        {
            CodeMappingRequestDto codeMappingRequestDto = new CodeMappingRequestDto();
            codeMappingRequestDto.RunTypeId = SqlHelper.GetLookupIdOnName(runType, context);
            codeMappingRequestDto.SegmentTypeId = SqlHelper.GetLookupIdOnName(pyAPIModel.Segment, context);
            codeMappingRequestDto.CodeMappingId = SqlHelper.GetLookupIdOnName(codeMappingType, context);
            codeMappingRequestDto.RunSchedule = pyAPIModel.Runschedule;
            codeMappingRequestDto.ClientId = "All";
            return codeMappingRequestDto;
        }
    }
}

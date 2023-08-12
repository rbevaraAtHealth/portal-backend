using CodeMatcherV2Api.ApiRequestModels;
using CodeMatcherV2Api.EntityFrameworkCore;
using CodeMappingEfCore.DatabaseModels;
using CodeMatcherV2Api.Middlewares.SqlHelper;

namespace CodeMatcherV2Api.RepoModelAdapter
{
    public class MonthlyEmbedTriggeredDbModelAdapter : IRepositoryModel<CodeMappingRequestDto, MonthlyEmbedTriggeredRunReqModel>
    {
        public CodeMappingRequestDto RequestModel_Get(MonthlyEmbedTriggeredRunReqModel pyAPIModel, string runType, string codeMappingType, CodeMatcherDbContext context)
        {
            CodeMappingRequestDto codeMappingRequestDto = new CodeMappingRequestDto();
            codeMappingRequestDto.RunTypeId = SqlHelper.GetLookupIdOnName(runType, context);
            codeMappingRequestDto.SegmentTypeId = SqlHelper.GetLookupIdOnName(pyAPIModel.Segment, context);
            codeMappingRequestDto.CodeMappingId = SqlHelper.GetLookupIdOnName(codeMappingType, context);
            codeMappingRequestDto.ClientId = "All";
            return codeMappingRequestDto;
        }
    }
}

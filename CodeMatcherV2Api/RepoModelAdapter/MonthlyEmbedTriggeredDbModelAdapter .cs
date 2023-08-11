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
            codeMappingRequestDto.RunTypeId = SqlHelper.GetLookupType(runType, context);
            codeMappingRequestDto.SegmentTypeId = SqlHelper.GetLookupType(pyAPIModel.Segment, context);
            codeMappingRequestDto.CodeMappingId = SqlHelper.GetLookupType(codeMappingType, context);
            codeMappingRequestDto.ClientId = "All";
            return codeMappingRequestDto;
        }
    }
}

using CodeMatcherV2Api.ApiRequestModels;
using CodeMappingEfCore.DatabaseModels;
using CodeMatcherV2Api.EntityFrameworkCore;
using CodeMatcherV2Api.Middlewares.SqlHelper;

namespace CodeMatcherV2Api.RepoModelAdapter
{
    public class WeeklyEmbedTriggeredDbModelAdapter : IRepositoryModel<CodeMappingRequestDto, WeeklyEmbedTriggeredRunReqModel>
    {
        public CodeMappingRequestDto RequestModel_Get(WeeklyEmbedTriggeredRunReqModel pyAPIModel, string runType, string codeMappingType, CodeMatcherDbContext context)
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

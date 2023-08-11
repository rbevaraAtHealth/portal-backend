using CodeMatcherV2Api.ApiRequestModels;
using CodeMappingEfCore.DatabaseModels;
using CodeMatcherV2Api.EntityFrameworkCore;
using CodeMatcherV2Api.Middlewares.SqlHelper;


namespace CodeMatcherV2Api.RepoModelAdapter
{
    public class CgTriggerDbModelAdapter : IRepositoryModel<CodeMappingRequestDto, CgTriggeredRunReqModel>
    {
        public CodeMappingRequestDto RequestModel_Get(CgTriggeredRunReqModel pyAPIModel, string runType, string codeMappingType, CodeMatcherDbContext context)
        {
            CodeMappingRequestDto codeMappingRequestDto = new CodeMappingRequestDto();
            codeMappingRequestDto.RunTypeId = SqlHelper.GetLookupIdOnName(runType, context);
            codeMappingRequestDto.SegmentTypeId = SqlHelper.GetLookupIdOnName(pyAPIModel.Segment, context);
            codeMappingRequestDto.CodeMappingId = SqlHelper.GetLookupIdOnName(codeMappingType, context);
            codeMappingRequestDto.Threshold = pyAPIModel.Threshold.ToString();
            codeMappingRequestDto.LatestLink = pyAPIModel.LatestLink;
            codeMappingRequestDto.ClientId = pyAPIModel.ClientId;
            return codeMappingRequestDto;
        }
    }
}

using CodeMatcherV2Api.ApiRequestModels;
using CodeMatcherV2Api.ApiResponseModel;
using CodeMatcherV2Api.BusinessLayer.Enums;
using CodeMappingEfCore.DatabaseModels;
using CodeMatcherV2Api.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CodeMatcher.Api.V2.BusinessLayer.Enums;
using System.Linq;
using CodeMatcherV2Api.Middlewares.SqlHelper;

namespace CodeMatcherV2Api.RepoModelAdapter
{
    public class MonthlyEmbedScheduledDbModelAdapter : IRepositoryModel<CodeMappingRequestDto, MonthlyEmbedScheduledRunReqModel>
    {
        public CodeMappingRequestDto RequestModel_Get(MonthlyEmbedScheduledRunReqModel pyAPIModel, RequestType type, CodeMappingType codeMappingType, CodeMatcherDbContext context)
        {
            CodeMappingRequestDto codeMappingRequestDto = new CodeMappingRequestDto();
            codeMappingRequestDto.RunTypeId = SqlHelper.GetLookupType((int)type, context);
            codeMappingRequestDto.SegmentTypeId = SqlHelper.GetLookupType(pyAPIModel.Segment, context);
            codeMappingRequestDto.CodeMappingId = SqlHelper.GetLookupType((int)codeMappingType, context);
            codeMappingRequestDto.RunSchedule = pyAPIModel.Runschedule;
            codeMappingRequestDto.ClientId = "All";
            return codeMappingRequestDto;
        }
    }
}

using CodeMatcherV2Api.ApiRequestModels;
using CodeMatcherV2Api.ApiResponseModel;
using CodeMatcherV2Api.BusinessLayer.Enums;
using CodeMappingEfCore.DatabaseModels;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using CodeMatcherV2Api.EntityFrameworkCore;
using CodeMatcher.Api.V2.BusinessLayer.Enums;
using System.Linq;
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

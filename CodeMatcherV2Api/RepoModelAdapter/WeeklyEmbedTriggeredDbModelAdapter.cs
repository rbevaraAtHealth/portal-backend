using CodeMatcherV2Api.ApiRequestModels;
using CodeMatcherV2Api.ApiResponeModel;
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
        public CodeMappingRequestDto RequestModel_Get(WeeklyEmbedTriggeredRunReqModel pyAPIModel, RequestType type, CodeMappingType codeMappingType, CodeMatcherDbContext context)
        {
            CodeMappingRequestDto codeMappingRequestDto = new CodeMappingRequestDto();
            codeMappingRequestDto.RunTypeId = SqlHelper.GetLookupType(type.ToString(), context);
            codeMappingRequestDto.SegmentTypeId = SqlHelper.GetLookupType(pyAPIModel.Segment, context);
            codeMappingRequestDto.CodeMappingId = SqlHelper.GetLookupType((int)codeMappingType, context);
            codeMappingRequestDto.ClientId = "All";
            return codeMappingRequestDto;
        }
    }
}

using CodeMatcherV2Api.ApiRequestModels;
using CodeMatcherV2Api.ApiResponeModel;
using CodeMatcherV2Api.BusinessLayer.Enums;
using CodeMappingEfCore.DatabaseModels;
using CodeMatcherV2Api.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http;
using CodeMatcher.Api.V2.BusinessLayer.Enums;
using System.Linq;
using CodeMatcherV2Api.Middlewares.SqlHelper;

namespace CodeMatcherV2Api.RepoModelAdapter
{
    public class CgTriggerDbModelAdapter : IRepositoryModel<CodeMappingRequestDto, CgTriggeredRunReqModel>
    {
        public CodeMappingRequestDto RequestModel_Get(CgTriggeredRunReqModel pyAPIModel, RequestType type, CodeMappingType codeMappingType, CodeMatcherDbContext context)
        {
            CodeMappingRequestDto codeMappingRequestDto = new CodeMappingRequestDto();
            codeMappingRequestDto.RunTypeId = SqlHelper.GetLookupType((int)type, context);
            codeMappingRequestDto.SegmentTypeId = SqlHelper.GetLookupType(pyAPIModel.Segment, context);
            codeMappingRequestDto.CodeMappingId = SqlHelper.GetLookupType((int)codeMappingType, context);
            codeMappingRequestDto.Threshold = pyAPIModel.Threshold.ToString();
            codeMappingRequestDto.LatestLink = pyAPIModel.LatestLink;
            codeMappingRequestDto.ClientId = pyAPIModel.ClientId;
            return codeMappingRequestDto;
        }
    }
}

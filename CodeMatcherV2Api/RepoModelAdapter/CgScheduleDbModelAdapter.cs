using CodeMatcherV2Api.ApiRequestModels;
using CodeMatcherV2Api.ApiResponseModel;
using CodeMatcherV2Api.BusinessLayer.Enums;
using CodeMappingEfCore.DatabaseModels;
using CodeMatcherV2Api.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using CodeMatcher.Api.V2.BusinessLayer.Enums;
using System.Linq;
using CodeMatcherV2Api.Middlewares.SqlHelper;

namespace CodeMatcherV2Api.RepoModelAdapter
{
    public class CgScheduleDbModelAdapter : IRepositoryModel<CodeMappingRequestDto, CgScheduledRunReqModel>
    {
        public CodeMappingRequestDto RequestModel_Get(CgScheduledRunReqModel pyAPIModel, RequestType type, CodeMappingType codeMappingType, CodeMatcherDbContext context)
        {
            CodeMappingRequestDto cgDBRequestModel = new CodeMappingRequestDto();
            cgDBRequestModel.RunTypeId = SqlHelper.GetLookupType((int)type, context);
            cgDBRequestModel.SegmentTypeId = SqlHelper.GetLookupType(pyAPIModel.Segment, context);
            cgDBRequestModel.Threshold = pyAPIModel.Threshold.ToString();
            cgDBRequestModel.LatestLink = pyAPIModel.LatestLink;
            cgDBRequestModel.RunSchedule = pyAPIModel.RunSchedule;
            cgDBRequestModel.ClientId = pyAPIModel.ClientId;
            return cgDBRequestModel;
        }
    }
}

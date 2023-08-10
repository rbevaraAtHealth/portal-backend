using CodeMatcher.Api.V2.BusinessLayer.Enums;
using CodeMatcherV2Api.BusinessLayer.Enums;
using CodeMatcherV2Api.EntityFrameworkCore;
using System.Net.Http;

namespace CodeMatcherV2Api.RepoModelAdapter
{
    public interface IRepositoryModel<ReqT, ReqU>
    {
        ReqT RequestModel_Get(ReqU pyAPIModel, string runType, string codeMappingType, CodeMatcherDbContext context);
    }
}

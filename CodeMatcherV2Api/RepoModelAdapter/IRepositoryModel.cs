using CodeMatcherV2Api.EntityFrameworkCore;

namespace CodeMatcherV2Api.RepoModelAdapter
{
    public interface IRepositoryModel<ReqT, ReqU>
    {
        ReqT RequestModel_Get(ReqU pyAPIModel, string runType, string codeMappingType);
    }
}

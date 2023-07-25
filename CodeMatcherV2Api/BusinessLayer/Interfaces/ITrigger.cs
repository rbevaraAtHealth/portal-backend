using CodeMatcherV2Api.ApiRequestModels;
using CodeMatcherV2Api.ApiResponeModel;
using CodeMatcherV2Api.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CodeMatcherV2Api.BusinessLayer.Interfaces
{
    public interface ITrigger
    {
        Task<string> GetCgTriggerJobAsync();
        Task<string> GetMonthlyTriggerJobAsync();
        Task<string> GetWeeklyTriggerJobAsync();
        CgTriggeredRunReqModel CgApiRequestGet(CgTriggerRunModel trigger);
        CgTriggeredRunResModel CgAPiResponseSave(HttpResponseMessage response);
        MonthlyEmbedTriggeredRunReqModel MonthlyEmbedApiRequestGet(MonthlyEmbedTriggeredRunModel trigger);
        MonthlyEmbedTriggeredRunResModel MonthlyEmbedApiResponseSave(HttpResponseMessage response);
        WeeklyEmbedTriggeredRunReqModel WeeklyEmbedApiRequestGet(WeeklyEmbedTriggeredRunModel trigger);
        WeeklyEmbedTriggeredRunResModel WeeklyEmbedApiResponseSave(HttpResponseMessage response);
    }
}

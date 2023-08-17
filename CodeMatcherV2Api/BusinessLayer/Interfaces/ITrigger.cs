using CodeMatcherV2Api.ApiRequestModels;
using CodeMatcherV2Api.ApiResponseModel;
using CodeMatcherV2Api.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CodeMatcherV2Api.BusinessLayer.Interfaces
{
    public interface ITrigger
    {
        //Task<string> GetCgTriggerJobAsync();
        //Task<string> GetMonthlyTriggerJobAsync();
        //Task<string> GetWeeklyTriggerJobAsync();
       Task<Tuple<CgTriggeredRunReqModel, int>> CgApiRequestGet(CgTriggerRunModel trigger,LoginModel user, string clientId);
        Task<CgTriggeredRunResModel> CgAPiResponseSave(HttpResponseMessage response, int RequestId,LoginModel user);
        Task<Tuple<MonthlyEmbedTriggeredRunReqModel, int>> MonthlyEmbedApiRequestGet(MonthlyEmbedTriggeredRunModel trigger,LoginModel user, string clientId);
       Task<MonthlyEmbedTriggeredRunResModel> MonthlyEmbedApiResponseSave(HttpResponseMessage response, int reuestId,LoginModel user);
        Task<Tuple<WeeklyEmbedTriggeredRunReqModel, int>> WeeklyEmbedApiRequestGet(WeeklyEmbedTriggeredRunModel trigger,LoginModel user,string clientId);
        Task<WeeklyEmbedTriggeredRunResModel> WeeklyEmbedApiResponseSave(HttpResponseMessage response, int requestId,LoginModel user);
    }
}


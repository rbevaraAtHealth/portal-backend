//using CodeMatcherV2Api.ApiRequestModels;
//using CodeMatcherV2Api.ApiResponseModel;
//using CodeMatcherV2Api.Models;
//using System.Collections.Generic;
//using System.Net.Http;
//using System.Threading.Tasks;

//namespace CodeMatcherV2Api.BusinessLayer.Interfaces
//{
//    public interface ITrigger
//    {
//        Task<string> GetCgTriggerJobAsync();
//        Task<string> GetMonthlyTriggerJobAsync();
//        Task<string> GetWeeklyTriggerJobAsync();
//        CgTriggeredRunReqModel CgApiRequestGet(CgTriggerRunModel trigger);
//        CgTriggeredRunResModel CgAPiResponseSave(HttpResponseMessage response);
//        MonthlyEmbedTriggeredRunReqModel MonthlyEmbedApiRequestGet(MonthlyEmbedTriggeredRunModel trigger);
//        MonthlyEmbedTriggeredRunResModel MonthlyEmbedApiResponseSave(HttpResponseMessage response);
//        WeeklyEmbedTriggeredRunReqModel WeeklyEmbedApiRequestGet(WeeklyEmbedTriggeredRunModel trigger);
//        WeeklyEmbedTriggeredRunResModel WeeklyEmbedApiResponseSave(HttpResponseMessage response);
//    }
//}
using CodeMatcherV2Api.ApiRequestModels;
using CodeMatcherV2Api.ApiResponseModel;
using CodeMatcherV2Api.Models;
using System;
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
        Tuple<CgTriggeredRunReqModel, int> CgApiRequestGet(CgTriggerRunModel trigger,LoginModel user, string clientId);
        CgTriggeredRunResModel CgAPiResponseSave(HttpResponseMessage response, int RequestId,LoginModel user);
        Tuple<MonthlyEmbedTriggeredRunReqModel, int> MonthlyEmbedApiRequestGet(MonthlyEmbedTriggeredRunModel trigger,LoginModel user, string clientId);
        MonthlyEmbedTriggeredRunResModel MonthlyEmbedApiResponseSave(HttpResponseMessage response, int reuestId,LoginModel user);
        Tuple<WeeklyEmbedTriggeredRunReqModel, int> WeeklyEmbedApiRequestGet(WeeklyEmbedTriggeredRunModel trigger,LoginModel user,string clientId);
        WeeklyEmbedTriggeredRunResModel WeeklyEmbedApiResponseSave(HttpResponseMessage response, int requestId,LoginModel user);
        //void GetCgTriggerRunMappingsPythApi(HttpResponseMessage httpResponse);
    }
}


using AutoMapper;
using CodeMatcherV2Api.ApiRequestModels;
using CodeMatcherV2Api.ApiResponeModel;
using CodeMatcherV2Api.BusinessLayer.Interfaces;
using CodeMatcherV2Api.Dtos;
using CodeMatcherV2Api.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CodeMatcherV2Api.BusinessLayer
{
    public class Trigger : ITrigger
    {
        public async Task<string> GetAllTriggerAsync(string segment)
        {
            return "Job Triggered Successfully";
        }
        public CgTriggeredRunReqModel CgApiRequestGet(CgTriggerRunModel trigger)
        {
            CgTriggeredRunReqModel requestModel = new CgTriggeredRunReqModel();
            requestModel.Segment = trigger.Segment;
            requestModel.Threshold = trigger.Threshold;
            requestModel.LatestLink = "32342";
            requestModel.ClientId = "All";
            return requestModel;
        }
        public CgTriggeredRunResModel CgAPiResponseSave(HttpResponseMessage response)
        {
            CgTriggeredRunResModel responseViewModel = new CgTriggeredRunResModel();
            if (response.IsSuccessStatusCode)
            {
                string httpResult = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                if (!string.IsNullOrWhiteSpace(httpResult))
                    responseViewModel = JsonConvert.DeserializeObject<CgTriggeredRunResModel>(httpResult);
                if (responseViewModel != null)
                {
                    return responseViewModel;
                }
            }
            return responseViewModel;
        }

        public MonthlyEmbedTriggeredRunReqModel MonthlyEmbedApiRequestGet(MonthlyEmbedTriggeredRunModel trigger)
        {
            MonthlyEmbedTriggeredRunReqModel requestModel = new MonthlyEmbedTriggeredRunReqModel();
            requestModel.Segment = trigger.Segment;
            return requestModel;
        }

        public WeeklyEmbedTriggeredRunReqModel WeeklyEmbedApiRequestGet(WeeklyEmbedTriggeredRunModel trigger)
        {
            WeeklyEmbedTriggeredRunReqModel requestModel = new WeeklyEmbedTriggeredRunReqModel();
            requestModel.Segment = trigger.Segment;
            requestModel.LatestLink = "32342";
            return requestModel;
        }

        public MonthlyEmbedTriggeredRunResModel MonthlyEmbedApiResponseSave(HttpResponseMessage httpResponse)
        {
            MonthlyEmbedTriggeredRunResModel responseModel = new MonthlyEmbedTriggeredRunResModel();
            if (httpResponse.IsSuccessStatusCode)
            {
                string httpResult = httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                if (!string.IsNullOrWhiteSpace(httpResult))
                    responseModel = JsonConvert.DeserializeObject<MonthlyEmbedTriggeredRunResModel>(httpResult);
                if (responseModel != null)
                {
                    return responseModel;
                }
            }
            return responseModel;
        }

        public WeeklyEmbedTriggeredRunResModel WeeklyEmbedApiResponseSave(HttpResponseMessage httpResponse)
        {
            WeeklyEmbedTriggeredRunResModel responseModel = new WeeklyEmbedTriggeredRunResModel();
            if (httpResponse.IsSuccessStatusCode)
            {
                string httpResult = httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                if (!string.IsNullOrWhiteSpace(httpResult))
                    responseModel = JsonConvert.DeserializeObject<WeeklyEmbedTriggeredRunResModel>(httpResult);
                 return responseModel;
            }
            return responseModel;
        }
    }
}

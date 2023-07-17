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
    public class Schedule: ISchedule
    {
       
        public CGScheduledRunReqModel ApiRequestGet(ScheduleModel schedule)
        {
            CGScheduledRunReqModel requestModel = new CGScheduledRunReqModel();
            requestModel.Segment = schedule.Segment;
            requestModel.RunSchedule = schedule.RunSchedule;
            requestModel.Threshold = schedule.Threshold;
            requestModel.LatestLink = schedule.LatestLink;
            requestModel.ClientId = schedule.Client_Id;
            return  requestModel;
        }

        public  CGScheduleRunResModel APiResponseSave(HttpResponseMessage response)
        {
            CGScheduleRunResModel responseViewModel = new CGScheduleRunResModel();
            if (response.IsSuccessStatusCode)
            {
                string httpResult =  response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                if (!string.IsNullOrWhiteSpace(httpResult))
                    responseViewModel = JsonConvert.DeserializeObject<CGScheduleRunResModel>(httpResult);
                if (responseViewModel != null)
                {
                    return responseViewModel;
                }
            }
            return responseViewModel;
        }
    }
}

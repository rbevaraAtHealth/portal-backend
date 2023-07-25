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
    public class Schedule : ISchedule
    {
        public async Task<string> GetCgScheduleJobAsync()
        {
            return "Code generation Job scheduled sucessfully";
        }

        public async Task<IEnumerable<string>> GetAllScheduleJobsAsync()
        {
            List<string> scheduledJobs = new List<string> { "Job One", "Job Two", "Job Three", "Job Four" };
            return scheduledJobs;
        }

        public async Task<string> GetMonthlyScheduleJobAsync()
        {
            return "Monthly Job scheduled successfully";
        }

        public async Task<string> GetweeklyJobScheduleAsync()
        {
            return "Weekly job scheduled successfully";
        }

        public CgScheduledRunReqModel ApiRequestGet(CgScheduledModel schedule)
        {
            CgScheduledRunReqModel requestModel = new CgScheduledRunReqModel();
            requestModel.Segment = schedule.Segment;
            requestModel.RunSchedule = schedule.RunSchedule;
            requestModel.Threshold = schedule.Threshold;
            requestModel.LatestLink = "32342";
            requestModel.ClientId = "All";
            return requestModel;
        }

        public CgScheduledRunResModel APiResponseSave(HttpResponseMessage response)
        {
            CgScheduledRunResModel responseViewModel = new CgScheduledRunResModel();
            if (response.IsSuccessStatusCode)
            {
                string httpResult = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                if (!string.IsNullOrWhiteSpace(httpResult))
                    responseViewModel = JsonConvert.DeserializeObject<CgScheduledRunResModel>(httpResult);
                if (responseViewModel != null)
                {
                    return responseViewModel;
                }
            }
            return responseViewModel;
        }
    }
}

using CodeMatcherV2Api.ApiRequestModels;
using CodeMatcherV2Api.ApiResponseModel;
using CodeMatcherV2Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CodeMatcherV2Api.BusinessLayer.Interfaces
{
    public interface ISchedule
    {
        Task<IEnumerable<string>> GetAllScheduleJobsAsync();
        Task<string>GetCgScheduleJobAsync();
        Task<string> GetMonthlyScheduleJobAsync();
        Task<string> GetweeklyJobScheduleAsync();
        CgScheduledRunReqModel ApiRequestGet(CgScheduledModel schedule);
        CgScheduledRunResModel APiResponseSave(HttpResponseMessage httpResponse);

    }
}

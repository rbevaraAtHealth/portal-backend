﻿using CodeMatcher.Api.V2.Models;
using CodeMatcherV2Api.ApiRequestModels;
using CodeMatcherV2Api.ApiResponseModel;
using CodeMatcherV2Api.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CodeMatcher.Api.V2.BusinessLayer.Interfaces
{
    public interface IScheduler
    {
        Task<List<SchedulerModel>> GetAllSchedulersAsync();
        Task<SchedulerModel> GetAllSchedulersByIdAsync(int schedulerId);
        Task<Tuple<CgScheduledRunReqModel, int>> GetMonthlyScheduleJobAsync(MonthlyEmbedScheduledRunModel schedule, LoginModel user, string clientId);
        Task<Tuple<CgScheduledRunReqModel, int>> GetweeklyJobScheduleAsync(WeeklyEmbedScheduledRunModel schedule, LoginModel user,string clientId);
        Task<Tuple<CgScheduledRunReqModel, int>> GetCodeGenerationScheduleAsync(CgScheduledModel schedule, LoginModel user,string clientId);

    }
}
using CodeMatcher.Api.V2.BusinessLayer.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using CodeMatcher.Api.V2.Models;

namespace CodeMatcher.Api.V2.BusinessLayer
{
    public class Scheduler : IScheduler
    {
        public async Task<List<SchedulerModel>> GetAllSchedulersAsync()
        {
            List<SchedulerModel> schedulerRecords = new List<SchedulerModel>
            {new SchedulerModel{ CronExpression = "*/2 * * * *" ,Segment="Insurance",Threshold="0.9",ClientId=9},
                new SchedulerModel { CronExpression = "0 0 0 0 0/5", Segment = "Hospital", Threshold = "0.8", ClientId = 5},
                new SchedulerModel { CronExpression = "0 0 0/3 * *", Segment = "State License", Threshold = "0.5", ClientId = 7},
                new SchedulerModel { CronExpression = "0/10 0/3 0/4 0/8 *", Segment = "School", Threshold = "0.6", ClientId = 8},
                new SchedulerModel { CronExpression = "0 0 0 0 0/5", Segment = "Insurance", Threshold = "0.7", ClientId = 6}
            };
            return schedulerRecords;
        }
    }
}
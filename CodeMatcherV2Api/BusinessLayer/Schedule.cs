using AutoMapper;
using CodeMatcherV2Api.BusinessLayer.Interfaces;
using CodeMatcherV2Api.Dtos;
using CodeMatcherV2Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeMatcherV2Api.BusinessLayer
{
    public class Schedule: ISchedule
    {
        public async Task<string> ScheduleJobAsync(ScheduleModel schedule)
        {
            return "Job Scheduled Successfully";
        }
    }
}

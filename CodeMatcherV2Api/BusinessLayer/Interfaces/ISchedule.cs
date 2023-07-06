using CodeMatcherV2Api.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeMatcherV2Api.BusinessLayer.Interfaces
{
    public interface ISchedule
    {
        Task<string> ScheduleJobAsync(ScheduleModel schedule);

    }
}

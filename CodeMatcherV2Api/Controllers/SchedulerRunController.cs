using CodeMatcherV2Api.BusinessLayer;
using CodeMatcherV2Api.BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

namespace CodeMatcherV2Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchedulerRunController : BaseController
    {
        private readonly ISchedule _schedule;
        public SchedulerRunController(ISchedule schedule)
        {
            _schedule = schedule;
        }

        [HttpGet]
        public async Task<IActionResult> ScheduleJob(string segment, string frequency, float threshold)
        {
            try
            {
                var scheduleJob= await _schedule.ScheduleJobAsync(segment, frequency, threshold);
                return Ok(scheduleJob);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}

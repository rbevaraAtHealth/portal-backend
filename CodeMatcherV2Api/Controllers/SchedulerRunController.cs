using CodeMatcherV2Api.BusinessLayer;
using CodeMatcherV2Api.BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using CodeMatcherV2Api.Models;

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

        [HttpPost]
        public async Task<IActionResult> ScheduleJob([FromBody] ScheduleModel schedule)
        {

            try
            {
                var scheduleJob= await _schedule.ScheduleJobAsync(schedule);
                return Ok(scheduleJob);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}

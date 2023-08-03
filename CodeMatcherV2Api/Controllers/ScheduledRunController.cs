using CodeMatcherV2Api.BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using System.Net.Http;
using CodeMatcherV2Api.Models;
using CodeMatcher.Api.V2.Models;

namespace CodeMatcherV2Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduledRunController : BaseController
    {
        private readonly ISchedule _schedule;
        private readonly IHttpClientFactory _httpClientFactory;
        public ScheduledRunController(ISchedule schedule, IHttpClientFactory httpClientFactory)
        {
            _schedule = schedule;
            _httpClientFactory = httpClientFactory;
        }
        
        [HttpPost,Route("ScheduleJob")]
        public async Task<IActionResult> ScheduleJob([FromBody] ScheduleJobModel scheduleJobModel)
        {
            return Ok("Job Scheduled Sucessfully");
        }
    }
}

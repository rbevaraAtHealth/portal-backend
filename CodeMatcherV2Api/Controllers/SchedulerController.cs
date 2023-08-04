using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using CodeMatcher.Api.V2.BusinessLayer.Interfaces;
using CodeMatcherV2Api.Controllers;
using Microsoft.AspNetCore.Http;

namespace CodeMatcher.Api.V2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchedulerController : BaseController
    {
        public readonly IScheduler _scheduler;
        public SchedulerController(IScheduler scheduler, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _scheduler = scheduler;
        }
        [HttpGet, Route("GetSchedulerRecords")]
        public async Task<IActionResult> GetAllSchedulers()
        {
            try
            {
                var records = await _scheduler.GetAllSchedulersAsync();
                return Ok(records);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
using CodeMatcher.Api.V2.BusinessLayer.Interfaces;
using CodeMatcherV2Api.Controllers;
using CodeMatcherV2Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CodeMatcher.Api.V2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchedulerController : BaseController
    {
        public readonly IScheduler _scheduler;

        public SchedulerController(IScheduler scheduler)
        {
            _scheduler = scheduler;
        }

        [AllowAnonymous]
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

        [HttpPost, Route("CodeGenerationSchedulerRun")]
        public async Task<IActionResult> CodeGenerationSchedulerRun([FromBody] CgScheduledModel schedulerModel)
        {
            try
            {
                var user = GetUserInfo();
                var requestModel = await _scheduler.GetCodeGenerationScheduleAsync(schedulerModel, user, getClientId());

                return Ok(requestModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost, Route("WeeklySchedulerRun")]
        public async Task<IActionResult> WeeklySchedulerRun([FromBody] CgScheduledModel schedulerModel)
        {
            try
            {
                var user = GetUserInfo();
                var requestModel = await _scheduler.GetweeklyJobScheduleAsync(schedulerModel, user, getClientId());

                return Ok(requestModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpPost, Route("MonthlySchedulerRun")]
        public async Task<IActionResult> MonthlySchedulerRun([FromBody] CgScheduledModel schedulerModel)
        {
            try
            {
                var user = GetUserInfo();
                var requestModel = await _scheduler.GetMonthlyScheduleJobAsync(schedulerModel, user, getClientId());

                return Ok(requestModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
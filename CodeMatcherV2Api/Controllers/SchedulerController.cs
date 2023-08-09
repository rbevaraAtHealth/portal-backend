using CodeMatcher.Api.V2.BusinessLayer.Interfaces;
using CodeMatcherV2Api.Controllers;
using CodeMatcherV2Api.Models;
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
        private IHttpClientFactory _httpClientFactory;

        public SchedulerController(IScheduler scheduler)
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

        [HttpPost, Route("CreatedSchedulerRun")]
        public async Task<IActionResult> CreateSchedulerRun([FromBody] CgScheduledModel schedulerModel)
        {
            try
            {
                var user = GetUserInfo();
                var requestModel = _scheduler.ApiRequestGet(schedulerModel,user);
                
                return Ok(requestModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
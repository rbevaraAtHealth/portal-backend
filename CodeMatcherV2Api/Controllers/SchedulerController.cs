using CodeMatcher.Api.V2.ApiResponseModel;
using CodeMatcher.Api.V2.BusinessLayer.Interfaces;
using CodeMatcher.Api.V2.Models;
using CodeMatcherV2Api.BusinessLayer.Interfaces;
using CodeMatcherV2Api.Controllers;
using CodeMatcherV2Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CodeMatcher.Api.V2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchedulerController : BaseController
    {
        public readonly IScheduler _scheduler;
        private readonly ResponseViewModel _responseViewModel;
        public readonly ITrigger _trigger;
        private readonly IHttpClientFactory _httpClientFactory;

        public SchedulerController(IScheduler scheduler, ITrigger trigger, IHttpClientFactory httpClientFactory)
        {
            _scheduler = scheduler;
            _responseViewModel = new ResponseViewModel();
            _httpClientFactory = httpClientFactory;
            _trigger = trigger;
        }

        [AllowAnonymous]
        [HttpGet, Route("GetSchedulerRecords")]
        public async Task<IActionResult> GetAllSchedulers()
        {
            try
            {
                var records = await _scheduler.GetAllSchedulersAsync();
                _responseViewModel.Model = records;
                return Ok(_responseViewModel);
            }
            catch (Exception ex)
            {
                _responseViewModel.ExceptionMessage = ex.Message;
                return BadRequest(_responseViewModel);
            }
        }

        [HttpPost, Route("CodeGenerationSchedulerRun")]
        public async Task<IActionResult> CodeGenerationSchedulerRun([FromBody] CgScheduledModel schedulerModel)
        {
            try
            {
                var user = GetUserInfo();
                var requestModel = await _scheduler.GetCodeGenerationScheduleAsync(schedulerModel, user, getClientId());
                _responseViewModel.Model = requestModel;

                return Ok(_responseViewModel);
            }
            catch (Exception ex)
            {
                _responseViewModel.ExceptionMessage = ex.Message;
                return BadRequest(_responseViewModel);
            }
        }

        [HttpPost, Route("WeeklySchedulerRun")]
        public async Task<IActionResult> WeeklySchedulerRun([FromBody] WeeklyEmbedScheduledRunModel schedulerModel)
        {
            try
            {
                var user = GetUserInfo();
                var requestModel = await _scheduler.GetweeklyJobScheduleAsync(schedulerModel, user, getClientId());
                _responseViewModel.Model = requestModel;

                return Ok(_responseViewModel);
            }
            catch (Exception ex)
            {
                _responseViewModel.ExceptionMessage = ex.Message;
                return BadRequest(_responseViewModel);
            }
        }


        [HttpPost, Route("MonthlySchedulerRun")]
        public async Task<IActionResult> MonthlySchedulerRun([FromBody] MonthlyEmbedScheduledRunModel schedulerModel)
        {
            try
            {
                var user = GetUserInfo();
                var requestModel = await _scheduler.GetMonthlyScheduleJobAsync(schedulerModel, user, getClientId());
                _responseViewModel.Model = requestModel;

                return Ok(_responseViewModel);
            }
            catch (Exception ex)
            {
                _responseViewModel.ExceptionMessage = ex.Message;
                return BadRequest(_responseViewModel);
            }
        }

        [HttpPost, Route("RunScheduleById")]
        public async Task<IActionResult> RunScheduleById([FromBody] int scheduleId)
        {
            try
            {
                var user = GetUserInfo();
                var item = await _scheduler.GetAllSchedulersByIdAsync(scheduleId);
                if (item.CodeMapping != null)
                {
                    TriggeredRunController triggered = new TriggeredRunController(_trigger, _httpClientFactory);

                    if (item.CodeMapping.ToLower() == "code generation")
                    {
                        var res = await triggered.CodeGenerationTriggerdRun(new CgTriggerRunModel { Segment = item.Segment, Threshold = item.Threshold });
                        return res;
                    }
                    else if (item.CodeMapping.ToLower() == "weekly embedding")
                    {
                        var res = await triggered.WeeklyEmbedTriggeredRun(new WeeklyEmbedTriggeredRunModel { Segment = item.Segment });
                        return res;

                    }
                    else if (item.CodeMapping.ToLower() == "monthly embedding")
                    {
                        var res = await triggered.MonthlyEmbedTriggereddRun(new MonthlyEmbedTriggeredRunModel { Segment = item.Segment });
                        return res;

                    }
                }
                else
                {
                    _responseViewModel.ExceptionMessage = "Scheduler Not Found";
                    return BadRequest(_responseViewModel);
                }
                return Ok(_responseViewModel);
            }
            catch (Exception ex)
            {
                _responseViewModel.ExceptionMessage = ex.Message;
                return BadRequest(_responseViewModel);
            }
        }

    }
}
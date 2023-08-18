using CodeMatcher.Api.V2.ApiResponseModel;
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
        private readonly ResponseViewModel _responseViewModel;

        public SchedulerController(IScheduler scheduler)
        {
            _scheduler = scheduler;
            _responseViewModel = new ResponseViewModel();
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
        public async Task<IActionResult> WeeklySchedulerRun([FromBody] CgScheduledModel schedulerModel)
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
        public async Task<IActionResult> MonthlySchedulerRun([FromBody] CgScheduledModel schedulerModel)
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
    }
}
using CodeMatcherV2Api.BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using System.Net.Http;

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
        [HttpGet,Route("Code Generation Schedule Run")]
        public async Task<IActionResult> CgScheduleJob()
        {
            try
            {
                var scheduleJob = await _schedule.GetCgScheduleJobAsync();
                return Ok(scheduleJob);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet, Route("Weekly Embedding Schedule Run")]
        public async Task<IActionResult> WeeklyScheduleJob()
        {
            try
            {
                var scheduleJob = await _schedule.GetweeklyJobScheduleAsync();
                return Ok(scheduleJob);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet, Route("Monthly Schedule Run")]
        public async Task<IActionResult> MonthlyScheduleJob()
        {
            try
            {
                var scheduleJob = await _schedule.GetMonthlyScheduleJobAsync();
                return Ok(scheduleJob);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
       
        [HttpGet, Route("Get All Scheduled Jobs List")]
        public async Task<IActionResult> GetAllScheduledJobs()
        {
            try
            {
                var scheduleJob = await _schedule.GetAllScheduleJobsAsync();
                return Ok(scheduleJob);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // public
        //[HttpPost,Route("CodeGenerationScheduleJob")]
        ////[HttpGet]
        //public async Task<IActionResult> CodeGenerationScheduleJob([FromBody] CgScheduledModel schedule)
        //{
        //    try
        //    {
        //        string url = "code-generation/scheduled-run";
        //        var requestModel =  _schedule.ApiRequestGet(schedule);
        //        var apiResponse=await  HttpHelper.Post_HttpClient(_httpClientFactory, requestModel,url);
        //        var SavedData= _schedule.APiResponseSave(apiResponse);
        //        return Ok(SavedData);

        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex);
        //    }
        //}
        //[HttpPost,Route("MonthlyEmbedScheduledRun")]
        //public async Task<IActionResult> MonthlyEmbedScheduledRun([FromBody] CgScheduledModel schedule)
        //{
        //    try
        //    {
        //        string url = "code-generation/scheduled-run";
        //        var requestModel = _schedule.ApiRequestGet(schedule);
        //        var apiResponse = await HttpHelper.Post_HttpClient(_httpClientFactory, requestModel, url);
        //        var SavedData = _schedule.APiResponseSave(apiResponse);
        //        return Ok(SavedData);

        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex);
        //    }

        //}

    }
}

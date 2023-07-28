using CodeMatcherV2Api.BusinessLayer.Interfaces;
using CodeMatcherV2Api.Middlewares.HttpHelper;
using CodeMatcherV2Api.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Threading.Tasks;


namespace CodeMatcherV2Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TriggeredRunController : BaseController
    {
        private readonly ITrigger _trigger;
        private readonly IHttpClientFactory _httpClientFactory;
        public TriggeredRunController(ITrigger trigger, IHttpClientFactory httpClientFactory)
        {
            _trigger = trigger;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet,Route("CodeGenerationTriggerRun")]
        public async Task<IActionResult> CgTriggerJob()
        {
            try
            {
                var triggerJob = await _trigger.GetCgTriggerJobAsync();
                return Ok(triggerJob);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet, Route("MonthlyTriggerRun")]
        public async Task<IActionResult> MonthlyTriggerJob()
        {
            try
            {
                var triggerJob = await _trigger.GetMonthlyTriggerJobAsync();
                return Ok(triggerJob);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet, Route("WeeklyTriggerRun")]
        public async Task<IActionResult> WeeklyTriggerJob()
        {
            try
            {
                var triggerJob = await _trigger.GetWeeklyTriggerJobAsync();
                return Ok(triggerJob);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost, Route("CodeGenerationTriggerRun")]
        public async Task<IActionResult> CodeGenerationTriggerdRun(CgTriggerRunModel trigger)
        {
            try
            {
                string url = "code-generation/triggered-run";
                var requestModel = _trigger.CgApiRequestGet(trigger);
                var apiResponse = await HttpHelper.Post_HttpClient(_httpClientFactory, requestModel.Item1, url);
                var SavedData = _trigger.CgAPiResponseSave(apiResponse,requestModel.Item2);
                return Ok(SavedData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost,Route("MonthlyEmbeddingTriggeredRun")]
        public async Task<IActionResult> MonthlyEmbedTriggereddRun(MonthlyEmbedTriggeredRunModel trigger)
        {
            try
            {
                string url = "monthly-embeddings/triggered-run";
                var requestModel = _trigger.MonthlyEmbedApiRequestGet(trigger);
                var apiResponse = await HttpHelper.Post_HttpClient(_httpClientFactory, requestModel.Item1, url);
                var SavedData = _trigger.MonthlyEmbedApiResponseSave(apiResponse,requestModel.Item2);
                return Ok(SavedData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        [HttpPost,Route("WeeklyEmeddingTriggerRun")]
        public async Task<IActionResult> WeeklyEmbedTriggeredRun(WeeklyEmbedTriggeredRunModel trigger)
        {
            try
            {
                string url = "weekly-embeddings/triggered-run";
                var requestModel = _trigger.WeeklyEmbedApiRequestGet(trigger);
                var apiResponse = await HttpHelper.Post_HttpClient(_httpClientFactory, requestModel.Item1, url);
                var SavedData = _trigger.WeeklyEmbedApiResponseSave(apiResponse,requestModel.Item2);
                return Ok(SavedData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}

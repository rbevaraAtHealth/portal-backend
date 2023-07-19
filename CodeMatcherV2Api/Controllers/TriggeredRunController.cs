using CodeMatcherV2Api.ApiRequestModels;
using CodeMatcherV2Api.BusinessLayer;
using CodeMatcherV2Api.BusinessLayer.Interfaces;
using CodeMatcherV2Api.Middlewares.HttpHelper;
using CodeMatcherV2Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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

        [HttpPost, Route("CodeGenerationTriggerRun")]
        //[HttpGet]
        public async Task<IActionResult> CodeGenerationTriggerdRun(CgTriggerRunModel trigger)
        {
            try
            {
                string url = "code-generation/triggered-run";
                var requestModel = _trigger.CgApiRequestGet(trigger);
                var apiResponse = await HttpHelper.Post_HttpClient(_httpClientFactory, requestModel, url);
                var SavedData = _trigger.CgAPiResponseSave(apiResponse);
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
                var apiResponse = await HttpHelper.Post_HttpClient(_httpClientFactory, requestModel, url);
                var SavedData = _trigger.MonthlyEmbedApiResponseSave(apiResponse);
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
                var apiResponse = await HttpHelper.Post_HttpClient(_httpClientFactory, requestModel, url);
                var SavedData = _trigger.WeeklyEmbedApiResponseSave(apiResponse);
                return Ok(SavedData);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

    }
}

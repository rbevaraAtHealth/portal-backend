﻿using CodeMatcherV2Api.BusinessLayer.Interfaces;
using CodeMatcherV2Api.Middlewares.HttpHelper;
using CodeMatcherV2Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        [AllowAnonymous]
        [HttpPost, Route("CodeGenerationTriggerRun")]
        public async Task<IActionResult> CodeGenerationTriggerdRun(CgTriggerRunModel trigger)
        {
            try
            {
                var user=GetUserInfo();
                string url = "code-generation/triggered-run";
                var requestModel = _trigger.CgApiRequestGet(trigger, user, getClientId());
                var apiResponse = await HttpHelper.Post_HttpClient(_httpClientFactory, requestModel.Item1, url);
                var savedData = _trigger.CgAPiResponseSave(apiResponse, requestModel.Item2, user);
                return Ok(savedData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [AllowAnonymous]
        [HttpPost,Route("MonthlyEmbeddingTriggeredRun")]
        public async Task<IActionResult> MonthlyEmbedTriggereddRun(MonthlyEmbedTriggeredRunModel trigger)
        {
            try
            {
                var user=GetUserInfo();
                string url = "monthly-embeddings/triggered-run";
                var requestModel = _trigger.MonthlyEmbedApiRequestGet(trigger, user, getClientId());
                var apiResponse = await HttpHelper.Post_HttpClient(_httpClientFactory, requestModel.Item1, url);
                var SavedData = _trigger.MonthlyEmbedApiResponseSave(apiResponse,requestModel.Item2,user);
                return Ok(SavedData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [AllowAnonymous]
        [HttpPost,Route("WeeklyEmeddingTriggerRun")]
        public async Task<IActionResult> WeeklyEmbedTriggeredRun(WeeklyEmbedTriggeredRunModel trigger)
        {
            try
            {
                var user = GetUserInfo();
                string url = "weekly-embeddings/triggered-run";
                var requestModel = _trigger.WeeklyEmbedApiRequestGet(trigger,user, getClientId());
                var apiResponse = await HttpHelper.Post_HttpClient(_httpClientFactory, requestModel.Item1, url);
                var SavedData = _trigger.WeeklyEmbedApiResponseSave(apiResponse,requestModel.Item2,user);
                return Ok(SavedData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}

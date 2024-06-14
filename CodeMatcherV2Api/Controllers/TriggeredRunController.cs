using CodeMatcher.Api.V2.ApiResponseModel;
using CodeMatcher.Api.V2.Models;
using CodeMatcherV2Api.BusinessLayer.Interfaces;
using CodeMatcherV2Api.Middlewares.HttpHelper;
using CodeMatcherV2Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly ResponseViewModel _responseViewModel;
        public TriggeredRunController(ITrigger trigger, IHttpClientFactory httpClientFactory)
        {
            _trigger = trigger;
            _httpClientFactory = httpClientFactory;
            _responseViewModel = new ResponseViewModel();
        }

        [AllowAnonymous]
        [HttpPost, Route("CodeGenerationTriggerRun")]
        public async Task<IActionResult> CodeGenerationTriggerdRun(CgTriggerRunModel trigger)
        {
            try
            {
                var user = GetUserInfo();
                string url = "code-generation/triggered-run";
                var requestModel = await _trigger.CgApiRequestGet(trigger, user, getClientId());
                var apiResponse = await HttpHelper.Post_HttpClient(_httpClientFactory, requestModel.Item1, url);
                var savedData = await _trigger.CgAPiResponseSave(apiResponse, requestModel.Item2, user);
                _responseViewModel.Model = savedData;
                return Ok(_responseViewModel);
            }
            catch (Exception ex)
            {
                _responseViewModel.ExceptionMessage = ex.Message;
                return BadRequest(_responseViewModel);
            }
        }

        [AllowAnonymous]
        [HttpPost, Route("MonthlyEmbeddingTriggeredRun")]
        public async Task<IActionResult> MonthlyEmbedTriggereddRun(MonthlyEmbedTriggeredRunModel trigger)
        {
            try
            {
                var user = GetUserInfo();
                string url = "monthly-embeddings/triggered-run";
                var requestModel = await _trigger.MonthlyEmbedApiRequestGet(trigger, user, getClientId());
                var apiResponse = await HttpHelper.Post_HttpClient(_httpClientFactory, requestModel.Item1, url);
                var SavedData = await _trigger.MonthlyEmbedApiResponseSave(apiResponse, requestModel.Item2, user);
                _responseViewModel.Model = SavedData;
                return Ok(_responseViewModel);
            }
            catch (Exception ex)
            {
                _responseViewModel.ExceptionMessage = ex.Message;
                return BadRequest(_responseViewModel);
            }
        }

        [AllowAnonymous]
        [HttpPost, Route("WeeklyEmeddingTriggerRun")]
        public async Task<IActionResult> WeeklyEmbedTriggeredRun(WeeklyEmbedTriggeredRunModel trigger)
        {
            try
            {
                var user = GetUserInfo();
                string url = "weekly-embeddings/triggered-run";
                var requestModel = await _trigger.WeeklyEmbedApiRequestGet(trigger, user, getClientId());
                var apiResponse = await HttpHelper.Post_HttpClient(_httpClientFactory, requestModel.Item1, url);
                var SavedData = await _trigger.WeeklyEmbedApiResponseSave(apiResponse, requestModel.Item2, user);
                _responseViewModel.Model = SavedData;
                return Ok(_responseViewModel);
            }
            catch (Exception ex)
            {
                _responseViewModel.ExceptionMessage = ex.Message;
                return BadRequest(_responseViewModel);
            }
        }

        [HttpGet, Route("GetProgress")]
        public async Task<IActionResult> GetProgress(string taskId)
        {
            try
            {
                var progressStatus = new List<StatusViewModel>
                {
                    new StatusViewModel { TimeStamp = DateTime.Now, Segment = "School", TotalRecords = "1000",ProcessedRecords = "200", PercentageCompletion = "20%" }
                };

                return Ok(progressStatus);
            }
            catch (Exception ex)
            {
                _responseViewModel.ExceptionMessage = ex.Message;
                return BadRequest(_responseViewModel);
            }
        }

        [HttpPost, Route("KillProcess")]
        public async Task<IActionResult> KillProcess(string taskId)
        {
            try
            {
                return Ok(new { TaskId = taskId });
            }
            catch (Exception ex)
            {
                _responseViewModel.ExceptionMessage = ex.Message;
                return BadRequest(_responseViewModel);
            }
        }
        private string GetCurrentTimeInMinutes()
        {
            var currentTime = DateTime.Now;
            return currentTime.ToString("mm:ss");
        }

        

        [AllowAnonymous]
        [HttpGet, Route("GetErrorLogs")]
        public async Task<IActionResult> GetErrorLogs()
        {
            try
            {

                var errorLogs = new List<ErrorLogViewModel>
                {
                    new ErrorLogViewModel { TimeStamp = DateTime.Now, Segment = "School", RunBy = "Scheduler Admin",RunType = "Triggered", ErrorMessage = "Error 1" },
                    new ErrorLogViewModel { TimeStamp = DateTime.Now, Segment = "Hospital", RunBy = "Uday",RunType = "Triggered", ErrorMessage = "Error 2" },
                    new ErrorLogViewModel { TimeStamp = DateTime.Now, Segment = "Insurance", RunBy = "Ramesh",RunType = "Triggered", ErrorMessage = "Error 3" },
                    new ErrorLogViewModel { TimeStamp = DateTime.Now, Segment = "School", RunBy = "Scheduler Admin",RunType = "Triggered", ErrorMessage = "Error 4" },
                    new ErrorLogViewModel { TimeStamp = DateTime.Now, Segment = "Degree", RunBy = "Ramesh",RunType = "Triggered", ErrorMessage = "Error 5" },
                    new ErrorLogViewModel { TimeStamp = DateTime.Now, Segment = "Hospital", RunBy = "Scheduler Admin",RunType = "Triggered", ErrorMessage = "Error 6" },
                    new ErrorLogViewModel { TimeStamp = DateTime.Now, Segment = "School", RunBy = "Uday",RunType = "Triggered", ErrorMessage = "Error 7" }
                };

                // Return the data as JSON
                return Ok(errorLogs);
            }
            catch (Exception ex)
            {
                _responseViewModel.ExceptionMessage = ex.Message;
                return BadRequest(_responseViewModel);
            }
        }


    

        public class StatusViewModel
        {
            public string TaskId { get; set; }
            public DateTime TimeStamp { get; set; }
            public string Segment { get; set; }
            public string TotalRecords { get; set; }
            public string ProcessedRecords { get; set; }
            public string PercentageCompletion { get; set; }
        }

    }
}

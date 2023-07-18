using CodeMatcherV2Api.BusinessLayer;
using CodeMatcherV2Api.BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using CodeMatcherV2Api.Models;
using System.Net.Http;
using CodeMatcherV2Api.ApiRequestModels;
using CodeMatcherV2Api.Middlewares.HttpHelper;
using CodeMatcherV2Api.ApiResponeModel;
using Newtonsoft.Json;

namespace CodeMatcherV2Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduledRunController : BaseController
    {
        //private readonly ISchedule _schedule;
        //private readonly IHttpClientFactory _httpClientFactory;
        //public ScheduledRunController(ISchedule schedule,IHttpClientFactory httpClientFactory)
        //{
        //    _schedule = schedule;
        //    _httpClientFactory = httpClientFactory;
        //}

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

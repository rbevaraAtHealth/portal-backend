using CodeMatcher.Api.V2.Models;
using CodeMatcher.Api.V2.Models.SummaryModel;
using CodeMatcherV2Api.BusinessLayer.Interfaces;
using CodeMatcherV2Api.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CodeMatcherV2Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CodeMappingController : BaseController
    {
        private readonly ICodeMapping _codeMapping;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly CodeMatcherDbContext _context;

        public CodeMappingController(ICodeMapping codeMapping, IHttpClientFactory httpClientFactory, CodeMatcherDbContext context)
        {
            _codeMapping = codeMapping;
            _httpClientFactory = httpClientFactory;
            _context = context;
        }
        //[HttpGet, Route("GetCodeMappingRecords")]
        //public async Task<IActionResult> GetCodeMappingRecords()
        //{
        //    try
        //    {
        //        var records = await _codeMapping.GetCodeMappingsRecordsAsync();
        //        return Ok(records);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
        [HttpGet,Route("GetCodeMappingSummary")]
        public async Task<IActionResult> GetCodeMappingSummary()
        {
           // var summaryViewModel = _codeMapping.GetMappings(taskId);
            var summaryViewModel = _codeMapping.GetCodeMappings();
            return Ok(summaryViewModel);
        }

        [HttpPost,Route("UpdateSummary")]
        public async Task<IActionResult> UpdateSummary([FromBody] CodeMappingSummaryViewModel response)
        {
            try
            {
                 int summaryId = _codeMapping.SaveSummary(response.TaskId,response.Summary);

                if (summaryId == 0)
                    return Ok("Task Id not found");
                else
                    return Ok("Summary Saved");
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }
    }

}


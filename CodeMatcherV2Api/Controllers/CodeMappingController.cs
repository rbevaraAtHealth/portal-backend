using CodeMatcherV2Api.BusinessLayer.Interfaces;
using CodeMatcherV2Api.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
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
        //[HttpGet,Route("GetCodeMappingRecords")]
        //public async Task<IActionResult> GetCodeMappingRecords()
        //{
        //    try
        //    {
        //       var records = await _codeMapping.GetCodeMappingsRecordsAsync();
        //        //var mappings = _codeMapping.GetCodeGenerationMappings();
        //        //return Ok(mappings);
        //        return Ok(records);
        //    }
        //    catch(Exception ex) 
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
        [HttpGet, Route("CodeGenerationMappingRecords")]
        public async Task<IActionResult> GetCodeGenerationMapping()
        {
            try
            {
                var records = _codeMapping.GetCodeGenerationMappingRecords();
                return Ok(records);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet, Route("MonthlyEmbeddingsMappingRecords")]
        public async Task<IActionResult> GetMonthlyEmbedMapping()
        {
            try
            {
                var records = _codeMapping.GetMonthlyEmbeddingMappingRecords();
                return Ok(records);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet, Route("WeeklyEmbeddingsMappingRecords")]
        public async Task<IActionResult> GetWeeklyEmbedMapping()
        {
            try
            {
                var records = _codeMapping.GetWeeklyEmbeddingsMappingRecords();
                return Ok(records);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet, Route("ResultsforPendingIds")]
        public async Task<IActionResult> GetSummaryForPendingTasks()
        {
            _codeMapping.GetMappingsInProcessTasks();
            return Ok();
        }
        //[HttpGet,Route("GetCodeGenerationMappingByTaskId")]
        //public async Task<IActionResult> GetCgMappingRecordsByTaskId(Guid taskId)
        //{
        //    var response= _codeMapping.GetJobResult(taskId);
        //    var records=_codeMapping.GetCgMappingsPythApi(response,taskId);
        //    return Ok(records);
        //}
        //[HttpGet, Route("GetCodeGenerationMappingByTaskIdTest")]
        //public async Task<IActionResult> GetCgMappingRecordsByTaskIdTest(Guid taskId)
        //{

        //    var response = _codeMapping.GetJobResult(taskId);

        //   // foreach (var item in codeMapping)
        //    //{


        //        int requestId = SqlHelper.GetRequestId(taskId, _context);
        //        int frequncyId = SqlHelper.GetCodeMappingId(requestId, _context);

        //        switch (frequncyId)
        //        {
        //            case ((int)CodeMappingType.CodeGeneration):
        //                _codeMapping.GetCgMappingsPythApi(response, taskId);
        //                break;
        //            case ((int)CodeMappingType.MonthlyEmbeddings):
        //                _codeMapping.GetMonthlyEmbedMappingsPythApi(response, taskId);
        //                break;
        //            case ((int)CodeMappingType.WeeklyEmbeddings):
        //                _codeMapping.GetWeeklyEmbedMappingsPythApi(response, taskId);
        //                break;
        //            default: break;
        //        }

        //        var records = _codeMapping.GetCgMappingsPythApi(response, taskId);
        //        return Ok(records);
        //  //  }


    }

}


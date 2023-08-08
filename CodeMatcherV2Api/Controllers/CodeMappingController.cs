﻿using CodeMatcher.Api.V2.Models;
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
        
        [HttpGet,Route("GetCodeMappings")]
        public async Task<IActionResult> GetCodeMappings()
        {
            var summaryViewModel = _codeMapping.GetCodeMappings();
            return Ok(summaryViewModel);
        }
        [HttpGet,Route("CodeGeneration/GetCodeMappings")]
        public async Task<IActionResult> GetCodeGenerationCodeMappings()
        {
            var cgCodeMappings = _codeMapping.GetCodeGenerationMappingRecords();
            return Ok(cgCodeMappings);
        }
        [HttpGet, Route("MonthlyEmbedings/GetEmbeddings")]
        public async Task<IActionResult> GetMonthlyEmbedings()
        {
            var embeddings = _codeMapping.GetMonthlyEmbeddingMappingRecords();
            return Ok(embeddings); ;
        }
        [HttpGet, Route("WeeklyEmbeddings/GetEmbeddings")]
        public async Task<IActionResult> GetWeeklyEmbeddings()
        {
            var embeddings = _codeMapping.GetWeeklyEmbeddingsMappingRecords();
            return Ok(embeddings); ;
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


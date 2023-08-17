﻿using CodeMatcher.Api.V2.ApiResponseModel;
using CodeMatcher.Api.V2.BusinessLayer;
using CodeMatcher.Api.V2.Models;
using CodeMatcherV2Api.BusinessLayer.Interfaces;
using CodeMatcherV2Api.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
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
        private readonly ResponseViewModel _responseViewModel;

        public CodeMappingController(ICodeMapping codeMapping, IHttpClientFactory httpClientFactory, CodeMatcherDbContext context)
        {
            _codeMapping = codeMapping;
            _httpClientFactory = httpClientFactory;
            _context = context;
            _responseViewModel = new ResponseViewModel();
        }
      
        [HttpGet,Route("CodeGeneration/GetCodeMappings")]
        public async Task<IActionResult> GetCodeGenerationCodeMappings()
        {
            var cgCodeMappings = _codeMapping.GetCodeGenerationMappingRecords();
            _responseViewModel.Model = cgCodeMappings;
            return Ok(_responseViewModel);
        }
        [HttpGet, Route("MonthlyEmbedings/GetEmbeddings")]
        public async Task<IActionResult> GetMonthlyEmbedings()
        {
            var embeddings = _codeMapping.GetMonthlyEmbeddingMappingRecords();
            _responseViewModel.Model = embeddings;
            return Ok(_responseViewModel);
        }
        [HttpGet, Route("WeeklyEmbeddings/GetEmbeddings")]
        public async Task<IActionResult> GetWeeklyEmbeddings()
        {
            var embeddings = _codeMapping.GetWeeklyEmbeddingMappingRecords();
            _responseViewModel.Model = embeddings;
            return Ok(_responseViewModel);
        }
        [AllowAnonymous]
        [HttpPost,Route("UpdateSummary")]
        public async Task<IActionResult> UpdateSummary([FromBody] CodeMappingSummaryViewModel response)
        {
            try
            {
                 int summaryId = _codeMapping.SaveSummary(response.TaskId,response.Summary,GetUserInfo());

                if (summaryId == 0)
                {
                    _responseViewModel.Message = "Task Id not found";
                    return Ok(_responseViewModel);
                }
                else
                {
                    _responseViewModel.Message = "Summary Saved Successfully";
                    return Ok(_responseViewModel);
                }
            }
            catch (Exception ex) 
            {
                _responseViewModel.ExceptionMessage = ex.Message;
                return BadRequest(_responseViewModel);
            }
        }
    }

}


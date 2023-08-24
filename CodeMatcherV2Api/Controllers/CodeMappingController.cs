using CodeMatcher.Api.V2.ApiResponseModel;
using CodeMatcher.Api.V2.Models;
using CodeMatcherV2Api.BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CodeMatcherV2Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CodeMappingController : BaseController
    {
        private readonly ICodeMapping _codeMapping;
        private readonly ResponseViewModel _responseViewModel;

        public CodeMappingController(ICodeMapping codeMapping)
        {
            _codeMapping = codeMapping;
            _responseViewModel = new ResponseViewModel();
        }

        [HttpGet, Route("CodeGeneration/GetCodeMappings")]
        public async Task<IActionResult> GetCodeGenerationCodeMappings()
        {
            var cgCodeMappings = await _codeMapping.GetCodeGenerationMappingRecords(getClientId());
            _responseViewModel.Model = cgCodeMappings;
            return Ok(_responseViewModel);
        }
        [HttpGet, Route("MonthlyEmbedings/GetEmbeddings")]
        public async Task<IActionResult> GetMonthlyEmbedings()
        {
            var embeddings = await _codeMapping.GetMonthlyEmbeddingMappingRecords(getClientId());
            _responseViewModel.Model = embeddings;
            return Ok(_responseViewModel);
        }
        [HttpGet, Route("WeeklyEmbeddings/GetEmbeddings")]
        public async Task<IActionResult> GetWeeklyEmbeddings()
        {
            var embeddings = await _codeMapping.GetWeeklyEmbeddingMappingRecords(getClientId());
            _responseViewModel.Model = embeddings;
            return Ok(_responseViewModel);
        }
        [AllowAnonymous]
        [HttpPost, Route("UpdateSummary")]
        public async Task<IActionResult> UpdateSummary([FromBody] CodeMappingSummaryViewModel response)
        {
            try
            {
                //int summaryId = await _codeMapping.SaveSummary(response.TaskId, response.Summary, GetUserInfo());
                int summaryId = await _codeMapping.SaveSummary(response.TaskId, response.Summary.ToString(), GetUserInfo());

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
        [HttpGet, Route("CodeMappingRequestGetAll")]
        public async Task<IActionResult> CodeMappingReqResGetAll()
        {
            try
            {
                _responseViewModel.Model = await _codeMapping.GetCodeMappingRequestResponse();
                return Ok(_responseViewModel);
            }
            catch (Exception ex)
            {
                _responseViewModel.ExceptionMessage = ex.Message;
                return BadRequest(_responseViewModel);
            }
        }
    }

}


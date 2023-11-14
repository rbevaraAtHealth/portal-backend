using CodeMatcher.Api.V2.ApiResponseModel;
using CodeMatcher.Api.V2.Models;
using CodeMatcherV2Api.BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeMatcherV2Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CodeGenerationOverwriteController : BaseController
    {
        private readonly ICodeGenerationOverwrite _codegenerationoverwrite;
        private readonly ResponseViewModel _responseViewModel;
        public CodeGenerationOverwriteController(ICodeGenerationOverwrite codegenerationoverwrite)
        {
            _codegenerationoverwrite = codegenerationoverwrite;
            _responseViewModel = new ResponseViewModel();
        }

        [HttpGet, Route("GetCodeGenerationOverwrite")]
        public async Task<IActionResult> CodeGenerationOverwriteGet(string taskId)
        {
            try
            {
                if (taskId != null)
                {
                    var data = await _codegenerationoverwrite.CodeGenerationOverwritegetAsync(taskId, GetUserInfo(), getClientId());
                    if(data!= null && data.Count > 0)
                       _responseViewModel.Model = data;
                    else 
                        _responseViewModel.ExceptionMessage = "No records Found";
                    return Ok(_responseViewModel);
                }
                _responseViewModel.ExceptionMessage = "Task Id cannot be null";
                return BadRequest(_responseViewModel);
            }
            catch (Exception ex)
            {
                _responseViewModel.ExceptionMessage = ex.Message;
                return BadRequest(_responseViewModel);
            }
        }

        [HttpPost, Route("UpdateCodeGenerationOverwrite")]
        public async Task<IActionResult> UpdateCodeGenerationOverwrite([FromBody] List<CgOverwriteUpdateModel> updateModels, string taskId)
        {
            if (string.IsNullOrWhiteSpace(taskId) || updateModels == null)
            {
                _responseViewModel.Message = "Fields cannot be null";
                return BadRequest(_responseViewModel);
            }
            try
            {
                var result = await _codegenerationoverwrite.UpdateCGSourceDB(updateModels, getClientId());

                if (result)
                {
                    _responseViewModel.Message = "Record updated in source database";

                    var saveHistory = await _codegenerationoverwrite.UpdateCGDestinationDB(taskId, updateModels, getClientId());
                    if (saveHistory)
                        _responseViewModel.Message = "Record updated into source and inserted into destination sucessfully";
                    else
                        _responseViewModel.Message = "Record updated into source but something went wrong while saving to destination";
                }
                return Ok(_responseViewModel);
            }
            catch (Exception ex)
            {
                _responseViewModel.ExceptionMessage = ex.Message;
                return BadRequest(_responseViewModel);
            }
        }

        [HttpGet, Route("GetCodeGenerationOverwriteBaseData")]
        public async Task<IActionResult> GetCodeGenerationOverwriteBaseData( string taskId)
        {
            try
            {
                var baseData = await _codegenerationoverwrite.GetCGOverwriteBaseDataModel(GetUserInfo(), getClientId(), taskId);
                _responseViewModel.Model = baseData;
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

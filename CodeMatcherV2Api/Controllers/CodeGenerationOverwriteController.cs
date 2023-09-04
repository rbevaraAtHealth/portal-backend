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

        [HttpGet,Route("GetCodeGenerationOverwrite")]
        public async Task<IActionResult> CodeGenerationOverwriteGet(string taskId)
        {
            try
            {
                if (taskId != null)
                {
                    var data = await _codegenerationoverwrite.CodeGenerationOverwritegetAsync(taskId, GetUserInfo(), getClientId());
                    _responseViewModel.Model = JsonConvert.SerializeObject(data);
                    return Ok(_responseViewModel);
                }
                _responseViewModel.ExceptionMessage = "TaskId cann't be null";
                return BadRequest(_responseViewModel);
            }
            catch (Exception ex)
            {
                _responseViewModel.ExceptionMessage = ex.Message;
                return BadRequest(_responseViewModel);
            }
        }

        [HttpPost,Route("UpdateCodeGenerationOverwrite")]
        public async Task<IActionResult> UpdateCodeGenerationOverwrite([FromBody]List<CgOverwriteUpdateModel> updateModels, string taskId)
        {
            if(string.IsNullOrWhiteSpace(taskId) || updateModels==null)
            {
                _responseViewModel.Message = "Fields cann't be null";
                return BadRequest(_responseViewModel);
            }
            try
            {
              var result= await _codegenerationoverwrite.UpdateCGSourceDB(updateModels, getClientId());
                _responseViewModel.Message = "Record updated in source database";
                if (result)
                {
                    var saveHistory =await _codegenerationoverwrite.UpdateCGDestinationDB(taskId, updateModels, getClientId());
                    _responseViewModel.Message = "Record updated into source and inserted into destination sucessfully";
                }
                    return Ok(_responseViewModel);
            }
            catch (Exception ex)
            {
                _responseViewModel.ExceptionMessage=ex.Message;
                return BadRequest(_responseViewModel);
            }
        }
        
    }
}

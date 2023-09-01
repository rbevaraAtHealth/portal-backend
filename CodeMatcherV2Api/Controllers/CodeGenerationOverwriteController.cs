using CodeMatcher.Api.V2.ApiResponseModel;
using CodeMatcher.Api.V2.Models;
using CodeMatcherV2Api.BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost,Route("GetCodeGenerationOverwrite")]
        public async Task<IActionResult> CodeGenerationOverwriteGet([FromBody]string taskId)
        {
            try
            {
                if (taskId != null)
                {
                    var data = await _codegenerationoverwrite.CodeGenerationOverwritegetAsync(taskId, GetUserInfo(), getClientId());
                    _responseViewModel.Model = data;
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
                if (result)
                {
                    var saveHistory = _codegenerationoverwrite.UpdateCGDestinationDB(taskId, updateModels, getClientId());
                }
                _responseViewModel.Model = result;
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

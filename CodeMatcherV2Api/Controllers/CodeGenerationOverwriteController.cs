using CodeMatcher.Api.V2.ApiResponseModel;
using CodeMatcherApiV2.Common;
using CodeMatcherV2Api.BusinessLayer;
using CodeMatcherV2Api.BusinessLayer.Interfaces;
using CodeMatcherV2Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CodeMatcherV2Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CodeGenerationOverwriteController : ControllerBase
    {
        private readonly ICodeGenerationOverwrite _codegenerationoverwrite;
        private readonly ResponseViewModel _responseViewModel;
        public CodeGenerationOverwriteController(ICodeGenerationOverwrite codegenerationoverwrite)
        {
            _codegenerationoverwrite = codegenerationoverwrite;
            _responseViewModel = new ResponseViewModel();
        }

        [HttpGet("GetAllCodeGenerationOverwrite")]
        public async Task<IActionResult> GetAllCodeGenerationOverwrite()
        {
            try
            {
                var data = await _codegenerationoverwrite.GetAllCodeGenerationOverwriteAsync();
                _responseViewModel.Model = data;
                return Ok(_responseViewModel);
            }
            catch (Exception ex)
            {
                _responseViewModel.ExceptionMessage = ex.Message;
                return BadRequest(_responseViewModel);
            }
        }
        [HttpGet("GetCodeGenerationOverwriteById/{id}")]
        public async Task<IActionResult> GetCodeGenerationOverwriteByIdAsync(int id)
        {
            try
            {
                var data = await _codegenerationoverwrite.GetCodeGenerationOverwriteByIdAsync(id);
                _responseViewModel.Model = data;
                return Ok(_responseViewModel);
            }
            catch (Exception ex)
            {
                _responseViewModel.ExceptionMessage = ex.Message;
                return BadRequest(_responseViewModel);
            }
        }
        
        [HttpPut, Route("UpdateCodeGenerationOverwrite")]
        public async Task<IActionResult> UpdateCodeGenerationOverwrite([FromBody] CodeGenerationOverwriteModel model)
        {
            try
            {
                var data = await _codegenerationoverwrite.UpdateCodeGenerationOverwriteAsync(model);
                _responseViewModel.Model = data;
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

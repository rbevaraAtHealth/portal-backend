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
        public CodeGenerationOverwriteController(ICodeGenerationOverwrite codegenerationoverwrite)
        {
            _codegenerationoverwrite = codegenerationoverwrite;
        }

        [HttpGet("GetAllCodeGenerationOverwrite")]
        public async Task<IActionResult> GetAllCodeGenerationOverwrite()
        {
            try
            {
                var data = await _codegenerationoverwrite.GetAllCodeGenerationOverwriteAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetCodeGenerationOverwriteByIdAsync/{id}")]
        public async Task<IActionResult> GetCodeGenerationOverwriteByIdAsync(int id)
        {
            try
            {
                var data = await _codegenerationoverwrite.GetCodeGenerationOverwriteByIdAsync(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPut, Route("UpdateCodeGenerationOverwrite")]
        public async Task<IActionResult> UpdateCodeGenerationOverwrite([FromBody] CodeGenerationOverwriteModel model)
        {
            try
            {
                var data = await _codegenerationoverwrite.UpdateCodeGenerationOverwriteAsync(model);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
    }
}

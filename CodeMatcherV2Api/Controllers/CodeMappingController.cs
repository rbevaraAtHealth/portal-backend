using CodeMatcherV2Api.BusinessLayer.Interfaces;
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
        public CodeMappingController(ICodeMapping codeMapping)
        {
            _codeMapping = codeMapping;
        }
        [HttpGet,Route("GetCodeMappingRecords")]
        public async Task<IActionResult> GetCodeMappingRecords()
        {
            try
            {
                var records =await _codeMapping.GetCodeMappingsRecordsAsync();
                return Ok(records);
            }
            catch(Exception ex) 
            {
                return BadRequest(ex);
            }
        }

    }
}

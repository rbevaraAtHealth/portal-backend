using CodeMatcherV2Api.BusinessLayer;
using CodeMatcherV2Api.BusinessLayer.Interfaces;
using CodeMatcherV2Api.Models;
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
       // private readonly ResponseViewModel _responseView;
        private object _responseViewModel;

        public CodeMappingController(ICodeMapping codeMapping)
        {
            _codeMapping = codeMapping;
          //  _responseView = new ResponseViewModel();
        }
        [HttpGet,Route("GetCodeMappingRecords")]
        public async Task<IActionResult> GetCodeMappingRecords()
        {
            try
            {
               var records = await _codeMapping.GetCodeMappingsRecordsAsync();
                return Ok(records);
            }
            catch(Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

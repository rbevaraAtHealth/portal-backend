using CodeMatcher.Api.V2.Models;
using CodeMatcherV2Api.BusinessLayer.Interfaces;
using CodeMatcherV2Api.EntityFrameworkCore;
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

        public CodeMappingController(ICodeMapping codeMapping, IHttpClientFactory httpClientFactory, CodeMatcherDbContext context)
        {
            _codeMapping = codeMapping;
            _httpClientFactory = httpClientFactory;
            _context = context;
        }
        
        [HttpPost,Route("UpdateSummary")]
        public async Task<IActionResult> UpdateSummary([FromBody]CodeMappingSummaryViewModel viewModel)
        {
            try
            {
                  int summaryId = _codeMapping.SaveSummary(viewModel.TaskId,viewModel.Summary);
                return Ok(summaryId);
                //return Ok();
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }
    }

}


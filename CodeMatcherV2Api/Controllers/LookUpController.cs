using CodeMatcherV2Api.Models;
using Gremlin.Net.Driver.Messages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeMatcherV2Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LookUpController : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> GetSegment()
        {
            List<string> segments = new List<string>();
            segments.Add("Insurance");
            segments.Add("School");
            segments.Add("Insurance");
            segments.Add("Hospital");
            segments.Add("State License");
            ResponseResult responseResult = new ResponseResult();
            responseResult.Code = 200;
            responseResult.Message = "Get Segments Clicked";
            responseResult.Data = segments;

            return Ok(segments);

        }
    }
}

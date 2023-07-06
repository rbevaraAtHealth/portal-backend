using CodeMatcherV2Api.BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CodeMatcherV2Api.Controllers
{
    [Route("api/[controller]")]
    public class LookUpController : BaseController
    {
        private readonly ILookUp _lookUp;
        public LookUpController(ILookUp lookUp)
        {
            _lookUp = lookUp;
        }

        [HttpGet]
        public async Task<IActionResult> GetSegment(string lookupType)
        {
            try
            {
                var segments =await _lookUp.GetLookupsAsync(lookupType);
                return Ok(segments);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}

using CodeMatcherV2Api.BusinessLayer.Interfaces;
using CodeMatcherV2Api.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeMatcherV2Api.Controllers
{
    [Route("api/[controller]")]
    public class LookUpController : BaseController
    {
        private readonly ILookUp _lookUp;
        private readonly ILookupTypes _lookupTypes;
        public LookUpController(ILookUp lookUp, ILookupTypes lookupTypes)
        {
            _lookUp = lookUp;
            _lookupTypes = lookupTypes; 
        }

        [HttpGet,Route("GetLookups")]
        public async Task<IActionResult> GetLookups(string lookupType)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(lookupType))
                    throw new ArgumentNullException("LookType cann't be null",nameof(lookupType));
                LookupTypeModel lookupdto =await _lookupTypes.GetLookupByNameAsync(lookupType);
                if(lookupdto == null)
                    throw new ArgumentNullException("No Looktype found", nameof(lookupType));
                var lookupsdto =await _lookUp.GetLookupByIdAsync(lookupdto.LookupTypeId);
                List<string> lookups = new List<string>();
                foreach (var item in lookupsdto)
                {
                    lookups.Add(item.Name);
                }
                return Ok(lookups);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}

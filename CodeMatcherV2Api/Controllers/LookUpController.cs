using CodeMatcher.Api.V2.ApiResponseModel;
using CodeMatcherV2Api.BusinessLayer.Interfaces;
using CodeMatcherV2Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeMatcherV2Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LookUpController : BaseController
    {
        private readonly ILookUp _lookUp;
        private readonly ILookupTypes _lookupTypes;
        private readonly ResponseViewModel _responseViewModel;
        public LookUpController(ILookUp lookUp, ILookupTypes lookupTypes)
        {
            _lookUp = lookUp;
            _lookupTypes = lookupTypes;
            _responseViewModel = new ResponseViewModel();
        }

        [HttpGet,Route("GetLookups")]
        public async Task<IActionResult> GetLookups(string lookupType)
        {
            try
            {
                var userInfo = GetUserInfo();
                if (string.IsNullOrWhiteSpace(lookupType))
                    throw new ArgumentNullException("Lookup Type cannot be null",nameof(lookupType));
                LookupTypeModel lookupTypes = _lookupTypes.GetLookupByNameAsync(lookupType);
                if(lookupTypes == null)
                    throw new ArgumentNullException("Lookup type not found", nameof(lookupType));
                var lookups = await _lookUp.GetLookupByIdAsync(lookupTypes.LookupTypeId);
                _responseViewModel.Model = lookups;
                return Ok(_responseViewModel);
            }
            catch(Exception ex)
            {
                _responseViewModel.ExceptionMessage = ex.Message;
                return BadRequest(_responseViewModel);
            }

        }


        [HttpGet, Route("GetAllLookups")]
        public async Task<IActionResult> GetAllLookups()
        {
            try
            {
                var userInfo = GetUserInfo();
                var lookups = await _lookUp.GetLookupsAsync();
                _responseViewModel.Model = lookups;
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

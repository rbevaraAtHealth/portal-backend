﻿using CodeMatcherV2Api.BusinessLayer.Interfaces;
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
                if (string.IsNullOrWhiteSpace(lookupType))
                    throw new ArgumentNullException("Lookup Type cannot be null",nameof(lookupType));
                LookupTypeModel lookupTypes =await _lookupTypes.GetLookupByNameAsync(lookupType);
                if(lookupTypes == null)
                    throw new ArgumentNullException("Looktype not found", nameof(lookupType));
                var lookups =await _lookUp.GetLookupByIdAsync(lookupTypes.LookupTypeId);                
                _responseViewModel.Model = lookups;
            }
            catch(Exception ex)
            {
                _responseViewModel.ExceptionMessage = ex.Message;
            }
            return Ok(_responseViewModel);
        }
    }
}

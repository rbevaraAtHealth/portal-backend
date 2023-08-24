using CodeMatcher.Api.V2.ApiResponseModel;
using CodeMatcherV2Api.BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CodeMatcherV2Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LookUpController : BaseController
    {
        private readonly ILookUp _lookUp;
        private readonly ResponseViewModel _responseViewModel;
        public LookUpController(ILookUp lookUp)
        {
            _lookUp = lookUp;
            _responseViewModel = new ResponseViewModel();
        }

        [HttpGet,Route("GetLookups")]
        public async Task<IActionResult> GetLookups(string lookupType)
        {
            try
            {
                var userInfo = GetUserInfo();
                var lookups = await _lookUp.GetLookupsByName(lookupType);
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
        [HttpGet, Route("GetDBConnectionString")]
        public IActionResult GetDBConnectionString()
        {
            try
            {
                _responseViewModel.Message = _lookUp.GetDBConnectionString();
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

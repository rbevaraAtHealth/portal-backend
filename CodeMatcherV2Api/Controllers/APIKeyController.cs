using ADODB;
using CodeMatcher.Api.V2.ApiResponseModel;
using CodeMatcher.Api.V2.BusinessLayer.Interfaces;
using CodeMatcher.Api.V2.Middlewares.CommonHelper;
using CodeMatcher.Api.V2.Models;
using CodeMatcherV2Api.BusinessLayer;
using CodeMatcherV2Api.BusinessLayer.Interfaces;
using CodeMatcherV2Api.Controllers;
using CodeMatcherV2Api.Middlewares.HttpHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CodeMatcher.Api.V2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIKeyController : BaseController
    {
        private readonly IApiKey _apiKey;
        private readonly ResponseViewModel _responseViewModel;
        private readonly ApiKeyHelper _apiKeyHelper;
        public APIKeyController(IApiKey apiKey)
        {
            _apiKey = apiKey;
            _responseViewModel = new ResponseViewModel();
            _apiKeyHelper = new ApiKeyHelper();
        }

        [AllowAnonymous]
        [HttpGet, Route("GetAllApiKeys")]
        public async Task<IActionResult> GetAllApiKeys()
        {
            try
            {
                var result = await _apiKey.GetAllApiKeysRecords();
                _responseViewModel.Model = result;
                return Ok(_responseViewModel);

            }
            catch (Exception ex)
            {
                _responseViewModel.ExceptionMessage = ex.Message;
                return BadRequest(_responseViewModel);
            }

        }

        [AllowAnonymous]
        [HttpPost, Route("CreateApiKey")]
        public async Task<IActionResult> CreateApiKey(APIKeyModel apiKey)
        {
            try
            {
                bool validateApiKey = await _apiKeyHelper.ValidateApiKey(apiKey.Api_Key);
                if (validateApiKey)
                {
                    var user = GetUserInfo();
                    var requestModel = await _apiKey.CreateNewApiKey(apiKey, user, getClientId());
                    _responseViewModel.Model = requestModel;
                    return Ok(_responseViewModel);
                }
                else
                {
                    _responseViewModel.Message = "Invalid API key.";
                    return Ok(_responseViewModel);
                }
            }
            catch (Exception ex)
            {
                _responseViewModel.ExceptionMessage = ex.Message;
                return BadRequest(_responseViewModel);
            }
        }

        [AllowAnonymous]
        [HttpDelete, Route("DeleteApiKey")]
        public async Task<IActionResult> DeleteApiKey(string apiKey)
        {
            try
            {
                // var user = GetUserInfo();
                var requestModel = await _apiKey.DeleteApiKey(apiKey);
                _responseViewModel.Model = requestModel;
                return Ok(_responseViewModel);
            }
            catch (Exception ex)
            {
                _responseViewModel.ExceptionMessage = ex.Message;
                return BadRequest(_responseViewModel);
            }
        }

        [AllowAnonymous]
        [HttpPost, Route("ValidateApiKey")]
        public async Task<bool> ValidateApiKey(APIKeyModel apiKey)
        {
            try
            {
                bool validateApiKey = await _apiKeyHelper.ValidateApiKey(apiKey.Api_Key);
                if (validateApiKey)
                {
                    //var user = GetUserInfo();
                    //var requestModel = await _apiKey.CreateNewApiKey(apiKey, user, getClientId());
                    _responseViewModel.Message = "Valid Api Key";
                    //return Ok(_responseViewModel);
                    return true;
                }
                else
                {
                    //_responseViewModel.Message = "Invalid API key.";
                    //return Ok(_responseViewModel);
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}

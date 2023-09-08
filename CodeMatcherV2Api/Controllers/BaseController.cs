using CodeMatcher.Api.V2.ApiResponseModel;
using CodeMatcher.Api.V2.Middlewares.CommonHelper;
using CodeMatcherV2Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System;
using System.Security.Claims;

namespace CodeMatcherV2Api.Controllers
{

    [Authorize]
    public class BaseController : ControllerBase
    {
        //private readonly IHttpContextAccessor _httpContextAccessor;
        public BaseController()
        {
        }
        [NonAction]
        public LoginModel GetUserInfo()
        {
            if (User != null)
                return new LoginModel() { UserName= User.FindFirstValue(ClaimTypes.Name),
                    Role= User.FindFirstValue(ClaimTypes.Role)
                };
            return new LoginModel();
        }
        internal string getClientId()
        {
            const string HeaderKeyName = "ClientID";
            if(Request != null && Request.Headers.Count > 0)
            {
                Request.Headers.TryGetValue(HeaderKeyName, out StringValues headerValue);
                try
                {
                    headerValue = CommonHelper.Decrypt(headerValue);
                }
                catch (Exception ex)
                {
                    
                }
                return headerValue;
            }
            return "No Client";
        }
    }
}

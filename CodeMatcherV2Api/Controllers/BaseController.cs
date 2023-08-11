using CodeMatcherV2Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
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
            Request.Headers.TryGetValue(HeaderKeyName, out StringValues headerValue);
            return headerValue;
        }
    }
}

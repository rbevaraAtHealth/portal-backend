using CodeMatcherV2Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CodeMatcherV2Api.Controllers
{
   
    [Authorize]
    public class BaseController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BaseController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        [NonAction]
        public LoginModel GetUserInfo()
        {
            if (_httpContextAccessor?.HttpContext?.User != null)
                return new LoginModel() { UserName= _httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.Name)};
            return new LoginModel();
        }
    }
}

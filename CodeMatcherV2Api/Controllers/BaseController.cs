using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeMatcherV2Api.Controllers
{
    [ApiController]
    [Authorize]
    public class BaseController : ControllerBase
    {
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodeMatcher.Api.V2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppVersionController : ControllerBase
    {
        [HttpGet, Route("GetCurrentAppVersion")]
        public string GetVersion()
        {
            string path = "version.txt";
            if (System.IO.File.Exists(path))
            {
                return System.IO.File.ReadAllText(path);
            }
            return "File not found";
        }
    }
}

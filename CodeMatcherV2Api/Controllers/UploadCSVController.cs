using CodeMatcherV2Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using CodeMatcherV2Api.BusinessLayer.Interfaces;

namespace CodeMatcherV2Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadCSVController : ControllerBase
    {
        private readonly IUploadCSV _Upload;
        public UploadCSVController(IUploadCSV upload)
        {
            _Upload = upload;
        }

        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {

            if (CheckIfCSVFile(file))
            {
                var fileUploader = await _Upload.WriteFile(file);
                return Ok(fileUploader);
            }
            else
            {
                return BadRequest(new { message = "Invlaid File Extension" });

            }
        }

        [HttpPost("UploadCsv")]
        public async Task<IActionResult> UploadCsv([FromBody] UploadModel upload)
        {
            try
            {
                var uploadModel = await _Upload.GetUploadCSVAsync(upload);
                return Ok(uploadModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        private bool CheckIfCSVFile(IFormFile file)
        {
            var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
            return (extension == ".csv" || extension == ".CSV");
        }

    }
}

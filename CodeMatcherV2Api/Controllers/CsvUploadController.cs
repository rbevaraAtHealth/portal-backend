using CodeMatcherV2Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using CodeMatcherV2Api.BusinessLayer.Interfaces;
using CodeMatcherV2Api.Middlewares.HttpHelper;
using System.Net.Http;

namespace CodeMatcherV2Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CsvUploadController : ControllerBase
    {
        private readonly ICsvUpload _Upload;
        private readonly IHttpClientFactory _httpClientFactory;
        public CsvUploadController(ICsvUpload upload,IHttpClientFactory httpClientFactory)
        {
            _Upload = upload;
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {

            if (CheckIfCsvFile(file))
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
        public async Task<IActionResult> UploadCsv([FromBody] CgCsvUploadModel upload)
        {
            try
            {
                var requestModel = _Upload.CgUploadCsvRequestGet(upload);
                var url = "code-generation/csv-upload";
                var response =await HttpHelper.Post_HttpClient(_httpClientFactory, requestModel, url);
                var responseModel = _Upload.CgUploadSaveResponse(response);
                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }
        private bool CheckIfCsvFile(IFormFile file)
        {
            var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
            return (extension == ".csv" || extension == ".CSV");
        }

    }
}

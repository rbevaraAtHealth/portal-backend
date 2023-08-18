using CodeMatcherV2Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using CodeMatcherV2Api.BusinessLayer.Interfaces;
using CodeMatcherV2Api.Middlewares.HttpHelper;
using System.Net.Http;
using CodeMatcher.Api.V2.ApiResponseModel;

namespace CodeMatcherV2Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CsvUploadController : BaseController
    {
        private readonly ICsvUpload _Upload;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ResponseViewModel _responseViewModel;
        public CsvUploadController(ICsvUpload upload, IHttpClientFactory httpClientFactory)
        {
            _Upload = upload;
            _httpClientFactory = httpClientFactory;
            _responseViewModel = new ResponseViewModel();
        }

        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {

            if (CheckIfCsvFile(file))
            {
                var fileUploader = await _Upload.WriteFile(file);
                _responseViewModel.Model = fileUploader;
                return Ok(_responseViewModel);
            }
            else
            {
                _responseViewModel.ExceptionMessage = "Invalid File Extension";
                return BadRequest(_responseViewModel);

            }
        }

        [HttpPost("UploadCsv")]
        public async Task<IActionResult> UploadCsv([FromBody] CgCsvUploadModel upload)
        {
            try
            {
                var requestModel = await _Upload.CgUploadCsvRequestGet(upload,GetUserInfo(),getClientId());
                var url = "code-generation/csv-upload";
                var response = await HttpHelper.Post_HttpClient(_httpClientFactory, requestModel.Item1, url);
                var responseModel = _Upload.CgUploadSaveResponse(response, requestModel.Item2, GetUserInfo());
                _responseViewModel.Model = responseModel;
                return Ok(_responseViewModel);
            }
            catch (Exception ex)
            {
                _responseViewModel.ExceptionMessage = ex.Message;
                return BadRequest(_responseViewModel);
            }

        }
        private bool CheckIfCsvFile(IFormFile file)
        {
            var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
            return (extension == ".csv" || extension == ".CSV");
        }
    }
}

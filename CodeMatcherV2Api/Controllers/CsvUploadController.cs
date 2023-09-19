using CodeMatcherV2Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using CodeMatcherV2Api.BusinessLayer.Interfaces;
using CodeMatcherV2Api.Middlewares.HttpHelper;
using System.Net.Http;
using CodeMatcher.Api.V2.ApiResponseModel;
using CodeMatcher.Api.V2.RepoModelAdapter;
using CodeMatcherV2Api.ApiResponseModel;
using CodeMatcherV2Api.Middlewares.SqlHelper;
using Newtonsoft.Json;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace CodeMatcherV2Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CsvUploadController : BaseController
    {
        private readonly ICsvUpload _Upload;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ResponseViewModel _responseViewModel;
        private readonly IConfiguration _configuration;
        public CsvUploadController(ICsvUpload upload, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _Upload = upload;
            _httpClientFactory = httpClientFactory;
            _responseViewModel = new ResponseViewModel();
            _configuration = configuration;
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
                var responseModel = await _Upload.CgUploadSaveResponse(response, requestModel.Item2, GetUserInfo());
                
                _responseViewModel.Model = responseModel;
                return Ok(_responseViewModel);
            }
            catch (Exception ex)
            {
                _responseViewModel.ExceptionMessage = ex.Message;
                return BadRequest(_responseViewModel);
            }
        }
        [HttpGet("DownloadFile")]
        public async Task<IActionResult> DownloadFile([FromQuery] string taskId)
        {
            var data = await _Upload.GetCsvOutputPath(taskId);
            if (data == null)
            {
                _responseViewModel.ExceptionMessage = "Task id is not found";
                return BadRequest(_responseViewModel);
            }
            else if (data != null && string.IsNullOrEmpty(data.UploadCsvOutputDirPath))
            {
                _responseViewModel.ExceptionMessage = @$"Output path is not found for taskId - {taskId}";
                return BadRequest(_responseViewModel);
            }
            string dirName = data.UploadCsvOutputDirPath.Replace(_configuration["AzureFileStorage:OutputDirPath"], string.Empty);
            string rootDirectory = dirName, zipFileName=dirName;
            if (dirName.Contains("/"))
            {
                zipFileName = dirName.Split('/').Last();
                rootDirectory = dirName.Split('/').First();
            }
            var filesInfo = await _Upload.DownloadFile(rootDirectory,zipFileName);
            byte[] zipBytesArr= await _Upload.FilesToZip(filesInfo.shareFiles,filesInfo.fileNames);
            if (zipBytesArr!=null)
            {
                return new FileContentResult(zipBytesArr.ToArray(), "application/zip") { FileDownloadName = @$"{zipFileName}.zip" };
            }
            return NoContent();
        }
        private bool CheckIfCsvFile(IFormFile file)
        {
            var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
            return (extension == ".csv" || extension == ".CSV");
        }
    }
}

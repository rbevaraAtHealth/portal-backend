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
        private readonly ResponseViewModel _responseViewModel;
        public UploadCSVController(IUploadCSV upload)
        {
            _Upload = upload;
            _responseViewModel = new ResponseViewModel();
        }

        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (CheckIfCSVFile(file))
            {
                var fileUploader = await _Upload.WriteFile(file);
                _responseViewModel.Message = fileUploader;                
            }
            else
            {
                _responseViewModel.ExceptionMessage = "Invlaid File Extension";
            }
            return Ok(_responseViewModel);
        }

        [HttpPost("UploadCsv")]
        public async Task<IActionResult> UploadCsv([FromBody] UploadModel upload)
        {
            try
            {
                var uploadModel = await _Upload.GetUploadCSVAsync(upload);                
                _responseViewModel.Message = uploadModel;                
            }
            catch (Exception ex)
            {
                _responseViewModel.ExceptionMessage = ex.Message;
            }
            return Ok(_responseViewModel);
        }

        private bool CheckIfCSVFile(IFormFile file)
        {
            var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
            return (extension == ".csv" || extension == ".CSV");
        }
    }
}

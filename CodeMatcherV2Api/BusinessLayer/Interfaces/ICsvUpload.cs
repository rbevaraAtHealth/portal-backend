using CodeMatcherV2Api.ApiRequestModels;
using CodeMatcherV2Api.ApiResponseModel;
using CodeMatcherV2Api.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CodeMatcherV2Api.BusinessLayer.Interfaces
{
    public interface ICsvUpload
    {
        //public Task<string> GetUploadCsvAsync(CgCsvUploadModel uploads);
        public Task<string> WriteFile(IFormFile file);
        public CgUploadCsvReqModel CgUploadCsvRequestGet(CgCsvUploadModel csvUpload);
        public CgUploadCsvResModel CgUploadSaveResponse(HttpResponseMessage httpResponse);
    }
}

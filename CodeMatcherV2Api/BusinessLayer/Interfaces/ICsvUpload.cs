using CodeMatcherV2Api.ApiRequestModels;
using CodeMatcherV2Api.ApiResponseModel;
using CodeMatcherV2Api.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CodeMatcherV2Api.BusinessLayer.Interfaces
{
    public interface ICsvUpload
    {
        public Task<string> WriteFile(IFormFile file);
        public Task<Tuple<CgUploadCsvReqModel, int>> CgUploadCsvRequestGet(CgCsvUploadModel csvUpload, LoginModel user,string clientId);
        public Task<CgUploadCsvResModel> CgUploadSaveResponse(HttpResponseMessage httpResponse, int requestId, LoginModel user);
        public Task<bool> DownloadFile(string tokenId);
    }
}

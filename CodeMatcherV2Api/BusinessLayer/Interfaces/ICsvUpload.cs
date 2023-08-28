﻿using Azure.Storage.Files.Shares.Models;
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
        Task<(List<ShareFileDownloadInfo> shareFiles, List<string> fileNames)> DownloadFile(string dirName);
        Task<byte[]> FilesToZip(List<ShareFileDownloadInfo> files, List<string> fileNames);
    }
}

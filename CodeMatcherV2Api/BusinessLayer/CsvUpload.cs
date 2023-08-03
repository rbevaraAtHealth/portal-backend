using AutoMapper;
using Azure;
using Azure.Storage.Files.Shares;
using CodeMatcherV2Api.ApiRequestModels;
using CodeMatcherV2Api.ApiResponseModel;
using CodeMatcherV2Api.BusinessLayer.Interfaces;
using CodeMatcherV2Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace CodeMatcherV2Api.BusinessLayer
{
    public class CsvUpload : ICsvUpload
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public CsvUpload(IMapper mapper, IConfiguration configuration)
        {
            _mapper = mapper;
            _configuration = configuration;
        }

        public CgUploadCsvReqModel CgUploadCsvRequestGet(CgCsvUploadModel csvUpload)
        {
            CgUploadCsvReqModel requestModel = new CgUploadCsvReqModel();
            requestModel.CsvInput = csvUpload.CsvFilePath;
            requestModel.Threshold = csvUpload.Threshold;
            requestModel.Segment = csvUpload.Segment;
            return requestModel;

        }

        public CgUploadCsvResModel CgUploadSaveResponse(HttpResponseMessage httpResponse)
        {
            CgUploadCsvResModel response = new CgUploadCsvResModel();
            if (httpResponse.IsSuccessStatusCode)
            {
                var httpResult = httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                if (!string.IsNullOrWhiteSpace(httpResult))
                    response = JsonConvert.DeserializeObject<CgUploadCsvResModel>(httpResult);
            }
            return response;

        }
        public async Task<string> WriteFile(IFormFile file)
        {
            try
            {
                return await WriteFiletoAzFileShare(file);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<string> WriteFiletoAzFileShare(IFormFile file)
        {
            string connectionString = _configuration["AzureFileStorage:ConnectionString"];

            string shareName = _configuration["AzureFileStorage:ShareName"];
            string dirName = _configuration["AzureFileStorage:DirectoryName"];
            string fileName = $"{DateTime.Now.Ticks}_{file.FileName}";

            ShareClient share = new(connectionString, shareName);
            share.CreateIfNotExists();

            ShareDirectoryClient directory = share.GetDirectoryClient(dirName);
            directory.CreateIfNotExists();

            // Get a reference to a file and upload it
            ShareFileClient _file = directory.GetFileClient(fileName);
            using (Stream stream = file.OpenReadStream())
            {
                await _file.CreateAsync(stream.Length);
                _file.UploadRange(
                    new HttpRange(0, stream.Length),
                    stream);
            }
            return fileName;
        }

    }
}

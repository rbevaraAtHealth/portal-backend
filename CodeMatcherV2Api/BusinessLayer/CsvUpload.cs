//using AutoMapper;
//using Azure;
//using Azure.Storage.Files.Shares;
//using CodeMatcherV2Api.ApiRequestModels;
//using CodeMatcherV2Api.ApiResponseModel;
//using CodeMatcherV2Api.BusinessLayer.Interfaces;
//using CodeMatcherV2Api.Models;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Configuration;
//using Newtonsoft.Json;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using System.Net.Http;
//using System.Runtime.Serialization.Formatters.Binary;
//using System.Threading.Tasks;

//namespace CodeMatcherV2Api.BusinessLayer
//{
//    public class CsvUpload : ICsvUpload
//    {
//        private readonly IMapper _mapper;
//        private readonly IConfiguration _configuration;
//        public CsvUpload(IMapper mapper, IConfiguration configuration)
//        {
//            _mapper = mapper;
//            _configuration = configuration;
//        }

//        public CgUploadCsvReqModel CgUploadCsvRequestGet(CgCsvUploadModel csvUpload)
//        {
//            CgUploadCsvReqModel requestModel = new CgUploadCsvReqModel();
//            requestModel.CsvInput = csvUpload.CsvFilePath;
//            requestModel.Threshold = csvUpload.Threshold;
//            requestModel.Segment = csvUpload.Segment;
//            return requestModel;

//        }

//        public CgUploadCsvResModel CgUploadSaveResponse(HttpResponseMessage httpResponse)
//        {
//            CgUploadCsvResModel response = new CgUploadCsvResModel();
//            if (httpResponse.IsSuccessStatusCode)
//            {
//                var httpResult = httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
//                if (!string.IsNullOrWhiteSpace(httpResult))
//                    response = JsonConvert.DeserializeObject<CgUploadCsvResModel>(httpResult);
//            }
//            return response;

//        }
//        public async Task<string> WriteFile(IFormFile file)
//        {
//            try
//            {
//                return await WriteFiletoAzFileShare(file);
//            }
//            catch (Exception ex)
//            {
//                throw;
//            }
//        }

//        public async Task<string> WriteFiletoAzFileShare(IFormFile file)
//        {
//            string connectionString = _configuration["AzureFileStorage:ConnectionString"];

//            string shareName = _configuration["AzureFileStorage:ShareName"];
//            string inputdirName = _configuration["AzureFileStorage:InputDirPath"];
//            string outputDirName = _configuration["AzureFileStorage:OutputDirPath"];
//            string fileName = $"{DateTime.Now.Ticks}_{file.FileName}";

//            ShareClient share = new(connectionString, shareName);
//            share.CreateIfNotExists();

//            ShareDirectoryClient directory = share.GetDirectoryClient(inputdirName);
//            directory.CreateIfNotExists();

//            // Get a reference to a file and upload it
//            ShareFileClient _file = directory.GetFileClient(fileName);
//            using (Stream stream = file.OpenReadStream())
//            {
//                await _file.CreateAsync(stream.Length);
//                _file.UploadRange(
//                    new HttpRange(0, stream.Length),
//                    stream);
//            }
//            return $"{outputDirName}{inputdirName}/{fileName}";
//        }

//    }

using AutoMapper;
using Azure;
using Azure.Core;
using Azure.Storage.Files.Shares;
using CodeMappingEfCore.DatabaseModels;
using CodeMatcher.Api.V2.BusinessLayer.Enums;
//using CodeMatcher.Api.V2.Middlewares.SqlHelper;
using CodeMatcherV2Api.ApiRequestModels;
using CodeMatcherV2Api.ApiResponseModel;
using CodeMatcherV2Api.BusinessLayer.Enums;
using CodeMatcherV2Api.BusinessLayer.Interfaces;
using CodeMatcherV2Api.EntityFrameworkCore;
using CodeMatcherV2Api.Middlewares.SqlHelper;
using CodeMatcherV2Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace CodeMatcherV2Api.BusinessLayer
{
    public class CsvUpload : ICsvUpload
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly CodeMatcherDbContext _context;
       // private readonly DatabaseHelper _databaseHelper;
        public CsvUpload(IMapper mapper, IConfiguration configuration, CodeMatcherDbContext context)//, DatabaseHelper databaseHelper)
        {
            _mapper = mapper;
            _configuration = configuration;
            _context = context;
            //_databaseHelper = databaseHelper;
        }

        public Tuple<CgUploadCsvReqModel, int> CgUploadCsvRequestGet(CgCsvUploadModel csvUpload, LoginModel user, string clientId)
        {
            CodeMappingRequestDto codeMappingRequestDto = new CodeMappingRequestDto();
            codeMappingRequestDto.RunTypeId = SqlHelper.GetLookupType((int)RequestType.UploadCsv,_context);
            codeMappingRequestDto.SegmentTypeId = SqlHelper.GetLookupType(csvUpload.Segment, _context);
            codeMappingRequestDto.CodeMappingId = SqlHelper.GetLookupType((int)CodeMappingType.CodeGeneration, _context);
            foreach (var item in csvUpload.Threshold)
            {
                if (codeMappingRequestDto.Threshold == null)
                    codeMappingRequestDto.Threshold = item;
                codeMappingRequestDto.Threshold = codeMappingRequestDto.Threshold + "," + item;
            }
            codeMappingRequestDto.LatestLink = "1";
            codeMappingRequestDto.ClientId = "All";
            codeMappingRequestDto.CsvFilePath = csvUpload.CsvFilePath;
            codeMappingRequestDto.CreatedBy = user.UserName;
            codeMappingRequestDto.ClientId= clientId;
            int reuestId = SqlHelper.SaveCodeMappingRequest(codeMappingRequestDto,_context);
            CgUploadCsvReqModel requestModel = new CgUploadCsvReqModel();
            requestModel.CsvInput = csvUpload.CsvFilePath;
            requestModel.Threshold = csvUpload.Threshold;
            requestModel.Segment = csvUpload.Segment;
            return new Tuple<CgUploadCsvReqModel, int>(requestModel, reuestId);
        }

        public CgUploadCsvResModel CgUploadSaveResponse(HttpResponseMessage httpResponse, int requestId, LoginModel user)
        {
            CodeMappingResponseDto responseDto = new CodeMappingResponseDto();
            responseDto.RequestId = requestId;
            responseDto.ResponseMessage = httpResponse.Content.ReadAsStringAsync().Result;
            responseDto.IsSuccess = (httpResponse.StatusCode == HttpStatusCode.OK) ? true : false;
            responseDto.CreatedBy = user.UserName;
            
            SqlHelper.SaveResponseseMessage(responseDto,requestId ,_context);
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
            string inputdirName = _configuration["AzureFileStorage:InputDirPath"];
            string outputDirName = _configuration["AzureFileStorage:OutputDirPath"];
            string fileName = $"{DateTime.Now.Ticks}_{file.FileName}";

            ShareClient share = new(connectionString, shareName);
            share.CreateIfNotExists();

            ShareDirectoryClient directory = share.GetDirectoryClient(inputdirName);
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
            return $"{outputDirName}{inputdirName}/{fileName}";
        }

    }
}


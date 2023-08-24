﻿using AutoMapper;
using Azure;
using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;
using CodeMappingEfCore.DatabaseModels;
using CodeMatcher.Api.V2.BusinessLayer;
using CodeMatcherV2Api.ApiRequestModels;
using CodeMatcherV2Api.ApiResponseModel;
using CodeMatcherV2Api.BusinessLayer.Interfaces;
using CodeMatcherV2Api.EntityFrameworkCore;
using CodeMatcherV2Api.Middlewares.SqlHelper;
using CodeMatcherV2Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CodeMatcherV2Api.BusinessLayer
{
    public class CsvUpload : ICsvUpload
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly CodeMatcherDbContext _context;
        private readonly SqlHelper _sqlHelper;
        public CsvUpload(IMapper mapper, IConfiguration configuration, CodeMatcherDbContext context, SqlHelper sqlHelper)
        {
            _mapper = mapper;
            _configuration = configuration;
            _context = context;
            _sqlHelper = sqlHelper;
        }

        public async Task<Tuple<CgUploadCsvReqModel, int>> CgUploadCsvRequestGet(CgCsvUploadModel csvUpload, LoginModel user, string clientId)
        {
            CodeMappingRequestDto codeMappingRequestDto = new CodeMappingRequestDto();
            codeMappingRequestDto.RunTypeId = (await _sqlHelper.GetLookupbyName(LookupTypeConst.RunType, RequestTypeConst.UploadCsv)).Id;
            codeMappingRequestDto.SegmentTypeId = (await _sqlHelper.GetLookupbyName(LookupTypeConst.Segment, csvUpload.Segment)).Id;
            codeMappingRequestDto.CodeMappingId = (await _sqlHelper.GetLookupbyName(LookupTypeConst.CodeMapping, CodeMappingTypeConst.CodeGeneration)).Id;
            foreach (var item in csvUpload.Threshold)
            {
                if (codeMappingRequestDto.Threshold == null)
                    codeMappingRequestDto.Threshold = item;
                codeMappingRequestDto.Threshold = codeMappingRequestDto.Threshold + "," + item;
            }
            codeMappingRequestDto.LatestLink = "1";
            codeMappingRequestDto.CsvFilePath = csvUpload.CsvFilePath;
            codeMappingRequestDto.CreatedBy = user.UserName;
            codeMappingRequestDto.ClientId= clientId;
            int reuestId = await _sqlHelper.SaveCodeMappingRequest(codeMappingRequestDto);
            CgUploadCsvReqModel requestModel = new CgUploadCsvReqModel();
            requestModel.CsvInput = csvUpload.CsvFilePath;
            requestModel.Threshold = csvUpload.Threshold;
            requestModel.Segment = csvUpload.Segment;
            return new Tuple<CgUploadCsvReqModel, int>(requestModel, reuestId);
        }

        public async Task<CgUploadCsvResModel> CgUploadSaveResponse(HttpResponseMessage httpResponse, int requestId, LoginModel user)
        {
            CodeMappingResponseDto responseDto = new CodeMappingResponseDto();
            responseDto.RequestId = requestId;
            responseDto.ResponseMessage = httpResponse.Content.ReadAsStringAsync().Result;
            responseDto.IsSuccess = (httpResponse.StatusCode == HttpStatusCode.OK) ? true : false;
            responseDto.CreatedBy = user.UserName;
            
           await _sqlHelper.SaveResponseseMessage(responseDto,requestId);
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
            catch 
            {
                throw ;
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

        public async Task<bool> DownloadFile(string dirName)
        {
            try
            {
                string connectionString = _configuration["AzureFileStorage:ConnectionString"];
                string shareName = _configuration["AzureFileStorage:ShareName"];
                ShareClient share = new(connectionString, shareName);
                
                ShareDirectoryClient directory = share.GetDirectoryClient(dirName);

                //Get the files from overall
                var files = directory.GetFilesAndDirectories();

                if (files == null || files.Count() == 0) // if no files then, returning false
                {
                    return false;
                }

                // Check path and remove if added already and added newly
                var filesPath = Environment.CurrentDirectory + @$"\{dirName}";
                if (System.IO.Directory.Exists(filesPath))
                {
                    Directory.Delete(filesPath, true);
                }
                if (!System.IO.Directory.Exists(filesPath))
                {
                    Directory.CreateDirectory(filesPath);
                }

                //Loop over the overall files and directories
                foreach (var file in files) 
                {
                    if (!file.IsDirectory)
                    {
                        var fileName = file.Name;
                        var filePath = Path.Combine(filesPath, fileName);
                        ShareFileClient fileclient = directory.GetFileClient(fileName);
                        ShareFileDownloadInfo downloadInfo = fileclient.Download();
                        using (FileStream stream = File.OpenWrite(filePath))
                        {
                            await downloadInfo.Content.CopyToAsync(stream);
                        }
                    }
                    if (file.IsDirectory)
                    {
                       await DownloadFile(Path.Combine(dirName, file.Name));
                    }
                }
                return true;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        

    }
}


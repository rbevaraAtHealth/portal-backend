using AutoMapper;
using CodeMappingEfCore.DatabaseModels;
using CodeMatcher.Api.V2.BusinessLayer.Dictionary;
using CodeMatcher.Api.V2.BusinessLayer.Interfaces;
using CodeMatcher.Api.V2.Models;
using CodeMatcher.Api.V2.RepoModelAdapter;
using CodeMatcher.EntityFrameworkCore.DatabaseModels;
using CodeMatcherV2Api.ApiRequestModels;
using CodeMatcherV2Api.ApiResponseModel;
using CodeMatcherV2Api.EntityFrameworkCore;
using CodeMatcherV2Api.Middlewares.SqlHelper;
using CodeMatcherV2Api.Models;
using CodeMatcherV2Api.RepoModelAdapter;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CodeMatcher.Api.V2.BusinessLayer
{
    public class ApiKey : IApiKey
    {
        private readonly CodeMatcherDbContext _context;
        private readonly IMapper _mapper;
        private readonly SqlHelper _sqlHelper;

        public ApiKey(CodeMatcherDbContext context, IMapper mapper, SqlHelper sqlHelper)
        {
            _context = context;
            _mapper = mapper;
            _sqlHelper = sqlHelper;
        }

        public async Task<List<APIKeyModel>> GetAllApiKeysRecords()
        {
            var apiKeyRecords = await _context.ApiKeys.AsNoTracking().OrderByDescending(x => x.CreatedTime).ToListAsync();
            List<APIKeyModel> result = new List<APIKeyModel>();
            foreach (var item in apiKeyRecords)
            {
                APIKeyModel apiKeyModel = new APIKeyModel();
                apiKeyModel.Api_Key = item.Api_Key;
                result.Add(apiKeyModel);
            }
            return result;
        }

        public async Task<APIKeyModel> CreateNewApiKey(APIKeyModel apiKey, LoginModel user, string clientId)
        {
            APIKeyDto apiKeyDto = new APIKeyDto();
            apiKeyDto.Api_Key = apiKey.Api_Key;
            apiKeyDto.CreatedBy = user.UserName != null ? user.UserName : "Scheduler Admin";
            apiKeyDto.CreatedTime = DateTime.Now;
            var aPiKey = await _sqlHelper.SaveApiKeyRequest(apiKeyDto);

            if (aPiKey != null)
            {
                var apiKeyModel = _mapper.Map<APIKeyModel>(aPiKey);
                return apiKeyModel;
            }
            //string apiKey = await _sqlHelper.GetApiKey();

            return new APIKeyModel(){ Api_Key="Key already exist"};
        }

        //Delete ApiKey
        public async Task<string> DeleteApiKey(string apiKey)
        {

            var keyId = await _context.ApiKeys.FirstOrDefaultAsync(x => x.Api_Key == apiKey);
            if (keyId != null)
            {
                _context.ApiKeys.Remove(keyId);
                _context.SaveChanges();
                return "ApiKey is deleted";
            }
            else
            {
                return "ApiKey is not deleted";
            }

        }
    }
}

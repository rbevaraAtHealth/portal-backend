using CodeMappingEfCore.DatabaseModels;
using CodeMatcherV2Api.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using CodeMatcher.Api.V2.BusinessLayer;
using System.Threading.Tasks;
using CodeMatcher.Api.V2.BusinessLayer.Interfaces;
using System;
using CodeMatcher.EntityFrameworkCore.DatabaseModels;
using CodeMatcher.Api.V2.Models;

namespace CodeMatcherV2Api.Middlewares.SqlHelper
{
    public class SqlHelper
    {
        private readonly CodeMatcherDbContext context;
        private readonly ICacheService _cacheService;

        public SqlHelper(CodeMatcherDbContext _context, ICacheService cacheService)
        {
            context = _context;
            _cacheService = cacheService;
        }
        public async Task<int> SaveCodeMappingRequest(CodeMappingRequestDto cgReqModel)
        {
            context.CodeMappingRequests.Add(cgReqModel);
            await context.SaveChangesAsync();
            return (cgReqModel.Id);
        }
        public async Task SaveResponseseMessage(CodeMappingResponseDto responseDto, int requestId)
        {

            await context.CodeMappingResponses.AddAsync(responseDto);
            context.SaveChanges();
        }

        public string GetLookupName(int id)
        {
            var lookup = context.Lookups.AsNoTracking().FirstOrDefault(x => x.Id == id);
            return lookup.Name;
        }
        public async Task<int> SaveCodeMappingData(CodeMappingDto codeMapping)
        {
            context.CodeMappings.Add(codeMapping);
            await context.SaveChangesAsync();
            return (codeMapping.Id);
        }

        public int GetRequestId(string taskId)
        {
            var codemap = context.CodeMappings.AsNoTracking().FirstOrDefault(x => x.Reference == taskId);
            return codemap.RequestId;
        }
        public int GetCodeMappingId(int requestId)
        {
            var codeMapping = context.CodeMappingRequests.AsNoTracking().FirstOrDefault(x => x.Id == requestId);
            return codeMapping.CodeMappingId;
        }
        public List<CodeMappingDto> GetCodeMappings()
        {
            var codeMappingList = context.CodeMappings.AsNoTracking().Where(x => x.Status.ToLower() == StatusConst.InProgress.ToLower()).ToList();
            return codeMappingList;
        }
        public void UpdateCodeMappingStatus(string taskId)
        {
            var codeMap = context.CodeMappings.FirstOrDefault(x => x.Reference == taskId);
            codeMap.Status = StatusConst.Success;
            context.Entry(codeMap).State = EntityState.Modified;
            context.SaveChanges();
        }
        public async Task<List<LookupDto>> GetLookups(string key)
        {
            var cacheData = _cacheService.GetData<List<LookupDto>>(key);
            if (cacheData != null && cacheData.Count != 0)
            {
                return cacheData;
            }
            var expirationTime = DateTimeOffset.Now.AddDays(7.0);
            var lookups = await context.Lookups.Include("LookupType").Where(x => x.LookupType.LookupTypeKey.ToLower() == key.ToLower()).AsNoTracking().ToListAsync();

            var result = _cacheService.SetData(key, lookups, expirationTime);
            return lookups;
        }
        public async Task<LookupDto> GetLookupbyName(string key, string type)
        {
            LookupDto filteredData;
            var cacheData = await GetLookups(key);
            filteredData = cacheData.Where(x => x.Name.ToLower() == type.ToLower()).FirstOrDefault();
            return filteredData;
        }

        public async Task<CodeMappingDto> UpdateTaskStatus(string taskId, CodeMappingDto updateStatus)
        {
            var requestId = context.CodeMappings.FirstOrDefault(x => x.Reference == taskId);
            if (requestId != null)
            {
                requestId.Reference = taskId;
                requestId.Status = updateStatus.Status;
                requestId.Progress = updateStatus.Progress;
                context.Update(requestId);
                context.Entry(requestId).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
            return requestId;
        }

        public async Task<int> UpdateCodeGenerationRequest(CodeMappingRequestDto cgReqModel, bool checkClientId)
        {
            var details = new CodeMappingRequestDto();
            if (checkClientId)
                details = await context.CodeMappingRequests.FirstOrDefaultAsync(x => x.SegmentTypeId.Equals(cgReqModel.SegmentTypeId)
                                    && x.CodeMappingId.Equals(cgReqModel.CodeMappingId) && x.RunTypeId == cgReqModel.RunTypeId
                                    && x.ClientId == cgReqModel.ClientId);
            else
                details = await context.CodeMappingRequests.FirstOrDefaultAsync(x => x.SegmentTypeId.Equals(cgReqModel.SegmentTypeId)
                                    && x.CodeMappingId.Equals(cgReqModel.CodeMappingId) && x.RunTypeId == cgReqModel.RunTypeId);

            if (details != null)
            {
                details.RunSchedule = cgReqModel.RunSchedule;
                details.Threshold = cgReqModel.Threshold;
                details.LatestLink = cgReqModel.LatestLink;
                details.CreatedBy = cgReqModel.CreatedBy;
                details.CreatedTime = cgReqModel.CreatedTime;
                details.ClientId = cgReqModel.ClientId;
                context.CodeMappingRequests.Update(details);
                context.Entry(details).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return details.Id;
            }
            else
            {
                context.CodeMappingRequests.Add(cgReqModel);
                context.Entry(cgReqModel).State = EntityState.Added;
                await context.SaveChangesAsync();
                return cgReqModel.Id;
            }
        }

        public async Task<int> SaveLogRequest(LogTableDto log)
        {
            context.LogTable.Add(log);
            context.Entry(log).State = EntityState.Added;
            await context.SaveChangesAsync();
            return (1);
        }

        public async Task<string> GetApiKey()
        {
            var apiKey = context.ApiKeys.FirstOrDefault();
            if (apiKey != null)
            {
                return apiKey.Api_Key;
            }
            else
            {
                return null;
            }
        }

        public async Task<APIKeyDto> SaveApiKeyRequest(APIKeyDto apiKeyModel)
        {
            var apiKey = await context.ApiKeys.FirstOrDefaultAsync(x => x.Api_Key == apiKeyModel.Api_Key);
            if (apiKey == null)
            {
                context.ApiKeys.Add(apiKeyModel);
                await context.SaveChangesAsync();
                return apiKeyModel;
            }
           // return "Api Key Already Exists";
            return null;
        }
    }
}

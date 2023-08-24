using CodeMatcher.Api.V2.Models;
using CodeMatcher.Api.V2.Models.SummaryModel;
using CodeMatcher.EntityFrameworkCore.DatabaseModels.SummaryTables;
using CodeMatcherV2Api.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CodeMatcherV2Api.BusinessLayer.Interfaces
{
    public interface ICodeMapping
    {
        // Task<List<CodeMappingModel>> GetCodeMappingsRecordsAsync();
        Task<List<GenericSummaryViewModel>> GetCodeGenerationMappingRecords(string clientId);
        Task<List<GenericSummaryViewModel>> GetWeeklyEmbeddingMappingRecords(string clientId);
        Task<List<GenericSummaryViewModel>> GetMonthlyEmbeddingMappingRecords(string clientId);
        Task<int> SaveCgMappingsPythApi(string taskId, string summary, int requestId, LoginModel loginModel);
        Task<int> SaveMonthlyEmbedMappingsPythApi(string taskId, string summary, int requestId, LoginModel loginModel);
        Task<int> SaveWeeklyEmbedMappingsPythApi(string taskId, string summary, int requestId, LoginModel loginModel);
        Task<int> SaveSummary(string taskId, string summary, LoginModel loginModel);
        Task<GenericSummaryViewModel> GetMappings(string taskId);
        Task<List<CodeMappingReqResDataModel>> GetCodeMappingRequestResponse();
    }
}

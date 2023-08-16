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
        Task<List<CodeMappingModel>> GetCodeMappingsRecordsAsync();
        List<GenericSummaryViewModel> GetCodeGenerationMappingRecords();
        List<GenericSummaryViewModel> GetWeeklyEmbeddingMappingRecords();
        List<GenericSummaryViewModel> GetMonthlyEmbeddingMappingRecords();
        int SaveCgMappingsPythApi(string taskId, string summary,int requestId, LoginModel loginModel);
        int SaveMonthlyEmbedMappingsPythApi(string taskId, string summary, int requestId, LoginModel loginModel);
        int SaveWeeklyEmbedMappingsPythApi(string taskId, string summary, int requestId, LoginModel loginModel);
        int SaveSummary(string taskId, string summary, LoginModel loginModel);
        GenericSummaryViewModel GetMappings(string taskId);        
    }
}

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
        List<GenericSummaryViewModel> GetEmbeddingMappingRecords(string codeMappingType);
        int SaveCgMappingsPythApi(string taskId, string summary,int requestId);
        int SaveMonthlyEmbedMappingsPythApi(string taskId, string summary, int requestId);
        int SaveWeeklyEmbedMappingsPythApi(string taskId, string summary, int requestId);
        int SaveSummary(string taskId, string summary);
        GenericSummaryViewModel GetMappings(string taskId);        
    }
}

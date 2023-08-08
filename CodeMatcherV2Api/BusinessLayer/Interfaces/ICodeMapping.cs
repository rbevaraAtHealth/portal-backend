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
        List<GenericSummaryViewModel> GetMonthlyEmbeddingMappingRecords();
        List<GenericSummaryViewModel> GetWeeklyEmbeddingsMappingRecords();
        int SaveCgMappingsPythApi(Guid taskId, string summary,int requestId);
        int SaveMonthlyEmbedMappingsPythApi(Guid taskId, string summary, int requestId);
        int SaveWeeklyEmbedMappingsPythApi(Guid taskId, string summary, int requestId);
        int SaveSummary(Guid taskId, string summary);
        GenericSummaryViewModel GetMappings(Guid taskId);
        List<GenericSummaryViewModel> GetCodeMappings();
    }
}

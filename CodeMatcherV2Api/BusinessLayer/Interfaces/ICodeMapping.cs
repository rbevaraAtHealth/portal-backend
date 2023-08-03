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
        List<CodeGenerationSummaryModel> GetCodeGenerationMappingRecords();
        List<MonthlyEmbedSummaryModel> GetMonthlyEmbeddingMappingRecords();
        List<WeeklyEmbedSummaryModel> GetWeeklyEmbeddingsMappingRecords();
        int GetCgMappingsPythApi(Guid taskId, string summary,int requestId);
        int GetMonthlyEmbedMappingsPythApi(Guid taskId, string summary, int requestId);
        int GetWeeklyEmbedMappingsPythApi(Guid taskId, string summary, int requestId);
        int SaveSummary(Guid taskId, string summary);
    }
}

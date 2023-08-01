using CodeMatcher.Api.V2.Models;
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
       // HttpResponseMessage GetJobResult(Guid taskId);
        CodeGenerationSummaryModel GetCgMappingsPythApi(HttpResponseMessage httpResponse, Guid taskId);
        MonthlyEmbedSummaryModel GetMonthlyEmbedMappingsPythApi(HttpResponseMessage httpResponse, Guid taskId);
        WeeklyEmbedSummaryModel GetWeeklyEmbedMappingsPythApi(HttpResponseMessage httpResponse, Guid taskId);
        void GetMappingsInProcessTasks();
    }
}

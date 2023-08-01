using CodeMappingEfCore.DatabaseModels;
using CodeMatcherV2Api.EntityFrameworkCore;
using System.Net.Http;
using System.Net;
using System.Linq;
using CodeMatcher.EntityFrameworkCore.DatabaseModels.SummaryTables;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace CodeMatcherV2Api.Middlewares.SqlHelper
{
    public static class SqlHelper
    {
        public static int SaveCodeMappingRequest(CodeMappingRequestDto cgReqModel, CodeMatcherDbContext context)
        {
            context.CodeMappingRequests.Add(cgReqModel);
            context.SaveChanges();
            return (cgReqModel.Id);
        }
        public static void SaveResponseseMessage(CodeMappingResponseDto responseDto, int requestId, CodeMatcherDbContext context)
        {

            context.CodeMappingResponses.Add(responseDto);
            context.SaveChanges();
        }
        public static int GetLookupType(int id,CodeMatcherDbContext context)
        {
            var lookup = context.Lookups.FirstOrDefault(x => x.Id == id);
            return lookup.Id;
        }
        public static int GetLookupType(string type, CodeMatcherDbContext context)
        {
            var lookup = context.Lookups.FirstOrDefault(x => x.Name == type);
            return lookup.Id;
        }
        public static int SaveCodeMappingData(CodeMappingDto codeMapping,CodeMatcherDbContext context)
        {
            context.CodeMappings.Add(codeMapping);
            context.SaveChanges();
            return (codeMapping.Id);
        }
        public static void SaveCodeGenerationSummary(CodeGenerationSummaryDto cgSummary,CodeMatcherDbContext context)
        {
            context.CodeGenerationSummary.Add(cgSummary);
            context.SaveChanges();
            int i = cgSummary.Id;
        }
        public static void SaveMonthlyEmbedSummary(MonthlyEmbeddingsSummaryDto monthlySummary, CodeMatcherDbContext context)
        {
            context.MonthlyEmbeddingsSummary.Add(monthlySummary);
            context.SaveChanges();
            int i = monthlySummary.Id;
        }
        public static void SaveWeeklyEmbedSummary(WeeklyEmbeddingsSummaryDto weeklySummary, CodeMatcherDbContext context)
        {
            context.WeeklyEmbeddingsSummary.Add(weeklySummary);
            context.SaveChanges();
            int i = weeklySummary.Id;
        }
        public static int GetRequestId(Guid taskId,CodeMatcherDbContext context)
        {
            var codemap=context.CodeMappings.FirstOrDefault(x => x.Reference == taskId.ToString());
            return codemap.Id;
        }
        public static int GetCodeMappingId(int requestId,CodeMatcherDbContext context)
        {
           var codeMapping= context.CodeMappingRequests.FirstOrDefault(x => x.Id == requestId);
            return codeMapping.CodeMappingId;
        }
        public static List<CodeMappingDto> GetCodeMappings(CodeMatcherDbContext context)
        {
            return context.CodeMappings.Where(x=>x.Status.Equals("In progress")).ToList();
           // return context.CodeMappings.AsNoTracking().ToList();
        }
    }
}

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
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace CodeMatcherV2Api.Middlewares.SqlHelper
{
    public  class SqlHelper
    {
        private readonly CodeMatcherDbContext context;
        public SqlHelper(CodeMatcherDbContext _context)
        {
            context = _context;
        }
        public  int SaveCodeMappingRequest(CodeMappingRequestDto cgReqModel)
        {
            context.CodeMappingRequests.Add(cgReqModel);
            context.SaveChanges();
            return (cgReqModel.Id);
        }
        public  void SaveResponseseMessage(CodeMappingResponseDto responseDto, int requestId)
        {

            context.CodeMappingResponses.Add(responseDto);
            context.SaveChanges();
        }
      
        public  int GetLookupIdOnName(string type)
        {
            var lookup = context.Lookups.FirstOrDefault(x => x.Name == type);
            return lookup.Id;
        }
        public string GetLookupName(int id)
        {
            var lookup = context.Lookups.FirstOrDefault(x => x.Id == id);
            return lookup.Name;
        }
        public int SaveCodeMappingData(CodeMappingDto codeMapping)
        {
            context.CodeMappings.Add(codeMapping);
            context.SaveChanges();
            return (codeMapping.Id);
        }
        public int SaveCodeGenerationSummary(CodeGenerationSummaryDto cgSummary)
        {
            context.CodeGenerationSummary.Add(cgSummary);
            context.SaveChanges();
            //context.CodeMappings.
            return cgSummary.Id;
        }
        public int SaveMonthlyEmbedSummary(MonthlyEmbeddingsSummaryDto monthlySummary)
        {
            context.MonthlyEmbeddingsSummary.Add(monthlySummary);
            context.SaveChanges();
            return monthlySummary.Id;
        }
        public int SaveWeeklyEmbedSummary(WeeklyEmbeddingsSummaryDto weeklySummary)
        {
            context.WeeklyEmbeddingsSummary.Add(weeklySummary);
            context.SaveChanges();
            return weeklySummary.Id;
        }
        public int GetRequestId(string taskId)
        {
            var codemap=context.CodeMappings.FirstOrDefault(x => x.Reference == taskId.ToString());
            return codemap.Id;
        }
        public int GetCodeMappingId(int requestId)
        {
           var codeMapping= context.CodeMappingRequests.FirstOrDefault(x => x.Id == requestId);
            return codeMapping.CodeMappingId;
        }
        public  List<CodeMappingDto> GetCodeMappings()
        {
           var codeMappingList= context.CodeMappings.Where(x=>x.Status.Equals("In progress")).ToList();
            return codeMappingList;
           // return context.CodeMappings.AsNoTracking().ToList();
        }
        public void UpdateCodeMappingStatus(string taskId)
        {
            var codeMap = context.CodeMappings.FirstOrDefault(x => x.Reference==taskId.ToString());
            codeMap.Status = "Completed";
            context.Entry(codeMap).State = EntityState.Modified;
            //context.CodeMappings.Update(codeMap);
            context.SaveChanges();
            
        }
    }
}

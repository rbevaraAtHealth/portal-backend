using CodeMappingEfCore.DatabaseModels;
using CodeMatcherV2Api.EntityFrameworkCore;
using System.Linq;
using CodeMatcher.EntityFrameworkCore.DatabaseModels.SummaryTables;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using CodeMatcher.Api.V2.BusinessLayer;
using System.Threading.Tasks;

namespace CodeMatcherV2Api.Middlewares.SqlHelper
{
    public class SqlHelper
    {
        private readonly CodeMatcherDbContext context;
        public SqlHelper(CodeMatcherDbContext _context)
        {
            context = _context;
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

        public int GetLookupIdOnName(string type)
        {
            var lookup = context.Lookups.FirstOrDefault(x => x.Name.ToLower() == type.ToLower());
            return lookup.Id;
        }
        public string GetLookupName(int id)
        {
            var lookup = context.Lookups.FirstOrDefault(x => x.Id == id);
            return lookup.Name;
        }
        public async Task<int> SaveCodeMappingData(CodeMappingDto codeMapping)
        {
            context.CodeMappings.Add(codeMapping);
            await context.SaveChangesAsync();
            return (codeMapping.Id);
        }
        public async Task<int> SaveCodeGenerationSummary(CodeGenerationSummaryDto cgSummary)
        {
            context.CodeGenerationSummary.Add(cgSummary);
            await context.SaveChangesAsync();
            return cgSummary.Id;
        }
        public async Task<int> SaveMonthlyEmbedSummary(MonthlyEmbeddingsSummaryDto monthlySummary)
        {
            context.MonthlyEmbeddingsSummary.Add(monthlySummary);
            await context.SaveChangesAsync();
            return monthlySummary.Id;
        }
        public async Task<int> SaveWeeklyEmbedSummary(WeeklyEmbeddingsSummaryDto weeklySummary)
        {
            context.WeeklyEmbeddingsSummary.Add(weeklySummary);
            await context.SaveChangesAsync();
            return weeklySummary.Id;
        }
        public int GetRequestId(string taskId)
        {
            var codemap = context.CodeMappings.FirstOrDefault(x => x.Reference == taskId);
            return codemap.Id;
        }
        public int GetCodeMappingId(int requestId)
        {
            var codeMapping = context.CodeMappingRequests.FirstOrDefault(x => x.Id == requestId);
            return codeMapping.CodeMappingId;
        }
        public List<CodeMappingDto> GetCodeMappings()
        {
            var codeMappingList = context.CodeMappings.Where(x => x.Status.ToLower() == Status.InProgress.ToLower()).ToList();
            return codeMappingList;
        }
        public void UpdateCodeMappingStatus(string taskId)
        {
            var codeMap = context.CodeMappings.FirstOrDefault(x => x.Reference == taskId);
            codeMap.Status = Status.Success;
            context.Entry(codeMap).State = EntityState.Modified;
            context.SaveChanges();

        }
    }
}

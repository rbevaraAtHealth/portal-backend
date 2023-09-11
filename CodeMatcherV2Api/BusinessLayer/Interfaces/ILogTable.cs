using CodeMatcher.Api.V2.Models;
using CodeMatcher.EntityFrameworkCore.DatabaseModels;
using CodeMatcherV2Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeMatcher.Api.V2.BusinessLayer.Interfaces
{
    public interface ILogTable
    {
        public Task<List<LogTableModel>> GetLogsAsync();
        public Task<List<LogTableModel>> GetLogsByName(string key);
        public Task<int> SaveLog(LogTableDto logTableDto);
    }
}

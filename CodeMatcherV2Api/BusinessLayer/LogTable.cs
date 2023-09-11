using AutoMapper;
using CodeMatcher.Api.V2.BusinessLayer.Interfaces;
using CodeMatcher.Api.V2.Models;
using CodeMatcher.EntityFrameworkCore.DatabaseModels;
using CodeMatcherV2Api.EntityFrameworkCore;
using CodeMatcherV2Api.Middlewares.SqlHelper;
using CodeMatcherV2Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeMatcher.Api.V2.BusinessLayer
{
    public class LogTable: ILogTable
    {
        private readonly IMapper _mapper;
        private CodeMatcherDbContext _context;
        private readonly SqlHelper _sqlHelper;
        public LogTable(IMapper mapper, CodeMatcherDbContext context, SqlHelper sqlHelper)
        {
            _context = context;
            _mapper = mapper;
            _sqlHelper = sqlHelper;
        }

        public async Task<List<LogTableModel>> GetLogsAsync()
        {
           var logs = await _context.LogTable.ToListAsync();
           return _mapper.Map<List<LogTableModel>>(logs);

        }

        public async Task<List<LogTableModel>> GetLogsByName(string key)
        {
            var logs = await _context.LogTable.Where(x => x.LogName.ToLower() == key.ToLower()).AsNoTracking().ToListAsync();
            return _mapper.Map<List<LogTableModel>>(logs);
        }

        public async Task<int> SaveLog(LogTableDto logTableDto)
        {
            return await _sqlHelper.SaveLogRequest(logTableDto);
        }
    }
}

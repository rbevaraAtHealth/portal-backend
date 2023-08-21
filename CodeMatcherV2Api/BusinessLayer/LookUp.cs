using AutoMapper;
using CodeMatcher.Api.V2.BusinessLayer.Interfaces;
using CodeMatcherV2Api.BusinessLayer.Interfaces;
using CodeMatcherV2Api.EntityFrameworkCore;
using CodeMatcherV2Api.Middlewares.SqlHelper;
using CodeMatcherV2Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeMatcherV2Api.BusinessLayer
{
    public class LookUp : ILookUp
    {
        private readonly IMapper _mapper;
        private CodeMatcherDbContext _context;
        private readonly SqlHelper _sqlHelper;
        public LookUp(IMapper mapper, CodeMatcherDbContext context, SqlHelper sqlHelper)
        {
            _context = context;
            _mapper = mapper;
            _sqlHelper= sqlHelper;
        }
        public async Task<List<LookupModel>> GetLookupsByName(string key)
        {
                var lookups = await _sqlHelper.GetLookups(key);
                return _mapper.Map<List<LookupModel>>(lookups);
        }
        public async Task<IEnumerable<LookupModel>> GetLookupsAsync()
        {
            var lookup = await _context.Lookups.ToListAsync();

            if (lookup != null && lookup.Count > 0)
            {
                return _mapper.Map<List<LookupModel>>(lookup);
            }
            else
            {
                throw new System.Exception("No lookups found");
            }

        }
        public string GetDBConnectionString()
        {
            return _context.Database.GetConnectionString();
        }
    }
}

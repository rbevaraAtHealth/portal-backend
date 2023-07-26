using AutoMapper;
using CodeMatcherV2Api.BusinessLayer.Interfaces;
using CodeMatcherV2Api.EntityFrameworkCore;
using CodeMatcherV2Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeMatcherV2Api.BusinessLayer
{
    public class LookUp : ILookUp
    {
        private readonly IMapper _mapper;
        private CodeMatcherDbContext _context;
        public LookUp(IMapper mapper, CodeMatcherDbContext context)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<LookupModel>> GetLookupByIdAsync(int lookUpTypeId)
        {
            var lookup = await _context.Lookups.Where(x => x.LookupTypeId == lookUpTypeId).ToListAsync();
            return _mapper.Map<List<LookupModel>>(lookup);
        }
    }
}

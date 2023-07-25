using AutoMapper;
using CodeMatcherV2Api.BusinessLayer.Interfaces;
using CodeMatcherV2Api.EntityFrameworkCore;
using CodeMatcherV2Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CodeMatcherV2Api.BusinessLayer
{
    public class LookupTypes : ILookupTypes
    {
        private CodeMatcherDbContext _context;
        private readonly IMapper _mapper;
        public LookupTypes(CodeMatcherDbContext context,IMapper mapper)
        {
            _context= context;
            _mapper= mapper;
        }
        public async Task<LookupTypeModel> GetLookupByNameAsync(string lookUpType)
        {
            var lookup = await _context.LookupTypes.FirstOrDefaultAsync(x => x.LookupTypeKey == lookUpType);
           return _mapper.Map<LookupTypeModel>(lookup);
           
        }
    }
}

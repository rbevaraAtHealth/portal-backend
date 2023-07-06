using AutoMapper;
using CodeMatcherV2Api.BusinessLayer.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeMatcherV2Api.BusinessLayer
{
    public class LookUp : ILookUp
    {
        private readonly IMapper _mapper;
        public LookUp(IMapper mapper)
        {
            _mapper= mapper;
        }
        public async Task<Dictionary<int, string>> GetLookupsAsync(string lookUpType)
        {
            Dictionary<int,string> segments=new Dictionary<int,string>();
            
            if (lookUpType.ToLower() == "segment")
            {
                segments.Add(1, "Insurance");
                segments.Add(2, "School");
                segments.Add(3, "Hospital");
                segments.Add(4, "State License");
            }
            return segments;
        }
    }
}

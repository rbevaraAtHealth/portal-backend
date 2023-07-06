using CodeMatcherV2Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeMatcherV2Api.BusinessLayer.Interfaces
{
    public interface ILookUp
    {
        public Task<Dictionary<int,string>> GetLookupsAsync(string lookUpType);
    }
}

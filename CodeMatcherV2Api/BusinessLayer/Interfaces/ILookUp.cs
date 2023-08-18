using CodeMatcherV2Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeMatcherV2Api.BusinessLayer.Interfaces
{
    public interface ILookUp
    {
        public Task<IEnumerable<LookupModel>> GetLookupByIdAsync(int lookUpTypeId);
        public Task<IEnumerable<LookupModel>> GetLookupsAsync();

    }
}

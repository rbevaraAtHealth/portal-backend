using CodeMatcherV2Api.Models;
using System.Threading.Tasks;

namespace CodeMatcherV2Api.BusinessLayer.Interfaces
{
    public interface ILookupTypes
    {
        public  LookupTypeModel GetLookupByNameAsync(string lookupType);
    }
}

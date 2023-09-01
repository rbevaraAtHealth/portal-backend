using CodeMatcher.Api.V2.Models;
using CodeMatcherV2Api.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace CodeMatcherV2Api.BusinessLayer.Interfaces
{
    public interface ICodeGenerationOverwrite
    {
        Task<DataSet> CodeGenerationOverwritegetAsync(string taskId, LoginModel userModel, string clientId);
        Task<bool> UpdateCGSourceDB(List<CgOverwriteUpdateModel> updateModels, string clientId);
        Task<bool> UpdateCGDestinationDB(string taskId, List<CgOverwriteUpdateModel> updateModels, string clientId);

    }
}

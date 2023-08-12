using CodeMatcherV2Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeMatcherV2Api.BusinessLayer.Interfaces
{
    public interface ICodeGenerationOverwrite
    {
        Task<List<CodeGenerationOverwriteModel>> GetAllCodeGenerationOverwriteAsync();
        Task<CodeGenerationOverwriteModel> GetCodeGenerationOverwriteByIdAsync(int id);
        Task<bool> UpdateCodeGenerationOverwriteAsync(CodeGenerationOverwriteModel codeGenerationOverwrite);
    }
}

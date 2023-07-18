using CodeMatcherV2Api.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace CodeMatcherV2Api.BusinessLayer.Interfaces
{
    public interface IUploadCSV
    {
        public Task<string> GetUploadCSVAsync(UploadModel uploads);
        public Task<string> WriteFile(IFormFile file);
    }
}

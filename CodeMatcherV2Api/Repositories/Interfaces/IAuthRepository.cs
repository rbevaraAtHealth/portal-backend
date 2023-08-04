using CodeMatcherV2Api.Models;
using System.Threading.Tasks;

namespace CodeMatcherApiV2.BusinessLayer.Interfaces
{
    public interface IAuthRepository
    {
        Task<bool> ProcessLogin(LoginModel model, string headerValue);
    }
}

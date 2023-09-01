using CodeMatcherV2Api.Models;
using System;
using System.Threading.Tasks;

namespace CodeMatcherApiV2.BusinessLayer.Interfaces
{
    public interface IAuthRepository
    {
        Task<Tuple<bool, LoginModel>> ProcessLogin(LoginModel model, string headerValue);
    }
}

﻿using CodeMatcherV2Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeMatcherV2Api.BusinessLayer.Interfaces
{
    public interface IUser
    {
        Task<IEnumerable<UserModel>> GetAllUsersAsync();
        Task<UserModel> GetUserByIdAsync(int id);
        Task<UserModel> UpdateUserAsync(UserModel user);
        Task<string> DeleteUserAsync(int id);
        Task<UserModel> CreateUserAsync(UserModel user);
    }
}

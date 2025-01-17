﻿using AutoMapper;
using CodeMappingEfCore.DatabaseModels;
using CodeMatcherV2Api.BusinessLayer.Interfaces;
using CodeMatcherV2Api.EntityFrameworkCore;
using CodeMatcherV2Api.Middlewares;
using CodeMatcherV2Api.Middlewares.HttpHelper;
using CodeMatcherV2Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeMatcherV2Api.BusinessLayer
{
    public class User : IUser
    {
        private readonly IMapper _mapper;
        private CodeMatcherDbContext _context;
        public User(IMapper mapper, CodeMatcherDbContext context)
        {
                _mapper= mapper;
            _context= context;
        }
        public async Task<UserModel> CreateUserAsync(UserModel user)
        {
            UserDto userDto= new UserDto();
             _mapper.Map<UserDto>(user);
            return user;
        }

        public async Task<string> DeleteUserAsync(int id)
        {
            return "User Deleted Successfully" ;
        }

        public async Task<IEnumerable<UserModel>> GetAllUsersAsync()
        {

            List<UserDto> users = new List<UserDto>();
            users = await _context.UserDetail.ToListAsync(); 
            return  _mapper.Map<IEnumerable<UserModel>>(users);
        }

        public async Task<UserModel> GetUserByIdAsync(int id)
        {
            UserDto user= new UserDto();
            user = await _context.UserDetail.FirstOrDefaultAsync(f => f.Id == id);
            return  _mapper.Map<UserModel>(user);
        }

        public async Task<UserModel> UpdateUserAsync(UserModel user)
        {
            return user;
        }
    }
}

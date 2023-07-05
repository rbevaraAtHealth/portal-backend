using AutoMapper;
using CodeMatcherV2Api.BusinessLayer.Interfaces;
using CodeMatcherV2Api.Dtos;
using CodeMatcherV2Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeMatcherV2Api.BusinessLayer
{
    public class User : IUser
    {
        private readonly IMapper _mapper;
        public User(IMapper mapper)
        {
                _mapper= mapper;
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
            List<UserDto> users = new List<UserDto>
            {
                new UserDto { Email = "teena@123@nstarxinc.com", FirstName = "teena", LastName = "gera" },
                new UserDto { Email = "anu@123@nstarxinc.com", FirstName = "anu", LastName = "sharma" },
                new UserDto { Email = "abc@123@nstarxinc.com", FirstName = "abc", LastName = "xyz" }
            };

            return  _mapper.Map<IEnumerable<UserModel>>(users);
        }

        public async Task<UserModel> GetUserByIdAsync(int id)
        {
            UserDto user= new UserDto();
            user.Id = id;
            user.FirstName = "Anu";
            user.LastName= "Arora";
            user.Email = "Anu@gmail.com";
            return  _mapper.Map<UserModel>(user);
        }

        public async Task<UserModel> UpdateUserAsync(UserModel user)
        {
            return user;
        }
    }
}

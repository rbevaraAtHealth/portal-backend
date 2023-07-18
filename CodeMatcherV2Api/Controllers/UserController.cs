using CodeMatcherV2Api.BusinessLayer;
using CodeMatcherV2Api.BusinessLayer.Interfaces;
using CodeMatcherV2Api.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using CodeMatcherV2Api.Middlewares.HttpHelper;

namespace CodeMatcherV2Api.Controllers
{
    [Route("api/[controller]")]
    public class UserController : BaseController
    {
        private readonly IUser _User;
        private readonly ResponseViewModel _responseViewModel;
        public UserController(IUser user)
        {
            _User = user;
            _responseViewModel = new ResponseViewModel();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _User.GetAllUsersAsync();

                _responseViewModel.Model = users;
            }
            catch (Exception ex)
            {
                _responseViewModel.ExceptionMessage = ex.Message;
            }
            return Ok(_responseViewModel);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _User.GetUserByIdAsync(id);
                _responseViewModel.Model = user;
            }
            catch (Exception ex)
            {
                _responseViewModel.ExceptionMessage = ex.Message;
            }
            return Ok(_responseViewModel);
        }

        [HttpPost, Route("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] UserModel user)
        {
            try
            {
                var userModel = await _User.CreateUserAsync(user);
                _responseViewModel.Model = userModel;
            }
            catch (Exception ex)
            {
                _responseViewModel.ExceptionMessage = ex.Message;
            }
            return Ok(_responseViewModel);
        }

        [HttpPut, Route("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UserModel user)
        {
            try
            {
                var userModel = await _User.UpdateUserAsync(user);
                _responseViewModel.Model = userModel;
            }
            catch (Exception ex)
            {
                _responseViewModel.ExceptionMessage = ex.Message;
            }
            return Ok(_responseViewModel);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var message = await _User.DeleteUserAsync(id);
                _responseViewModel.Message = message;
            }
            catch (Exception ex)
            {
                _responseViewModel.ExceptionMessage = ex.Message;
            }
            return Ok(_responseViewModel);
        }
    }
}

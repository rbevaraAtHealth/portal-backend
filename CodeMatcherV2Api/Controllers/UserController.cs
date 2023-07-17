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
        public UserController(IUser user)
        {
            _User = user;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _User.GetAllUsersAsync();
                return Ok(users);

            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _User.GetUserByIdAsync(id);
                return Ok(user);
            }
            catch(Exception ex)
            {
                return BadRequest(ex); 
            }
        }

        [HttpPost,Route("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] UserModel user)
        {
            try
            {
                var userModel = await _User.CreateUserAsync(user);
                return Ok(userModel);
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut,Route("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UserModel user)
        {
            try
            {
                var userModel = await _User.UpdateUserAsync(user);
                return Ok(userModel);
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var message = await _User.DeleteUserAsync(id);
                return Ok(message);
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}

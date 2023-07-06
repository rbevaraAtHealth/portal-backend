using CodeMatcherV2Api.BusinessLayer.Interfaces;
using CodeMatcherV2Api.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

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
        public async Task<IActionResult> GetAllUsersAsync()
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
        public async Task<IActionResult> GetUserByIdAsync(int id)
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


        [HttpPost]
        public async Task<IActionResult> CreateUserAsync([FromBody] UserModel user)
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

        [HttpPut]
        public async Task<IActionResult> UpdateUserAsync([FromBody] UserModel user)
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
        [HttpDelete]
        public async Task<IActionResult> DeleteUserAsync(int id)
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

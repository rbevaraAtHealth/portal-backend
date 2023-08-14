using CodeMatcherApiV2.Common;
using CodeMatcherV2Api.BusinessLayer.Interfaces;
using CodeMatcherV2Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CodeMatcherV2Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser _User;
        public UserController(IUser user)
        {
            _User = user;
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _User.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetUserById/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _User.GetUserByIdAsync(id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [NonAction]
        [HttpGet("Encrypt/{connStr}")]
        public IActionResult GetEncryptConn(string connStr)
        {
            try
            {
                Encrypt _encrypt = new Encrypt();
                EncDecModel _res = _encrypt.EncryptString(connStr);
                return Ok(_res.outPut);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [NonAction]
        [HttpPost("Decrypt")]
        public IActionResult GetDecryptConn([FromBody] string connStr)
        {
            try
            {
                Decrypt _encrypt = new Decrypt();
                EncDecModel _res = _encrypt.DecryptString(connStr);
                return Ok(_res.outPut);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

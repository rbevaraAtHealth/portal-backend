using CodeMatcherApiV2.Common;
using CodeMatcherV2Api.BusinessLayer.Interfaces;
using CodeMatcherV2Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
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
        //[NonAction]
        [HttpPost("Encrypt")]
        public IActionResult GetEncryptConn([FromBody] string connStr)
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
        [HttpPost("CheckDBConnectivity")]
        public IActionResult CheckDBConnectivity([FromBody] List<string> connStrlist)
        {
            var outputlist = new List<Tuple<string, string>>();
            try
            {
                foreach (var conn in connStrlist)
                {
                    var output = new Tuple<string, string>(conn, "Checking");
                    try
                    {
                        using (SqlConnection myCon = new SqlConnection(conn))
                        {
                            myCon.Open();
                            output = new Tuple<string, string>(conn, "Connection established.");
                            myCon.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        output = new Tuple<string, string>(conn, $"Connection failed with exception. {ex}");
                    }
                    outputlist.Add(output);
                }  
                return Ok(outputlist);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

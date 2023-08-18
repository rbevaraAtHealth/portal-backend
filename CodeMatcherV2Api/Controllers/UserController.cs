using CodeMatcher.Api.V2.ApiResponseModel;
using CodeMatcherApiV2.Common;
using CodeMatcherV2Api.BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace CodeMatcherV2Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser _User;
        private readonly ResponseViewModel _responseViewModel;
        public UserController(IUser user)
        {
            _User = user;
            _responseViewModel = new ResponseViewModel();
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _User.GetAllUsersAsync();
                _responseViewModel.Model = users;
                return Ok(_responseViewModel);
            }
            catch (Exception ex)
            {
                _responseViewModel.ExceptionMessage = ex.Message;
                return BadRequest(_responseViewModel);
            }
        }
        [HttpGet("GetUserById/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _User.GetUserByIdAsync(id);
                _responseViewModel.Model = user;
                return Ok(_responseViewModel);
            }
            catch (Exception ex)
            {
                _responseViewModel.ExceptionMessage = ex.Message;
                return BadRequest(_responseViewModel);
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
                _responseViewModel.Model = outputlist;
                return Ok(_responseViewModel);
            }
            catch (Exception ex)
            {
                _responseViewModel.ExceptionMessage = ex.Message;
                return BadRequest(_responseViewModel);
            }
        }

        [HttpPost("UpdateBaseData")]
        public IActionResult UpdateBaseData([FromBody] string connStr)
        {
            try
            {
                var sqlFile = Path.Combine(Environment.CurrentDirectory, @"Resources\Files");
                _responseViewModel.Message = sqlFile;
                int i = 0;
                foreach (var file in Directory.GetFiles(sqlFile))
                {
                    if (file.ToLower().Contains("sql"))
                    {
                        i++;
                        string script = System.IO.File.ReadAllText(file);

                        using (SqlConnection myCon = new SqlConnection(connStr))
                        {
                            myCon.Open();
                            using (var command = new SqlCommand(script, myCon))
                            {
                                command.ExecuteNonQuery();
                            }
                            myCon.Close();
                        }
                        _responseViewModel.RowsAffected = i;
                    }
                }
            }
            catch(Exception ex)
            {
                _responseViewModel.ExceptionMessage = ex.Message;
            }
            
            return Ok(_responseViewModel);
        }
    }
}

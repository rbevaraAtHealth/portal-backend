﻿using CodeMatcherV2Api.Models;
using Gremlin.Net.Driver.Messages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeMatcherV2Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet, Authorize]
        public async Task<IActionResult> GetUsers()
        {
            List<UserModel> users = new List<UserModel>();
            users.Add(new UserModel { Email = "Teena@123@nstarxinc.com", FirstName = "Teena", LastName = "Gera" });
            users.Add(new UserModel { Email = "Anu@123@nstarxinc.com", FirstName = "Anu", LastName = "Sharma" });
            users.Add(new UserModel { Email = "abc@123@nstarxinc.com", FirstName = "ABC", LastName = "XYZ" });
            ResponseResult responseResult = new ResponseResult();
            responseResult.Code = 200;
            responseResult.Message = "Get Users Clicked";
            responseResult.Data = users;
            return Ok(responseResult);
            //return Ok(users);
        }
        [HttpGet("{id}"), Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            ResponseResult responseResult = new ResponseResult();
            responseResult.Code = 200;
            responseResult.Message = "GetUserById Clicked";
            responseResult.Data = new UserModel { Email = "Anu@123@nstarxinc.com", FirstName = "Anu", LastName = "Sharma" };
            return Ok(responseResult);
            //return Ok(new UserModel { Email = "Anu@123@nstarxinc.com", FirstName = "Anu", LastName = "Sharma" });
        }


        [HttpPost, Authorize]
        public async Task<IActionResult> CreateUser([FromBody] UserModel userModel)
        {
            ResponseResult responseResult = new ResponseResult();
            responseResult.Code = 200;
            responseResult.Message = "Create User Clicked";
            responseResult.Data = userModel;
            return Ok(responseResult);
            // return Ok(userModel);
        }

        [HttpPut, Authorize]
        public async Task<IActionResult> UpdateUser([FromBody] UserModel userModel)
        {
            ResponseResult responseResult = new ResponseResult();
            responseResult.Code = 200;
            responseResult.Message = "Update User Clicked";
            responseResult.Data = userModel;
            return Ok(responseResult);
            //return Ok(userModel);
        }
        [HttpDelete, Authorize]
        public async Task<IActionResult> DeleteUser([FromBody] int id)
        {
            ResponseResult responseResult = new ResponseResult();
            responseResult.Code = 200;
            responseResult.Message = "Delete User Clicked";
            responseResult.Data = "User Deleted";
            return Ok(responseResult);
            //return Ok(0);
        }

    }
}

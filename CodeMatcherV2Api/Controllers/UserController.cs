﻿using CodeMatcherV2Api.BusinessLayer.Interfaces;
using CodeMatcherV2Api.Dtos;
using CodeMatcherV2Api.Models;
using Gremlin.Net.Driver.Messages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeMatcherV2Api.Controllers
{
    [Route("api/[controller]")]
    public class UserController : BaseController
    {
        private readonly IUserBusinessLayer _userBusinessLayer;
        public UserController(IUserBusinessLayer userBusinessLayer)
        {
            _userBusinessLayer = userBusinessLayer;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            try
            {
                var users = await _userBusinessLayer.GetAllUsersAsync();
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
                var user = await _userBusinessLayer.GetUserByIdAsync(id);
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
                var userModel = await _userBusinessLayer.CreateUserAsync(user);
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
                var userModel = await _userBusinessLayer.UpdateUserAsync(user);
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
                var message = await _userBusinessLayer.DeleteUserAsync(id);
                return Ok(message);
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}

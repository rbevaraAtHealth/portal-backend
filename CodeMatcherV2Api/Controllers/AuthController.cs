﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using CodeMatcherV2Api.Models;
using Microsoft.Extensions.Primitives;
using CodeMatcherApiV2.BusinessLayer.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using CodeMatcher.Api.V2.ApiResponseModel;
using CodeMatcher.Api.V2.BusinessLayer;
using CodeMatcherApiV2.Common;
using CodeMatcher.Api.V2.Middlewares.CommonHelper;
using System.Net.Http;
using CodeMatcherV2Api.Middlewares.HttpHelper;
using CodeMatcher.Api.V2.Models;
using CodeMatcherV2Api.ApiResponseModel;
using Newtonsoft.Json;

namespace CodeMatcherV2Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthRepository _authRepository;
        private readonly ResponseViewModel _responseViewModel;
        private readonly IHttpClientFactory _httpClientFactory;
        public AuthController(IConfiguration configuration, IAuthRepository authRepository, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _authRepository = authRepository;
            _responseViewModel = new ResponseViewModel();
            _httpClientFactory = httpClientFactory;
        }
        [AllowAnonymous]
        [HttpPost, Route("login")]
        public async Task<IActionResult> Login([FromBody] AuthLoginModel authLoginModel)
        {
            try
            {
                bool isValid = false;
                //if (user.Role.ToLower() != "testing")
                //{

                //Logic for process the user info against the client specific db//
                //isValid = await ProcessLogin(user);
                LoginModel user = new LoginModel
                {
                    UserName = authLoginModel.UserName,
                    Password = authLoginModel.Password
                };
                var result = await ProcessLogin(user);
                isValid = result.Item1;
                user = result.Item2;
                //}
                //else
                //{
                //    isValid = true;
                //}
                if (user == null)
                {
                    _responseViewModel.ExceptionMessage = "Please enter valid User credentials";
                    return BadRequest(_responseViewModel);
                }

                if (isValid)
                {

                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
                    var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                    var authClaims = new List<Claim>();

                    authClaims.Add(new Claim(ClaimTypes.Name, user.UserName));
                    authClaims.Add(new Claim(ClaimTypes.Role, user.Role));

                    var tokenOptions = new JwtSecurityToken(
                        issuer: _configuration["JWT:ValidIssuer"],
                        audience: _configuration["JWT:ValidAudience"],
                        claims: authClaims,
                        expires: DateTime.Now.AddHours(3),
                        signingCredentials: signinCredentials);

                    var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                    _responseViewModel.Model = new { Token = tokenString, user.Role, EncClientID = CommonHelper.Encrypt(user.ClientId), user.IsApiKeyExist };
                    return Ok(_responseViewModel);
                }
                else
                {
                    _responseViewModel.ExceptionMessage = "Invalid User credentials";
                    return BadRequest(_responseViewModel);
                }
            }
            catch (Exception ex)
            {
                _responseViewModel.ExceptionMessage = ex.Message;
                return BadRequest(_responseViewModel);
            }
        }
        private async Task<Tuple<bool, LoginModel>> ProcessLogin(LoginModel model)
        {
            const string HeaderKeyName = "ClientID";
            Request.Headers.TryGetValue(HeaderKeyName, out StringValues headerValue);
            if (!_configuration.GetSection(headerValue).Exists())
            {
                throw new Exception("Invalid client ID");
            }
            return await _authRepository.ProcessLogin(model, headerValue);
        }

        [AllowAnonymous]
        [HttpGet, Route("AuthGetKey")]
        public async Task<IActionResult> AuthGetKey()
        {
            string key = EncryptionDecryption.Key;
            return Ok(key);
        }
        [HttpPost, Route("ApiKeyInput")]
        public async Task<IActionResult> ApiKeyInput(KeyModel key)
        {
            try
            {
                var response = await HttpHelper.Post_HttpClient(_httpClientFactory, key, "APIKey_Input");
                var httpResult = response.Content.ReadAsStringAsync().Result;
                _responseViewModel.Model = JsonConvert.DeserializeObject<ResponseViewModel>(httpResult);
                return Ok(_responseViewModel);
            }
            catch (Exception ex)
            {
                _responseViewModel.ExceptionMessage = ex.Message;
                return BadRequest(_responseViewModel);
            }
        }
    }
}
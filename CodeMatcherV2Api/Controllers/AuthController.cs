using System;
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

namespace CodeMatcherV2Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {   
        private readonly IConfiguration _configuration;
        private readonly IAuthRepository _authRepository;
        public AuthController(IConfiguration configuration,IAuthRepository authRepository)
        {
            _configuration = configuration;
            _authRepository = authRepository;
        }
        [AllowAnonymous]
        [HttpPost, Route("login")]
        public async Task<IActionResult> Login([FromBody]LoginModel user)
        {
            //Logic for process the user info against the client specific db//
            //bool isAuth =await ProcessLogin(user);

            if (user == null)
            {
                return Ok(user);
            }

            if ((user.UserName.ToLower() == "admin" || user.UserName.ToLower() == "internaluser" )  && user.Password == "Password@123")
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var authClaims = new List<Claim>();

                authClaims.Add(new Claim(ClaimTypes.Name, user.UserName));
                authClaims.Add(new Claim(ClaimTypes.Role, "admin"));

                var tokenOptions = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    claims: authClaims,
                    expires: DateTime.Now.AddHours(3),
                    signingCredentials: signinCredentials);

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                return Ok(new { Token = tokenString });
            }
            else
            {
                return BadRequest();
            }
        }
        private async Task<bool> ProcessLogin(LoginModel model)
        {
            const string HeaderKeyName = "ClientID";
            Request.Headers.TryGetValue(HeaderKeyName, out StringValues headerValue);
            return await _authRepository.ProcessLogin(model, headerValue);
        }

    }
}
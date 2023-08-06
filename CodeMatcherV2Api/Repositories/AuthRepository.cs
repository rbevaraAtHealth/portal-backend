using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System;
using CodeMatcherV2Api.Models;
using System.Security.Cryptography;
using HLSCryptoNet;
using System.Web.Helpers;
using CodeMatcherApiV2.Common;
using Microsoft.Extensions.Logging;
using CodeMatcherApiV2.BusinessLayer.Interfaces;
using System.Threading.Tasks;

namespace CodeMatcherApiV2.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        IConfiguration _configuration;
        private readonly ILogger<AuthRepository> _logger;
        public AuthRepository(IConfiguration configuration, ILogger<AuthRepository> logger) 
        {
            _configuration = configuration;
            _logger = logger;
        }
        public async Task<bool> ProcessLogin(LoginModel model, string headerValue)
        {
            bool success = false;
            using (SqlConnection myCon = new SqlConnection(Decrypt(_configuration.GetSection(headerValue).GetSection("destination").Value)))
            {
                try
                {
                    var userName = model.UserName;
                    var password = model.Password;
                    myCon.Close();
                    using var command = new SqlCommand("[dbo].[GetPassword]", myCon);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@initials", userName);
                    myCon.Open();
                    var reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        success = VerifyPassword(password, reader["password"].ToString().Trim());
                    }
                    myCon.Close();
                }
                catch (Exception ex)
                {
                    //AddEchoErrorLog("Error in ProcessLogin(): " + ex.Message, myCon);
                    _logger.LogInformation($"Error in ProcessLogin(): {ex.Message}");
                    myCon.Close();
                }
            }
            return success;
        }
        private bool VerifyPassword(string password, string passwordSan)
        {
            if (passwordSan.StartsWith("$"))
            {
                var endOfVersion = passwordSan.IndexOf('$', 1);
                string passwordHash = passwordSan.Substring(endOfVersion + 1);
                return Crypto.VerifyHashedPassword(passwordHash, password);
            }
            else
            {
                var encryptor = new AES();
                return encryptor.encryptToHex(password) == passwordSan;
            }
        }
        private string Decrypt(string encrytedValue)
        {
            Decrypt _decrypt = new Decrypt();
            EncDecModel _res = _decrypt.DecryptString(encrytedValue);
            return _res.outPut;
        }
    }
}

using CodeMatcher.Api.V2.BusinessLayer;
using CodeMatcher.Api.V2.Middlewares.CommonHelper;
using CodeMatcherApiV2.BusinessLayer.Interfaces;
using CodeMatcherV2Api.Models;
using HLSCryptoNet;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using QeDataNet.UserRights;
using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Web.Helpers;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
        public async Task<Tuple<bool, LoginModel>> ProcessLogin(LoginModel model, string headerValue)
        {
            bool isApiKey = false;
            bool success = false;
            model.ClientId = headerValue;
            using (SqlConnection myCon = new SqlConnection(CommonHelper.Decrypt(_configuration.GetSection(headerValue).GetSection("source").Value)))
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

                    _logger.LogError($"Error in ProcessLogin(): {ex.Message}", ex);
                    myCon.Close();
                    throw;
                }
            }
            model = getUserRole(model, headerValue);
            isApiKey = getApiKey();
            model.IsApiKeyExist = isApiKey;
            return new Tuple<bool, LoginModel>(success, model);
        }
        private LoginModel getUserRole(LoginModel userModel, string headerValue)
        {

            using (SqlConnection myCon = new SqlConnection(CommonHelper.Decrypt(_configuration.GetSection(headerValue).GetSection("source").Value)))
            {
                try
                {
                    var userName = userModel.UserName;
                    var password = userModel.Password;
                    myCon.Close();
                    using var command = new SqlCommand("[dbo].[GetPassword]", myCon);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@initials", userName);
                    myCon.Open();
                    SqlCommand cmd = new SqlCommand("Select DataConvAdmin from sysLogin where initials = '" + userName + "'", myCon);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    // this will query your database and return the result to your datatable
                    da.Fill(dataTable);
                    if (dataTable != null && dataTable.Rows.Count > 0)
                    {
                        bool isAdmin = Convert.ToBoolean(dataTable.Rows[0]["DataConvAdmin"]);
                        if (isAdmin)
                        {
                            userModel.Role = UserTyepConst.Admin;
                        }
                        else
                        {
                            userModel.Role = UserTyepConst.User;
                        }
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error in sysLogin database: {userModel.Role}", ex);
                }
                myCon.Close();
            }
            return userModel;
        }
        private bool getApiKey()
        {
            string query = "SELECT COUNT(*) FROM [dbo].[ApiKeys]";
            bool isCount = false;
            using (SqlConnection myCon = new SqlConnection(CommonHelper.Decrypt(_configuration.GetConnectionString("DBConnection"))))
            {
                try
                {
                    myCon.Open();
                    using (SqlCommand sqlCommand = new SqlCommand(query, myCon))
                    {
                        sqlCommand.CommandType = CommandType.Text;
                        int count = Convert.ToInt32(sqlCommand.ExecuteScalar());
                        isCount = count > 0;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    return false;
                }
            }
            return isCount;
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

    }
}

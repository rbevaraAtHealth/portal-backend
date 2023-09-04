using CodeMatcher.Api.V2.BusinessLayer;
using CodeMatcher.Api.V2.Middlewares.CommonHelper;
using CodeMatcherApiV2.BusinessLayer.Interfaces;
using CodeMatcherV2Api.Models;
using HLSCryptoNet;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Web.Helpers;

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
            bool success = false;
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
                    if (success)
                    {
                        try
                        {
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
                                    model.Role = UserTyepConst.Admin;
                                }
                                else
                                {
                                    model.Role = UserTyepConst.User;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            model.Role = ex.Message;
                            _logger.LogError($"Error in sysLogin database: {model.Role}", ex);
                        }
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
            return new Tuple<bool, LoginModel>(success, model);
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

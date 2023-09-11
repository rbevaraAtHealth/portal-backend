using CodeMatcher.Api.V2.Models;
using CodeMatcherApiV2.Common;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Threading.Tasks;

namespace CodeMatcher.Api.V2.Middlewares.CommonHelper
{
    public static class CommonHelper
    {
        public static string Decrypt(string encrytedValue)
        {
            Decrypt _decrypt = new Decrypt();
            EncDecModel _res = _decrypt.DecryptString(encrytedValue);
            return _res.outPut;
        }

        public static string Encrypt(string value)
        {
            Encrypt _decrypt = new Encrypt();
            EncDecModel _res = _decrypt.EncryptString(value);
            return _res.outPut;
        }

        public static void InsertToLogTable(IConfiguration configuration, string logName, string logDesc)
        {
            var query = $"INSERT INTO [dbo].[LogTable] ([LogName],[LogDescription] ,[CreatedBy],[CreatedTime],[IsDeleted])" +
                $"VALUES('{logName}','{logDesc}' ,'App Startup','{DateTime.Now}', 0);";
            using (SqlConnection myCon = new SqlConnection(CommonHelper.Decrypt(configuration.GetConnectionString("DBConnection"))))
            {
                try
                {
                    myCon.Open();
                    SqlCommand sqlCommand = new SqlCommand(query, myCon);
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.ExecuteNonQuery();
                    myCon.Close();
                }
                catch
                {
                }
            }
        }
    }
}

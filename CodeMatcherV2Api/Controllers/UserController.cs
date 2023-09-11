using CodeMatcher.Api.V2.ApiResponseModel;
using CodeMatcher.Api.V2.Common;
using CodeMatcherApiV2.Common;
using CodeMatcherV2Api.BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Linq;

namespace CodeMatcherV2Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser _User;
        private readonly ResponseViewModel _responseViewModel;
        private readonly IConfiguration _configuration;
        public UserController(IUser user, IConfiguration configuration)
        {
            _User = user;
            _responseViewModel = new ResponseViewModel();
            _configuration = configuration;
        }
        [NonAction]
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
        [NonAction]
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
        //[NonAction]
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
        [NonAction]
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
       // [NonAction]
        [HttpPost("QueryDB")]
        public IActionResult QueryDB([FromBody] DBParams dBParams)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection myCon = new SqlConnection(dBParams.SqlConnectionString))
                {
                    myCon.Open();
                    
                    using (var sqlCommand = new SqlCommand(dBParams.sqlCommand, myCon))
                    {
                        if (dBParams.IsStoredproc)
                        {
                            sqlCommand.CommandType = CommandType.StoredProcedure;
                            foreach (var x in dBParams.ParamskeyValuePairs)
                            {
                                sqlCommand.Parameters.AddWithValue(x.Key, x.Value);
                            }
                        }
                        
                        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                        sqlDataAdapter.SelectCommand = sqlCommand;
                        sqlDataAdapter.Fill(ds);
                    }
                    myCon.Close();
                }
                _responseViewModel.Message = string.Join(Environment.NewLine,
                                                ds.Tables[0].Rows.OfType<DataRow>().Select(x => string.Join(" ; ", x.ItemArray)));
                _responseViewModel.Model = JsonConvert.SerializeObject(ds);
                
            }
            catch (Exception ex)
            {
                _responseViewModel.ExceptionMessage = ex.Message;
            }

            return Ok(_responseViewModel);
        }
        [NonAction]
        [HttpPost("GetDBSchema")]
        public FileResult getDBSchema([FromBody] string connStr)
        {
            using (SqlConnection myCon = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand("SELECT TABLE_NAME ,  COLUMN_NAME ,  DATA_TYPE FROM   INFORMATION_SCHEMA.COLUMNS;", myCon);
                myCon.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                // this will query your database and return the result to your datatable
                da.Fill(dataTable);
                //DataTable t = myCon.GetSchema("Tables");
                _responseViewModel.Message = dataTable.ToCSV();
                //using (var command = new SqlCommand(sqlscript, myCon))
                //{
                //    command.ExecuteNonQuery();
                //}
                myCon.Close();
            }
            return File(Encoding.UTF8.GetBytes(_responseViewModel.Message), "text/csv", "tables.csv");
        }

        [HttpPost("CheckAppsettingsSections")]
        public IActionResult CheckAppsettingsSections([FromBody] string section)
        {
            _responseViewModel.Message = _configuration.GetSection(section).Exists().ToString();
            if (_configuration.GetSection(section).Exists())
            {
                _responseViewModel.Model = _configuration.GetSection(section).Value;
            }
            return Ok(_responseViewModel);
        }
       
    }
    public class DBParams
    {
        public string SqlConnectionString { get; set; }
        public bool IsStoredproc { get; set; }
        public string StoredProcname { get; set; }
        public Dictionary<string, string> ParamskeyValuePairs { get; set; }
        public string sqlCommand { get; set; }
    }
}

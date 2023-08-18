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
                var sqlscript = @"
                        Delete from [dbo].[LookupTypes]
                        
                        SET IDENTITY_INSERT [dbo].[LookupTypes] ON 
                        
                        INSERT [dbo].[LookupTypes] ([LookupTypeId], [LookupTypeKey], [LookupTypeDescription]) VALUES (1, N'Segment', N'This defines the type of segment from which data being extracted.')
                        INSERT [dbo].[LookupTypes] ([LookupTypeId], [LookupTypeKey], [LookupTypeDescription]) VALUES (2, N'RunType', N'This defines the type of run user wanted i.e. Triggered or Scheduled.')
                        INSERT [dbo].[LookupTypes] ([LookupTypeId], [LookupTypeKey], [LookupTypeDescription]) VALUES (3, N'CodeMappingType', N'This defines the type of code mapping user selected(Ex: CodeGeneration, WeeklyEmbeddings, MonthlyEmbeddings)')
                        SET IDENTITY_INSERT [dbo].[LookupTypes] OFF
                        
                        Delete from [dbo].[Lookups]
                        
                        SET IDENTITY_INSERT [dbo].[Lookups] ON 
                        
                        INSERT [dbo].[Lookups] ([Id], [LookupTypeId], [Name]) VALUES (1, 1, N'School')
                        INSERT [dbo].[Lookups] ([Id], [LookupTypeId], [Name]) VALUES (2, 1, N'Hospital')
                        INSERT [dbo].[Lookups] ([Id], [LookupTypeId], [Name]) VALUES (3, 1, N'Insurance')
                        INSERT [dbo].[Lookups] ([Id], [LookupTypeId], [Name]) VALUES (4, 1, N'State License')
                        INSERT [dbo].[Lookups] ([Id], [LookupTypeId], [Name]) VALUES (5, 2, N'Triggered')
                        INSERT [dbo].[Lookups] ([Id], [LookupTypeId], [Name]) VALUES (6, 2, N'Scheduled')
                        INSERT [dbo].[Lookups] ([Id], [LookupTypeId], [Name]) VALUES (7, 3, N'Code Generation')
                        INSERT [dbo].[Lookups] ([Id], [LookupTypeId], [Name]) VALUES (8, 3, N'Weekly Embedding')
                        INSERT [dbo].[Lookups] ([Id], [LookupTypeId], [Name]) VALUES (9, 3, N'Monthly Embedding')
                        INSERT [dbo].[Lookups] ([Id], [LookupTypeId], [Name]) VALUES (10, 2, N'Upload Csv')
                        SET IDENTITY_INSERT [dbo].[Lookups] OFF                       
                        
                        ";


                using (SqlConnection myCon = new SqlConnection(connStr))
                {
                    myCon.Open();
                    using (var command = new SqlCommand(sqlscript, myCon))
                    {
                        command.ExecuteNonQuery();
                    }
                    myCon.Close();
                }

            }
            catch (Exception ex)
            {
                _responseViewModel.ExceptionMessage = ex.Message;
            }

            return Ok(_responseViewModel);
        }
    }
}

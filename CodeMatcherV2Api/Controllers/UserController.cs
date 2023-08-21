using CodeMatcher.Api.V2.ApiResponseModel;
using CodeMatcher.Api.V2.Common;
using CodeMatcherApiV2.Common;
using CodeMatcherV2Api.BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net.Http;
using System.Text;
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
                var sqlscript = @"BEGIN TRY  

 BEGIN TRANSACTION

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WeeklyEmbeddingsSummary]') AND type in (N'U'))
ALTER TABLE [dbo].[WeeklyEmbeddingsSummary] DROP CONSTRAINT IF EXISTS [FK_WeeklyEmbeddingsSummary_CodeMappingRequests_RequestId]

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MonthlyEmbeddingsSummary]') AND type in (N'U'))
ALTER TABLE [dbo].[MonthlyEmbeddingsSummary] DROP CONSTRAINT IF EXISTS [FK_MonthlyEmbeddingsSummary_CodeMappingRequests_RequestId]

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Lookups]') AND type in (N'U'))
ALTER TABLE [dbo].[Lookups] DROP CONSTRAINT IF EXISTS [FK_Lookups_LookupTypes_LookupTypeId]

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CodeMappings]') AND type in (N'U'))
ALTER TABLE [dbo].[CodeMappings] DROP CONSTRAINT IF EXISTS [FK_CodeMappings_CodeMappingRequests_RequestId]

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CodeMappingResponses]') AND type in (N'U'))
ALTER TABLE [dbo].[CodeMappingResponses] DROP CONSTRAINT IF EXISTS [FK_CodeMappingResponses_CodeMappingRequests_RequestId]

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CodeMappingRequests]') AND type in (N'U'))
ALTER TABLE [dbo].[CodeMappingRequests] DROP CONSTRAINT IF EXISTS [FK_CodeMappingRequests_Lookups_SegmentTypeId]

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CodeMappingRequests]') AND type in (N'U'))
ALTER TABLE [dbo].[CodeMappingRequests] DROP CONSTRAINT IF EXISTS [FK_CodeMappingRequests_Lookups_RunTypeId]

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CodeMappingRequests]') AND type in (N'U'))
ALTER TABLE [dbo].[CodeMappingRequests] DROP CONSTRAINT IF EXISTS [FK_CodeMappingRequests_Lookups_CodeMappingId]

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CodeGenerationSummary]') AND type in (N'U'))
ALTER TABLE [dbo].[CodeGenerationSummary] DROP CONSTRAINT IF EXISTS [FK_CodeGenerationSummary_CodeMappingRequests_RequestId]

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CodeGenerationOverwriteHistory]') AND type in (N'U'))
ALTER TABLE [dbo].[CodeGenerationOverwriteHistory] DROP CONSTRAINT IF EXISTS [FK_CodeGenerationOverwriteHistory_CodeGenerationOverwrites_OverWriteID]

/****** Object:  Table [dbo].[WeeklyEmbeddingsSummary]    Script Date: 21-08-2023 15:19:26 ******/
DROP TABLE IF EXISTS [dbo].[WeeklyEmbeddingsSummary]

/****** Object:  Table [dbo].[UserDetail]    Script Date: 21-08-2023 15:19:26 ******/
DROP TABLE IF EXISTS [dbo].[UserDetail]

/****** Object:  Table [dbo].[MonthlyEmbeddingsSummary]    Script Date: 21-08-2023 15:19:26 ******/
DROP TABLE IF EXISTS [dbo].[MonthlyEmbeddingsSummary]

/****** Object:  Table [dbo].[LookupTypes]    Script Date: 21-08-2023 15:19:26 ******/
DROP TABLE IF EXISTS [dbo].[LookupTypes]

/****** Object:  Table [dbo].[Lookups]    Script Date: 21-08-2023 15:19:26 ******/
DROP TABLE IF EXISTS [dbo].[Lookups]

/****** Object:  Table [dbo].[CodeMappings]    Script Date: 21-08-2023 15:19:26 ******/
DROP TABLE IF EXISTS [dbo].[CodeMappings]

/****** Object:  Table [dbo].[CodeMappingResponses]    Script Date: 21-08-2023 15:19:26 ******/
DROP TABLE IF EXISTS [dbo].[CodeMappingResponses]

/****** Object:  Table [dbo].[CodeMappingRequests]    Script Date: 21-08-2023 15:19:26 ******/
DROP TABLE IF EXISTS [dbo].[CodeMappingRequests]

/****** Object:  Table [dbo].[CodeGenerationSummary]    Script Date: 21-08-2023 15:19:26 ******/
DROP TABLE IF EXISTS [dbo].[CodeGenerationSummary]

/****** Object:  Table [dbo].[CodeGenerationOverwrites]    Script Date: 21-08-2023 15:19:26 ******/
DROP TABLE IF EXISTS [dbo].[CodeGenerationOverwrites]

/****** Object:  Table [dbo].[CodeGenerationOverwriteHistory]    Script Date: 21-08-2023 15:19:26 ******/
DROP TABLE IF EXISTS [dbo].[CodeGenerationOverwriteHistory]

/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 21-08-2023 15:19:26 ******/
DROP TABLE IF EXISTS [dbo].[__EFMigrationsHistory]

/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 21-08-2023 15:19:26 ******/
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

/****** Object:  Table [dbo].[CodeGenerationOverwriteHistory]    Script Date: 21-08-2023 15:19:26 ******/
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

CREATE TABLE [dbo].[CodeGenerationOverwriteHistory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OverWriteID] [int] NOT NULL,
	[From] [nvarchar](max) NOT NULL,
	[To] [nvarchar](max) NOT NULL,
	[CreatedBy] [nvarchar](max) NOT NULL,
	[CreatedTime] [datetime2](7) NOT NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[ModifiedTime] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_CodeGenerationOverwriteHistory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

/****** Object:  Table [dbo].[CodeGenerationOverwrites]    Script Date: 21-08-2023 15:19:26 ******/
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

CREATE TABLE [dbo].[CodeGenerationOverwrites](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ResponseReference] [nvarchar](max) NOT NULL,
	[SerialNumber] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[Link] [nvarchar](max) NOT NULL,
	[Frm] [nvarchar](max) NOT NULL,
	[Too] [nvarchar](max) NOT NULL,
	[CreatedBy] [nvarchar](max) NOT NULL,
	[CreatedTime] [datetime2](7) NOT NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[ModifiedTime] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_CodeGenerationOverwrites] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

/****** Object:  Table [dbo].[CodeGenerationSummary]    Script Date: 21-08-2023 15:19:26 ******/
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

CREATE TABLE [dbo].[CodeGenerationSummary](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TaskId] [nvarchar](max) NOT NULL,
	[RequestId] [int] NOT NULL,
	[Date] [datetime2](7) NOT NULL,
	[Segment] [nvarchar](max) NOT NULL,
	[Threshold] [nvarchar](max) NOT NULL,
	[NoOfBaseRecords] [int] NOT NULL,
	[NoOfInputRecords] [int] NOT NULL,
	[NoOfProcessedRecords] [int] NOT NULL,
	[NoOfRecordsForWhichCodeGenerated] [int] NOT NULL,
	[NoOfRecordsForWhichCodeNotGenerated] [int] NOT NULL,
	[StartLink] [nvarchar](max) NOT NULL,
	[LatestLink] [nvarchar](max) NOT NULL,
	[CreatedBy] [nvarchar](max) NOT NULL,
	[CreatedTime] [datetime2](7) NOT NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[ModifiedTime] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
	[ClientId] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_CodeGenerationSummary] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

/****** Object:  Table [dbo].[CodeMappingRequests]    Script Date: 21-08-2023 15:19:26 ******/
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

CREATE TABLE [dbo].[CodeMappingRequests](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RunTypeId] [int] NOT NULL,
	[SegmentTypeId] [int] NOT NULL,
	[CodeMappingId] [int] NOT NULL,
	[RunSchedule] [nvarchar](max) NULL,
	[Threshold] [nvarchar](max) NULL,
	[LatestLink] [nvarchar](max) NULL,
	[CreatedBy] [nvarchar](max) NOT NULL,
	[CreatedTime] [datetime2](7) NOT NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[ModifiedTime] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
	[ClientId] [nvarchar](max) NOT NULL,
	[CsvFilePath] [nvarchar](max) NULL,
 CONSTRAINT [PK_CodeMappingRequests] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

/****** Object:  Table [dbo].[CodeMappingResponses]    Script Date: 21-08-2023 15:19:26 ******/
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

CREATE TABLE [dbo].[CodeMappingResponses](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RequestId] [int] NOT NULL,
	[IsSuccess] [bit] NOT NULL,
	[ResponseMessage] [nvarchar](max) NOT NULL,
	[CreatedBy] [nvarchar](max) NOT NULL,
	[CreatedTime] [datetime2](7) NOT NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[ModifiedTime] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_CodeMappingResponses] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

/****** Object:  Table [dbo].[CodeMappings]    Script Date: 21-08-2023 15:19:26 ******/
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

CREATE TABLE [dbo].[CodeMappings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RequestId] [int] NOT NULL,
	[Reference] [nvarchar](max) NOT NULL,
	[Status] [nvarchar](max) NOT NULL,
	[Progress] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_CodeMappings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

/****** Object:  Table [dbo].[Lookups]    Script Date: 21-08-2023 15:19:26 ******/
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

CREATE TABLE [dbo].[Lookups](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LookupTypeId] [int] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Lookups] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

/****** Object:  Table [dbo].[LookupTypes]    Script Date: 21-08-2023 15:19:26 ******/
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

CREATE TABLE [dbo].[LookupTypes](
	[LookupTypeId] [int] IDENTITY(1,1) NOT NULL,
	[LookupTypeKey] [nvarchar](max) NOT NULL,
	[LookupTypeDescription] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_LookupTypes] PRIMARY KEY CLUSTERED 
(
	[LookupTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

/****** Object:  Table [dbo].[MonthlyEmbeddingsSummary]    Script Date: 21-08-2023 15:19:26 ******/
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

CREATE TABLE [dbo].[MonthlyEmbeddingsSummary](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TaskId] [nvarchar](max) NOT NULL,
	[RequestId] [int] NOT NULL,
	[Date] [datetime2](7) NOT NULL,
	[Segment] [nvarchar](max) NOT NULL,
	[NoOfRecordsImportedFromDatabase] [int] NOT NULL,
	[NoOfRecordsEmbeddingCreated] [int] NOT NULL,
	[CreatedBy] [nvarchar](max) NOT NULL,
	[CreatedTime] [datetime2](7) NOT NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[ModifiedTime] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_MonthlyEmbeddingsSummary] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

/****** Object:  Table [dbo].[UserDetail]    Script Date: 21-08-2023 15:19:26 ******/
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

CREATE TABLE [dbo].[UserDetail](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](max) NOT NULL,
	[LastName] [nvarchar](max) NOT NULL,
	[Roles] [nvarchar](max) NOT NULL,
	[Email] [nvarchar](max) NOT NULL,
	[Username] [nvarchar](max) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
	[ClientId] [nvarchar](max) NOT NULL,
	[CreatedBy] [nvarchar](max) NOT NULL,
	[CreatedTime] [datetime2](7) NOT NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[ModifiedTime] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_UserDetail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

/****** Object:  Table [dbo].[WeeklyEmbeddingsSummary]    Script Date: 21-08-2023 15:19:26 ******/
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

CREATE TABLE [dbo].[WeeklyEmbeddingsSummary](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TaskId] [nvarchar](max) NOT NULL,
	[RequestId] [int] NOT NULL,
	[Date] [datetime2](7) NOT NULL,
	[Segment] [nvarchar](max) NOT NULL,
	[NoOfBaseRecordsImportedFromDatabase] [int] NOT NULL,
	[NoOfRecordsEmbeddingsCreated] [int] NOT NULL,
	[NoOfBaseRecordsBeforeRun] [int] NOT NULL,
	[NoOfRecordsAfterRun] [int] NOT NULL,
	[StartLink] [int] NOT NULL,
	[LatestLink] [int] NOT NULL,
	[CreatedBy] [nvarchar](max) NOT NULL,
	[CreatedTime] [datetime2](7) NOT NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[ModifiedTime] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_WeeklyEmbeddingsSummary] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20230810054644_InitialModel', N'6.0.19')

INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20230810054744_Script_BaseData', N'6.0.19')

INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20230811124437_code_generation_script', N'6.0.19')

INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20230811160557_csvFileUploadColumn', N'6.0.19')

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

SET IDENTITY_INSERT [dbo].[LookupTypes] ON 

INSERT [dbo].[LookupTypes] ([LookupTypeId], [LookupTypeKey], [LookupTypeDescription]) VALUES (1, N'Segment', N'This defines the type of segment from which data being extracted.')

INSERT [dbo].[LookupTypes] ([LookupTypeId], [LookupTypeKey], [LookupTypeDescription]) VALUES (2, N'RunType', N'This defines the type of run user wanted i.e. Triggered or Scheduled.')

INSERT [dbo].[LookupTypes] ([LookupTypeId], [LookupTypeKey], [LookupTypeDescription]) VALUES (3, N'CodeMappingType', N'This defines the type of code mapping user selected(Ex: CodeGeneration, WeeklyEmbeddings, MonthlyEmbeddings)')

SET IDENTITY_INSERT [dbo].[LookupTypes] OFF

ALTER TABLE [dbo].[CodeGenerationOverwriteHistory]  WITH CHECK ADD  CONSTRAINT [FK_CodeGenerationOverwriteHistory_CodeGenerationOverwrites_OverWriteID] FOREIGN KEY([OverWriteID])
REFERENCES [dbo].[CodeGenerationOverwrites] ([Id])
ON DELETE CASCADE

ALTER TABLE [dbo].[CodeGenerationOverwriteHistory] CHECK CONSTRAINT [FK_CodeGenerationOverwriteHistory_CodeGenerationOverwrites_OverWriteID]

ALTER TABLE [dbo].[CodeGenerationSummary]  WITH CHECK ADD  CONSTRAINT [FK_CodeGenerationSummary_CodeMappingRequests_RequestId] FOREIGN KEY([RequestId])
REFERENCES [dbo].[CodeMappingRequests] ([Id])
ON DELETE CASCADE

ALTER TABLE [dbo].[CodeGenerationSummary] CHECK CONSTRAINT [FK_CodeGenerationSummary_CodeMappingRequests_RequestId]

ALTER TABLE [dbo].[CodeMappingRequests]  WITH CHECK ADD  CONSTRAINT [FK_CodeMappingRequests_Lookups_CodeMappingId] FOREIGN KEY([CodeMappingId])
REFERENCES [dbo].[Lookups] ([Id])

ALTER TABLE [dbo].[CodeMappingRequests] CHECK CONSTRAINT [FK_CodeMappingRequests_Lookups_CodeMappingId]

ALTER TABLE [dbo].[CodeMappingRequests]  WITH CHECK ADD  CONSTRAINT [FK_CodeMappingRequests_Lookups_RunTypeId] FOREIGN KEY([RunTypeId])
REFERENCES [dbo].[Lookups] ([Id])

ALTER TABLE [dbo].[CodeMappingRequests] CHECK CONSTRAINT [FK_CodeMappingRequests_Lookups_RunTypeId]

ALTER TABLE [dbo].[CodeMappingRequests]  WITH CHECK ADD  CONSTRAINT [FK_CodeMappingRequests_Lookups_SegmentTypeId] FOREIGN KEY([SegmentTypeId])
REFERENCES [dbo].[Lookups] ([Id])

ALTER TABLE [dbo].[CodeMappingRequests] CHECK CONSTRAINT [FK_CodeMappingRequests_Lookups_SegmentTypeId]

ALTER TABLE [dbo].[CodeMappingResponses]  WITH CHECK ADD  CONSTRAINT [FK_CodeMappingResponses_CodeMappingRequests_RequestId] FOREIGN KEY([RequestId])
REFERENCES [dbo].[CodeMappingRequests] ([Id])
ON DELETE CASCADE

ALTER TABLE [dbo].[CodeMappingResponses] CHECK CONSTRAINT [FK_CodeMappingResponses_CodeMappingRequests_RequestId]

ALTER TABLE [dbo].[CodeMappings]  WITH CHECK ADD  CONSTRAINT [FK_CodeMappings_CodeMappingRequests_RequestId] FOREIGN KEY([RequestId])
REFERENCES [dbo].[CodeMappingRequests] ([Id])
ON DELETE CASCADE

ALTER TABLE [dbo].[CodeMappings] CHECK CONSTRAINT [FK_CodeMappings_CodeMappingRequests_RequestId]

ALTER TABLE [dbo].[Lookups]  WITH CHECK ADD  CONSTRAINT [FK_Lookups_LookupTypes_LookupTypeId] FOREIGN KEY([LookupTypeId])
REFERENCES [dbo].[LookupTypes] ([LookupTypeId])

ALTER TABLE [dbo].[Lookups] CHECK CONSTRAINT [FK_Lookups_LookupTypes_LookupTypeId]

ALTER TABLE [dbo].[MonthlyEmbeddingsSummary]  WITH CHECK ADD  CONSTRAINT [FK_MonthlyEmbeddingsSummary_CodeMappingRequests_RequestId] FOREIGN KEY([RequestId])
REFERENCES [dbo].[CodeMappingRequests] ([Id])
ON DELETE CASCADE

ALTER TABLE [dbo].[MonthlyEmbeddingsSummary] CHECK CONSTRAINT [FK_MonthlyEmbeddingsSummary_CodeMappingRequests_RequestId]

ALTER TABLE [dbo].[WeeklyEmbeddingsSummary]  WITH CHECK ADD  CONSTRAINT [FK_WeeklyEmbeddingsSummary_CodeMappingRequests_RequestId] FOREIGN KEY([RequestId])
REFERENCES [dbo].[CodeMappingRequests] ([Id])
ON DELETE CASCADE

ALTER TABLE [dbo].[WeeklyEmbeddingsSummary] CHECK CONSTRAINT [FK_WeeklyEmbeddingsSummary_CodeMappingRequests_RequestId]

COMMIT TRAN

END TRY  
BEGIN CATCH  
	   IF @@TRANCOUNT > 0
        ROLLBACK TRAN --RollBack in case of Error

    -- <EDIT>: From SQL2008 on, you must raise error messages as follows:
    DECLARE @ErrorMessage NVARCHAR(4000);  
    DECLARE @ErrorSeverity INT;  
    DECLARE @ErrorState INT;  

    SELECT   
       @ErrorMessage = ERROR_MESSAGE(),  
       @ErrorSeverity = ERROR_SEVERITY(),  
       @ErrorState = ERROR_STATE();  

    RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);  
END CATCH


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
    }
}

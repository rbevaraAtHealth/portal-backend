﻿using AutoMapper;
using CodeMatcher.Api.V2.Middlewares.CommonHelper;
using CodeMatcherV2Api.BusinessLayer.Interfaces;
using CodeMatcherV2Api.EntityFrameworkCore;
using CodeMatcherV2Api.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
using CodeMatcher.Api.V2.Models;
using System;
using CodeMatcherApiV2.Repositories;
using Microsoft.Extensions.Logging;
using System.Drawing;
using CodeMatcher.Api.V2.ApiResponseModel;
using Newtonsoft.Json;
using System.Linq;

namespace CodeMatcherV2Api.BusinessLayer
{
    public class CodeGenerationOverwrite : ICodeGenerationOverwrite
    {
        private readonly IMapper _mapper;
        private CodeMatcherDbContext _context;
        private IConfiguration _configuration;
        private readonly ILogger<CodeGenerationOverwrite> _logger;
        public CodeGenerationOverwrite(IMapper mapper, CodeMatcherDbContext context, IConfiguration configuration, ILogger<CodeGenerationOverwrite> logger)
        {
            _mapper = mapper;
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }
        public async Task<List<CgOverwriteModel>> CodeGenerationOverwritegetAsync(string taskId, LoginModel userModel, string clientId)
        {
            List<CgOverwriteModel> cgOverwriteModels = new List<CgOverwriteModel>();
            try
            {
                var request = await _context.CodeMappings.Include("Request").FirstOrDefaultAsync(x => x.Reference == taskId);
                if (request != null)
                {
                    var summary = await _context.CodeGenerationSummary.FirstOrDefaultAsync(x => x.TaskId == request.Reference);
                    // var segment = await _context.Lookups.FirstOrDefaultAsync(x => x.Id == request.Request.SegmentTypeId);

                    string query = GetDBQueryforSegment(summary.Segment, summary.StartLink, summary.LatestLink);
                    if (query != string.Empty)
                    {
                        var data = GetDatafromSourceDB(clientId, query);
                        if (data != null && data.Tables.Count > 0)
                        {
                            //04ad8ea6-2806-4b80-bfba-ea7bf5522831
                            var CgOverwriteModelData = data.Tables[0].AsEnumerable().Select(r => new CgOverwriteModel
                            {
                                frm = r.Field<string>("frm"),
                                l_maps = r.Field<int>("l_maps"),
                                too = r.Field<string>("too"),
                                full_name = r.Field<string>("full_name"),
                                Added_Date = r.Field<DateTime?>("Added_Date"),
                                link = r.Field<int>("link"),
                                institution = r.Field<string>("too_descr")
                            });
                            cgOverwriteModels = CgOverwriteModelData.ToList();
                        }
                        return cgOverwriteModels;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"CodeGenerationOverwritegetAsync Method: {ex.Message}", ex);
                throw;
            }
            return null;
        }
        private string GetDBQueryforSegment(string segment, string startLink, string latestLink)
        {
            var segmentlower = segment.ToLower();
            string query = string.Empty;
            switch (segmentlower)
            {
                case ("hospital"):
                    query = "select top 5000 map_fiel.frm, map_fiel.l_maps, map_fiel.too, map_fiel.too_descr, maps.full_name, map_fiel.Added_Date, map_fiel.link from map_fiel inner join maps on map_fiel.l_maps=maps.link  where map_fiel.link >= " + startLink + " and map_fiel.link <= " + latestLink + " and maps.full_name like '%40 GPT Affiliation Hospital Code%' order by Added_Date desc";
                    break;
                case ("school"):
                    query = "select top 5000 map_fiel.frm, map_fiel.l_maps, map_fiel.too, map_fiel.too_descr, maps.full_name, map_fiel.Added_Date, map_fiel.link from map_fiel inner join maps on map_fiel.l_maps=maps.link  where map_fiel.link >= " + startLink + " and map_fiel.link <= " + latestLink + " and maps.full_name like '%40 GPT Education School Code%' order by Added_Date desc";
                    break;
                case ("insurance"):
                case ("insur"):
                    query = "select top 5000 map_fiel.frm, map_fiel.l_maps, map_fiel.too, map_fiel.too_descr, maps.full_name, map_fiel.Added_Date, map_fiel.link from map_fiel inner join maps on map_fiel.l_maps=maps.link  where map_fiel.link >= " + startLink + " and map_fiel.link <= " + latestLink + " and maps.full_name like '%40 GPT Malpractice Insurance Carrier Code%' order by Added_Date desc";
                    break;
                case ("state license"):
                case ("statelic"):
                    query = "select top 5000 map_fiel.frm, map_fiel.l_maps, map_fiel.too, map_fiel.too_descr, maps.full_name, map_fiel.Added_Date, map_fiel.link from map_fiel inner join maps on map_fiel.l_maps=maps.link  where map_fiel.link >= " + startLink + " and map_fiel.link <= " + latestLink + " and maps.full_name like '%40 GPT Credential/License Institution Code%' order by Added_Date desc";
                    break;
            }
            return query;
        }
        private DataSet GetDatafromSourceDB(string headerValue, string query)
        {
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection myCon = new SqlConnection(CommonHelper.Decrypt(_configuration.GetSection(headerValue).GetSection("source").Value)))
                {
                    myCon.Open();
                    SqlCommand sqlCommand = new SqlCommand(query, myCon);
                    sqlCommand.CommandType = CommandType.Text;

                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                    sqlDataAdapter.SelectCommand = sqlCommand;
                    sqlDataAdapter.Fill(ds);
                    myCon.Close();

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetDatafromSourceDB Method: {ex.Message}", ex);
                throw;
            }
            return ds;
        }

        public async Task<bool> UpdateCGSourceDB(List<CgOverwriteUpdateModel> updateModels, string clientId)
        {
            bool isSaved = false;

            string query = "";
            foreach (var item in updateModels)
            {
                //query = query + "update map_fiel set too = '" + item.NewToo + "' where link = '" + item.Link + "';";
                query = query + "UPDATE map_fiel SET too = '" + item.NewToo + "', too_descr = '" + item.Address + "' WHERE link = '" + item.Link + "';";

            }
            using (SqlConnection myCon = new SqlConnection(CommonHelper.Decrypt(_configuration.GetSection(clientId).GetSection("source").Value)))
            {
                try
                {
                    myCon.Open();
                    SqlCommand sqlCommand = new SqlCommand(query, myCon);
                    sqlCommand.CommandType = CommandType.Text;
                    await sqlCommand.ExecuteNonQueryAsync();
                    myCon.Close();
                    isSaved = true;
                }
                catch
                {
                    isSaved = false;
                }
            }
            return isSaved;
        }
        public async Task<bool> UpdateCGDestinationDB(string taskId, List<CgOverwriteUpdateModel> updateModels, string clientId)
        {
            bool isSaved = false;
            var request = await _context.CodeMappings.Include("Request").FirstOrDefaultAsync(x => x.Reference == taskId);
            if (request == null)
            {
                return isSaved = false;
            }
            var summary = await _context.CodeGenerationSummary.FirstOrDefaultAsync(x => x.TaskId == request.Reference);
            string query = "";
            foreach (var item in updateModels)
            {
                query = query + GetInsertQuery(summary.Segment, item, clientId) + ";";
            }
            using (SqlConnection myCon = new SqlConnection(CommonHelper.Decrypt(_configuration.GetSection(clientId).GetSection("destination").Value)))
            {
                try
                {
                    myCon.Open();
                    SqlCommand sqlCommand = new SqlCommand(query, myCon);
                    sqlCommand.CommandType = CommandType.Text;
                    await sqlCommand.ExecuteNonQueryAsync();
                    myCon.Close();
                    return isSaved = true;
                }
                catch
                {
                    return isSaved;

                }
            }
        }
        private string GetInsertQuery(string segment, CgOverwriteUpdateModel updateModel, string clientId)
        {
            var segmentlower = segment.ToLower();
            string query = string.Empty;
            switch (segmentlower)
            {
                case ("hospital"):
                    query = "insert into CodeMatcherOverrideHospital (frm,too,newtoo,ClientID,Added_Date) VALUES ('" + updateModel.From + "','" + updateModel.OldToo + "','" + updateModel.NewToo + "','" + clientId + "',getdate())";
                    break;
                case ("school"):
                    query = "insert into CodeMatcherOverrideSchool (frm,too,newtoo,ClientID,Added_Date) VALUES ('" + updateModel.From + "','" + updateModel.OldToo + "','" + updateModel.NewToo + "','" + clientId + "',getdate())";
                    break;
                case ("insurance"):
                case ("insur"):
                    query = "insert into CodeMatcherOverrideInsur (frm,too,newtoo,ClientID,Added_Date) VALUES ('" + updateModel.From + "','" + updateModel.OldToo + "','" + updateModel.NewToo + "','" + clientId + "',getdate())";
                    break;
                case ("state license"):
                case ("statelic"):
                    query = "insert into CodeMatcherOverrideStatelic (frm,too,newtoo,ClientID,Added_Date) VALUES  ('" + updateModel.From + "','" + updateModel.OldToo + "','" + updateModel.NewToo + "','" + clientId + "',getdate())";
                    break;
            }
            return query;
        }

        private DataSet GetDatafromDestinationDB(string headerValue, string query)
        {
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection myCon = new SqlConnection(CommonHelper.Decrypt(_configuration.GetSection(headerValue).GetSection("destination").Value)))
                {
                    myCon.Open();
                    SqlCommand sqlCommand = new SqlCommand(query, myCon);
                    sqlCommand.CommandType = CommandType.Text;

                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                    sqlDataAdapter.SelectCommand = sqlCommand;
                    sqlDataAdapter.Fill(ds);
                    myCon.Close();

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetDatafromSourceDB Method: {ex.Message}", ex);
                throw;
            }
            return ds;
        }

        private string GetDBQueryforToo(string segment)
        {
            var segmentlower = segment.ToLower();
            string query = string.Empty;
            switch (segmentlower)
            {
                case ("hospital"):
                    query = "select cd,txt,(subname+':'+addr+':'+addr2+':'+city+':'+state+':'+zip+':'+nation) as Address from hospital";
                    break;
                case ("school"):
                    query = "select cd,txt,(subname+':'+addr+':'+addr2+':'+city+':'+state+':'+zip+':'+nation) as Address from school";
                    break;
                case ("insurance"):
                case ("insur"):
                    query = "select cd,txt,(addr+':'+addr2+':'+city+':'+state+':'+zip) as Address from insur";
                    break;
                case ("state license"):
                case ("statelic"):
                    query = "select cd,txt,(addr+':'+addr2+':'+city+':'+state+':'+zip) as Address from statelic";
                    break;
            }
            return query;
        }



        public async Task<List<CGOverwriteBaseDataModel>> GetCGOverwriteBaseDataModel(LoginModel loginModel, string clientId, string taskId)
        {
            List<CGOverwriteBaseDataModel> cgOverwriteModels = new List<CGOverwriteBaseDataModel>();
            try
            {
                var request = await _context.CodeMappings.Include("Request").FirstOrDefaultAsync(x => x.Reference == taskId);
                if (request != null)
                {
                    var summary = await _context.CodeGenerationSummary.FirstOrDefaultAsync(x => x.TaskId == request.Reference);
                    // var segment = await _context.Lookups.FirstOrDefaultAsync(x => x.Id == request.Request.SegmentTypeId);

                    string query = GetDBQueryforToo(summary.Segment);
                    if (query != string.Empty)
                    {
                        var data = GetDatafromDestinationDB(clientId, query);
                        if (data != null && data.Tables.Count > 0)
                        {
                            //04ad8ea6-2806-4b80-bfba-ea7bf5522831
                            var CgOverwriteModelData = data.Tables[0].AsEnumerable().Select(r => new CGOverwriteBaseDataModel
                            {
                                Code = r.Field<string>("cd"),
                                Name = r.Field<string>("txt"),
                                Address = r.Field<string>("Address"),
                            });
                            cgOverwriteModels = CgOverwriteModelData.ToList();
                        }
                        return cgOverwriteModels;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"CodeGenerationOverwritegetAsync Method: {ex.Message}", ex);
                throw;
            }
            return null;
        }
    }
}

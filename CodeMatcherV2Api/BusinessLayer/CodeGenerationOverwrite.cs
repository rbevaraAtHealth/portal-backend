using AutoMapper;
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

namespace CodeMatcherV2Api.BusinessLayer
{
    public class CodeGenerationOverwrite : ICodeGenerationOverwrite
    {
        private readonly IMapper _mapper;
        private CodeMatcherDbContext _context;
        private IConfiguration _configuration;
        public CodeGenerationOverwrite(IMapper mapper, CodeMatcherDbContext context, IConfiguration configuration)
        {
            _mapper = mapper;
            _context = context;
            _configuration = configuration;
        }
        public async Task<DataSet> CodeGenerationOverwritegetAsync(string taskId, LoginModel userModel, string clientId)
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
                    return data;
                }
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
                    query = "select top 5000 map_fiel.frm, map_fiel.l_maps, map_fiel.too, maps.full_name, map_fiel.Added_Date, map_fiel.link from map_fiel inner join maps on map_fiel.l_maps=maps.link  where maps.link >= " + startLink + " and maps.link <= " + latestLink + " and maps.full_name like '%40 Affiliation Hospital Code%' order by Added_Date desc";
                    break;
                case ("school"):
                    query = "select top 5000 map_fiel.frm, map_fiel.l_maps, map_fiel.too, maps.full_name, map_fiel.Added_Date, map_fiel.link from map_fiel inner join maps on map_fiel.l_maps=maps.link  where maps.link >= " + startLink + " and maps.link <= " + latestLink + " and maps.full_name like '%40 Education School Code%' order by Added_Date desc";
                    break;
                case ("insurance"):
                    query = "select top 5000 map_fiel.frm, map_fiel.l_maps, map_fiel.too, maps.full_name, map_fiel.Added_Date, map_fiel.link from map_fiel inner join maps on map_fiel.l_maps=maps.link  where maps.link >= " + startLink + " and maps.link <= " + latestLink + " and maps.full_name like '%40 Malpractice Insurance Carrier Code%' order by Added_Date desc";
                    break;
                case ("state license"):
                    query = "select top 5000 map_fiel.frm, map_fiel.l_maps, map_fiel.too, maps.full_name, map_fiel.Added_Date, map_fiel.link from map_fiel inner join maps on map_fiel.l_maps=maps.link  where maps.link >= " + startLink + " and maps.link <= " + latestLink + " and maps.full_name like '%40 Credential/License Institution Code%' order by Added_Date desc";
                    break;
            }
            return query;
        }
        private DataSet GetDatafromSourceDB(string headerValue, string query)
        {
            DataSet ds = new DataSet();
            using (SqlConnection myCon = new SqlConnection(CommonHelper.Decrypt(_configuration.GetSection(headerValue).GetSection("source").Value)))
            {
                SqlCommand sqlCommand = new SqlCommand(query, myCon);
                sqlCommand.CommandType = CommandType.Text;
                myCon.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                sqlDataAdapter.SelectCommand = sqlCommand;
                sqlDataAdapter.Fill(ds);
                myCon.Close();
            }
            return ds;
        }

        public async Task<bool> UpdateCGSourceDB(List<CgOverwriteUpdateModel> updateModels, string clientId)
        {
            bool isSaved = false;
            using (SqlConnection myCon = new SqlConnection(CommonHelper.Decrypt(_configuration.GetSection(clientId).GetSection("source").Value)))
            {
                try
                {
                    string query = "";
                    foreach (var item in updateModels)
                    {
                        query = query + "update map_fiel set too = " + item.NewToo + " where link = " + item.Link + ";";
                    }
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
                    return isSaved = false;
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
                    query = "insert into CodeMatcherOverrideHospital (frm,too,newtoo,ClientID,Added_Date,link) VALUES ('" + updateModel.From + "','" + updateModel.OldToo + "','" + updateModel.NewToo + "','" + clientId + "',getdate(),'')";
                    break;
                case ("school"):
                    query = "insert into CodeMatcherOverrideInsur (frm,too,newtoo,ClientID,Added_Date,link) VALUES ('" + updateModel.From + "','" + updateModel.OldToo + "','" + updateModel.NewToo + "','" + clientId + "',getdate(),'')";
                    break;
                case ("insurance"):
                    query = "insert into CodeMatcherOverrideSchool (frm,too,newtoo,ClientID,Added_Date,link) VALUES ('" + updateModel.From + "','" + updateModel.OldToo + "','" + updateModel.NewToo + "','" + clientId + "',getdate(),'')";
                    break;
                case ("state license"):
                    query = "insert into CodeMatcherOverrideStatelic (frm,too,newtoo,ClientID,Added_Date,link) VALUES  ('" + updateModel.From + "','" + updateModel.OldToo + "','" + updateModel.NewToo + "','" + clientId + "',getdate(),'')";
                    break;
            }
            return query;
        }
    }
}

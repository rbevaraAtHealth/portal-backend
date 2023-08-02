using AutoMapper;
using CodeMappingEfCore.DatabaseModels;
using CodeMatcher.Api.V2.BusinessLayer.Enums;
using CodeMatcher.Api.V2.Models.JsonResultModels;
using CodeMatcher.Api.V2.Models.SummaryModel;
using CodeMatcher.EntityFrameworkCore.DatabaseModels.SummaryTables;
using CodeMatcherV2Api.BusinessLayer.Interfaces;
using CodeMatcherV2Api.EntityFrameworkCore;
using CodeMatcherV2Api.Middlewares.HttpHelper;
using CodeMatcherV2Api.Middlewares.SqlHelper;
using CodeMatcherV2Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CodeMatcherV2Api.BusinessLayer
{
    public class CodeMapping : ICodeMapping
    {
        private readonly CodeMatcherDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _httpClientFactory;
        public CodeMapping(CodeMatcherDbContext context, IMapper mapper, IHttpClientFactory httpClientFactory1)
        {
            _context = context;
            _mapper = mapper;
            _httpClientFactory = httpClientFactory1;
        }
        public async Task<List<CodeMappingModel>> GetCodeMappingsRecordsAsync()
        {
            List<CodeMappingModel> codeMappingRecords = new List<CodeMappingModel>
            {new CodeMappingModel{ TimeStamp = DateTime.Now ,Segment="Insurance",NumberOfRecord=100,NumberOfMatches=97,RunBy="John Smith",RunType="Scheduled",CodeMappingSummary =new CodeMappingSummary{TimeStamp=DateTime.Now,RecordParticulars=38983,Segment_type="State License",Threshhold=87,BaseRecords=2599,InputRecords=107,NoisyRecords=3,ProcessedRecords=104,CodeGeneratedRecords=99,CodeNotGeneratedRecords=5} },
                new CodeMappingModel { TimeStamp = DateTime.Now, Segment = "Hospital", NumberOfRecord = 110, NumberOfMatches = 95, RunBy = "Joe Smith", RunType = "Triggered" ,CodeMappingSummary =new CodeMappingSummary{TimeStamp=DateTime.Now,RecordParticulars=38983,Segment_type="State License",Threshhold=87,BaseRecords=2599,InputRecords=107,NoisyRecords=3,ProcessedRecords=104,CodeGeneratedRecords=99,CodeNotGeneratedRecords=5}},
                new CodeMappingModel { TimeStamp = DateTime.Now, Segment = "State License", NumberOfRecord = 110, NumberOfMatches = 97, RunBy = "John Smith", RunType = "Scheduled" , CodeMappingSummary = new CodeMappingSummary{ TimeStamp = DateTime.Now, RecordParticulars = 38983, Segment_type = "State License", Threshhold = 87, BaseRecords = 2599, InputRecords = 107, NoisyRecords = 3, ProcessedRecords = 104, CodeGeneratedRecords = 99, CodeNotGeneratedRecords = 5 }},
                new CodeMappingModel { TimeStamp = DateTime.Now, Segment = "School", NumberOfRecord = 100, NumberOfMatches = 97, RunBy = "John Smith", RunType = "Scheduled" , CodeMappingSummary = new CodeMappingSummary{ TimeStamp = DateTime.Now, RecordParticulars = 38983, Segment_type = "State License", Threshhold = 87, BaseRecords = 2599, InputRecords = 107, NoisyRecords = 3, ProcessedRecords = 104, CodeGeneratedRecords = 99, CodeNotGeneratedRecords = 5 }},
                new CodeMappingModel { TimeStamp = DateTime.Now, Segment = "Insurance", NumberOfRecord = 110, NumberOfMatches = 97, RunBy = "John Smith", RunType = "Scheduled" , CodeMappingSummary = new CodeMappingSummary{ TimeStamp = DateTime.Now, RecordParticulars = 38983, Segment_type = "State License", Threshhold = 87, BaseRecords = 2599, InputRecords = 107, NoisyRecords = 3, ProcessedRecords = 104, CodeGeneratedRecords = 99, CodeNotGeneratedRecords = 5 }}
            };

            return codeMappingRecords;
        }
        public List<CodeGenerationSummaryModel> GetCodeGenerationMappingRecords()
        {
            var codeGeneration = _context.CodeGenerationSummary.AsNoTracking().ToList();
            var codeGenerationModel = _mapper.Map<List<CodeGenerationSummaryModel>>(codeGeneration);
            return codeGenerationModel;
        }
        public List<MonthlyEmbedSummaryModel> GetMonthlyEmbeddingMappingRecords()
        {
            var monthlyEmbed = _context.MonthlyEmbeddingsSummary.AsNoTracking().ToList();
            var monthlyEmbedModel = _mapper.Map<List<MonthlyEmbedSummaryModel>>(monthlyEmbed);
            return monthlyEmbedModel;
        }
        public List<WeeklyEmbedSummaryModel> GetWeeklyEmbeddingsMappingRecords()
        {
            var weeklyEmbed = _context.WeeklyEmbeddingsSummary.AsNoTracking().ToList();
            var weeklyEmbedModel = _mapper.Map<List<WeeklyEmbedSummaryModel>>(weeklyEmbed);
            return weeklyEmbedModel;
        }
        public int GetCgMappingsPythApi( Guid taskId,string summary)

        {
            int requestId = SqlHelper.GetRequestId(taskId, _context);
            if (requestId == 0)
                return 0;
            var result= JsonConvert.DeserializeObject<Root>(summary);
            var cgSummary = JsonConvert.DeserializeObject<CodeGenerationSummaryModel>(result.result.run_summary);
            cgSummary.TaskId = taskId;
            var cgSummaryDto = _mapper.Map<CodeGenerationSummaryDto>(cgSummary);
            cgSummaryDto.RequestId = requestId;
            int summaryid=SqlHelper.SaveCodeGenerationSummary(cgSummaryDto, _context);
            SqlHelper.UpdateCodeMappingStatus(taskId, _context);
            return summaryid;
        }

        public int GetMonthlyEmbedMappingsPythApi(Guid taskId,string summary)
        {
            int requestId = SqlHelper.GetRequestId(taskId, _context);
            if (requestId == 0)
                return 0;
            var cgSummary = JsonConvert.DeserializeObject<MonthlyEmbedSummaryModel>(summary);
            cgSummary.TaskId = taskId;
            var monthlySummaryDto = _mapper.Map<MonthlyEmbeddingsSummaryDto>(cgSummary);
            monthlySummaryDto.RequestId = requestId;
            int summaryId=SqlHelper.SaveMonthlyEmbedSummary(monthlySummaryDto, _context);
            SqlHelper.UpdateCodeMappingStatus(taskId, _context);
            return summaryId;
        }
        public int GetWeeklyEmbedMappingsPythApi(Guid taskId,string summary)
        {
            int requestId = SqlHelper.GetRequestId(taskId, _context);
            if (requestId == 0)
                return 0;
            var cgSummary = JsonConvert.DeserializeObject<WeeklyEmbedSummaryModel>(summary);
            cgSummary.TaskId = taskId;
            var weeklySummaryDto = _mapper.Map<WeeklyEmbeddingsSummaryDto>(cgSummary);
            weeklySummaryDto.RequestId = requestId;
           int summaryId= SqlHelper.SaveWeeklyEmbedSummary(weeklySummaryDto, _context);
            SqlHelper.UpdateCodeMappingStatus(taskId, _context);
            return summaryId;
        }
        
        public int SaveSummary(Guid taskId,string summary)
        {
            int requestId = SqlHelper.GetRequestId(taskId, _context);
            int frequncyId = SqlHelper.GetCodeMappingId(requestId, _context);
            int summaryId = 0;
            switch (frequncyId)
            {
                case ((int)CodeMappingType.CodeGeneration):
                     summaryId=GetCgMappingsPythApi(taskId,summary);
                    break;
                case ((int)CodeMappingType.MonthlyEmbeddings):
                     summaryId = GetMonthlyEmbedMappingsPythApi(taskId, summary);
                    break;
                case ((int)CodeMappingType.WeeklyEmbeddings):
                     summaryId = GetWeeklyEmbedMappingsPythApi(taskId, summary);
                    break;
                default: break;
            }
            return summaryId;
        }

    }
}

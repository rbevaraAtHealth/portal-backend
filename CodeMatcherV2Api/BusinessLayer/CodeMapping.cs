using AutoMapper;
using CodeMappingEfCore.DatabaseModels;
using CodeMatcher.Api.V2.BusinessLayer.Enums;
using CodeMatcher.Api.V2.Models;
using CodeMatcher.Api.V2.Models.JsonResultModels;
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
        public CodeGenerationSummaryModel GetCgMappingsPythApi(HttpResponseMessage httpResponse, Guid taskId)
        {
            int requestId = SqlHelper.GetRequestId(taskId, _context);
            if (requestId == 0)
                return null;
            string httpResult = httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<Root>(httpResult);
            var cgSummary = JsonConvert.DeserializeObject<CodeGenerationSummaryModel>(result.result.run_summary);
            cgSummary.TaskId = taskId;
            var cgSummaryDto = _mapper.Map<CodeGenerationSummaryDto>(cgSummary);
            cgSummaryDto.RequestId = requestId;
            SqlHelper.SaveCodeGenerationSummary(cgSummaryDto, _context);
            return cgSummary;
        }

        public MonthlyEmbedSummaryModel GetMonthlyEmbedMappingsPythApi(HttpResponseMessage httpResponse, Guid taskId)
        {
            int requestId = SqlHelper.GetRequestId(taskId, _context);
            if (requestId == 0)
                return null;
            string httpResult = httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<Root>(httpResult);
            var cgSummary = JsonConvert.DeserializeObject<MonthlyEmbedSummaryModel>(result.result.run_summary);
            cgSummary.TaskId = taskId;
            var monthlySummaryDto = _mapper.Map<MonthlyEmbeddingsSummaryDto>(cgSummary);
            monthlySummaryDto.RequestId = requestId;
            SqlHelper.SaveMonthlyEmbedSummary(monthlySummaryDto, _context);
            return cgSummary;
        }
        public WeeklyEmbedSummaryModel GetWeeklyEmbedMappingsPythApi(HttpResponseMessage httpResponse, Guid taskId)
        {
            int requestId = SqlHelper.GetRequestId(taskId, _context);
            if (requestId == 0)
                return null;
            string httpResult = httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var result = JsonConvert.DeserializeObject<Root>(httpResult);
            var cgSummary = JsonConvert.DeserializeObject<WeeklyEmbedSummaryModel>(result.result.run_summary);
            cgSummary.TaskId = taskId;
            var weeklySummaryDto = _mapper.Map<WeeklyEmbeddingsSummaryDto>(cgSummary);
            weeklySummaryDto.RequestId = requestId;
            SqlHelper.SaveWeeklyEmbedSummary(weeklySummaryDto, _context);
            return cgSummary;
        }
        public void GetMappingsInProcessTasks()
        {
            var codeMapping = SqlHelper.GetCodeMappings(_context);

            foreach (var item in codeMapping)
            {
                var taskId = new Guid(item.Reference);
                var httpresponse= HttpHelper.Get_HttpClient(_httpClientFactory, "task/" + taskId + "/result/");
                var response = httpresponse.Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    int requestId = SqlHelper.GetRequestId(taskId, _context);
                    int frequncyId = SqlHelper.GetCodeMappingId(requestId, _context);
                    switch (frequncyId)
                    {
                        case ((int)CodeMappingType.CodeGeneration):
                            GetCgMappingsPythApi(response, taskId);
                            break;
                        case ((int)CodeMappingType.MonthlyEmbeddings):
                            GetMonthlyEmbedMappingsPythApi(response, taskId);
                            break;
                        case ((int)CodeMappingType.WeeklyEmbeddings):
                            GetWeeklyEmbedMappingsPythApi(response, taskId);
                            break;
                        default: break;
                    }
                }
            }
            //return codeMapping;
        }

    }
}

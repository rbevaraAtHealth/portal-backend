using AutoMapper;
using Azure.Core;
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
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        public GenericSummaryViewModel GetMappings(Guid taskId)
        {
            var codemap = _context.CodeMappings.FirstOrDefault(x => x.Reference == taskId.ToString());
            GenericSummaryViewModel summaryViewModel = new GenericSummaryViewModel();

            if (codemap.Status == "Completed")
            {
                int requestId = SqlHelper.GetRequestId(taskId, _context);
                int codeMappingId = SqlHelper.GetCodeMappingId(requestId, _context);
                CodeGenerationSummaryDto cgSummaryDto = new CodeGenerationSummaryDto();
                MonthlyEmbeddingsSummaryDto monthlyembedSummaryDto = new MonthlyEmbeddingsSummaryDto();
                WeeklyEmbeddingsSummaryDto weeklyEmbedSummaryDto = new WeeklyEmbeddingsSummaryDto();
                switch (codeMappingId)
                {
                    case ((int)CodeMappingType.CodeGeneration):
                        cgSummaryDto = _context.CodeGenerationSummary.FirstOrDefault(x => x.TaskId == taskId);
                        //summaryViewModel.TaskId = cgSummaryDto.TaskId;
                        //summaryViewModel.RequestId = cgSummaryDto.RequestId;
                        //summaryViewModel.Date = cgSummaryDto.Date.ToString();
                        //summaryViewModel.Segment = cgSummaryDto.Segment;
                        //summaryViewModel.Threshold = cgSummaryDto.Threshold;
                        //summaryViewModel.NoOfBaseRecords = cgSummaryDto.NoOfBaseRecords;
                        //summaryViewModel.NoOfInputRecords = cgSummaryDto.NoOfInputRecords;
                        //summaryViewModel.NoOfProcessedRecords = cgSummaryDto.NoOfProcessedRecords;
                        //summaryViewModel.NoOfRecordsForWhichCodeNotGenerated = cgSummaryDto.NoOfRecordsForWhichCodeNotGenerated;
                        //summaryViewModel.StartLink = cgSummaryDto.StartLink;
                        //summaryViewModel.LatestLink = cgSummaryDto.LatestLink;
                        //summaryViewModel.ClientId = cgSummaryDto.ClientId;
                        break;
                    case ((int)CodeMappingType.MonthlyEmbeddings):
                        monthlyembedSummaryDto = _context.MonthlyEmbeddingsSummary.FirstOrDefault(x => x.TaskId == taskId);
                        summaryViewModel.TaskId = monthlyembedSummaryDto.TaskId.ToString();
                        summaryViewModel.RequestId = monthlyembedSummaryDto.RequestId;
                        //summaryViewModel.Date = monthlyembedSummaryDto.Date.ToString();
                        //summaryViewModel.Segment = monthlyembedSummaryDto.Segment;
                        //summaryViewModel.NoOfBaseRecordsImportedFromDatabase = monthlyembedSummaryDto.NoOfRecordsImportedFromDatabase;
                        //summaryViewModel.NoOfRecordsEmbeddingCreated = monthlyembedSummaryDto.NoOfRecordsEmbeddingCreated;
                        break;
                    case ((int)CodeMappingType.WeeklyEmbeddings):
                        weeklyEmbedSummaryDto = _context.WeeklyEmbeddingsSummary.FirstOrDefault(x => x.TaskId == taskId);
                        summaryViewModel.TaskId = weeklyEmbedSummaryDto.TaskId.ToString();
                        summaryViewModel.RequestId = weeklyEmbedSummaryDto.RequestId;
                        //summaryViewModel.Date = weeklyEmbedSummaryDto?.Date.ToString();
                        //summaryViewModel.Segment = weeklyEmbedSummaryDto?.Segment;
                        //summaryViewModel.NoOfBaseRecordsImportedFromDatabase = weeklyEmbedSummaryDto.NoOfBaseRecordsImportedFromDatabase;
                        //summaryViewModel.NoOfBaseRecordsBeforeRun = weeklyEmbedSummaryDto.NoOfBaseRecordsBeforeRun;
                        //summaryViewModel.NoOfRecordsAfterRun = weeklyEmbedSummaryDto.NoOfRecordsAfterRun;
                        //summaryViewModel.StartLink = weeklyEmbedSummaryDto.StartLink.ToString();
                        //summaryViewModel.LatestLink = weeklyEmbedSummaryDto.LatestLink.ToString();
                        break;
                    default: break;
                }
            }
            return summaryViewModel;
        }
        public List<GenericSummaryViewModel> GetCodeMappings()
        {
            List<GenericSummaryViewModel> viewModel= new List<GenericSummaryViewModel>();
            var codeMappings=_context.CodeMappings.Include("Request").AsNoTracking();
            List<int> ids = new List<int>();
            foreach (var codeMapping in codeMappings)
            {
                ids.Add(codeMapping.RequestId);
            }
            var reuests = _context.CodeMappingRequests.Where(x => ids.Contains(x.Id)).Include("RunType").Include("SegmentType").Include("CodeMappingType").ToList();
            var cgsummary = _context.CodeGenerationSummary.Include("CodeMappingRequest");
            var monthlyEmbedSummary = _context.MonthlyEmbeddingsSummary.Include("CodeMappingRequest");
            var weeklyEmbedSummary = _context.WeeklyEmbeddingsSummary.Include("CodeMappingRequest");
            foreach (var item in codeMappings)
            {
                GenericSummaryViewModel model = new GenericSummaryViewModel();
                model.TaskId = item.Reference;
                model.RequestId = item.RequestId;
                model.TimeStamp = item.Request.CreatedTime;
                var requestDto = reuests.FirstOrDefault(x => x.Id == item.RequestId);
                model.Segment = requestDto.SegmentType.Name;
               // model.Threshold =requestDto.Threshold;
                model.RunType = requestDto.RunType.Name;
                model.RunBy = requestDto.CreatedBy;

                switch (requestDto.RunTypeId)
                {
                    case ((int)CodeMappingType.CodeGeneration):
                        model.Summary = _context.CodeGenerationSummary.FirstOrDefault(x => x.RequestId == item.RequestId);
                        break;
                    case ((int)CodeMappingType.MonthlyEmbeddings):
                        model.Summary = _context.MonthlyEmbeddingsSummary.FirstOrDefault(x => x.RequestId == item.RequestId);
                        break;
                    case ((int)CodeMappingType.WeeklyEmbeddings):
                        model.Summary = _context.WeeklyEmbeddingsSummary.FirstOrDefault(x => x.RequestId == item.RequestId);
                        break;
                    default: break;
                }
            }

            
            
            
            return viewModel;
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
        public int GetCgMappingsPythApi(Guid taskId, string summary, int requestId)

        {
            var cgSummary = JsonConvert.DeserializeObject<CodeGenerationSummaryModel>(summary);
            cgSummary.TaskId = taskId;
            var cgSummaryDto = _mapper.Map<CodeGenerationSummaryDto>(cgSummary);
            cgSummaryDto.RequestId = requestId;
            int summaryid = SqlHelper.SaveCodeGenerationSummary(cgSummaryDto, _context);
            SqlHelper.UpdateCodeMappingStatus(taskId, _context);
            return summaryid;
        }

        public int GetMonthlyEmbedMappingsPythApi(Guid taskId, string summary, int requestId)
        {
            var cgSummary = JsonConvert.DeserializeObject<MonthlyEmbedSummaryModel>(summary);
            cgSummary.TaskId = taskId;
            var monthlySummaryDto = _mapper.Map<MonthlyEmbeddingsSummaryDto>(cgSummary);
            monthlySummaryDto.RequestId = requestId;
            int summaryId = SqlHelper.SaveMonthlyEmbedSummary(monthlySummaryDto, _context);
            SqlHelper.UpdateCodeMappingStatus(taskId, _context);
            return summaryId;
        }
        public int GetWeeklyEmbedMappingsPythApi(Guid taskId, string summary, int requestId)
        {
            var cgSummary = JsonConvert.DeserializeObject<WeeklyEmbedSummaryModel>(summary);
            cgSummary.TaskId = taskId;
            var weeklySummaryDto = _mapper.Map<WeeklyEmbeddingsSummaryDto>(cgSummary);
            weeklySummaryDto.RequestId = requestId;
            int summaryId = SqlHelper.SaveWeeklyEmbedSummary(weeklySummaryDto, _context);
            SqlHelper.UpdateCodeMappingStatus(taskId, _context);
            return summaryId;
        }

        public int SaveSummary(Guid taskId, string summary)
        {
            int requestId = SqlHelper.GetRequestId(taskId, _context);
            int frequncyId = SqlHelper.GetCodeMappingId(requestId, _context);
            int summaryId = 0;
            switch (frequncyId)
            {
                case ((int)CodeMappingType.CodeGeneration):
                    summaryId = GetCgMappingsPythApi(taskId, summary, requestId);
                    break;
                case ((int)CodeMappingType.MonthlyEmbeddings):
                    summaryId = GetMonthlyEmbedMappingsPythApi(taskId, summary, requestId);
                    break;
                case ((int)CodeMappingType.WeeklyEmbeddings):
                    summaryId = GetWeeklyEmbedMappingsPythApi(taskId, summary, requestId);
                    break;
                default: break;
            }
            return summaryId;
        }

    }
}

﻿using AutoMapper;
using CodeMatcher.Api.V2.BusinessLayer;
using CodeMatcher.Api.V2.Models.SummaryModel;
using CodeMatcher.EntityFrameworkCore.DatabaseModels.SummaryTables;
using CodeMatcherV2Api.BusinessLayer.Interfaces;
using CodeMatcherV2Api.EntityFrameworkCore;
using CodeMatcherV2Api.Middlewares.SqlHelper;
using CodeMatcherV2Api.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;

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
            {new CodeMappingModel{ TimeStamp = DateTime.Now ,Segment="Insurance",NumberOfRecord=100,NumberOfMatches=97,RunBy="John Smith",RunType="Scheduled",CodeMappingSummary =new CodeMappingSummary{TimeStamp=DateTime.Now,RecordParticulars=38983,Segment_type="State License",Threshhold="87",BaseRecords=2599,InputRecords=107,NoisyRecords=3,ProcessedRecords=104,CodeGeneratedRecords=99,CodeNotGeneratedRecords=5} },
                new CodeMappingModel { TimeStamp = DateTime.Now, Segment = "Hospital", NumberOfRecord = 110, NumberOfMatches = 95, RunBy = "Joe Smith", RunType = "Triggered" ,CodeMappingSummary =new CodeMappingSummary{TimeStamp=DateTime.Now,RecordParticulars=38983,Segment_type="State License",Threshhold="87",BaseRecords=2599,InputRecords=107,NoisyRecords=3,ProcessedRecords=104,CodeGeneratedRecords=99,CodeNotGeneratedRecords=5}},
                new CodeMappingModel { TimeStamp = DateTime.Now, Segment = "State License", NumberOfRecord = 110, NumberOfMatches = 97, RunBy = "John Smith", RunType = "Scheduled" , CodeMappingSummary = new CodeMappingSummary{ TimeStamp = DateTime.Now, RecordParticulars = 38983, Segment_type = "State License", Threshhold = "87", BaseRecords = 2599, InputRecords = 107, NoisyRecords = 3, ProcessedRecords = 104, CodeGeneratedRecords = 99, CodeNotGeneratedRecords = 5 }},
                new CodeMappingModel { TimeStamp = DateTime.Now, Segment = "School", NumberOfRecord = 100, NumberOfMatches = 97, RunBy = "John Smith", RunType = "Scheduled" , CodeMappingSummary = new CodeMappingSummary{ TimeStamp = DateTime.Now, RecordParticulars = 38983, Segment_type = "State License", Threshhold = "87", BaseRecords = 2599, InputRecords = 107, NoisyRecords = 3, ProcessedRecords = 104, CodeGeneratedRecords = 99, CodeNotGeneratedRecords = 5 }},
                new CodeMappingModel { TimeStamp = DateTime.Now, Segment = "Insurance", NumberOfRecord = 110, NumberOfMatches = 97, RunBy = "John Smith", RunType = "Scheduled" , CodeMappingSummary = new CodeMappingSummary{ TimeStamp = DateTime.Now, RecordParticulars = 38983, Segment_type = "State License", Threshhold = "87", BaseRecords = 2599, InputRecords = 107, NoisyRecords = 3, ProcessedRecords = 104, CodeGeneratedRecords = 99, CodeNotGeneratedRecords = 5 }}
            };

            return codeMappingRecords;
        }

        public GenericSummaryViewModel GetMappings(string taskId)
        {
            var codemap = _context.CodeMappings.FirstOrDefault(x => x.Reference == taskId.ToString());
            GenericSummaryViewModel summaryViewModel = new GenericSummaryViewModel();

            if (codemap.Status == "Completed")
            {
                int requestId = SqlHelper.GetRequestId(taskId, _context);
                int codeMappingId = SqlHelper.GetCodeMappingId(requestId, _context);
                string codemappingType = SqlHelper.GetLookupTypeName(codeMappingId,_context);
                CodeGenerationSummaryDto cgSummaryDto = new CodeGenerationSummaryDto();
                MonthlyEmbeddingsSummaryDto monthlyembedSummaryDto = new MonthlyEmbeddingsSummaryDto();
                WeeklyEmbeddingsSummaryDto weeklyEmbedSummaryDto = new WeeklyEmbeddingsSummaryDto();
                switch (codemappingType)
                {
                    case CodeMappingTypeConst.CodeGeneration:
                        cgSummaryDto = _context.CodeGenerationSummary.FirstOrDefault(x => x.TaskId == taskId);
                        break;
                    case (CodeMappingTypeConst.MonthlyEmbeddings):
                        monthlyembedSummaryDto = _context.MonthlyEmbeddingsSummary.FirstOrDefault(x => x.TaskId == taskId);
                        summaryViewModel.TaskId = monthlyembedSummaryDto.TaskId.ToString();
                        summaryViewModel.RequestId = monthlyembedSummaryDto.RequestId;
                        break;
                    case (CodeMappingTypeConst.WeeklyEmbeddings):
                        weeklyEmbedSummaryDto = _context.WeeklyEmbeddingsSummary.FirstOrDefault(x => x.TaskId == taskId);
                        summaryViewModel.TaskId = weeklyEmbedSummaryDto.TaskId.ToString();
                        summaryViewModel.RequestId = weeklyEmbedSummaryDto.RequestId;
                        break;
                    default: break;
                }
            }
            return summaryViewModel;
        }
        public List<GenericSummaryViewModel> GetCodeGenerationMappingRecords()
        {
            var viewModels = (from cr in _context.CodeMappingRequests.Include("RunType").Include("SegmentType").Include("CodeMappingType")
                              join cm in _context.CodeMappings on cr.Id equals cm.RequestId
                              join cs in _context.CodeGenerationSummary on cr.Id equals cs.RequestId
                              into csA from csB in csA.DefaultIfEmpty()
                              where (cr.CodeMappingType.Name == CodeMappingTypeConst.CodeGeneration && cr.RunType.Name != RequestTypeConst.Scheduled)
                              select new GenericSummaryViewModel
                              {
                                  TaskId = cm.Reference,
                                  RequestId = cr.Id,
                                  TimeStamp = cr.CreatedTime,
                                  Segment = cr.SegmentType.Name,
                                  RunType = cr.RunType.Name,
                                  CodeMappingType = cr.CodeMappingType.Name,
                                  RunBy = cr.CreatedBy,
                                  Summary = csB
                              }).ToList();
            
            return viewModels;
        }
        public List<GenericSummaryViewModel> GetEmbeddingMappingRecords(string codeMappingType)
        {
            var viewModels = (from cr in _context.CodeMappingRequests.Include("RunType").Include("SegmentType").Include("CodeMappingType")
                              join cm in _context.CodeMappings on cr.Id equals cm.RequestId
                              join cs in _context.CodeGenerationSummary on cr.Id equals cs.RequestId
                              into csA
                              from csB in csA.DefaultIfEmpty()
                              where (cr.CodeMappingType.Name == codeMappingType)
                              select new GenericSummaryViewModel
                              {
                                  TaskId = cm.Reference,
                                  RequestId = cr.Id,
                                  TimeStamp = cr.CreatedTime,
                                  Segment = cr.SegmentType.Name,
                                  RunType = cr.RunType.Name,
                                  CodeMappingType = cr.CodeMappingType.Name,
                                  RunBy = cr.CreatedBy,
                                  Summary = csB
                              }).ToList();

            return viewModels;
        }
        
        public int SaveCgMappingsPythApi(string taskId, string summary, int requestId)

        {
            var cgSummary = JsonConvert.DeserializeObject<CodeGenerationSummaryModel>(summary);
            cgSummary.TaskId = taskId;
            var cgSummaryDto = _mapper.Map<CodeGenerationSummaryDto>(cgSummary);
            cgSummaryDto.RequestId = requestId;
            cgSummaryDto.CreatedBy = "Admin";
            int summaryid = SqlHelper.SaveCodeGenerationSummary(cgSummaryDto, _context);
            SqlHelper.UpdateCodeMappingStatus(taskId, _context);
            return summaryid;
        }
        public int SaveMonthlyEmbedMappingsPythApi(string taskId, string summary, int requestId)
        {
            var cgSummary = JsonConvert.DeserializeObject<MonthlyEmbedSummaryModel>(summary);
            cgSummary.TaskId = taskId;
            var monthlySummaryDto = _mapper.Map<MonthlyEmbeddingsSummaryDto>(cgSummary);
            monthlySummaryDto.RequestId = requestId;
            monthlySummaryDto.CreatedBy = "Admin";
            int summaryId = SqlHelper.SaveMonthlyEmbedSummary(monthlySummaryDto, _context);
            SqlHelper.UpdateCodeMappingStatus(taskId, _context);
            return summaryId;
        }
        public int SaveWeeklyEmbedMappingsPythApi(string taskId, string summary, int requestId)
        {
            var cgSummary = JsonConvert.DeserializeObject<WeeklyEmbedSummaryModel>(summary);
            cgSummary.TaskId = taskId;
            var weeklySummaryDto = _mapper.Map<WeeklyEmbeddingsSummaryDto>(cgSummary);
            weeklySummaryDto.RequestId = requestId;
            weeklySummaryDto.CreatedBy = "Admin";
            int summaryId = SqlHelper.SaveWeeklyEmbedSummary(weeklySummaryDto, _context);
            SqlHelper.UpdateCodeMappingStatus(taskId, _context);
            return summaryId;
        }

        public int SaveSummary(string taskId, string summary)
        {
            int requestId = SqlHelper.GetRequestId(taskId, _context);
            int frequncyId = SqlHelper.GetCodeMappingId(requestId, _context);
            string codemappingType = SqlHelper.GetLookupTypeName(frequncyId, _context);
            int summaryId = 0;
            switch (codemappingType)
            {
                case (CodeMappingTypeConst.CodeGeneration):
                    summaryId = SaveCgMappingsPythApi(taskId, summary, requestId);
                    break;
                case (CodeMappingTypeConst.MonthlyEmbeddings):
                    summaryId = SaveMonthlyEmbedMappingsPythApi(taskId, summary, requestId);
                    break;
                case (CodeMappingTypeConst.WeeklyEmbeddings):
                    summaryId = SaveWeeklyEmbedMappingsPythApi(taskId, summary, requestId);
                    break;
                default: break;
            }
            return summaryId;
        }
    }
}

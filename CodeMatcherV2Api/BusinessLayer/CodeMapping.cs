using AutoMapper;
using CodeMappingEfCore.DatabaseModels;
using CodeMatcher.Api.V2.BusinessLayer.Enums;
using CodeMatcher.Api.V2.Models.SummaryModel;
using CodeMatcher.EntityFrameworkCore.DatabaseModels.SummaryTables;
using CodeMatcherV2Api.BusinessLayer.Enums;
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
using System.Runtime.CompilerServices;
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
                CodeGenerationSummaryDto cgSummaryDto = new CodeGenerationSummaryDto();
                MonthlyEmbeddingsSummaryDto monthlyembedSummaryDto = new MonthlyEmbeddingsSummaryDto();
                WeeklyEmbeddingsSummaryDto weeklyEmbedSummaryDto = new WeeklyEmbeddingsSummaryDto();
                switch (codeMappingId)
                {
                    case ((int)CodeMappingType.CodeGeneration):
                        cgSummaryDto = _context.CodeGenerationSummary.FirstOrDefault(x => x.TaskId == taskId);
                        break;
                    case ((int)CodeMappingType.MonthlyEmbeddings):
                        monthlyembedSummaryDto = _context.MonthlyEmbeddingsSummary.FirstOrDefault(x => x.TaskId == taskId);
                        summaryViewModel.TaskId = monthlyembedSummaryDto.TaskId.ToString();
                        summaryViewModel.RequestId = monthlyembedSummaryDto.RequestId;
                        break;
                    case ((int)CodeMappingType.WeeklyEmbeddings):
                        weeklyEmbedSummaryDto = _context.WeeklyEmbeddingsSummary.FirstOrDefault(x => x.TaskId == taskId);
                        summaryViewModel.TaskId = weeklyEmbedSummaryDto.TaskId.ToString();
                        summaryViewModel.RequestId = weeklyEmbedSummaryDto.RequestId;
                        break;
                    default: break;
                }
            }
            return summaryViewModel;
        }
        public List<GenericSummaryViewModel> GetCodeMappings()
        {
            List<GenericSummaryViewModel> viewModel = new List<GenericSummaryViewModel>();
            var codeMappings = _context.CodeMappings.Include("Request").AsNoTracking();

            foreach (var item in codeMappings)
            {
                GenericSummaryViewModel model = new GenericSummaryViewModel();
                model.TaskId = item.Reference;
                model.RequestId = item.RequestId;
                model.TimeStamp = item.Request.CreatedTime;
                var reuestDto = _context.CodeMappingRequests.Include("RunType").Include("SegmentType").Include("CodeMappingType").FirstOrDefault(x => x.Id == item.Id);
                model.Segment = reuestDto.SegmentType.Name;
                model.RunType = reuestDto.RunType.Name;
                model.CodeMappingType = reuestDto.CodeMappingType.Name;
                model.RunBy = reuestDto.CreatedBy;

                switch (reuestDto.CodeMappingType.Id)
                {
                    case ((int)CodeMappingType.CodeGeneration):
                        model.Summary = _context.CodeGenerationSummary.AsNoTracking().FirstOrDefault(x => x.RequestId == item.RequestId);
                        break;
                    case ((int)CodeMappingType.MonthlyEmbeddings):
                        model.Summary = _context.MonthlyEmbeddingsSummary.AsNoTracking().FirstOrDefault(x => x.RequestId == item.RequestId);
                        break;
                    case ((int)CodeMappingType.WeeklyEmbeddings):
                        model.Summary = _context.WeeklyEmbeddingsSummary.AsNoTracking().FirstOrDefault(x => x.RequestId == item.RequestId);
                        break;
                    default: break;
                }
                viewModel.Add(model);
            }
            return viewModel;
        }
        public List<GenericSummaryViewModel> GetCodeGenerationMappingRecords()
        {
            // List<GenericSummaryViewModel> viewModel = new List<GenericSummaryViewModel>();
            //       var item = (
            //from ai in context.app_information
            //join al in context.app_language on ai.languageid equals al.id
            //where (al.languagecode == "es")
            //select new AppInformation
            //{
            //    id = ai.id,
            //    title = ai.title,
            //    description = ai.description,
            //    coverimageurl = ai.coverimageurl
            //})
            var viewModels = (from cr in _context.CodeMappingRequests.Include("RunType").Include("SegmentType").Include("CodeMappingType")
                              join cm in _context.CodeMappings on cr.Id equals cm.RequestId
                              join cs in _context.CodeGenerationSummary on cr.Id equals cs.RequestId
                              where (cr.CodeMappingId == (int)CodeMappingType.CodeGeneration && cr.RunTypeId!=(int)RequestType.scheduled)
                              select new GenericSummaryViewModel
                              {
                                  TaskId = cm.Reference,
                                  RequestId = cr.Id,
                                  TimeStamp = cr.CreatedTime,
                                  Segment = cr.SegmentType.Name,
                                  RunType = cr.RunType.Name,
                                  CodeMappingType = cr.CodeMappingType.Name,
                                  RunBy = cr.CreatedBy,
                                  Summary=cs
                              }).ToList();
            //var viewModels2 = (from cr in _context.CodeMappingRequests.Include("RunType").Include("SegmentType").Include("CodeMappingType")
            //                  join cm in _context.CodeMappings on cr.Id equals cm.RequestId
                             
            //                  where (cr.CodeMappingId == (int)CodeMappingType.CodeGeneration && cr.RunTypeId != (int)RequestType.scheduled)
            //                   into  in _context.CodeGenerationSummary.DefaultIfEmpty()
            //                  select new GenericSummaryViewModel
            //                  {
            //                      TaskId = cm.Reference,
            //                      RequestId = cr.Id,
            //                      TimeStamp = cr.CreatedTime,
            //                      Segment = cr.SegmentType.Name,
            //                      RunType = cr.RunType.Name,
            //                      CodeMappingType = cr.CodeMappingType.Name,
            //                      RunBy = cr.CreatedBy,
            //                      Summary = cs
            //                  }).ToList();
            //  var codeMappings = _context.CodeMappings.Include("Request").Where(e=>e.RequestId==).AsNoTracking();
            //var records = _context.CodeMappingRequests.Join(_context.CodeMappings, cmRequest => cmRequest.Id, codeMapping => codeMapping.RequestId, (cmRequest, CodeMapping) => new { cmRequest, CodeMapping }).Where(joinedTable => joinedTable.cmRequest.CodeMappingId == (int)CodeMappingType.CodeGeneration);
            // foreach (var item in codeMappings)
            //{
            // var reuestDto = _context.CodeMappingRequests.Include("RunType").Include("SegmentType").Include("CodeMappingType").FirstOrDefault(x => x.Id == item.Id);
            //   var requestDto = _context.CodeMappingRequests.Include("RunType").Include("SegmentType").Include("CodeMappingType").Join(_context.CodeMappings, cmRequest => cmRequest.Id, codeMapping => codeMapping.RequestId, (cmRequest, CodeMapping) => new { cmRequest, CodeMapping }).Where(joinedTable => joinedTable.cmRequest.CodeMappingId == (int)CodeMappingType.CodeGeneration).AsNoTracking().ToList();

            // foreach (var item in requestDto)
            //{
            //    GenericSummaryViewModel model = new GenericSummaryViewModel();

            //    model.TaskId = item.cmRequest.Reference;
            //    model.RequestId = item.RequestId;
            //    model.TimeStamp = item.Request.CreatedTime;
            //    model.Segment= item.
            //    //model.Segment = requestDto.SegmentType.Name;
            //    model.RunType = requestDto.RunType.Name;
            //    model.CodeMappingType = requestDto.CodeMappingType.Name;
            //    model.RunBy = requestDto.CreatedBy;
            //    model.Summary = _context.CodeGenerationSummary.AsNoTracking().FirstOrDefault(x => x.RequestId == item.RequestId);
            //    viewModel.Add(model);
            //}
            //if (requestDto.CodeMappingType.Id == (int)CodeMappingType.CodeGeneration)
            //{

            //}
            //}
            return viewModels;
        }
        public List<GenericSummaryViewModel> GetMonthlyEmbeddingMappingRecords()
        {
            List<GenericSummaryViewModel> viewModel = new List<GenericSummaryViewModel>();
            var codeMappings = _context.CodeMappings.Include("Request").AsNoTracking();

            foreach (var item in codeMappings)
            {
                var reuestDto = _context.CodeMappingRequests.Include("RunType").Include("SegmentType").Include("CodeMappingType").FirstOrDefault(x => x.Id == item.Id);
                if (reuestDto.CodeMappingType.Id == (int)CodeMappingType.MonthlyEmbeddings)
                {
                    GenericSummaryViewModel model = new GenericSummaryViewModel();
                    model.TaskId = item.Reference;
                    model.RequestId = item.RequestId;
                    model.TimeStamp = item.Request.CreatedTime;
                    model.Segment = reuestDto.SegmentType.Name;
                    model.RunType = reuestDto.RunType.Name;
                    model.CodeMappingType = reuestDto.CodeMappingType.Name;
                    model.RunBy = reuestDto.CreatedBy;
                    model.Summary = _context.CodeGenerationSummary.AsNoTracking().FirstOrDefault(x => x.RequestId == item.RequestId);
                    viewModel.Add(model);
                }
            }
            return viewModel;
        }
        public List<GenericSummaryViewModel> GetWeeklyEmbeddingsMappingRecords()
        {
            List<GenericSummaryViewModel> viewModel = new List<GenericSummaryViewModel>();
            var codeMappings = _context.CodeMappings.Include("Request").AsNoTracking();

            foreach (var item in codeMappings)
            {
                var reuestDto = _context.CodeMappingRequests.Include("RunType").Include("SegmentType").Include("CodeMappingType").FirstOrDefault(x => x.Id == item.Id);
                if (reuestDto.CodeMappingType.Id == (int)CodeMappingType.WeeklyEmbeddings)
                {
                    GenericSummaryViewModel model = new GenericSummaryViewModel();

                    model.TaskId = item.Reference;
                    model.RequestId = item.RequestId;
                    model.TimeStamp = item.Request.CreatedTime;
                    model.Segment = reuestDto.SegmentType.Name;
                    model.RunType = reuestDto.RunType.Name;
                    model.CodeMappingType = reuestDto.CodeMappingType.Name;
                    model.RunBy = reuestDto.CreatedBy;
                    model.Summary = _context.CodeGenerationSummary.AsNoTracking().FirstOrDefault(x => x.RequestId == item.RequestId);
                    viewModel.Add(model);
                }
            }
            return viewModel;
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
            int summaryId = 0;
            switch (frequncyId)
            {
                case ((int)CodeMappingType.CodeGeneration):
                    summaryId = SaveCgMappingsPythApi(taskId, summary, requestId);
                    break;
                case ((int)CodeMappingType.MonthlyEmbeddings):
                    summaryId = SaveMonthlyEmbedMappingsPythApi(taskId, summary, requestId);
                    break;
                case ((int)CodeMappingType.WeeklyEmbeddings):
                    summaryId = SaveWeeklyEmbedMappingsPythApi(taskId, summary, requestId);
                    break;
                default: break;
            }
            return summaryId;
        }
    }
}

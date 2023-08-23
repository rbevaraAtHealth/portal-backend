using AutoMapper;
using CodeMatcher.Api.V2.BusinessLayer;
using CodeMatcher.Api.V2.Models;
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
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace CodeMatcherV2Api.BusinessLayer
{
    public class CodeMapping : ICodeMapping
    {
        private readonly CodeMatcherDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly SqlHelper _sqlHelper;
        public CodeMapping(CodeMatcherDbContext context, IMapper mapper, IHttpClientFactory httpClientFactory1, SqlHelper sqlHelper)
        {
            _context = context;
            _mapper = mapper;
            _httpClientFactory = httpClientFactory1;
            _sqlHelper = sqlHelper;
        }
        //public  async Task<List<CodeMappingModel>> GetCodeMappingsRecordsAsync()
        //{
        //    List<CodeMappingModel> codeMappingRecords = new List<CodeMappingModel>
        //    {new CodeMappingModel{ TimeStamp = DateTime.Now ,Segment="Insurance",NumberOfRecord=100,NumberOfMatches=97,RunBy="John Smith",RunType="Scheduled",CodeMappingSummary =new CodeMappingSummary{TimeStamp=DateTime.Now,RecordParticulars=38983,Segment_type="State License",Threshhold="87",BaseRecords=2599,InputRecords=107,NoisyRecords=3,ProcessedRecords=104,CodeGeneratedRecords=99,CodeNotGeneratedRecords=5} },
        //        new CodeMappingModel { TimeStamp = DateTime.Now, Segment = "Hospital", NumberOfRecord = 110, NumberOfMatches = 95, RunBy = "Joe Smith", RunType = "Triggered" ,CodeMappingSummary =new CodeMappingSummary{TimeStamp=DateTime.Now,RecordParticulars=38983,Segment_type="State License",Threshhold="87",BaseRecords=2599,InputRecords=107,NoisyRecords=3,ProcessedRecords=104,CodeGeneratedRecords=99,CodeNotGeneratedRecords=5}},
        //        new CodeMappingModel { TimeStamp = DateTime.Now, Segment = "State License", NumberOfRecord = 110, NumberOfMatches = 97, RunBy = "John Smith", RunType = "Scheduled" , CodeMappingSummary = new CodeMappingSummary{ TimeStamp = DateTime.Now, RecordParticulars = 38983, Segment_type = "State License", Threshhold = "87", BaseRecords = 2599, InputRecords = 107, NoisyRecords = 3, ProcessedRecords = 104, CodeGeneratedRecords = 99, CodeNotGeneratedRecords = 5 }},
        //        new CodeMappingModel { TimeStamp = DateTime.Now, Segment = "School", NumberOfRecord = 100, NumberOfMatches = 97, RunBy = "John Smith", RunType = "Scheduled" , CodeMappingSummary = new CodeMappingSummary{ TimeStamp = DateTime.Now, RecordParticulars = 38983, Segment_type = "State License", Threshhold = "87", BaseRecords = 2599, InputRecords = 107, NoisyRecords = 3, ProcessedRecords = 104, CodeGeneratedRecords = 99, CodeNotGeneratedRecords = 5 }},
        //        new CodeMappingModel { TimeStamp = DateTime.Now, Segment = "Insurance", NumberOfRecord = 110, NumberOfMatches = 97, RunBy = "John Smith", RunType = "Scheduled" , CodeMappingSummary = new CodeMappingSummary{ TimeStamp = DateTime.Now, RecordParticulars = 38983, Segment_type = "State License", Threshhold = "87", BaseRecords = 2599, InputRecords = 107, NoisyRecords = 3, ProcessedRecords = 104, CodeGeneratedRecords = 99, CodeNotGeneratedRecords = 5 }}
        //    };

        //    return codeMappingRecords;
        //}

        public async Task<GenericSummaryViewModel> GetMappings(string taskId)
        {
            var codemap = await _context.CodeMappings.FirstOrDefaultAsync(x => x.Reference == taskId.ToString());
            GenericSummaryViewModel summaryViewModel = new GenericSummaryViewModel();

            if (codemap.Status == StatusConst.Success)
            {
                int requestId = _sqlHelper.GetRequestId(taskId);
                int codeMappingId = _sqlHelper.GetCodeMappingId(requestId);
                string codemappingType = _sqlHelper.GetLookupName(codeMappingId);
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
        public async Task<List<GenericSummaryViewModel>> GetCodeGenerationMappingRecords()
        {
            var viewModels = await (from cr in _context.CodeMappingRequests.Include("RunType").Include("SegmentType").Include("CodeMappingType")
                                    join cm in _context.CodeMappings on cr.Id equals cm.RequestId
                                    join cs in _context.CodeGenerationSummary on cr.Id equals cs.RequestId
                                    into csA
                                    from csB in csA.DefaultIfEmpty()
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
                                    }).ToListAsync();

            return viewModels;
        }
        public async Task<List<GenericSummaryViewModel>> GetWeeklyEmbeddingMappingRecords()
        {
            var viewModels = await (from cr in _context.CodeMappingRequests.Include("RunType").Include("SegmentType").Include("CodeMappingType")
                                    join cm in _context.CodeMappings on cr.Id equals cm.RequestId
                                    join cs in _context.WeeklyEmbeddingsSummary on cr.Id equals cs.RequestId
                                    into csA
                                    from csB in csA.DefaultIfEmpty()
                                    where (cr.CodeMappingType.Name == CodeMappingTypeConst.WeeklyEmbeddings)
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
                                    }).ToListAsync();

            return viewModels;
        }

        public async Task<List<GenericSummaryViewModel>> GetMonthlyEmbeddingMappingRecords()
        {
            var viewModels = await (from cr in _context.CodeMappingRequests.Include("RunType").Include("SegmentType").Include("CodeMappingType")
                                    join cm in _context.CodeMappings on cr.Id equals cm.RequestId
                                    join cs in _context.MonthlyEmbeddingsSummary on cr.Id equals cs.RequestId
                                    into csA
                                    from csB in csA.DefaultIfEmpty()
                                    where (cr.CodeMappingType.Name == CodeMappingTypeConst.MonthlyEmbeddings)
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
                                    }).ToListAsync();

            return viewModels;
        }

        public async Task<int> SaveCgMappingsPythApi(string taskId, string summary, int requestId, LoginModel loginModel)

        {
            var cgSummary = JsonConvert.DeserializeObject<CodeGenerationSummaryModel>(summary);
            cgSummary.TaskId = taskId;
            var cgSummaryDto = _mapper.Map<CodeGenerationSummaryDto>(cgSummary);
            cgSummaryDto.RequestId = requestId;
            cgSummaryDto.CreatedBy = loginModel.UserName;
            int summaryid =await _sqlHelper.SaveCodeGenerationSummary(cgSummaryDto);
            _sqlHelper.UpdateCodeMappingStatus(taskId);
            return summaryid;
        }
        public async Task<int> SaveMonthlyEmbedMappingsPythApi(string taskId, string summary, int requestId, LoginModel loginModel)
        {
            var cgSummary = JsonConvert.DeserializeObject<MonthlyEmbedSummaryModel>(summary);
            cgSummary.TaskId = taskId;
            var monthlySummaryDto = _mapper.Map<MonthlyEmbeddingsSummaryDto>(cgSummary);
            monthlySummaryDto.RequestId = requestId;
            monthlySummaryDto.CreatedBy = loginModel.UserName;
            int summaryId = await _sqlHelper.SaveMonthlyEmbedSummary(monthlySummaryDto);
            _sqlHelper.UpdateCodeMappingStatus(taskId);
            return summaryId;
        }
        public async Task<int> SaveWeeklyEmbedMappingsPythApi(string taskId, string summary, int requestId, LoginModel loginModel)
        {
            var cgSummary = JsonConvert.DeserializeObject<WeeklyEmbedSummaryModel>(summary);
            cgSummary.TaskId = taskId;
            var weeklySummaryDto = _mapper.Map<WeeklyEmbeddingsSummaryDto>(cgSummary);
            weeklySummaryDto.RequestId = requestId;
            weeklySummaryDto.CreatedBy = loginModel.UserName;
            var summaryId = await _sqlHelper.SaveWeeklyEmbedSummary(weeklySummaryDto);
            _sqlHelper.UpdateCodeMappingStatus(taskId);
            return summaryId;
        }

        public async Task<int> SaveSummary(string taskId, string summary, LoginModel loginModel)
        {
            int requestId = _sqlHelper.GetRequestId(taskId);
            int frequncyId = _sqlHelper.GetCodeMappingId(requestId);
            string codemappingType = _sqlHelper.GetLookupName(frequncyId);
            int summaryId = 0;
            switch (codemappingType)
            {
                case (CodeMappingTypeConst.CodeGeneration):
                    summaryId = await SaveCgMappingsPythApi(taskId, summary, requestId, loginModel);
                    break;
                case (CodeMappingTypeConst.MonthlyEmbeddings):
                    summaryId = await SaveMonthlyEmbedMappingsPythApi(taskId, summary, requestId, loginModel);
                    break;
                case (CodeMappingTypeConst.WeeklyEmbeddings):
                    summaryId = await SaveWeeklyEmbedMappingsPythApi(taskId, summary, requestId, loginModel);
                    break;
                default: break;
            }
            return summaryId;
        }
        public async Task<List<CodeMappingReqResDataModel>> GetCodeMappingRequestResponse()
        {
            var codeMappingdata = await (from cr in _context.CodeMappingRequests.Include("RunType").Include("SegmentType").Include("CodeMappingType")
                                         join cs in _context.CodeMappingResponses on cr.Id equals cs.RequestId into crM from cmR in crM.DefaultIfEmpty()
                                         join cm in _context.CodeMappings on cr.Id equals cm.RequestId
                                         into csA
                                         from csB in csA.DefaultIfEmpty()
                                         select new CodeMappingReqResDataModel
                                         {
                                             CodeMappingRequest = cr,
                                             CodeMappingResponse = cmR,
                                             CodeMapping = csB

                                         }).ToListAsync();
            return codeMappingdata;
        }
    }
}

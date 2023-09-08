using AutoMapper;
using CodeMappingEfCore.DatabaseModels;
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
        public async Task<List<GenericSummaryViewModel>> GetCodeGenerationMappingRecords(string clientId)
        {
            var viewModels = await (from cr in _context.CodeMappingRequests.Include("RunType").Include("SegmentType").Include("CodeMappingType").OrderByDescending(cr=>cr.CreatedTime)
                                    join cm in _context.CodeMappings on cr.Id equals cm.RequestId
                                    join cs in _context.CodeGenerationSummary on cr.Id equals cs.RequestId
                                    into csA
                                    from csB in csA.DefaultIfEmpty()
                                    where (cr.CodeMappingType.Name == CodeMappingTypeConst.CodeGeneration && cr.RunType.Name != RequestTypeConst.Scheduled && cr.ClientId == clientId)
                                    select new GenericSummaryViewModel
                                    {
                                        TaskId = cm.Reference,
                                        RequestId = cr.Id,
                                        TimeStamp = cr.CreatedTime,
                                        Segment = cr.SegmentType.Name,
                                        RunType = cr.RunType.Name,
                                        CodeMappingType = cr.CodeMappingType.Name,
                                        Threshold = csB.Threshold,
                                        NoofMatches = csB.NoOfRecordsForWhichCodeGenerated,
                                        NoOfRecords = csB.NoOfProcessedRecords,
                                        RunBy = cr.CreatedBy,
                                        Status = cm.Status,
                                        Summary = csB
                                    }).ToListAsync();

            return viewModels;
        }
        public async Task<List<GenericSummaryViewModel>> GetWeeklyEmbeddingMappingRecords(string clientId)
        {
            var viewModels = await (from cr in _context.CodeMappingRequests.Include("RunType").Include("SegmentType").Include("CodeMappingType").OrderByDescending(cr => cr.CreatedTime)
                                    join cm in _context.CodeMappings on cr.Id equals cm.RequestId
                                    join cs in _context.WeeklyEmbeddingsSummary on cr.Id equals cs.RequestId
                                    into csA
                                    from csB in csA.DefaultIfEmpty()
                                    where (cr.CodeMappingType.Name == CodeMappingTypeConst.WeeklyEmbeddings && cr.ClientId == clientId)
                                    select new GenericSummaryViewModel
                                    {
                                        TaskId = cm.Reference,
                                        RequestId = cr.Id,
                                        TimeStamp = cr.CreatedTime,
                                        Segment = cr.SegmentType.Name,
                                        RunType = cr.RunType.Name,
                                        CodeMappingType = cr.CodeMappingType.Name,
                                        NoofMatches = csB.NoOfRecordsAfterRun,
                                        NoOfRecords = csB.NoOfBaseRecordsBeforeRun,
                                        Status = cm.Status,
                                        RunBy = cr.CreatedBy,
                                        Summary = csB
                                    }).ToListAsync();

            return viewModels;
        }

        public async Task<List<GenericSummaryViewModel>> GetMonthlyEmbeddingMappingRecords(string clientId)
        {
            var viewModels = await (from cr in _context.CodeMappingRequests.Include("RunType").Include("SegmentType").Include("CodeMappingType").OrderByDescending(cr=>cr.CreatedTime)
                                    join cm in _context.CodeMappings on cr.Id equals cm.RequestId
                                    join cs in _context.MonthlyEmbeddingsSummary on cr.Id equals cs.RequestId
                                    into csA
                                    from csB in csA.DefaultIfEmpty()
                                    where (cr.CodeMappingType.Name == CodeMappingTypeConst.MonthlyEmbeddings && cr.ClientId == clientId)
                                    select new GenericSummaryViewModel
                                    {
                                        TaskId = cm.Reference,
                                        RequestId = cr.Id,
                                        TimeStamp = cr.CreatedTime,
                                        Segment = cr.SegmentType.Name,
                                        RunType = cr.RunType.Name,
                                        CodeMappingType = cr.CodeMappingType.Name,
                                        NoofMatches = csB.NoOfRecordsEmbeddingCreated,
                                        NoOfRecords = csB.NoOfRecordsImportedFromDatabase,
                                        Status = cm.Status,
                                        RunBy = cr.CreatedBy,
                                        Summary = csB
                                    }).ToListAsync();

            return viewModels;
        }

        public async Task<int> SaveCgMappingsPythApi(string taskId, string summary, int requestId, LoginModel loginModel)

        {
            var cgSummary = JsonConvert.DeserializeObject<CodeGenerationSummaryModel>(summary);
            var cgDto = await _context.CodeGenerationSummary.FirstOrDefaultAsync(x => x.TaskId == taskId);
            if (cgDto != null)
            {
                cgDto.Segment = cgSummary.Segment;
                cgDto.Threshold = cgSummary.Threshold;
                cgDto.NoOfBaseRecords = cgSummary.NoOfBaseRecords;
                cgDto.NoOfInputRecords = cgSummary.NoOfInputRecords;
                cgDto.NoOfProcessedRecords = cgSummary.NoOfProcessedRecords;
                cgDto.NoOfRecordsForWhichCodeGenerated = cgSummary.NoOfRecordsForWhichCodeGenerated;
                cgDto.NoOfRecordsForWhichCodeNotGenerated = cgSummary.NoOfRecordsForWhichCodeNotGenerated;
                cgDto.StartLink = cgSummary.StartLink;
                cgDto.LatestLink = cgSummary.LatestLink;
                cgDto.ClientId = cgSummary.ClientId;
                cgDto.NoOfNoiseRecords = cgSummary.NoOfNoiseRecords;
                cgDto.ModifiedBy = loginModel.UserName;
                cgDto.ModifiedTime = DateTime.Now;
                _context.CodeGenerationSummary.Update(cgDto);
                _context.Entry(cgDto).State = EntityState.Modified;
            }
            else
            {
                var cgnew = _mapper.Map<CodeGenerationSummaryDto>(cgSummary);
                cgnew.TaskId = taskId;
                cgnew.RequestId = requestId;
                cgnew.CreatedBy = loginModel.UserName;
                cgnew.CreatedTime = DateTime.Now;
                _context.CodeGenerationSummary.Add(cgnew);
            }
            var id = await _context.SaveChangesAsync();
            _sqlHelper.UpdateCodeMappingStatus(taskId);
            return id;
        }
        public async Task<int> SaveMonthlyEmbedMappingsPythApi(string taskId, string summary, int requestId, LoginModel loginModel)
        {
            var cgSummary = JsonConvert.DeserializeObject<MonthlyEmbedSummaryModel>(summary);
            var embedDto = await _context.MonthlyEmbeddingsSummary.FirstOrDefaultAsync(x => x.TaskId == taskId);
            if (embedDto != null)
            {
                embedDto.Segment = cgSummary.Segment;
                embedDto.NoOfRecordsImportedFromDatabase = cgSummary.NoOfRecordsImportedFromDatabase;
                embedDto.NoOfRecordsEmbeddingCreated = cgSummary.NoOfRecordsEmbeddingCreated;
                embedDto.ModifiedBy = loginModel.UserName;
                embedDto.ModifiedTime = DateTime.Now;
                _context.MonthlyEmbeddingsSummary.Update(embedDto);
                _context.Entry(embedDto).State = EntityState.Modified;
            }
            else
            {
                cgSummary.TaskId = taskId;
                var monthlySummaryDto = _mapper.Map<MonthlyEmbeddingsSummaryDto>(cgSummary);
                monthlySummaryDto.RequestId = requestId;
                monthlySummaryDto.CreatedBy = loginModel.UserName;
                monthlySummaryDto.CreatedTime = DateTime.Now;
                _context.MonthlyEmbeddingsSummary.Add(monthlySummaryDto);
            }
            var id = await _context.SaveChangesAsync();
            _sqlHelper.UpdateCodeMappingStatus(taskId);
            return id;
        }
        public async Task<int> SaveWeeklyEmbedMappingsPythApi(string taskId, string summary, int requestId, LoginModel loginModel)
        {
            var cgSummary = JsonConvert.DeserializeObject<WeeklyEmbedSummaryModel>(summary);
            var embedDto = await _context.WeeklyEmbeddingsSummary.FirstOrDefaultAsync(x => x.TaskId == taskId);
            if (embedDto != null)
            {
                embedDto.ModifiedBy = loginModel.UserName;
                embedDto.ModifiedTime = DateTime.Now;
                embedDto.Segment = cgSummary.Segment;
                embedDto.NoOfBaseRecordsImportedFromDatabase = cgSummary.NoOfBaseRecordsImportedFromDatabase;
                embedDto.NoOfRecordsEmbeddingsCreated = cgSummary.NoOfRecordsEmbeddingsCreated;
                embedDto.NoOfBaseRecordsBeforeRun = cgSummary.NoOfBaseRecordsBeforeRun;
                embedDto.NoOfRecordsAfterRun = cgSummary.NoOfRecordsAfterRun;
                embedDto.StartLink = cgSummary.StartLink;
                embedDto.LatestLink = cgSummary.LatestLink;
                _context.WeeklyEmbeddingsSummary.Update(embedDto);
                _context.Entry(embedDto).State = EntityState.Modified;
            }
            else
            {
                var summaryDto = _mapper.Map<WeeklyEmbeddingsSummaryDto>(cgSummary);
                summaryDto.TaskId = taskId;
                summaryDto.CreatedBy = loginModel.UserName;
                summaryDto.CreatedTime = DateTime.Now;
                summaryDto.RequestId = requestId;
                _context.WeeklyEmbeddingsSummary.Add(summaryDto);
            }
            var id = await _context.SaveChangesAsync();
            _sqlHelper.UpdateCodeMappingStatus(taskId);
            return id;
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
            var codeMappingdata = await (from cr in _context.CodeMappingRequests.Include("RunType").Include("SegmentType").Include("CodeMappingType").OrderByDescending(cr => cr.CreatedTime)
                                         join cs in _context.CodeMappingResponses on cr.Id equals cs.RequestId into crM
                                         from cmR in crM.DefaultIfEmpty()
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

        public async Task<CodeMappingDto> UpdateTaskStatus(CodeMappingUpdateStatus codeMappingUpdate, LoginModel loginModel)
        {
            CodeMappingDto data = new CodeMappingDto();
            data.Reference = codeMappingUpdate.TaskId;
            data.Status = codeMappingUpdate.Status;
            data.Progress = codeMappingUpdate.Progress;

            var details = await _sqlHelper.UpdateTaskStatus(codeMappingUpdate.TaskId, data);

            return details;

        }

    }
}

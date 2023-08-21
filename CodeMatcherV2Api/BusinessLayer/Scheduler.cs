using AutoMapper;
using CodeMappingEfCore.DatabaseModels;
using CodeMatcher.Api.V2.BusinessLayer.Interfaces;
using CodeMatcher.Api.V2.Models;
using CodeMatcherV2Api.ApiRequestModels;
using CodeMatcherV2Api.EntityFrameworkCore;
using CodeMatcherV2Api.Middlewares.SqlHelper;
using CodeMatcherV2Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeMatcher.Api.V2.BusinessLayer
{
    public class Scheduler : IScheduler
    {

        private readonly CodeMatcherDbContext _context;

        private readonly IMapper _mapper;
        private readonly SqlHelper _sqlHelper;
        public Scheduler(CodeMatcherDbContext context, IMapper mapper, SqlHelper sqlHelper)
        {
            _context = context;
            _mapper = mapper;
            _sqlHelper = sqlHelper;
        }
        public async Task<List<SchedulerModel>> GetAllSchedulersAsync()
        {
            var schedulerRecords = await _context.CodeMappingRequests.Include("RunType").Include("SegmentType").Include("CodeMappingType").Where(x => x.RunType.Name.Equals("Scheduled")).ToListAsync();
            List<SchedulerModel> schedulerModels = new List<SchedulerModel>();

            foreach (var item in schedulerRecords)
            {
                SchedulerModel schedulerModel = new SchedulerModel();
                schedulerModel.ClientId = item.ClientId;
                schedulerModel.Segment = item.SegmentType.Name;
                schedulerModel.Threshold = item.Threshold;
                schedulerModel.CronExpression = item.RunSchedule;
                schedulerModel.CodeMapping = item.CodeMappingType.Name;
                schedulerModels.Add(schedulerModel);
            }
            return schedulerModels;
        }


        public async Task<Tuple<CgScheduledRunReqModel, int>> GetMonthlyScheduleJobAsync(CgScheduledModel schedule, LoginModel user, string clientId)
        {
            CodeMappingRequestDto cgDBRequestModel = new CodeMappingRequestDto();
            cgDBRequestModel.RunTypeId = (await _sqlHelper.GetLookupbyName(LookupTypeConst.RunType, RequestTypeConst.Scheduled)).Id;
            cgDBRequestModel.SegmentTypeId = (await _sqlHelper.GetLookupbyName(LookupTypeConst.Segment, schedule.Segment)).Id;
            cgDBRequestModel.CodeMappingId = (await _sqlHelper.GetLookupbyName(LookupTypeConst.CodeMapping, schedule.CodeMapping)).Id;
            cgDBRequestModel.Threshold = schedule.Threshold;
            cgDBRequestModel.LatestLink = "32345";
            cgDBRequestModel.RunSchedule = schedule.RunSchedule;
            cgDBRequestModel.ClientId = clientId;
            cgDBRequestModel.CreatedBy = user.UserName;
            int reuestId = await _sqlHelper.SaveCodeMappingRequest(cgDBRequestModel);

            CgScheduledRunReqModel requestModel = new CgScheduledRunReqModel();
            requestModel.Segment = schedule.Segment;
            requestModel.RunSchedule = schedule.RunSchedule;
            requestModel.Threshold = schedule.Threshold;
            requestModel.LatestLink = cgDBRequestModel.LatestLink;
            requestModel.ClientId = cgDBRequestModel.ClientId;

            return new Tuple<CgScheduledRunReqModel, int>(requestModel, reuestId);
        }

        public async Task<Tuple<CgScheduledRunReqModel, int>> GetweeklyJobScheduleAsync(CgScheduledModel schedule, LoginModel user, string clientId)
        {
            CodeMappingRequestDto cgDBRequestModel = new CodeMappingRequestDto();
            cgDBRequestModel.RunTypeId = (await _sqlHelper.GetLookupbyName(LookupTypeConst.RunType, RequestTypeConst.Scheduled)).Id;
            cgDBRequestModel.SegmentTypeId = (await _sqlHelper.GetLookupbyName(LookupTypeConst.Segment, schedule.Segment)).Id;
            cgDBRequestModel.CodeMappingId = (await _sqlHelper.GetLookupbyName(LookupTypeConst.CodeMapping, schedule.CodeMapping)).Id;
            cgDBRequestModel.Threshold = schedule.Threshold;
            cgDBRequestModel.LatestLink = "32345";
            cgDBRequestModel.RunSchedule = schedule.RunSchedule;
            cgDBRequestModel.ClientId = clientId;
            cgDBRequestModel.CreatedBy = user.UserName;
            int reuestId = await _sqlHelper.SaveCodeMappingRequest(cgDBRequestModel);

            CgScheduledRunReqModel requestModel = new CgScheduledRunReqModel();
            requestModel.Segment = schedule.Segment;
            requestModel.RunSchedule = schedule.RunSchedule;
            requestModel.Threshold = schedule.Threshold;
            requestModel.LatestLink = cgDBRequestModel.LatestLink;
            requestModel.ClientId = cgDBRequestModel.ClientId;


            return new Tuple<CgScheduledRunReqModel, int>(requestModel, reuestId);
        }

        public async Task<Tuple<CgScheduledRunReqModel, int>> GetCodeGenerationScheduleAsync(CgScheduledModel schedule, LoginModel user, string clientId)
        {
            CodeMappingRequestDto cgDBRequestModel = new CodeMappingRequestDto();
            cgDBRequestModel.RunTypeId = (await _sqlHelper.GetLookupbyName(LookupTypeConst.RunType, RequestTypeConst.Scheduled)).Id;
            cgDBRequestModel.SegmentTypeId = (await _sqlHelper.GetLookupbyName(LookupTypeConst.Segment, schedule.Segment)).Id;
            cgDBRequestModel.CodeMappingId = (await _sqlHelper.GetLookupbyName(LookupTypeConst.CodeMapping, schedule.CodeMapping)).Id;
            cgDBRequestModel.Threshold = schedule.Threshold;
            cgDBRequestModel.LatestLink = "32345";
            cgDBRequestModel.RunSchedule = schedule.RunSchedule;
            cgDBRequestModel.ClientId = clientId;
            cgDBRequestModel.CreatedBy = user.UserName;
            int reuestId = await _sqlHelper.SaveCodeMappingRequest(cgDBRequestModel);

            CgScheduledRunReqModel requestModel = new CgScheduledRunReqModel();
            requestModel.Segment = schedule.Segment;
            requestModel.RunSchedule = schedule.RunSchedule;
            requestModel.Threshold = schedule.Threshold;
            requestModel.LatestLink = cgDBRequestModel.LatestLink;
            requestModel.ClientId = cgDBRequestModel.ClientId;

            return new Tuple<CgScheduledRunReqModel, int>(requestModel, reuestId);

        }
    }
}
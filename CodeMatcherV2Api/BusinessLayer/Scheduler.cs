using AutoMapper;
using CodeMappingEfCore.DatabaseModels;
using CodeMatcher.Api.V2.BusinessLayer.Enums;
using CodeMatcher.Api.V2.BusinessLayer.Interfaces;
using CodeMatcher.Api.V2.Models;
using CodeMatcherV2Api.ApiRequestModels;
using CodeMatcherV2Api.BusinessLayer.Enums;
using CodeMatcherV2Api.EntityFrameworkCore;
using CodeMatcherV2Api.Middlewares.SqlHelper;
using CodeMatcherV2Api.Models;
using CodeMatcherV2Api.RepoModelAdapter;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeMatcher.Api.V2.BusinessLayer
{
    public class Scheduler : IScheduler
    {
        private readonly CgScheduleDbModelAdapter _cgScheduleDbModelAdapter;

        private readonly CodeMatcherDbContext _context;

        private readonly IMapper _mapper;
        public Scheduler(CodeMatcherDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _cgScheduleDbModelAdapter = new CgScheduleDbModelAdapter();
        }
        public async Task<List<SchedulerModel>> GetAllSchedulersAsync()
        {
            var schedulerRecords = _context.CodeMappingRequests.Include("SegmentType").ToList();
            List<SchedulerModel> schedulerModels = new List<SchedulerModel>();

            foreach (var item in schedulerRecords)
            {
                SchedulerModel schedulerModel = new SchedulerModel();
                schedulerModel.ClientId = Convert.ToInt32(item.ClientId);
                schedulerModel.Segment = item.SegmentType.Name;
                schedulerModel.Threshold = item.Threshold;
                schedulerModel.CronExpression = item.RunSchedule;
                schedulerModels.Add(schedulerModel);
            }
            return schedulerModels;
        }


        public async Task<string> GetMonthlyScheduleJobAsync()
        {
            return "Monthly Job scheduled successfully";
        }

        public async Task<string> GetweeklyJobScheduleAsync()
        {
            return "Weekly job scheduled successfully";
        }

        public Tuple<CgScheduledRunReqModel, int> ApiRequestGet(CgScheduledModel schedule, LoginModel user)
        {
            CodeMappingRequestDto cgDBRequestModel = new CodeMappingRequestDto();
            cgDBRequestModel.RunTypeId = SqlHelper.GetLookupType((int)RequestType.Triggered, _context);
            cgDBRequestModel.SegmentTypeId = SqlHelper.GetLookupType(schedule.Segment, _context);
            cgDBRequestModel.CodeMappingId = SqlHelper.GetLookupType((int)CodeMappingType.CodeGeneration, _context);
            cgDBRequestModel.Threshold = schedule.Threshold;
            cgDBRequestModel.LatestLink = "32345";
            cgDBRequestModel.RunSchedule = schedule.RunSchedule;
            cgDBRequestModel.ClientId = "1";
            cgDBRequestModel.CreatedBy = user.UserName;
            int reuestId = SqlHelper.SaveCodeMappingRequest(cgDBRequestModel, _context);

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
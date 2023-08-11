using AutoMapper;
using CodeMappingEfCore.DatabaseModels;
using CodeMatcher.Api.V2.BusinessLayer;
using CodeMatcher.Api.V2.RepoModelAdapter;
using CodeMatcherV2Api.ApiRequestModels;
using CodeMatcherV2Api.ApiResponseModel;
using CodeMatcherV2Api.BusinessLayer.Interfaces;
using CodeMatcherV2Api.Controllers;
using CodeMatcherV2Api.EntityFrameworkCore;
using CodeMatcherV2Api.Middlewares.SqlHelper;
using CodeMatcherV2Api.Models;
using CodeMatcherV2Api.RepoModelAdapter;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CodeMatcherV2Api.BusinessLayer
{
    public class Trigger : ITrigger
    {
        private readonly CgTriggerDbModelAdapter _cgTriggerDbModelAdapter;
        private readonly CodeMatcherDbContext _context;
        private readonly IMapper _mapper;
        public Trigger(CodeMatcherDbContext context, IMapper mapper)
        {
            BaseController baseController = new BaseController();
            _context = context;
            _cgTriggerDbModelAdapter = new CgTriggerDbModelAdapter();
            _mapper = mapper;
            var user = baseController.GetUserInfo();
        }
        public async Task<string> GetAllTriggerAsync()
        {
            return "Job Triggered Successfully";
        }
        public async Task<string> GetCgTriggerJobAsync()
        {
            return "Code generation job triggered successfully";
        }

        public async Task<string> GetMonthlyTriggerJobAsync()
        {
            return "Monthly job triggered successfully";
        }

        public async Task<string> GetWeeklyTriggerJobAsync()
        {
            return "Weekly job triggered successfully";
        }
        public Tuple<CgTriggeredRunReqModel, int> CgApiRequestGet(CgTriggerRunModel trigger, LoginModel user, string clientId)
        {
            CodeMappingRequestDto codeMappingRequestDto = new CodeMappingRequestDto();
            codeMappingRequestDto.RunTypeId = SqlHelper.GetLookupType(RequestTypeConst.Triggered, _context);
            codeMappingRequestDto.SegmentTypeId = SqlHelper.GetLookupType(trigger.Segment, _context);
            codeMappingRequestDto.CodeMappingId = SqlHelper.GetLookupType(CodeMappingTypeConst.CodeGeneration, _context);
            codeMappingRequestDto.Threshold = trigger.Threshold.ToString();
            codeMappingRequestDto.LatestLink = "1";
            codeMappingRequestDto.ClientId = clientId;
            if (user.UserName != null)
            {
                codeMappingRequestDto.CreatedBy = user.UserName;
            }
            else
            {
                codeMappingRequestDto.CreatedBy = "Scheduler Admin"; 
            }
            int reuestId = SqlHelper.SaveCodeMappingRequest(codeMappingRequestDto, _context);
            CgTriggeredRunReqModel requestModel = new CgTriggeredRunReqModel();
            requestModel.Segment = trigger.Segment;
            requestModel.Threshold = trigger.Threshold;
            requestModel.LatestLink = codeMappingRequestDto.LatestLink;
            requestModel.ClientId = codeMappingRequestDto.ClientId;
            return new Tuple<CgTriggeredRunReqModel, int>(requestModel, reuestId);
        }
        public CgTriggeredRunResModel CgAPiResponseSave(HttpResponseMessage httpResponse, int requestId, LoginModel user)
        {
            CgTriggeredRunResModel responseViewModel = new CgTriggeredRunResModel();
            CodeMappingResponseDbModelAdapter adapter = new CodeMappingResponseDbModelAdapter();
            var responseDto = adapter.DbResponseModelGet(httpResponse, requestId); 
            if (user.UserName != null)
            {
                responseDto.CreatedBy = user.UserName;
            }
            else
            {
                responseDto.CreatedBy = "Scheduler Admin";
            }
            //responseDto.CreatedBy = user.UserName;
            SqlHelper.SaveResponseseMessage(responseDto, requestId, _context);
            if (httpResponse.IsSuccessStatusCode)
            {
                string httpResult = httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                if (!string.IsNullOrWhiteSpace(httpResult))
                {
                    responseViewModel = JsonConvert.DeserializeObject<CgTriggeredRunResModel>(httpResult);
                    var codeMappingDto = CodeMappingDbModelAdapter.GetCodeMappingModel(responseDto);
                    int codeMappingId = SqlHelper.SaveCodeMappingData(codeMappingDto, _context);
                }
            }

            return responseViewModel;
        }
        public Tuple<MonthlyEmbedTriggeredRunReqModel, int> MonthlyEmbedApiRequestGet(MonthlyEmbedTriggeredRunModel trigger, LoginModel user, string clientId)
        {
            CodeMappingRequestDto codeMappingRequestDto = new CodeMappingRequestDto();
            codeMappingRequestDto.RunTypeId = SqlHelper.GetLookupType(RequestTypeConst.Triggered, _context);
            codeMappingRequestDto.SegmentTypeId = SqlHelper.GetLookupType(trigger.Segment, _context);
            codeMappingRequestDto.CodeMappingId = SqlHelper.GetLookupType(CodeMappingTypeConst.MonthlyEmbeddings, _context);
            codeMappingRequestDto.CreatedBy = user.UserName;
            codeMappingRequestDto.ClientId = clientId;
            if (user.UserName != null)
            {
                codeMappingRequestDto.CreatedBy = user.UserName;
            }
            else
            {
                codeMappingRequestDto.CreatedBy = "Scheduler Admin";
            }
            int requestId = SqlHelper.SaveCodeMappingRequest(codeMappingRequestDto, _context);
            MonthlyEmbedTriggeredRunReqModel requestModel = new MonthlyEmbedTriggeredRunReqModel();
            requestModel.Segment = trigger.Segment;
            return new Tuple<MonthlyEmbedTriggeredRunReqModel, int>(requestModel, requestId);
        }
        public Tuple<WeeklyEmbedTriggeredRunReqModel, int> WeeklyEmbedApiRequestGet(WeeklyEmbedTriggeredRunModel trigger, LoginModel user, string clientId)
        {
            CodeMappingRequestDto codeMappingRequestDto = new CodeMappingRequestDto();
            codeMappingRequestDto.RunTypeId = SqlHelper.GetLookupType(RequestTypeConst.Triggered, _context);
            codeMappingRequestDto.SegmentTypeId = SqlHelper.GetLookupType(trigger.Segment, _context);
            codeMappingRequestDto.CodeMappingId = SqlHelper.GetLookupType(CodeMappingTypeConst.WeeklyEmbeddings, _context);
            codeMappingRequestDto.CreatedBy=user.UserName;
            codeMappingRequestDto.ClientId = clientId;
            if (user.UserName != null)
            {
                codeMappingRequestDto.CreatedBy = user.UserName;
            }
            else
            {
                codeMappingRequestDto.CreatedBy = "Scheduler Admin";
            }
            int requestId = SqlHelper.SaveCodeMappingRequest(codeMappingRequestDto, _context);
            WeeklyEmbedTriggeredRunReqModel requestModel = new WeeklyEmbedTriggeredRunReqModel();
            requestModel.Segment = trigger.Segment;
            requestModel.LatestLink = "6";

            return new Tuple<WeeklyEmbedTriggeredRunReqModel, int>(requestModel, requestId);
        }
        public MonthlyEmbedTriggeredRunResModel MonthlyEmbedApiResponseSave(HttpResponseMessage httpResponse, int requestId, LoginModel user)
        {
            MonthlyEmbedTriggeredRunResModel responseModel = new MonthlyEmbedTriggeredRunResModel();
            if (httpResponse.IsSuccessStatusCode)
            {
                string httpResult = httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                CodeMappingResponseDbModelAdapter adapter = new CodeMappingResponseDbModelAdapter();
                var responseDto = adapter.DbResponseModelGet(httpResponse, requestId);
                if (user.UserName != null)
                {
                    responseDto.CreatedBy = user.UserName;
                }
                else
                {
                    responseDto.CreatedBy = "Scheduler Admin";
                }
                SqlHelper.SaveResponseseMessage(responseDto, requestId, _context);
                if (!string.IsNullOrWhiteSpace(httpResult))
                {
                    responseModel = JsonConvert.DeserializeObject<MonthlyEmbedTriggeredRunResModel>(httpResult);
                    var codeMappingDto = CodeMappingDbModelAdapter.GetCodeMappingModel(responseDto);
                    int codeMappingId = SqlHelper.SaveCodeMappingData(codeMappingDto, _context);
                }
                if (responseModel != null)
                {
                    return responseModel;
                }
            }
            return responseModel;
        }
        public WeeklyEmbedTriggeredRunResModel WeeklyEmbedApiResponseSave(HttpResponseMessage httpResponse, int requestId, LoginModel user)
        {
            WeeklyEmbedTriggeredRunResModel responseModel = new WeeklyEmbedTriggeredRunResModel();
            CodeMappingResponseDbModelAdapter adapter = new CodeMappingResponseDbModelAdapter();
            var responseDto = adapter.DbResponseModelGet(httpResponse, requestId);
            responseDto.CreatedBy = user.UserName;
            SqlHelper.SaveResponseseMessage(responseDto, requestId, _context);
            if (httpResponse.IsSuccessStatusCode)
            {
                string httpResult = httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                if (!string.IsNullOrWhiteSpace(httpResult))
                {
                    responseModel = JsonConvert.DeserializeObject<WeeklyEmbedTriggeredRunResModel>(httpResult);
                    var codeMappingDto = CodeMappingDbModelAdapter.GetCodeMappingModel(responseDto);
                    int codeMappingId = SqlHelper.SaveCodeMappingData(codeMappingDto, _context);
                }
                return responseModel;
            }
            return responseModel;
        }

        public Tuple<CgTriggeredRunReqModel, int> CgApiRequestGet(CgTriggerRunModel trigger, LoginModel user)
        {
            throw new NotImplementedException();
        }
    }
}

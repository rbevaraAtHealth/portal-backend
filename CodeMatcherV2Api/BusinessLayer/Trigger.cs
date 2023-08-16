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
        private readonly SqlHelper _sqlHelper;
        private readonly IMapper _mapper;
        public Trigger(CodeMatcherDbContext context, IMapper mapper,SqlHelper sqlHelper)
        {
            BaseController baseController = new BaseController();
            _context = context;
            _cgTriggerDbModelAdapter = new CgTriggerDbModelAdapter(_sqlHelper);
            _mapper = mapper;
            var user = baseController.GetUserInfo();
            _sqlHelper = sqlHelper;
        }
        //public async Task<string> GetAllTriggerAsync()
        //{
        //    return "Job Triggered Successfully";
        //}
        //public async Task<string> GetCgTriggerJobAsync()
        //{
        //    return "Code generation job triggered successfully";
        //}

        //public async Task<string> GetMonthlyTriggerJobAsync()
        //{
        //    return "Monthly job triggered successfully";
        //}

        //public async Task<string> GetWeeklyTriggerJobAsync()
        //{
        //    return "Weekly job triggered successfully";
        //}
        public Tuple<CgTriggeredRunReqModel, int> CgApiRequestGet(CgTriggerRunModel trigger, LoginModel user, string clientId)
        {
            CodeMappingRequestDto codeMappingRequestDto = new CodeMappingRequestDto();
            codeMappingRequestDto.RunTypeId = _sqlHelper.GetLookupIdOnName(RequestTypeConst.Triggered);
            codeMappingRequestDto.SegmentTypeId = _sqlHelper.GetLookupIdOnName(trigger.Segment);
            codeMappingRequestDto.CodeMappingId = _sqlHelper.GetLookupIdOnName(CodeMappingTypeConst.CodeGeneration);
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
            int reuestId = _sqlHelper.SaveCodeMappingRequest(codeMappingRequestDto);
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
            _sqlHelper.SaveResponseseMessage(responseDto, requestId);
            if (httpResponse.IsSuccessStatusCode)
            {
                string httpResult = httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                if (!string.IsNullOrWhiteSpace(httpResult))
                {
                    responseViewModel = JsonConvert.DeserializeObject<CgTriggeredRunResModel>(httpResult);
                    var codeMappingDto = CodeMappingDbModelAdapter.GetCodeMappingModel(responseDto);
                    int codeMappingId = _sqlHelper.SaveCodeMappingData(codeMappingDto);
                }
            }

            return responseViewModel;
        }
        public Tuple<MonthlyEmbedTriggeredRunReqModel, int> MonthlyEmbedApiRequestGet(MonthlyEmbedTriggeredRunModel trigger, LoginModel user, string clientId)
        {
            CodeMappingRequestDto codeMappingRequestDto = new CodeMappingRequestDto();
            codeMappingRequestDto.RunTypeId = _sqlHelper.GetLookupIdOnName(RequestTypeConst.Triggered);
            codeMappingRequestDto.SegmentTypeId = _sqlHelper.GetLookupIdOnName(trigger.Segment);
            codeMappingRequestDto.CodeMappingId = _sqlHelper.GetLookupIdOnName(CodeMappingTypeConst.MonthlyEmbeddings);
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
            int requestId = _sqlHelper.SaveCodeMappingRequest(codeMappingRequestDto);
            MonthlyEmbedTriggeredRunReqModel requestModel = new MonthlyEmbedTriggeredRunReqModel();
            requestModel.Segment = trigger.Segment;
            return new Tuple<MonthlyEmbedTriggeredRunReqModel, int>(requestModel, requestId);
        }
        public Tuple<WeeklyEmbedTriggeredRunReqModel, int> WeeklyEmbedApiRequestGet(WeeklyEmbedTriggeredRunModel trigger, LoginModel user, string clientId)
        {
            CodeMappingRequestDto codeMappingRequestDto = new CodeMappingRequestDto();
            codeMappingRequestDto.RunTypeId = _sqlHelper.GetLookupIdOnName(RequestTypeConst.Triggered);
            codeMappingRequestDto.SegmentTypeId = _sqlHelper.GetLookupIdOnName(trigger.Segment);
            codeMappingRequestDto.CodeMappingId = _sqlHelper.GetLookupIdOnName(CodeMappingTypeConst.WeeklyEmbeddings);
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
            int requestId = _sqlHelper.SaveCodeMappingRequest(codeMappingRequestDto);
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
                _sqlHelper.SaveResponseseMessage(responseDto, requestId);
                if (!string.IsNullOrWhiteSpace(httpResult))
                {
                    responseModel = JsonConvert.DeserializeObject<MonthlyEmbedTriggeredRunResModel>(httpResult);
                    var codeMappingDto = CodeMappingDbModelAdapter.GetCodeMappingModel(responseDto);
                    int codeMappingId = _sqlHelper.SaveCodeMappingData(codeMappingDto);
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
            _sqlHelper.SaveResponseseMessage(responseDto, requestId);
            if (httpResponse.IsSuccessStatusCode)
            {
                string httpResult = httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                if (!string.IsNullOrWhiteSpace(httpResult))
                {
                    responseModel = JsonConvert.DeserializeObject<WeeklyEmbedTriggeredRunResModel>(httpResult);
                    var codeMappingDto = CodeMappingDbModelAdapter.GetCodeMappingModel(responseDto);
                    int codeMappingId = _sqlHelper.SaveCodeMappingData(codeMappingDto);
                }
                return responseModel;
            }
            return responseModel;
        }

        //public Tuple<CgTriggeredRunReqModel, int> CgApiRequestGet(CgTriggerRunModel trigger, LoginModel user)
        //{
        //    throw new NotImplementedException();
        //}

        //public Tuple<MonthlyEmbedTriggeredRunReqModel, int> MonthlyEmbedApiRequestGet(MonthlyEmbedTriggeredRunModel trigger, LoginModel user)
        //{
        //    throw new NotImplementedException();
        //}

        //public Tuple<WeeklyEmbedTriggeredRunReqModel, int> WeeklyEmbedApiRequestGet(WeeklyEmbedTriggeredRunModel trigger, LoginModel user)
        //{
        //    throw new NotImplementedException();
        //}
    }
}

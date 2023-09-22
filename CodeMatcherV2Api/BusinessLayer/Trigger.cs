using AutoMapper;
using CodeMappingEfCore.DatabaseModels;
using CodeMatcher.Api.V2.BusinessLayer;
using CodeMatcher.Api.V2.BusinessLayer.Dictionary;
using CodeMatcher.Api.V2.RepoModelAdapter;
using CodeMatcherV2Api.ApiRequestModels;
using CodeMatcherV2Api.ApiResponseModel;
using CodeMatcherV2Api.BusinessLayer.Interfaces;
using CodeMatcherV2Api.Controllers;
using CodeMatcherV2Api.Middlewares.SqlHelper;
using CodeMatcherV2Api.Models;
using CodeMatcherV2Api.RepoModelAdapter;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CodeMatcherV2Api.BusinessLayer
{
    public class Trigger : ITrigger
    {
        private readonly SqlHelper _sqlHelper;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public Trigger(IMapper mapper, SqlHelper sqlHelper, IConfiguration configuration)
        {
            BaseController baseController = new BaseController();
            _mapper = mapper;
            var user = baseController.GetUserInfo();
            _sqlHelper = sqlHelper;
            _configuration = configuration;
        }

        public async Task<Tuple<CgTriggeredRunReqModel, int>> CgApiRequestGet(CgTriggerRunModel trigger, LoginModel user, string clientId)
        {
            CodeMappingRequestDto codeMappingRequestDto = new CodeMappingRequestDto();
            codeMappingRequestDto.RunTypeId = (await _sqlHelper.GetLookupbyName(LookupTypeConst.RunType, RequestTypeConst.Triggered)).Id;
            codeMappingRequestDto.SegmentTypeId = (await _sqlHelper.GetLookupbyName(LookupTypeConst.Segment, trigger.Segment)).Id;
            codeMappingRequestDto.CodeMappingId = (await _sqlHelper.GetLookupbyName(LookupTypeConst.CodeMapping, CodeMappingTypeConst.CodeGeneration)).Id;
            codeMappingRequestDto.Threshold = trigger.Threshold.ToString();
            codeMappingRequestDto.LatestLink = "1";
            codeMappingRequestDto.CreatedBy = user.UserName != null ? user.UserName : "Scheduler Admin";
            codeMappingRequestDto.ClientId = !string.IsNullOrEmpty(clientId)? clientId: "No Client";
            int reuestId = await _sqlHelper.SaveCodeMappingRequest(codeMappingRequestDto);
            var requestModel = _mapper.Map<CgTriggeredRunReqModel>(codeMappingRequestDto);
            requestModel.ConnectionString = _configuration.GetSection(codeMappingRequestDto.ClientId).GetSection("source").Value;
            //requestModel.Segment = trigger.Segment;
            requestModel.Segment = SegmentDictionary.GetSegmentValueByKey(trigger.Segment);
            return new Tuple<CgTriggeredRunReqModel, int>(requestModel, reuestId);
        }
        public async Task<CgTriggeredRunResModel> CgAPiResponseSave(HttpResponseMessage httpResponse, int requestId, LoginModel user)
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
            await _sqlHelper.SaveResponseseMessage(responseDto, requestId);
            if (httpResponse.IsSuccessStatusCode)
            {
                string httpResult = httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                if (!string.IsNullOrWhiteSpace(httpResult))
                {
                    responseViewModel = JsonConvert.DeserializeObject<CgTriggeredRunResModel>(httpResult);
                    var codeMappingDto = CodeMappingDbModelAdapter.GetCodeMappingModel(responseDto);
                    int codeMappingId = await _sqlHelper.SaveCodeMappingData(codeMappingDto);
                }
            }

            return responseViewModel;
        }
        public async Task<Tuple<MonthlyEmbedTriggeredRunReqModel, int>> MonthlyEmbedApiRequestGet(MonthlyEmbedTriggeredRunModel trigger, LoginModel user, string clientId)
        {
            CodeMappingRequestDto codeMappingRequestDto = new CodeMappingRequestDto();
            codeMappingRequestDto.RunTypeId = (await _sqlHelper.GetLookupbyName(LookupTypeConst.RunType, RequestTypeConst.Triggered)).Id;
            codeMappingRequestDto.SegmentTypeId = (await _sqlHelper.GetLookupbyName(LookupTypeConst.Segment, trigger.Segment)).Id;
            codeMappingRequestDto.CodeMappingId = (await _sqlHelper.GetLookupbyName(LookupTypeConst.CodeMapping, CodeMappingTypeConst.MonthlyEmbeddings)).Id; codeMappingRequestDto.CreatedBy = user.UserName;
            codeMappingRequestDto.CreatedBy = user.UserName != null ? user.UserName : "Scheduler Admin";
            codeMappingRequestDto.ClientId = !string.IsNullOrEmpty(clientId) ? clientId : "No Client";
            int requestId = await _sqlHelper.SaveCodeMappingRequest(codeMappingRequestDto);
            MonthlyEmbedTriggeredRunReqModel requestModel = new MonthlyEmbedTriggeredRunReqModel();
            requestModel.Segment = SegmentDictionary.GetSegmentValueByKey(trigger.Segment);
            requestModel.ConnectionString= _configuration.GetSection(codeMappingRequestDto.ClientId).GetSection("destination").Value;
            return new Tuple<MonthlyEmbedTriggeredRunReqModel, int>(requestModel, requestId);
        }
        public async Task<Tuple<WeeklyEmbedTriggeredRunReqModel, int>> WeeklyEmbedApiRequestGet(WeeklyEmbedTriggeredRunModel trigger, LoginModel user, string clientId)
        {
            CodeMappingRequestDto codeMappingRequestDto = new CodeMappingRequestDto();
            codeMappingRequestDto.RunTypeId = (await _sqlHelper.GetLookupbyName(LookupTypeConst.RunType, RequestTypeConst.Triggered)).Id;
            codeMappingRequestDto.SegmentTypeId = (await _sqlHelper.GetLookupbyName(LookupTypeConst.Segment, trigger.Segment)).Id;
            codeMappingRequestDto.CodeMappingId = (await _sqlHelper.GetLookupbyName(LookupTypeConst.CodeMapping, CodeMappingTypeConst.WeeklyEmbeddings)).Id;
            codeMappingRequestDto.CreatedBy = user.UserName;
            codeMappingRequestDto.CreatedBy = user.UserName != null ? user.UserName : "Scheduler Admin";
            codeMappingRequestDto.ClientId = !string.IsNullOrEmpty(clientId) ? clientId : "No Client";
            int requestId = await _sqlHelper.SaveCodeMappingRequest(codeMappingRequestDto);
            WeeklyEmbedTriggeredRunReqModel requestModel = new WeeklyEmbedTriggeredRunReqModel();
            requestModel.Segment = SegmentDictionary.GetSegmentValueByKey(trigger.Segment);
            requestModel.LatestLink = "1";
            requestModel.ConnectionString = _configuration.GetSection(codeMappingRequestDto.ClientId).GetSection("destination").Value ;

            return new Tuple<WeeklyEmbedTriggeredRunReqModel, int>(requestModel, requestId);
        }
        public async Task<MonthlyEmbedTriggeredRunResModel> MonthlyEmbedApiResponseSave(HttpResponseMessage httpResponse, int requestId, LoginModel user)
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
                await _sqlHelper.SaveResponseseMessage(responseDto, requestId);
                if (!string.IsNullOrWhiteSpace(httpResult))
                {
                    responseModel = JsonConvert.DeserializeObject<MonthlyEmbedTriggeredRunResModel>(httpResult);
                    var codeMappingDto = CodeMappingDbModelAdapter.GetCodeMappingModel(responseDto);
                        int codeMappingId = await _sqlHelper.SaveCodeMappingData(codeMappingDto);
                }
                if (responseModel != null)
                {
                    return responseModel;
                }
            }
            return responseModel;
        }
        public async Task<WeeklyEmbedTriggeredRunResModel> WeeklyEmbedApiResponseSave(HttpResponseMessage httpResponse, int requestId, LoginModel user)
        {
            WeeklyEmbedTriggeredRunResModel responseModel = new WeeklyEmbedTriggeredRunResModel();
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
            await _sqlHelper.SaveResponseseMessage(responseDto, requestId);
            if (httpResponse.IsSuccessStatusCode)
            {
                string httpResult = httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                if (!string.IsNullOrWhiteSpace(httpResult))
                {
                    responseModel = JsonConvert.DeserializeObject<WeeklyEmbedTriggeredRunResModel>(httpResult);
                    var codeMappingDto = CodeMappingDbModelAdapter.GetCodeMappingModel(responseDto);
                    await _sqlHelper.SaveCodeMappingData(codeMappingDto);
                }
                return responseModel;
            }
            return responseModel;
        }
    }
}

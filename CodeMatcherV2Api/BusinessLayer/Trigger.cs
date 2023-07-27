using AutoMapper;
using CodeMatcher.Api.V2.BusinessLayer.Enums;
using CodeMatcherV2Api.ApiRequestModels;
using CodeMatcherV2Api.ApiResponeModel;
using CodeMatcherV2Api.BusinessLayer.Enums;
using CodeMatcherV2Api.BusinessLayer.Interfaces;
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
            _context = context;
            _cgTriggerDbModelAdapter = new CgTriggerDbModelAdapter();
            _mapper = mapper;
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
        public Tuple<CgTriggeredRunReqModel, int> CgApiRequestGet(CgTriggerRunModel trigger)
        {
            CgTriggeredRunReqModel requestModel = new CgTriggeredRunReqModel();
            requestModel.Segment = trigger.Segment;
            requestModel.Threshold = trigger.Threshold;
            requestModel.LatestLink = "1";
            requestModel.ClientId = "All";
            var cgReqDbModel = _cgTriggerDbModelAdapter.RequestModel_Get(requestModel, RequestType.Triggered,CodeMappingType.CodeGeneration, _context);
            int reuestId = SqlHelper.SaveCodeMappingRequest(cgReqDbModel, _context);
            return new Tuple<CgTriggeredRunReqModel, int>(requestModel, reuestId);
        }
        public CgTriggeredRunResModel CgAPiResponseSave(HttpResponseMessage httpResponse, int requestId)
        {
            CgTriggeredRunResModel responseViewModel = new CgTriggeredRunResModel();
            CodeMappingResponseDbModelAdapter adapter=new CodeMappingResponseDbModelAdapter();
            var responseDto = adapter.DbResponseModelGet(httpResponse, requestId);
            SqlHelper.SaveResponseseMessage(responseDto, requestId, _context);
            if (httpResponse.IsSuccessStatusCode)
            {
                string httpResult = httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                if (!string.IsNullOrWhiteSpace(httpResult))
                {
                    responseViewModel = JsonConvert.DeserializeObject<CgTriggeredRunResModel>(httpResult);
                }
            }
            return responseViewModel;
        }
        public Tuple<MonthlyEmbedTriggeredRunReqModel, int> MonthlyEmbedApiRequestGet(MonthlyEmbedTriggeredRunModel trigger)
        {
            MonthlyEmbedTriggeredRunReqModel requestModel = new MonthlyEmbedTriggeredRunReqModel();
            requestModel.Segment = trigger.Segment;
            MonthlyEmbedTriggeredDbModelAdapter adapter = new MonthlyEmbedTriggeredDbModelAdapter();
            var EmbedDbModel = adapter.RequestModel_Get(requestModel, RequestType.Triggered,CodeMappingType.MonthlyEmbeddings, _context);
            int requestId = SqlHelper.SaveCodeMappingRequest(EmbedDbModel, _context);
            return new Tuple<MonthlyEmbedTriggeredRunReqModel, int>(requestModel, requestId);
        }
        public Tuple<WeeklyEmbedTriggeredRunReqModel, int> WeeklyEmbedApiRequestGet(WeeklyEmbedTriggeredRunModel trigger)
        {
            WeeklyEmbedTriggeredRunReqModel requestModel = new WeeklyEmbedTriggeredRunReqModel();
            requestModel.Segment = trigger.Segment;
            requestModel.LatestLink = "6";
            WeeklyEmbedTriggeredDbModelAdapter adapter = new WeeklyEmbedTriggeredDbModelAdapter();
            var EmbedDbModel = adapter.RequestModel_Get(requestModel, RequestType.Triggered,CodeMappingType.WeeklyEmbeddings, _context);
            int requestId = SqlHelper.SaveCodeMappingRequest(EmbedDbModel, _context);
            return new Tuple<WeeklyEmbedTriggeredRunReqModel, int>(requestModel, requestId);
        }
        public MonthlyEmbedTriggeredRunResModel MonthlyEmbedApiResponseSave(HttpResponseMessage httpResponse, int requestId)
        {
            MonthlyEmbedTriggeredRunResModel responseModel = new MonthlyEmbedTriggeredRunResModel();
            if (httpResponse.IsSuccessStatusCode)
            {
                string httpResult = httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                CodeMappingResponseDbModelAdapter adapter = new CodeMappingResponseDbModelAdapter();
                var responseDto = adapter.DbResponseModelGet(httpResponse, requestId);
                SqlHelper.SaveResponseseMessage(responseDto, requestId, _context);
                if (!string.IsNullOrWhiteSpace(httpResult))
                    responseModel = JsonConvert.DeserializeObject<MonthlyEmbedTriggeredRunResModel>(httpResult);
                if (responseModel != null)
                {
                    return responseModel;
                }
            }
            return responseModel;
        }
        public WeeklyEmbedTriggeredRunResModel WeeklyEmbedApiResponseSave(HttpResponseMessage httpResponse, int requestId)
        {
            WeeklyEmbedTriggeredRunResModel responseModel = new WeeklyEmbedTriggeredRunResModel();
            CodeMappingResponseDbModelAdapter adapter = new CodeMappingResponseDbModelAdapter();
            var responseDto = adapter.DbResponseModelGet(httpResponse, requestId);
            SqlHelper.SaveResponseseMessage(responseDto, requestId, _context);
            if (httpResponse.IsSuccessStatusCode)
            {
                string httpResult = httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                if (!string.IsNullOrWhiteSpace(httpResult))
                    responseModel = JsonConvert.DeserializeObject<WeeklyEmbedTriggeredRunResModel>(httpResult);
                return responseModel;
            }
            return responseModel;
        }
    }
}

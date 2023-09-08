using SchedulerProcessor.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NCrontab.Advanced;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SchedulerProcessor
{
    public class SchedulerProcessor
    {
        public IConfiguration _configuration;
        public HttpClient _httpClient;
        public ResponseViewModel _responseViewModel;

        public SchedulerProcessor(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = httpClientFactory.CreateClient("AzureFunction");
            _responseViewModel = new ResponseViewModel();
        }
        [FunctionName("Processor")]
        public async Task SchedulerTimeRun([TimerTrigger("*/1 * * * *")] TimerInfo myTimer, ILogger log)
        {
            var curExecutionDate = DateTime.Now;
            var nextRunSchedule = myTimer.Schedule.GetNextOccurrence(DateTime.Now);
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            string url = "Scheduler/GetSchedulerRecords";
            var address = _httpClient.BaseAddress + url;
            log.LogInformation($"Api Url: {address}");

            HttpResponseMessage response = await _httpClient.GetAsync(address);
            log.LogInformation($"Response from the url: {response}");
            try
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    var data = JsonConvert.DeserializeObject<ResponseViewModel>(result);
                    if (data != null)
                    {
                        log.LogInformation($"All schedules Data: {data.Model}");
                        var list = JsonConvert.DeserializeObject<List<SchedulerModel>>(data.Model.ToString());
                        
                        foreach (var details in list)
                        {
                            var schedule = CrontabSchedule.TryParse(details.CronExpression).GetNextOccurrence(curExecutionDate);
                            if (schedule >= curExecutionDate && schedule <= nextRunSchedule)
                            {
                                log.LogInformation($"Call Job API of CLientId: {details.ClientId}");
                                if (details.CronExpression != null)
                                {
                                    _httpClient.DefaultRequestHeaders.Add("ClientID", details.ClientId);
                                    if (details.CodeMapping.ToLower() == "code generation")
                                    {
                                        CgTriggerRunModel models = new CgTriggerRunModel();
                                        models.Segment = details.Segment;
                                        models.Threshold = details.Threshold;
                                        var requestContent = JsonConvert.SerializeObject(models);
                                        var content = new StringContent(requestContent, System.Text.Encoding.UTF8, "application/json");

                                        var cgTriggeredUrl = _httpClient.BaseAddress + "TriggeredRun/CodeGenerationTriggerRun";
                                        var cgTriggeredResponse = await _httpClient.PostAsync(cgTriggeredUrl, content);
                                        var cgData = cgTriggeredResponse.Content.ReadAsAsync<List<CgTriggerRunModel>>();
                                    }
                                    else if (details.CodeMapping.ToLower() == "weekly embedding")
                                    {
                                        WeeklyEmbedTriggeredRunModel models = new WeeklyEmbedTriggeredRunModel();
                                        models.Segment = details.Segment;
                                        var requestContent = JsonConvert.SerializeObject(models);
                                        var content = new StringContent(requestContent, System.Text.Encoding.UTF8, "application/json");

                                        var cgTriggeredUrl = _httpClient.BaseAddress + "TriggeredRun/WeeklyEmbeddingTriggerRun";
                                        var cgTriggeredResponse = await _httpClient.PostAsync(cgTriggeredUrl, content);
                                        var cgData = cgTriggeredResponse.Content.ReadAsAsync<List<WeeklyEmbedTriggeredRunModel>>();
                                    }
                                    else if (details.CodeMapping.ToLower() == "monthly embedding")
                                    {
                                        MonthlyEmbedTriggeredRunModel models = new MonthlyEmbedTriggeredRunModel();
                                        models.Segment = details.Segment;
                                        var requestContent = JsonConvert.SerializeObject(models);
                                        var content = new StringContent(requestContent, System.Text.Encoding.UTF8, "application/json");

                                        var cgTriggeredUrl = _httpClient.BaseAddress + "TriggeredRun/MonthlyEmbeddingTriggerRun";
                                        var cgTriggeredResponse = await _httpClient.PostAsync(cgTriggeredUrl, content);
                                        var cgData = cgTriggeredResponse.Content.ReadAsAsync<List<MonthlyEmbedTriggeredRunModel>>();
                                    }
                                    else
                                    {
                                        log.LogInformation($"");
                                    }
                                }
                            }
                            else
                            {
                                log.LogInformation($"Next schedule for clientId - {details.ClientId} is at - {schedule}");
                            }

                        }
                    }
                    else
                        log.LogInformation("Data cannot be null.");
                }
                else
                    log.LogError($"{response.StatusCode} {response.ReasonPhrase}: ");
            }
            catch
            {
                log.LogError($"Error: {response.StatusCode} {response.ReasonPhrase}: ");
            }

        }
    }
}

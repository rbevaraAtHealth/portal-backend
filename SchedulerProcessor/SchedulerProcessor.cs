using CodeMatcher.Api.V2.Models;
using CodeMatcherV2Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NCrontab.Advanced;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

        public SchedulerProcessor(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = httpClientFactory.CreateClient("AzureFunction");
        }
        [FunctionName("Processor")]
        public async Task SchedulerTimeRun([TimerTrigger("*/1 * * * *",
           #if DEBUG
            RunOnStartup =true
           #endif
            )]TimerInfo myTimer, ILogger log)
        {
            var curExecutionDate = DateTime.Now;
            var nextRunSchedule = myTimer.Schedule.GetNextOccurrence(DateTime.Now);
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            string url = "Scheduler/GetSchedulerRecords";
            var address = _httpClient.BaseAddress + url;
            var response = await _httpClient.GetAsync(address);
            try
            {
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsAsync<List<SchedulerModel>>();
                    if (data != null)
                    {
                        foreach (var details in data)
                        {
                            var schedule = CrontabSchedule.TryParse(details.CronExpression).GetNextOccurrence(curExecutionDate);
                            if (schedule >= curExecutionDate && schedule <= nextRunSchedule)
                            {
                                log.LogInformation($"Call Job API");
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
            catch (Exception ex)
            {
                log.LogError($"Error: {response.StatusCode} {response.ReasonPhrase}: ");
            }

        }
    }
}

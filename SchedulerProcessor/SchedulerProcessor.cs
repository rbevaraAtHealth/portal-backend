using CodeMatcher.Api.V2.Models;
using CodeMatcherV2Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NCrontab.Advanced;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SchedulerProcessor
{
    public class SchedulerProcessor
    {
        public IConfiguration _configuration;
        public HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SchedulerProcessor(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpClient = httpClientFactory.CreateClient("AzureFunction");
            _httpContextAccessor = httpContextAccessor;
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
                            if (details.CronExpression != null)
                            {
                                if (details.CodeMapping == "Code Generation")
                                {
                                    var cgTriggeredUrl = _httpClient.BaseAddress + "TriggeredRun/CodeGenerationTriggerRun";
                                    var cgTriggeredResponse = await _httpClient.GetAsync(cgTriggeredUrl);
                                    var cgData = cgTriggeredResponse.Content.ReadAsAsync<List<CgTriggerRunModel>>();
                                }
                                else if (details.CodeMapping == "Weekly Embedding")
                                {
                                    var cgTriggeredUrl = _httpClient.BaseAddress + "TriggeredRun/WeeklyEmbeddingTriggerRun";
                                    var cgTriggeredResponse = await _httpClient.GetAsync(cgTriggeredUrl);
                                    var cgData = cgTriggeredResponse.Content.ReadAsAsync<List<WeeklyEmbedTriggeredRunModel>>();
                                }
                                else if (details.CodeMapping == "Monthly Embedding")
                                {
                                    var cgTriggeredUrl = _httpClient.BaseAddress + "TriggeredRun/MonthlyEmbeddingTriggerRun";
                                    var cgTriggeredResponse = await _httpClient.GetAsync(cgTriggeredUrl);
                                    var cgData = cgTriggeredResponse.Content.ReadAsAsync<List<MonthlyEmbedTriggeredRunModel>>();
                                }
                                else
                                {

                                }

                                var schedule = CrontabSchedule.TryParse(details.CronExpression).GetNextOccurrence(curExecutionDate);
                                if (schedule >= curExecutionDate && schedule <= nextRunSchedule)
                                {
                                    log.LogInformation($"Call Job API");
                                }
                                else
                                {
                                    log.LogInformation($"Next schedule for clientId - {details.ClientId} is at - {schedule}");
                                }

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

using CodeMatcher.Api.V2.BusinessLayer;
using CodeMatcher.Api.V2.BusinessLayer.Interfaces;
using CodeMatcher.Api.V2.Common;
using CodeMatcherApiV2.Repositories;
using CodeMatcherV2Api.Models;
using Microsoft.Extensions.Logging;
using NCrontab;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Timers;

namespace CodeMatcher.Api.V2
{
    public class TimerJob
    {
        private readonly IScheduler _scheduler;
        private double _interval;
        private readonly HttpClient _httpClient;
        public TimerJob(IScheduler scheduler, IHttpClientFactory httpClientFactory) { 
            _scheduler = scheduler;
            _httpClient = httpClientFactory.CreateClient("BackendApi");
        }

        public void InvokeTimerJob(double interval)
        {
            var aTimer = new Timer(interval); //one hour in milliseconds
            _interval = interval;
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Start();
        }
        private async void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            var schedulerList = await _scheduler.GetAllSchedulersAsync();
            var curExecutionDate = DateTime.Now.RoundDownToMinutes();
            Console.WriteLine($"Current Job Run Time: {curExecutionDate}");
            var nextRunSchedule = curExecutionDate.AddMilliseconds(_interval);
            Console.WriteLine($"Next Job Run Time: {nextRunSchedule}");
            foreach (var details in schedulerList)
            {
                var schedule = CrontabSchedule.TryParse(details.CronExpression).GetNextOccurrence(curExecutionDate);
                if (schedule >= curExecutionDate && schedule <= nextRunSchedule)
                {
                    Console.WriteLine($"Call Job API of CLientId: {details.ClientId}");
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
                            if (!cgTriggeredResponse.IsSuccessStatusCode)
                            {
                                Console.WriteLine($"Unable to schedule triggeredrun for clientId - {details}");
                            }
                        }
                        else if (details.CodeMapping.ToLower() == "weekly embedding")
                        {
                            WeeklyEmbedTriggeredRunModel models = new WeeklyEmbedTriggeredRunModel();
                            models.Segment = details.Segment;
                            var requestContent = JsonConvert.SerializeObject(models);
                            var content = new StringContent(requestContent, System.Text.Encoding.UTF8, "application/json");

                            var cgTriggeredUrl = _httpClient.BaseAddress + "TriggeredRun/WeeklyEmbeddingTriggerRun";
                            var cgTriggeredResponse = await _httpClient.PostAsync(cgTriggeredUrl, content);
                            if (!cgTriggeredResponse.IsSuccessStatusCode)
                            {
                                Console.WriteLine($"Unable to schedule weekly embedding for clientId - {details}");
                            }
                        }
                        else if (details.CodeMapping.ToLower() == "monthly embedding")
                        {
                            MonthlyEmbedTriggeredRunModel models = new MonthlyEmbedTriggeredRunModel();
                            models.Segment = details.Segment;
                            var requestContent = JsonConvert.SerializeObject(models);
                            var content = new StringContent(requestContent, System.Text.Encoding.UTF8, "application/json");

                            var cgTriggeredUrl = _httpClient.BaseAddress + "TriggeredRun/MonthlyEmbeddingTriggerRun";
                            var cgTriggeredResponse = await _httpClient.PostAsync(cgTriggeredUrl, content);
                            if (!cgTriggeredResponse.IsSuccessStatusCode)
                            {
                                Console.WriteLine($"Unable to schedule monthly embedding for clientId - {details}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Cannot process with empty cronexpression {details}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Next schedule for clientId - {details.ClientId} is at - {schedule}");
                }
            }
            Console.WriteLine("Job triggred every second");
            //Do the stuff you want to be done every hour;
        }
    }
}

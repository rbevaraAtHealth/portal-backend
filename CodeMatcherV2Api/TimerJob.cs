﻿using CodeMatcher.Api.V2.BusinessLayer;
using CodeMatcher.Api.V2.BusinessLayer.Interfaces;
using CodeMatcher.Api.V2.Common;
using CodeMatcher.EntityFrameworkCore.DatabaseModels;
using CodeMatcherApiV2.Repositories;
using CodeMatcherV2Api.BusinessLayer;
using CodeMatcherV2Api.BusinessLayer.Interfaces;
using CodeMatcherV2Api.Middlewares.HttpHelper;
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
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITrigger _trigger;
        private readonly ILogTable _log;
        public TimerJob(IScheduler scheduler, IHttpClientFactory httpClientFactory, ITrigger trigger, ILogTable logTable) { 
            _scheduler = scheduler;
            _httpClient = httpClientFactory.CreateClient("BackendApi");
            _httpClientFactory = httpClientFactory;
            _trigger = trigger;
            _log = logTable;
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
            await AddLog($"Current Job Run Time: {curExecutionDate}");
            var nextRunSchedule = curExecutionDate.AddMilliseconds(_interval);
            await AddLog($"Next Job Run Time: {nextRunSchedule}");
            var user = new LoginModel { UserName = "Scheduler Admin" };
            foreach (var details in schedulerList)
            {
                var schedule = CrontabSchedule.TryParse(details.CronExpression).GetNextOccurrence(curExecutionDate);
                if (schedule >= curExecutionDate && schedule <= nextRunSchedule)
                {
                    await AddLog($"Call Job API of CLientId: {details.ClientId}");
                    if (details.CronExpression != null)
                    {
                        _httpClient.DefaultRequestHeaders.Add("ClientID", details.ClientId);
                        if (details.CodeMapping.ToLower() == "code generation")
                        {
                            CgTriggerRunModel models = new CgTriggerRunModel();
                            models.Segment = details.Segment;
                            models.Threshold = details.Threshold;
                            string url = "code-generation/triggered-run";
                            var requestModel = await _trigger.CgApiRequestGet(models, user, details.ClientId);
                            var apiResponse = await HttpHelper.Post_HttpClient(_httpClientFactory, requestModel.Item1, url);
                            var savedData = await _trigger.CgAPiResponseSave(apiResponse, requestModel.Item2, user);
                            if (!apiResponse.IsSuccessStatusCode)
                            {
                                await AddLog($"Unable to schedule triggeredrun for clientId - {details}");
                            }
                        }
                        else if (details.CodeMapping.ToLower() == "weekly embedding")
                        {
                            WeeklyEmbedTriggeredRunModel models = new WeeklyEmbedTriggeredRunModel();
                            models.Segment = details.Segment;
                            string url = "weekly-embeddings/triggered-run";
                            var requestModel = await _trigger.WeeklyEmbedApiRequestGet(models, user, details.ClientId);
                            var apiResponse = await HttpHelper.Post_HttpClient(_httpClientFactory, requestModel.Item1, url);
                            var SavedData = await _trigger.WeeklyEmbedApiResponseSave(apiResponse, requestModel.Item2, user);
                            if (!apiResponse.IsSuccessStatusCode)
                            {
                                await AddLog($"Unable to schedule weekly embedding for clientId - {details}");
                            }
                        }
                        else if (details.CodeMapping.ToLower() == "monthly embedding")
                        {
                            MonthlyEmbedTriggeredRunModel models = new MonthlyEmbedTriggeredRunModel();
                            models.Segment = details.Segment;
                            string url = "monthly-embeddings/triggered-run";
                            var requestModel = await _trigger.MonthlyEmbedApiRequestGet(models, user, details.ClientId);
                            var apiResponse = await HttpHelper.Post_HttpClient(_httpClientFactory, requestModel.Item1, url);
                            var SavedData = await _trigger.MonthlyEmbedApiResponseSave(apiResponse, requestModel.Item2, user);
                            if (!apiResponse.IsSuccessStatusCode)
                            {
                                await AddLog($"Unable to schedule monthly embedding for clientId - {details}");
                            }
                        }
                        else
                        {
                            await AddLog($"Cannot process with empty cronexpression {details}");
                        }
                    }
                }
                else
                {
                    await AddLog($"Next schedule for clientId - {details.ClientId} is at - {schedule}");
                }
            }
            await AddLog("Job triggred every minute");
            //Do the stuff you want to be done every hour;
        }
        private async Task AddLog(string message)
        {
            //await _log.SaveLog(new LogTableDto { LogName = "TimerJob", LogDescription = message, CreatedBy = "Scheduler Admin", CreatedTime = DateTime.Now });
            Console.WriteLine(message);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace SchedulerProcessor
{
    public class SchedulerProcessor
    {
        public IConfiguration _configuration;
        public HttpClient _httpClient;
        public SchedulerProcessor(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = httpClientFactory.CreateClient();
        }
        [FunctionName("Processor")]
        public async Task SchedulerTimeRun([TimerTrigger("*/1 * * * *",
           #if DEBUG
            RunOnStartup =true
           #endif
            )]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            string url = "Scheduler/GetSchedulerRecords";
            var address = _configuration["ApiUrl"];
            var response = await _httpClient.GetAsync("http://localhost:5000/api/" + url);
            //var apiResponse = HttpHelper.Post_HttpClient(_htttpClient, requestModel.Item1, url);
            if (response.IsSuccessStatusCode)
                log.LogInformation("True");
            else
                log.LogError($"{response.StatusCode} {response.ReasonPhrase}: ");
            //var urlAddress = _configuration["ApiUrl"] + url;
            //var requestContent = JsonConvert.SerializeObject(requestModel);
            //var content = new StringContent(requestContent, System.Text.Encoding.UTF8, "application/json");
            ////var result = await _htttpClient.(url, content);
            //return new OkObjectResult($"true");
        }
    }
}

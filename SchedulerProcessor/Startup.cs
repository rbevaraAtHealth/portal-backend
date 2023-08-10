using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http.Headers;

[assembly: FunctionsStartup(typeof(SchedulerProcessor.Startup))]

namespace SchedulerProcessor
{
    public class Startup : FunctionsStartup
    {
        //public IConfiguration Configuration { get; }

        //public Startup(IConfiguration configuration)
        //{
        //    Configuration = configuration;
        //}

        public override void Configure(IFunctionsHostBuilder builder)
        {
            var configuration = new ConfigurationBuilder()
               .SetBasePath(Environment.CurrentDirectory)
               .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
               .AddEnvironmentVariables()
               .Build();
            
            builder.Services.AddHttpClient("AzureFunction", c => {
                c.BaseAddress = new Uri(configuration["DotNetApi:ApiUrl"]);
                c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            builder.Services.AddHttpClient();


        }
    }
}

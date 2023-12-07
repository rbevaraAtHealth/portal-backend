using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using AutoMapper;
using CodeMatcher.Api.V2;
using CodeMatcher.Api.V2.BusinessLayer;
using CodeMatcher.Api.V2.BusinessLayer.Interfaces;
using CodeMatcher.Api.V2.Middlewares.CommonHelper;
using CodeMatcher.Api.V2.Models;
using CodeMatcherApiV2.BusinessLayer.Interfaces;
using CodeMatcherApiV2.Repositories;
using CodeMatcherV2Api.BusinessLayer;
using CodeMatcherV2Api.BusinessLayer.Interfaces;
using CodeMatcherV2Api.EntityFrameworkCore;
using CodeMatcherV2Api.Middlewares.SqlHelper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace CodeMatcherV2Api
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{(!string.IsNullOrEmpty(env.EnvironmentName) ? env.EnvironmentName : "Development") }.json", optional: true)
            .AddEnvironmentVariables();
            this.Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
            services.AddDbContext<CodeMatcherDbContext>(options =>
                                         options.UseSqlServer(CommonHelper.Decrypt(Configuration.GetConnectionString("DBConnection")), sqlServerOptionsAction: sqlOptions =>
                                         {
                                             sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(10), errorNumbersToAdd: null);
                                         }));
            //services.AddDbContext<CodeMatcherDbContext>(options =>
            //                             options.UseSqlServer(Configuration.GetConnectionString("DBConnection"), sqlServerOptionsAction: sqlOptions =>
            //                             {
            //                                 sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(10), errorNumbersToAdd: null);
            //                             }));
            services.AddControllers();

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    ValidAudience = Configuration["JWT:ValidAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                };
            });
            IMapper mapper = MapperConfig.RegisterMaps().CreateMapper();
            services.AddSingleton(mapper);
            services.AddControllers();
            services.AddScoped<IUser, User>();
            services.AddScoped<ITrigger, Trigger>();
           // services.AddTransient<ISchedule, Schedule>();
            services.AddScoped<ILookUp, LookUp>();
            services.AddScoped<ICsvUpload, CsvUpload>();
            services.AddScoped<ICodeMapping, CodeMapping>();
            services.AddScoped<ILookupTypes, LookupTypes>();
            services.AddScoped<IScheduler, Scheduler>();
            services.AddScoped<ICodeGenerationOverwrite, CodeGenerationOverwrite>();
            services.AddScoped<SqlHelper>();
            services.AddScoped<ConvertTimeZoneHelper>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IApiKey, ApiKey>();

            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<ILogTable, LogTable>();

            //services.AddHttpClient();
            //services.AddHttpClient("CodeMatcher", c =>
            //{
            //    c.BaseAddress = new Uri("http://codeconv-app02.azurewebsites.net/");
            //    c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //});
            services.AddHttpClient("CodeMatcher", c => { c.BaseAddress = new Uri(Configuration["PythonApi:BaseUrl"]);
                c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });

            services.AddHttpClient("BackendApi", c => {
                c.BaseAddress = new Uri(Configuration["BackendApi:BaseUrl"]);
                c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });


            services
                .AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("1.0.0", new OpenApiInfo
                    {
                        Version = "1.0.0",
                        Title = "Code Matcher V2 API",
                        Description = "Code Matcher V2 API",
                    });
                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Please insert token appended with 'Bearer' into field",
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer"
                    });
                    c.OperationFilter<CustomHeaderSwagger>();
                    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }});
                    c.CustomSchemaIds(type => type.FullName);

                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, CodeMatcherDbContext dataContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.UseHttpsRedirection();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("AllowOrigin");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwagger();
           
            try
            {
                var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
                var schedulerObj = scope.ServiceProvider.GetService<IScheduler>();
                var httpClientObj = scope.ServiceProvider.GetService<IHttpClientFactory>();
                var triggerObj = scope.ServiceProvider.GetService<ITrigger>();
                var logObj = scope.ServiceProvider.GetService<ILogTable>();
                CommonHelper.InsertToLogTable(Configuration, "App_StartUp", "Starting scheduler timer job");
                TimerJob t = new(schedulerObj, httpClientObj, triggerObj, logObj);
                t.InvokeTimerJob(Configuration, Convert.ToDouble(Configuration["TimerJob:Frequency"]));
                dataContext.Database.Migrate();
            }
            catch(Exception ex)
            {
                CommonHelper.InsertToLogTable(Configuration, "App_StartUp", ex.Message);
            }
            app.UseSwaggerUI(c =>
            {
                //TODO: Either use the SwaggerGen generated Swagger contract (generated from C# classes)
                c.SwaggerEndpoint("/swagger/1.0.0/swagger.json", "Code Matcher API");
                //TODO: Or alternatively use the original Swagger contract that's included in the static files
                // c.SwaggerEndpoint("/swagger-original.json", "Your API Original");
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

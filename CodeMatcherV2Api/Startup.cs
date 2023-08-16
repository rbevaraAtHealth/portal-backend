using System;
using System.Net.Http.Headers;
using System.Text;
using AutoMapper;
using CodeMatcher.Api.V2;
using CodeMatcher.Api.V2.BusinessLayer;
using CodeMatcher.Api.V2.BusinessLayer.Interfaces;
using CodeMatcher.Api.V2.Middlewares.CommonHelper;
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
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
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
                                         options.UseSqlServer(Configuration.GetConnectionString("DBConnection")));
                                         //options.UseSqlServer(CommonHelper.Decrypt(Configuration.GetConnectionString("DBConnection"))));
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
            services.AddTransient<IUser, User>();
            services.AddTransient<ITrigger, Trigger>();
           // services.AddTransient<ISchedule, Schedule>();
            services.AddTransient<ILookUp, LookUp>();
            services.AddTransient<ICsvUpload, CsvUpload>();
            services.AddTransient<ICodeMapping, CodeMapping>();
            services.AddTransient<ILookupTypes, LookupTypes>();
            services.AddTransient<IScheduler, Scheduler>();
            services.AddTransient<ICodeGenerationOverwrite, CodeGenerationOverwrite>();
            services.AddTransient<SqlHelper>();

            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //services.AddHttpClient();
            //services.AddHttpClient("CodeMatcher", c =>
            //{
            //    c.BaseAddress = new Uri("http://codeconv-app02.azurewebsites.net/");
            //    c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //});
            services.AddHttpClient("CodeMatcher", c => { c.BaseAddress = new Uri(Configuration["PythonApi:BaseUrl"]);
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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using luftborn.Ground;
using luftborn.Presistance;
using luftborn.Presistance.DependencyResolution;
using luftborn.Service.DependencyResolution;
using luftborn.Service.Features.Common.Interfaces;
using System;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace luftborn.Api
{
    /// <summary>
    /// Startup
    /// </summary>
    public class Startup
    {
        private readonly IWebHostEnvironment _env;
        //private static ConnectionMultiplexer _redisHangfire;
        /// <summary>
        /// Startup
        /// </summary>
        /// <param name="configurations"></param>
        /// <param name="env"></param>
        public Startup(IConfiguration configurations, IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder().SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configurations.ConfigurationManager = builder.Build();
            _env = env;
            //_redisHangfire = ConnectionMultiplexer.Connect(Configurations.HangfireConnectionString);
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        /// <summary>
        /// ConfigureServices
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            //services.AddLocalization(options => options.ResourcesPath = "Resources");
            //Define supported languages 
            var supportedCultures = new[]
            {
                new CultureInfo("en-US"),
                new CultureInfo("ar-EG"),
            };

            //services.AddMvcCore()
            //    .AddAuthorization()
            //    .AddJsonFormatters();

            //Register Cors Origin
            services.AddCors(options =>
            {
                // this defines a CORS policy called "default"
                options.AddPolicy("default", policy =>
                {
                    policy.WithOrigins($"{Configurations.FrontendAngularBaseUrl}",
                        "http://localhost:4200")
                        .AllowCredentials()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .SetPreflightMaxAge(TimeSpan.FromSeconds(1728000));

                });
            });

            //Register Localization
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });
            //// Add the processing server as IHostedService
            //services.AddHangfireServer(options =>
            //{
            //    options.SchedulePollingInterval = TimeSpan.FromSeconds(1);
            //});



            //Register Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "luftborn.Api",
                    Description = "Swagger document for luftborn Apis",
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                //c.IncludeXmlComments(string.Format(@"{0}\bin\luftborn.Api.xml", Directory.GetCurrentDirectory()));
                //c.DocumentFilter<AuthenticationTokenOperation>(););
            });

            // register redis cache
            services.AddStackExchangeRedisCache(options =>
            {
                //options.ConfigurationOptions = new ConfigurationOptions()
                //{
                //    EndPoints = { Configurations.RedisCacheConnectionString },
                //    ChannelPrefix = $"RedisCache-{ _env.EnvironmentName }",

                //};
                options.Configuration = Configurations.RedisCacheConnectionString;
            });



            //Register https redirection
            if (!_env.IsDevelopment())
            {
                //services.AddHttpsRedirection(options =>
                //{
                //    options.HttpsPort = 443;
                //});
            }

            // profiling
            services.AddMiniProfiler(options =>
               options.RouteBasePath = "/profiler"
            );
            services.AddHealthChecks();
            ConfigureIoC(services);
        }
        /// <summary>
        /// ConfigureIoC
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureIoC(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddDbContext<IluftbornEntities, luftbornEntities>(o => o.UseSqlServer(Configurations.luftbornConnectionString));
            //register AuditDbContext
            services.AddDbContext<IluftbornAuditDbContext, luftbornAuditDbContext>(o => o.UseSqlServer(Configurations.luftbornAuditConnectionString));


            services.AddCoreService();
            services.AddInfrastructure();







        }
        /// <summary>
        /// Configure
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="errorLogger"></param>
        /// <param name="context"></param>
        /// <param name="schedular"></param>
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            IluftbornEntities context)
        {
            //prevent Iframe, clickjacking attempts
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Frame-Options", "DENY");
                await next();
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }
            else
            {
                app.UseHsts();
                var forwardOptions = new ForwardedHeadersOptions
                {
                    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
                    RequireHeaderSymmetry = false
                };

                forwardOptions.KnownNetworks.Clear();
                forwardOptions.KnownProxies.Clear();

                app.UseForwardedHeaders(forwardOptions);
            }
            if (env.IsDevelopment() || env.IsEnvironment("DockerQA"))
            {
                // profiling, url to see last profile check: http://localhost:xxxxx/profiler/results
                app.UseMiniProfiler();
                //app.UseStaticFiles(new StaticFileOptions
                //{
                //    FileProvider = new PhysicalFileProvider(
                //        Path.Combine(Directory.GetCurrentDirectory(), "Logs")),
                //    RequestPath = "/Logs"
                //});

                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "luftborn Documentation");
                    c.OAuthClientId("swaggerui");
                    c.OAuth2RedirectUrl($"{Configurations.ApiBaseUrl}/swagger/oauth2-redirect.html");
                    c.EnableValidator(null);
                    // this custom html has miniprofiler integration
                    c.IndexStream = () => GetType().GetTypeInfo().Assembly.GetManifestResourceStream("luftborn.Api.SwaggerIndex.html");
                });

            }
            app.UseHttpsRedirection();
            app.UseRequestLocalization();
            app.UseCors("default");
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

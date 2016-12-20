using System;
using System.IO;
using MemberTrack.Common;
using MemberTrack.Common.Contracts;
using MemberTrack.Data;
using MemberTrack.Services;
using MemberTrack.Services.Contracts;
using MemberTrack.WebApi.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Serilog;

namespace MemberTrack.WebApi
{
    public class Startup
    {
        private const string PolicyName = "CorsPolicy";

        public Startup(IHostingEnvironment env)
        {
            var builder =
                new ConfigurationBuilder().SetBasePath(env.ContentRootPath).AddJsonFile("appsettings.json", false, true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true).AddEnvironmentVariables();

            Configuration = builder.Build();

            Log.Logger =
                new LoggerConfiguration().MinimumLevel.Warning().WriteTo.RollingFile(
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs/WebApi-{Date}.txt")).CreateLogger();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(
                opt =>
                {
                    opt.AddPolicy(PolicyName,
                        builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials());
                });

            services.AddMvc().AddJsonOptions(
                opt => { opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver(); });

            services.AddDbContext<DatabaseContext>(opt => opt.UseSqlServer(Configuration["ConnectionString"]));

            services.AddSingleton<IHashProvider, HashProvider>();

            services.AddTransient<IUserService, UserService>();

            services.AddTransient<IPersonService, PersonService>();

            services.AddTransient<IAddressService, AddressService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            var iisAppMapping = Configuration["IisAppMapping"];

            if (string.IsNullOrEmpty(iisAppMapping))
            {
                OnConfigure(app, env, loggerFactory);

                return;
            }

            app.Map(iisAppMapping, builder => OnConfigure(builder, env, loggerFactory));
        }

        private void OnConfigure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddSerilog();

            if (env.IsDevelopment())
            {
                app.UseDatabaseErrorPage();
            }

            app.UseCors(PolicyName);
            app.UseCustomExceptionHandler(loggerFactory);
            app.UseTokenProvider(Configuration["Audience"]);
            app.UseMvc();
        }
    }
}
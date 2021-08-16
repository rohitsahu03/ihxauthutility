using System;
using AuthUtility.Common;
using AuthUtility.Interfaces;
using AuthUtility.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AuthUtility.DBRepository;
using AuthUtility.Provider;
using LinqToDB.Data;
using AuthUtility.MediBuddyDBFactory;

namespace AuthUtility
{
    public class Startup
    {
        public Startup(IHostingEnvironment env, IOptions<MemoryCacheOptions> optionsAccessor)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
            OptionsAccessor = optionsAccessor;
        }
        private readonly IOptions<MemoryCacheOptions> OptionsAccessor;
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            DataConnection.DefaultSettings = new DBSettings(Configuration);
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.Lax;
            });
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(40);
            });
            services.AddHttpContextAccessor();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddMemoryCache();
            services.AddHttpClient();
            AddServiceToCollection(services);
            services.AddLogging(
                builder =>
                {
                    builder.ClearProviders();
                    builder.AddFilter("Microsoft", LogLevel.Warning)
                           .AddFilter("System", LogLevel.Warning)
                           .AddConsole();
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            CacheBuilder.Init(app.ApplicationServices);
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Login}/{action=Authorize}/{id?}");
            });
            //loggerFactory.AddLog4Net(Configuration["Log4NetConfigFile:Name"]);
        }

        private void AddServiceToCollection(IServiceCollection services)
        {
            services.AddSingleton(new ConfigManager(Configuration));
            services.AddSingleton<IRestClient, RestClient>();
            services.AddSingleton<ISyncHelper, SyncHelper>();
            services.AddSingleton<IUtilityRepo, UtilityRepo>();
            services.AddSingleton<IDBRepo, DBRepo>();
            services.AddSingleton<IUtilityProvider, UtilityProvider>();
            services.AddSingleton<IDBProvider, DBProvider>();
            services.AddSingleton<ICachingHelper, CachingHelper>();
            services.AddSingleton<ICacheProvider, CacheProvider>();
            services.AddSingleton<IBulkUpdateProvider, BulkUpdateProvider>();
        }
    }
}
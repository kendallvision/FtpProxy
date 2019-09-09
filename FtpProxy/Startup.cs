namespace FtpProxy
{
    using FtpProxy.DataObjects;
    using FtpProxy.Interfaces;
    using FtpProxy.Services;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using System;

    public class Startup
    {
        private readonly ILogger<Startup> Logger;

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            Logger = logger;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            try
            {
                services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

                services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

                services.AddScoped<IFileSender, FileSender>();
                services.AddScoped<IFileWatcher, FileWatcher>();
                services.AddScoped<IFileGetter, FileGetter>();
                services.AddScoped<INotifier, Notifier>();

                services.AddHostedService<FileWatcherService>();
            }
            catch (Exception except)
            {
                Logger.LogError(except, "Error during Startup.ConfigureServices");
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            try
            {
                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }
                else
                {
                    app.UseHsts();
                }

                app.UseMvc();
            }
            catch( Exception except )
            {
                Logger.LogError(except, "Error during Startup.Configure");
            }
        }
    }
}

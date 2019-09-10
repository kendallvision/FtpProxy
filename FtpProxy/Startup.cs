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
        private readonly ILogger<Startup> _logger;
        private readonly IConfiguration _config;

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            _config = configuration;
            _logger = logger;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            try
            {
                services.AddMvc()
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                    .AddXmlSerializerFormatters();

                services.Configure<AppSettings>(_config.GetSection("AppSettings"));

                services.AddScoped<IFileSender, FileSender>();
                services.AddScoped<IFileWatcher, FileWatcher>();
                services.AddScoped<IFileGetter, FileGetter>();
                services.AddScoped<INotifier, Notifier>();

                services.AddHostedService<FileWatcherService>();
            }
            catch (Exception except)
            {
                _logger.LogError(except, "Error during Startup.ConfigureServices");
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
                _logger.LogError(except, "Error during Startup.Configure");
            }
        }
    }
}

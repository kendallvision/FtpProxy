namespace FtpProxy.Services
{
    using FtpProxy.DataObjects;
    using FtpProxy.Interfaces;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class FileWatcherService : IHostedService
    {
        private readonly AppSettings _settings;
        private readonly IServiceProvider _services;
        private readonly ILogger<FileWatcherService> _logger;

        private static Timer _timer;
        private static object _locker = new object();

        public FileWatcherService(IOptions<AppSettings> settings, 
            IServiceProvider services, 
            ILogger<FileWatcherService> logger)
        {
            _settings = settings.Value;
            _services = services;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var timeout = _settings.FrequencyToCheckFilesInSeconds;

            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(timeout));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            lock (_locker)
            {
                try
                {
                    using (var scope = _services.CreateScope())
                    {
                        var scopedService = scope.ServiceProvider.GetRequiredService<IFileWatcher>();
                        scopedService.CheckForReadyFiles();
                    }
                }
                catch (Exception except)
                {
                    _logger.LogError(except, "Error running file watcher service task.");
                }
            }
        }
    }
}

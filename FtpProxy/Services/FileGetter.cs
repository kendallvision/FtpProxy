namespace FtpProxy.Services
{
    using FtpProxy.DataObjects;
    using FtpProxy.Extensions;
    using FtpProxy.Interfaces;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using System.IO;
    using System.Net;

    public class FileGetter : IFileGetter
    {
        private readonly AppSettings _settings;
        private readonly ILogger<FileGetter> _logger;

        public FileGetter(IOptions<AppSettings> settings, ILogger<FileGetter> logger)
        {
            _settings = settings.Value;
            _logger = logger;
        }

        public bool CopyFileLocal(string filename)
        {
            filename.CheckNullOrEmpty(nameof(filename));

            CheckDestinationDirectory();

            var destination = _settings.LocalDestination;
            var ftpServer = GetServerAddress();
            var user = _settings.FtpUser;
            var password = _settings.FtpPassword;
            var sourceFile = $"{ftpServer}/{_settings.PickupFolder}/{Path.GetFileName(filename)}";
            var localFileName = Path.Combine(destination, Path.GetFileName(filename));

            if (File.Exists(localFileName))
            {
                _logger.LogInformation($"Skipping file because it already exists locally: {localFileName}");
                return false;
            }

            using (var client = new WebClient())
            {
                client.Credentials = new NetworkCredential(user, password);
                client.BaseAddress = string.Empty;
                client.DownloadFile(sourceFile, localFileName);

                if (File.Exists(localFileName) == false)
                {
                    throw new BusinessException($"File does not exist after download ({localFileName})");
                }

                return true;
            }
        }

        private string GetServerAddress()
        {
            var ftpServer = this._settings.FtpServerAddress;
            return FilePathHelper.StripTrailingSlash(ftpServer);
        }

        private void CheckDestinationDirectory()
        {
            var destination = _settings.LocalDestination;

            if ( Directory.Exists(destination) == false )
            {
                throw new BusinessException($"Destination directory not found {destination}");
            }
        }
    }
}

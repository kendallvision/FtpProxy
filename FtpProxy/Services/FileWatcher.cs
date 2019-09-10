namespace FtpProxy.Services
{
    using FtpProxy.DataObjects;
    using FtpProxy.Extensions;
    using FtpProxy.Interfaces;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using System;
    using System.IO;
    using System.Net;

    public class FileWatcher : IFileWatcher
    {
        private readonly AppSettings _settings;
        private readonly ILogger<FileWatcher> _logger;
        private readonly IFileGetter _fileGetter;
        private readonly INotifier _notifier;

        public FileWatcher(
            IOptions<AppSettings> settings,
            ILogger<FileWatcher> logger,
            INotifier notifier,
            IFileGetter getter)
        {
            _settings = settings.Value;
            _logger = logger;
            _fileGetter = getter;
            _notifier = notifier;
        }

        public void CheckForReadyFiles()
        {
            try
            {
                var ftpServer = GetServerAddress();
                var user = _settings.FtpUser;
                var password = _settings.FtpPassword;
                var pickupFolder = _settings.PickupFolder;

                var addressToCheck = $"{ftpServer}/{pickupFolder}";

                var request = WebRequest.Create(addressToCheck);
                request.Method = WebRequestMethods.Ftp.ListDirectory;

                request.Credentials = new NetworkCredential(user, password);

                var response = (FtpWebResponse)request.GetResponse();

                var responseStream = response.GetResponseStream();
                var reader = new StreamReader(responseStream);

                var test = reader.ReadToEnd();

                var list = test.Split('\n');

                foreach (var item in list)
                {
                    var file = item.Replace("\r", "");

                    if (IsValidFile(file))
                    {
                        _logger.LogDebug($"Timed Background working on {file}");

                        if ( _fileGetter.CopyFileLocal(file) )
                        {
                            var notifyResponse = _notifier.SendNotification(file).Result;
                            _logger.LogDebug($"Notify: {notifyResponse}");
                        }
                    }
                }

                reader.Close();
                response.Close();
            }
            catch (Exception except)
            {
                _logger.LogError(except, "Error checking for files for pickup.");
            }
        }

        private string GetServerAddress()
        {
            var ftpServer = _settings.FtpServerAddress;
            return FilePathHelper.StripTrailingSlash(ftpServer);
        }

        private bool IsValidFile(string name)
        {
            return string.IsNullOrEmpty(name) == false
                && name != "."
                && name != "..";
        }
    }
}

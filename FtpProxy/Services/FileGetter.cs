namespace FtpProxy.Services
{
    using FtpProxy.DataObjects;
    using FtpProxy.Extensions;
    using FtpProxy.Interfaces;
    using Microsoft.Extensions.Options;
    using System.IO;
    using System.Net;

    public class FileGetter : IFileGetter
    {
        private readonly AppSettings AppSettings;

        public FileGetter(IOptions<AppSettings> settings)
        {
            AppSettings = settings.Value;
        }

        public void CopyFileLocal(string filename)
        {
            filename.CheckNullOrEmpty(nameof(filename));

            CheckDestinationDirectory();

            var destination = AppSettings.LocalDestination;

            var ftpServer = GetServerAddress();
            var user = AppSettings.FtpUser;
            var password = AppSettings.FtpPassword;

            var sourceFile = Path.Combine(ftpServer, AppSettings.PickupFolder, filename);

            var localFileName = Path.Combine(destination, Path.GetFileName(filename));

            using (var client = new WebClient())
            {
                client.Credentials = new NetworkCredential(user, password);
                client.DownloadFile(sourceFile, localFileName);
            }
        }

        private string GetServerAddress()
        {
            var ftpServer = this.AppSettings.FtpServerAddress;
            return FilePathHelper.StripTrailingSlash(ftpServer);
        }

        private void CheckDestinationDirectory()
        {
            var destination = AppSettings.LocalDestination;

            if ( Directory.Exists(destination) == false )
            {
                throw new BusinessException($"Destination directory not found {destination}");
            }
        }
    }
}

namespace FtpProxy.Services
{
    using FtpProxy.DataObjects;
    using FtpProxy.Extensions;
    using FtpProxy.Helpers;
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

            var destination = AppSettings.LocalDestination;

            Directory.CreateDirectory(destination);

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
    }
}

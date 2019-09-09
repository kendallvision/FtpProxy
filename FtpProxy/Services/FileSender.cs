namespace FtpProxy.Services
{
    using FtpProxy.DataObjects;
    using FtpProxy.Extensions;
    using FtpProxy.Helpers;
    using FtpProxy.Interfaces;
    using Microsoft.Extensions.Options;
    using System;
    using System.IO;
    using System.Net;

    public class FileSender : IFileSender
    {
        private readonly AppSettings AppSettings;

        public FileSender(IOptions<AppSettings> settings)
        {
            this.AppSettings = settings.Value;
        }

        public void SendFile(string sourceFile)
        {
            sourceFile.CheckNullOrEmpty(nameof(sourceFile));

            var fileName = Path.GetFileName(sourceFile);

            var ftpServer = this.GetServerAddress();

            fileName = string.IsNullOrEmpty(AppSettings.DropFolder) ? fileName : $"{this.AppSettings.DropFolder}/{fileName}";

            var destination = ftpServer + "/" + fileName + " " + DateTime.Now.Ticks;

            var ftpUsername = this.AppSettings.FtpUser;
            var ftpPassword = this.AppSettings.FtpPassword;

            this.CreateDirectory(ftpServer, AppSettings.DropFolder, ftpUsername, ftpPassword);

            using (var client = new WebClient())
            {
                client.Credentials = new NetworkCredential(ftpUsername, ftpPassword);
                client.UploadFile(destination, WebRequestMethods.Ftp.UploadFile, sourceFile);
            }
        }

        private string GetServerAddress()
        {
            var ftpServer = this.AppSettings.FtpServerAddress;
            return FilePathHelper.StripTrailingSlash(ftpServer);
        }

        private void CreateDirectory(string server, string folder, string user, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(folder) == false)
                {
                    var request = WebRequest.Create($"{ server}/{folder}");
                    request.Method = WebRequestMethods.Ftp.MakeDirectory;
                    request.Credentials = new NetworkCredential(user, password);
                    request.GetResponse();
                }
            }
            catch
            {
                // Do nothing - just attempt it
            }
        }
    }
}

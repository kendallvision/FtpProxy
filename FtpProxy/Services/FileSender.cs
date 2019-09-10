namespace FtpProxy.Services
{
    using FtpProxy.DataObjects;
    using FtpProxy.Extensions;
    using FtpProxy.Interfaces;
    using Microsoft.Extensions.Options;
    using System.IO;
    using System.Net;

    public class FileSender : IFileSender
    {
        private readonly AppSettings _settings;

        public FileSender(IOptions<AppSettings> settings)
        {
            this._settings = settings.Value;
        }

        public void SendFile(string sourceFile)
        {
            sourceFile.CheckNullOrEmpty(nameof(sourceFile));

            var fileName = Path.GetFileName(sourceFile);
            fileName = string.IsNullOrEmpty(_settings.DropFolder) ? fileName : $"{this._settings.DropFolder}/{fileName}";

            var ftpServer = this.GetServerAddress();
            var destination = ftpServer + "/" + fileName;
            var ftpUsername = this._settings.FtpUser;
            var ftpPassword = this._settings.FtpPassword;

            this.CreateDirectory(ftpServer, _settings.DropFolder, ftpUsername, ftpPassword);

            using (var client = new WebClient())
            {
                client.Credentials = new NetworkCredential(ftpUsername, ftpPassword);
                client.UploadFile(destination, WebRequestMethods.Ftp.UploadFile, sourceFile);
            }
        }

        private string GetServerAddress()
        {
            var ftpServer = this._settings.FtpServerAddress;
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

namespace FtpProxy.Services
{
    using FtpProxy.DataObjects;
    using FtpProxy.Helpers;
    using FtpProxy.Interfaces;
    using Microsoft.Extensions.Options;
    using System;
    using System.IO;
    using System.Net;

    public class FileWatcher : IFileWatcher
    {
        private readonly AppSettings _settings;

        public FileWatcher(IOptions<AppSettings> settings)
        {
            _settings = settings.Value;
        }

        public void CheckForReadyFiles()
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

            foreach( var item in list )
            {
                if ( IsValidFile(item) )
                {
                    // TODO
                    // Logging
                    // Accept filename in body of receiving 
                    // Copy files local and delete when confirmed present

                    Console.Write("Timed Background working...");
                }
            }
            
            reader.Close();
            response.Close();
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

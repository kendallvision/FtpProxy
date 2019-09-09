namespace FtpProxy.Services
{
    using FtpProxy.DataObjects;
    using FtpProxy.Interfaces;
    using Microsoft.Extensions.Options;

    public class FileDeleter : IFileDeleter
    {
        private readonly AppSettings Settings;

        public FileDeleter(IOptions<AppSettings> settings)
        {
            Settings = settings.Value;
        }

        public void DeleteFromFTP(string filename)
        {
            throw new System.NotImplementedException();
        }
    }
}

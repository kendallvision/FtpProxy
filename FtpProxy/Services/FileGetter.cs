namespace FtpProxy.Services
{
    using FtpProxy.DataObjects;
    using FtpProxy.Interfaces;
    using Microsoft.Extensions.Options;
    using System;

    public class FileGetter : IFileGetter
    {
        private readonly IOptions<AppSettings> Settings;

        public FileGetter(IOptions<AppSettings> settings)
        {
            Settings = settings;
        }

        public void CopyFileLocal(string filename)
        {
            
        }
    }
}

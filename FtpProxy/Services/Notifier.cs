﻿namespace FtpProxy.Services
{
    using FtpProxy.DataObjects;
    using FtpProxy.Interfaces;
    using Microsoft.Extensions.Options;

    public class Notifier : INotifier
    {
        private readonly AppSettings Settings;

        public Notifier(IOptions<AppSettings> settings)
        {
            Settings = settings.Value;
        }

        public void SendNotification(string filename)
        {
            throw new System.NotImplementedException();
        }
    }
}

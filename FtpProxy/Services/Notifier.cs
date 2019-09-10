namespace FtpProxy.Services
{
    using FtpProxy.DataObjects;
    using FtpProxy.Extensions;
    using FtpProxy.Interfaces;
    using Microsoft.Extensions.Options;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class Notifier : INotifier
    {
        private readonly AppSettings _settings;

        public Notifier(IOptions<AppSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task<string> SendNotification(string filename)
        {
            filename.CheckNull(nameof(filename));

            var address = _settings.NotificationAddress + $"?filename={filename}";

            var request = new HttpRequestMessage(HttpMethod.Post, address);

            var response = await HttpHelper.HttpClient.SendAsync(request);

            return await response.Content.ReadAsStringAsync();
        }
    }
}

namespace FtpProxy.Interfaces
{
    using System.Threading.Tasks;

    public interface INotifier
    {
        Task<string> SendNotification(string filename);
    }
}

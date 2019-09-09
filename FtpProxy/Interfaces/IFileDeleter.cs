namespace FtpProxy.Interfaces
{
    public interface IFileDeleter
    {
        void DeleteFromFTP(string filename);
    }
}

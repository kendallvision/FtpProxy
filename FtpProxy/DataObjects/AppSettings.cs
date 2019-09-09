namespace FtpProxy.DataObjects
{
    public class AppSettings
    {
        public string DropFolder { get; set; }

        public int FrequencyToCheckFilesInSeconds { get; set; }

        public string PickupFolder { get; set; }

        public string FtpServerAddress { get; set; }

        public string FtpUser { get; set; }

        public string FtpPassword { get; set; }

        public string LocalDestination { get; set; }
    }
}

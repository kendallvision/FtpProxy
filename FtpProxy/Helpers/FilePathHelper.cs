namespace FtpProxy.Helpers
{
    public class FilePathHelper
    {
        public static string StripTrailingSlash(string input)
        {
            if (input[input.Length - 1] == '/')
            {
                input = input.Substring(0, input.Length - 1);
            }

            return input;
        }
    }
}

namespace FtpProxy.Extensions
{
    using System;

    public static class General
    {
        public static void CheckNull(this object input, string name)
        {
            if ( input  == null )
            {
                throw new ArgumentNullException(name);
            }
        }

        public static void CheckNullOrEmpty(this string input, string name)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentException($"Argument cannot be null or empty ({name})");
            }
        }
    }
}

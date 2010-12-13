using System;

namespace CopyFileContentToClipboard
{
    internal class Utils
    {
        public const long Kilo = 1000;
        public const long Mega = 1000 * 1000;

        public static string PrettyPrint(long byteCount)
        {
            return byteCount > Kilo
                       ? (
                             byteCount > Mega
                                 ? String.Format("{0}MB", (byteCount / (double) Mega).ToString("N"))
                                 : String.Format("{0}kB", Math.Ceiling(byteCount / (double) Kilo))
                         )
                       : String.Format("{0} bytes", byteCount.ToString("N"));
        }
    }
}
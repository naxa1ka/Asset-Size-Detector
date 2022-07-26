using System.IO;

namespace AssetSizeDetector
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmptyOrWhitespace(this string str)
        {
            return string.IsNullOrEmpty(str) || str.Trim().Equals(string.Empty); 
        }

        public static string MakeOneFormatDirectorySeparators(this string str)
        {
            str = str.Replace("/", "\\");
            str = str.Replace("\\", Path.DirectorySeparatorChar.ToString());
            return str;
        }
    }
}
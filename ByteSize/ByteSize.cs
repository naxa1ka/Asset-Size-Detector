using System;
using System.Globalization;
using System.Text;

namespace AssetSizeDetector
{
    public struct ByteSize
    {
        public const int B = 1;
        public const int KB = 1024 * B;
        public const int MB = KB * KB;
        public const int GB = MB * KB;

        private readonly long _bytes;

        public ByteSize(long bytes)
        {
            _bytes = bytes;
        }

        public ByteSize(double bytes)
        {
            _bytes = (long) Math.Ceiling(bytes);
        }

        public long ToB() => ToB(_bytes);
        public long ToKB() => ToKB(_bytes);
        public long ToMB() => ToMB(_bytes);
        public long ToGB() => ToGB(_bytes);

        public static long ToB(long bytes) => bytes;
        public static long ToKB(long bytes) => bytes / KB;
        public static long ToMB(long bytes) => bytes / MB;
        public static long ToGB(long bytes) => bytes / GB;
        
        public static ByteSize Parse(string s)
        {
            return Parse(s, NumberFormatInfo.CurrentInfo);
        }
        
        public static ByteSize Parse(string s, IFormatProvider formatProvider)
        {
            return Parse(s, NumberStyles.Float | NumberStyles.AllowThousands, formatProvider);
        }
        
        public static ByteSize Parse(string s, NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            if (s.IsNullOrEmptyOrWhitespace())
                throw new ArgumentNullException("s", "String is null or whitespace or empty");

            s = s.TrimStart();
            
            var numberFormatInfo = NumberFormatInfo.GetInstance(formatProvider);
            var decimalSeparator = Convert.ToChar(numberFormatInfo.NumberDecimalSeparator);
            var groupSeparator = Convert.ToChar(numberFormatInfo.NumberGroupSeparator);

            s = s.Replace('.', decimalSeparator);
            s = s.Replace(',', decimalSeparator);

            int endIndexOfNumber;
            var isNumberFound = false;
            for (endIndexOfNumber = 0; endIndexOfNumber < s.Length; endIndexOfNumber++)
            {
                if (!(char.IsDigit(s[endIndexOfNumber]) || s[endIndexOfNumber] == decimalSeparator || s[endIndexOfNumber] == groupSeparator))
                {
                    isNumberFound = true;
                    break;
                }
            }

            if (isNumberFound == false)
                throw new FormatException($"No number indicator found in value '{s}'.");

            var numberPart = s.Substring(0, endIndexOfNumber).Trim();
            var sizePart = s.Substring(endIndexOfNumber, s.Length - endIndexOfNumber).Trim();

            if (!double.TryParse(numberPart, numberStyles, formatProvider, out var number))
                throw new FormatException($"No number found in value '{s}'.");

            return sizePart.ToLowerInvariant() switch
            {
                "kb" => FromKiloBytes(number),
                "mb" => FromMegaBytes(number),
                "gb" => FromGigaBytes(number),
                _ => throw new FormatException($"Bytes of magnitude '{sizePart}' is not supported.")
            };
        }

        private static ByteSize FromKiloBytes(double value) => new(value * KB);

        private static ByteSize FromMegaBytes(double value) => new(value * MB);

        private static ByteSize FromGigaBytes(double value) => new(value * GB);

        public override string ToString()
        {
            //TODO: make this optimal
            var stringBuilder = new StringBuilder();
            stringBuilder.Clear();

            var intB = ToB();
            var intKb = ToKB();
            var intMb = ToMB();
            var intGb = ToGB();
            
            if (intGb > 0)
            {
                stringBuilder.Append($"GB: {intGb}.{intMb:2}"); 
                return stringBuilder.ToString();
            }
            
            if (intMb > 0)
            {
                stringBuilder.Append($"MB: {intMb}.{intKb:2}"); 
                return stringBuilder.ToString();
            }

            if (intKb > 0)
            {
                stringBuilder.Append($"KB: {intKb}.{intB:2}");
                return stringBuilder.ToString();
            }

            stringBuilder.Append($"B: {intB}");
            return stringBuilder.ToString();
        }
    }
}
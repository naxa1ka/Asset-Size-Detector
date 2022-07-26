using System.Collections.Generic;

namespace AssetSizeDetector
{
    public static class MathExtensions
    {
        public static long ClampLong(long value, long min, long max)
        {
            if (value < min)
                value = min;
            else if (value > max)
                value = max;
            return value;
        }
        
        public static List<int> GetListWithPowersOfTwo(int borderFilling)
        {
            List<int> powersOfTwo = new List<int>();
            for (int i = 0; i <= borderFilling; i++)
            {
                if (((i - 1) & i) == 0 && i != 0)
                    powersOfTwo.Add(i);
            }

            return powersOfTwo;
        }
    }
}
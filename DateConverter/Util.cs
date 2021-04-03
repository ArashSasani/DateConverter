using System;
using System.Collections.Generic;
using System.Text;

namespace DateConverter
{
    public static class Util
    {
        public static string PadWithLeadingZeros(int minuteValue)
        {
            return minuteValue < 10 ? minuteValue.PadWithLeadingZeros(1) : minuteValue.ToString();
        }

        private static string PadWithLeadingZeros(this int value, int numberOfLeadingZeros)
        {
            int decimalLength = value.ToString("D").Length + numberOfLeadingZeros;
            return value.ToString("D" + decimalLength.ToString());
        }
    }
}

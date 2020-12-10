using System;

namespace Net.HungryBug.Core.Utility
{
    /// <summary>
    /// Utilities for conversions with Time
    /// </summary>
    public static class TimeConverter
    {
        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);

        /// <summary>
        /// Convert UNIX timestamp miliseconds to UTC datetime
        /// </summary>
        /// <param name="timestamp">UNIX Timestamp in miliseconds</param>
        /// <returns>Datetime in UTC timezone</returns>
        public static DateTime TimestampToDateTime(double timestamp)
        {
            return Epoch.AddMilliseconds(timestamp);
        }

        /// <summary>
        /// Convert datetime to UNIX timestamp miliseconds
        /// </summary>
        /// <param name="datetime">Datetime to convert</param>
        /// <returns>UNIX Timestamp in miliseconds</returns>
        public static double DateTimeToTimestamp(DateTime datetime)
        {
            return datetime.Subtract(Epoch).TotalMilliseconds;
        }
        public static string ConvertTimeSpanToString(TimeSpan span)
        {
            string formatted = string.Format("{0}{1}{2}{3}",
                span.Duration().Days > 0 ? $"{span.Days}d " : string.Empty,
                span.Duration().Hours > 0 ? $"{span.Hours}h " : string.Empty,
                (span.Duration().Minutes > 0 && span.Duration().Days == 0) ? $"{span.Minutes}m " : string.Empty,
                (span.Duration().Seconds > 0 && span.Duration().Days == 0 && span.Duration().Hours == 0) ? $"{span.Seconds}s " : string.Empty);
            if (formatted.EndsWith(", ")) formatted = formatted.Substring(0, formatted.Length - 2);
            return formatted;
        }
    }

    public static class NumberConverter
    {
        public static string ConvertValue(long inputvalue)
        {
            return string.Format("{0:n0}", inputvalue);
        }
        public static string ConvertValueToDotSeperate(float numberFormat)
        {
            if (numberFormat >= 1000000000)
                return (numberFormat / 1000000000).ToString("#.##") + "B";
            else if(numberFormat >= 1000000)
                return (numberFormat / 1000000).ToString("#.##") + "M";
            else if (numberFormat >= 1000)
                return (numberFormat / 1000).ToString("#.##") + "K";
            else
                return numberFormat.ToString();
        }
        public static string ConvertValueToCommaSeperate(float numberFormat)
        {
            if (numberFormat >= 1000000000)
                return (numberFormat / 1000000000).ToString("#.##") + "B";
            else if (numberFormat >= 1000000)
                return (numberFormat / 1000000).ToString("#.##") + "M";
            else if (numberFormat >= 1000)
                return (numberFormat / 1000).ToString("#.##") + "K";
            else
                return numberFormat.ToString();
        }
    }

    public static class CurrencyConverter
    {
        public static string ConvertValue(long inputvalue)
        {
            if (inputvalue < 1000000)
            {
                return string.Format("{0:n0}", inputvalue);
            }
            else
            {
                return string.Format("{0:n0}", inputvalue / 1000) + "k";
            }
        }

        public static string ConvertValue(this int inputvalue)
        {
            if (inputvalue < 1000000)
            {
                return string.Format("{0:n0}", inputvalue);
            }
            else
            {
                return string.Format("{0:n0}", inputvalue / 1000) + "k";
            }
        }

        public static string ConvertValue(this float inputvalue)
        {
            return ConvertValue((int)inputvalue);
        }

        public static string ConvertSmallValue(this int inputvalue)
        {
            return string.Format("{0:n0}", inputvalue / 1000) + "k";
        }



    }
}

using System;
using System.Globalization;
using static DateConverter.Util;

namespace DateConverter
{
    public static class Converter
    {
        public enum IncludeTime
        {
            No,
            JustHour,
            HourAndMinute,
            Complete,
        }

        //adjust HijriAdjustment based on the using year
        private static readonly HijriCalendar HC = new HijriCalendar { HijriAdjustment = -1 };
        private static readonly PersianCalendar PC = new PersianCalendar();
        private static readonly GregorianCalendar GC = new GregorianCalendar();

        /// <summary>
        /// Converts Gregorian date to Shamsi(Persian)
        /// </summary>
        public static string GregorianToPersian(this DateTime date, IncludeTime includeTime)
        {
            if (date < GC.MinSupportedDateTime)
            {
                throw new ArgumentOutOfRangeException("date", "input date time object is invalid" +
                        $", minimum gregorian calendar date is: '{date:D}'.");
            }
            if (date > GC.MaxSupportedDateTime)
            {
                throw new ArgumentOutOfRangeException("date", "input date time object is invalid" +
                    $", maximum gregorian calendar date is: '{date:D}'.");
            }

            string result = string.Format("{0}/{1}/{2}"
                , PC.GetYear(date), PC.GetMonth(date), PC.GetDayOfMonth(date));
            switch (includeTime)
            {
                case IncludeTime.No:
                    break;
                case IncludeTime.JustHour:
                    result = string.Concat(result, string.Format(" - {0}:00"
                        , PadWithLeadingZeros(PC.GetHour(date))));
                    break;
                case IncludeTime.HourAndMinute:
                    result = string.Concat(result, string.Format(" - {0}:{1}"
                        , PadWithLeadingZeros(PC.GetHour(date))
                        , PadWithLeadingZeros(PC.GetMinute(date))));
                    break;
                case IncludeTime.Complete:
                    result = string.Concat(result, string.Format(" - {0}:{1}:{2}"
                        , PadWithLeadingZeros(PC.GetHour(date))
                        , PadWithLeadingZeros(PC.GetMinute(date))
                        , PadWithLeadingZeros(PC.GetSecond(date))));
                    break;
                default:
                    break;
            }
            return result;
        }

        /// <summary>
        /// Converts Gregorian date to Hijri(Arabic)
        /// </summary>
        public static string GregorianToHijri(this DateTime date, IncludeTime includeTime)
        {
            if (date < GC.MinSupportedDateTime)
            {
                throw new ArgumentOutOfRangeException("date", "input date time object is invalid" +
                        $", minimum gregorian calendar date is: '{date:D}'.");
            }
            if (date > GC.MaxSupportedDateTime)
            {
                throw new ArgumentOutOfRangeException("date", "input date time object is invalid" +
                    $", maximum gregorian calendar date is: '{date:D}'.");
            }

            string result = string.Format("{0}/{1}/{2}"
                , HC.GetYear(date), HC.GetMonth(date), HC.GetDayOfMonth(date));

            switch (includeTime)
            {
                case IncludeTime.No:
                    break;
                case IncludeTime.JustHour:
                    result = string.Concat(result, string.Format(" - {0}:00"
                        , PadWithLeadingZeros(PC.GetHour(date))));
                    break;
                case IncludeTime.HourAndMinute:
                    result = string.Concat(result, string.Format(" - {0}:{1}"
                        , PadWithLeadingZeros(PC.GetHour(date))
                        , PadWithLeadingZeros(PC.GetMinute(date))));
                    break;
                case IncludeTime.Complete:
                    result = string.Concat(result, string.Format(" - {0}:{1}:{2}"
                        , PadWithLeadingZeros(PC.GetHour(date))
                        , PadWithLeadingZeros(PC.GetMinute(date))
                        , PadWithLeadingZeros(PC.GetSecond(date))));
                    break;
                default:
                    break;
            }

            return result;
        }

        /// <summary>
        /// Converts Persian(Shamsi) date to Gregorian
        /// </summary>
        /// <param name="date">for date param please use the corrent format: "year/month/day-hour:minutes:second"</param>
        public static DateTime PersianToGregorian(this string date, IncludeTime includeTime)
        {
            if (string.IsNullOrEmpty(date))
            {
                throw new ArgumentNullException("input persian date is null or empty!");
            }

            var dateAndTimeArr = date.Split('-');
            var persianDateArr = dateAndTimeArr[0].Split('/');

            var result = PC.ToDateTime(int.Parse(persianDateArr[0]), int.Parse(persianDateArr[1])
                , int.Parse(persianDateArr[2]), 0, 0, 0, 0);

            #region time
            int hour = DateTime.Now.Hour;
            int minute = DateTime.Now.Minute;
            int second = DateTime.Now.Second;

            if (dateAndTimeArr.Length > 1)
            {
                var timeArr = dateAndTimeArr[1];
                if (timeArr != string.Empty)
                {
                    var timeSplits = timeArr.Split(':');
                    hour = timeSplits.Length > 0 ? int.Parse(timeSplits[0]) : 0;
                    minute = timeSplits.Length > 1 ? int.Parse(timeSplits[1]) : 0;
                    second = timeSplits.Length > 2 ? int.Parse(timeSplits[2]) : 0;
                }
            }
            #endregion

            switch (includeTime)
            {
                case IncludeTime.No:
                    result = result.Date
                        .Add(TimeSpan.Zero);
                    break;
                case IncludeTime.JustHour:
                    result = result.Date
                        .Add(new TimeSpan(hour, 0, 0));
                    break;
                case IncludeTime.HourAndMinute:
                    result = result.Date
                        .Add(new TimeSpan(hour, minute, 0));
                    break;
                case IncludeTime.Complete:
                    result = result.Date
                        .Add(new TimeSpan(hour, minute, second));
                    break;
                default:
                    break;
            }
            return result;
        }

        /// <summary>
        /// Converts Hijri date to Gregorian
        /// </summary>
        /// <param name="date">for date param please use the corrent format: "year/month/day-hour:minutes:second"</param>
        public static DateTime HijriToGregorian(this string date, IncludeTime includeTime)
        {
            if (string.IsNullOrEmpty(date))
            {
                throw new ArgumentNullException("input hijri date is null or empty!");
            }

            var dateAndTimeArr = date.Split('-');
            var hijriDateArr = dateAndTimeArr[0].Split('/');

            var result = HC.ToDateTime(int.Parse(hijriDateArr[0]), int.Parse(hijriDateArr[1])
                , int.Parse(hijriDateArr[2]), 0, 0, 0, 0);

            #region time
            int hour = DateTime.Now.Hour;
            int minute = DateTime.Now.Minute;
            int second = DateTime.Now.Second;

            if (dateAndTimeArr.Length > 1)
            {
                var timeArr = dateAndTimeArr[1];
                if (timeArr != string.Empty)
                {
                    var timeSplits = timeArr.Split(':');
                    hour = timeSplits.Length > 0 ? int.Parse(timeSplits[0]) : 0;
                    minute = timeSplits.Length > 1 ? int.Parse(timeSplits[1]) : 0;
                    second = timeSplits.Length > 2 ? int.Parse(timeSplits[2]) : 0;
                }
            }
            #endregion

            switch (includeTime)
            {
                case IncludeTime.No:
                    result = result.Date
                        .Add(TimeSpan.Zero);
                    break;
                case IncludeTime.JustHour:
                    result = result.Date
                        .Add(new TimeSpan(hour, 0, 0));
                    break;
                case IncludeTime.HourAndMinute:
                    result = result.Date
                        .Add(new TimeSpan(hour, minute, 0));
                    break;
                case IncludeTime.Complete:
                    result = result.Date
                        .Add(new TimeSpan(hour, minute, second));
                    break;
                default:
                    break;
            }

            return result;
        }
    }
}

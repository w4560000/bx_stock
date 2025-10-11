using System;
using System.Collections.Generic;
using System.Linq;

namespace BX_Stock.Helper
{
    /// <summary>
    /// DateTimeHelper
    /// </summary>
    public static class DateTimeHelper
    {
        /// <summary>
        /// 取得起始時間到結束時間這段的所有日期
        /// </summary>
        /// <param name="dateFrom">起始時間</param>
        /// <param name="dateTo">結束時間</param>
        /// <returns></returns>
        public static IEnumerable<DateTime> EachDayTo(this DateTime dateFrom, DateTime dateTo)
        {
            static IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
            {
                for (DateTime day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                    yield return day;
            }

            return EachDay(dateFrom, dateTo);
        }

        /// <summary>
        /// 取得起始時間到結束時間這段的所有月份
        /// </summary>
        /// <param name="dateFrom">起始時間</param>
        /// <param name="dateTo">結束時間</param>
        /// <returns></returns>
        public static IEnumerable<DateTime> EachMonthTo(this DateTime dateFrom, DateTime dateTo)
        {
            static IEnumerable<DateTime> EachMonth(DateTime from, DateTime thru)
            {
                for (DateTime month = from.Date; month.Date <= thru.Date || month.Month == thru.Month; month = month.AddMonths(1))
                    yield return month;
            }

            return EachMonth(dateFrom, dateTo);
        }

        /// <summary>
        /// DateTime轉換 民國轉西元 (114/09/01 => 2025/09/01)
        /// </summary>
        public static string ConvertToADType(this string dateTimeOfTaiwanType)
        {
            string[] date = dateTimeOfTaiwanType.Split('/');
            date[0] = (Convert.ToInt32(date.FirstOrDefault()) + 1911).ToString();

            return string.Join('/', date);
        }

        /// <summary>
        /// DateTime轉換 民國轉西元 (1140901 => DateTime)
        /// </summary>
        public static DateTime ConvertToADType2(this string dateTimeOfTaiwanType)
        {
            var year = dateTimeOfTaiwanType[..3];
            var month = dateTimeOfTaiwanType.Substring(3, 2);
            var day = dateTimeOfTaiwanType[5..];

            return DateTime.Parse($"{int.Parse(year) + 1911}/{month}/{day}");
        }

        /// <summary>
        /// 轉成民國時間
        /// </summary>
        /// <param name="dateTime">dateTime</param>
        /// <returns>民國時間</returns>
        public static string ConvertToTaiwanType(this DateTime dateTime)
        {
            string date = $"{dateTime.Year - 1911}/{dateTime.Month}";

            return date;
        }

        /// <summary>
        /// 取得該週 週一And週五 日期
        /// </summary>
        /// <param name="now">現在時間</param>
        /// <returns>(該週 週一日期, 該週 週五日期)</returns>
        public static (DateTime, DateTime) GetCurrentMondayAndFridayOfWeek(this DateTime now)
        {
            static int GetDayOfWeek(DateTime now) => now.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)now.DayOfWeek;

            DateTime currentMondayOfWeek = now.AddDays(1 - GetDayOfWeek(now));
            DateTime currentFridayOfWeek = now.AddDays(5 - GetDayOfWeek(now));

            return (new DateTime(currentMondayOfWeek.Year, currentMondayOfWeek.Month, currentMondayOfWeek.Day),
                    new DateTime(currentFridayOfWeek.Year, currentFridayOfWeek.Month, currentFridayOfWeek.Day));
        }

        /// <summary>
        /// 是否為假日
        /// </summary>
        /// <returns>是否為假日</returns>
        public static bool IsHoliday(DateTime date) => date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
    }
}
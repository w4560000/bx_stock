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
        /// DateTime轉換 民國轉西元
        /// </summary>
        /// <param name="dateTimeOfTaiwanType"></param>
        /// <returns></returns>
        public static string ConvertToADType(this string dateTimeOfTaiwanType)
        {
            string[] date = dateTimeOfTaiwanType.Split('/');
            date[0] = (Convert.ToInt32(date.FirstOrDefault()) + 1911).ToString();

            return string.Join('/', date);
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
    }
}
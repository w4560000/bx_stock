using System;

namespace BX_Stock.Helper
{
    /// <summary>
    /// 轉換 Helper
    /// </summary>
    public static class ConvertHelper
    {
        /// <summary>
        /// 字串包含其他符號 則回傳0
        /// </summary>
        /// <param name="value">數字(字串型態)</param>
        /// <returns>數值</returns>
        public static double CheckDoubleValue(this string value)
        {
            if (value.Contains("X") || value.Equals("--"))
            {
                return 0;
            }

            return Convert.ToDouble(value);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

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

        /// <summary>
        /// 將 List<T> 轉成 DataTable
        /// </summary>
        public static DataTable ToDataTable<T>(this List<T> data)
        {
            var table = new DataTable();
            if (data == null || data.Count == 0)
                return table;

            // 取得型別的所有公共屬性
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // 建立 DataTable 欄位
            foreach (var prop in props)
            {
                Type columnType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                table.Columns.Add(prop.Name, columnType);
            }

            // 填入資料
            foreach (var item in data)
            {
                var row = table.NewRow();
                foreach (var prop in props)
                {
                    var value = prop.GetValue(item, null) ?? DBNull.Value;
                    row[prop.Name] = value;
                }
                table.Rows.Add(row);
            }

            return table;
        }
    }
}
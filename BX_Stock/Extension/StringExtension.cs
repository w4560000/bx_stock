using System;
using System.Text;

namespace BX_Stock.Extension
{
    /// <summary>
    /// string 擴充方法
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// Base64解碼
        /// </summary>
        /// <param name="value">Base 64 字串</param>
        /// <returns>解碼結果</returns>
        public static string DecodeBase64(string value)
        {
            try
            {
                return Encoding.UTF8.GetString(Convert.FromBase64String(value));
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Base64編碼
        /// </summary>
        /// <param name="value">原始字串</param>
        /// <returns>編碼結果</returns>
        public static string EncodeBase64(string value)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(value);
                return Convert.ToBase64String(data);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 是否為NULL或是空值
        /// </summary>
        /// <param name="source">目標字串</param>
        /// <returns>判斷結果</returns>
        public static bool IsNullOrEmpty(this string source)
        {
            return string.IsNullOrEmpty(source);
        }

        /// <summary>
        /// RemoveLast
        /// </summary>
        /// <param name="text">text</param>
        /// <param name="character">character</param>
        /// <returns>新字串</returns>
        public static string RemoveLast(this string text, string character)
        {
            if (text.Length < 1) return text;
            return text.Remove(text.LastIndexOf(character, StringComparison.Ordinal), character.Length);
        }

        /// <summary>
        /// 將 System.String 類別的新執行個體初始化為由重複指定次數的指定 Unicode 字元所指示的值。
        /// </summary>
        /// <param name="chatToRepeat">Unicode 字元所指示的值</param>
        /// <param name="repeat">重複指定次數</param>
        /// <returns>新字串</returns>
        public static string Repeat(char chatToRepeat, int repeat)
        {
            return new string(chatToRepeat, repeat);
        }

        /// <summary>
        /// 新執行個體初始化為由重複指定次數的指定字串所指示的值。
        /// </summary>
        /// <param name="stringToRepeat">字串</param>
        /// <param name="repeat">重複指定次數</param>
        /// <returns>新字串</returns>
        public static string Repeat(this string stringToRepeat, int repeat)
        {
            StringBuilder builder = new StringBuilder(repeat * stringToRepeat.Length);
            for (int i = 0; i < repeat; i++)
            {
                builder.Append(stringToRepeat);
            }
            return builder.ToString();
        }

        /// <summary>
        /// 取代字串特定位置的內容
        /// </summary>
        /// <param name="original">原始字串</param>
        /// <param name="startIndex">開始位置</param>
        /// <param name="replacement">替換內容</param>
        /// <returns>子字串</returns>
        public static string ReplaceStringAt(this string original, int startIndex, int Length, string replacement)
        {
            return original.Substring(0, startIndex) + replacement + original.Substring(startIndex + Length);
        }

        /// <summary>
        /// 轉為小駝峰
        /// </summary>
        /// <param name="orignnal">原始字串</param>
        /// <returns>小駝峰字串</returns>
        public static string ToLowerCamel(this string orignnal)
        {
            return orignnal.Replace(orignnal.Substring(0, 1), orignnal.Substring(0, 1).ToLower());
        }
    }
}
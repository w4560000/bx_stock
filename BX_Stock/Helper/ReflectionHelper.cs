using BX_Stock.Extension;

namespace BX_Stock
{
    /// <summary>
    /// 映射Helper
    /// </summary>
    public static class ReflectionHelper
    {
        /// <summary>
        /// 傳入PropertyName取得DisplayName
        /// </summary>
        /// <typeparam name="T">型別</typeparam>
        /// <param name="propertyName">propertyName</param>
        /// <returns>DisplayName</returns>
        public static string GetDisplayName<T>(string propertyName)
        {
            return typeof(T).GetProperty(propertyName).GetDisplayName();
        }
    }
}
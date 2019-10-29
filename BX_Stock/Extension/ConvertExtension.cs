using System.Reflection;

namespace BX_Stock.Extension
{
    public static class ConvertExtension
    {
        public static string ConvertToGetMethodUrlParam<T>(this T param)
        {
            string result = string.Empty;

            foreach (PropertyInfo prop in typeof(T).GetProperties())
            {
                result += $"{prop.Name.ToLowerCamel()}={prop.GetValue(param)}&";
            }

            return result.TrimEnd('&');
        }
    }
}
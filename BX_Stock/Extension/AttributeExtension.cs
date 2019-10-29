using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace BX_Stock.Extension
{
    /// <summary>
    /// 屬性擴充類別
    /// </summary>
    public static class AttributeExtensions
    {
        /// <summary>
        /// 取得DisplayAttribute公開屬性內容
        /// </summary>
        /// <param name="attr">取得DisplayAttribute公開屬性內容</param>
        /// <param name="propName">公開屬性名稱，預設Name</param>
        /// <returns>The value that is used for display in the UI</returns>
        public static string GetDisplay(this DisplayAttribute attr, string propName = "Name")
        {
            string result = string.Empty;
            Type tp = attr.GetType();
            PropertyInfo prop = tp.GetProperty(propName);
            if (prop != null)
            {
                result = (string)prop.GetValue(attr);
            }

            return result;
        }

        /// <summary>
        /// 取得DisplayAttribute公開屬性內容
        /// </summary>
        /// <param name="pInfo">
        /// Discovers the attributes of a property and provides access to property metadata.
        /// </param>
        /// <returns>The value that is used for display in the UI.</returns>
        public static string GetDisplayName(this PropertyInfo pInfo)
        {
            if (null == pInfo)
            {
                return string.Empty;
            }

            DisplayAttribute display = pInfo.GetCustomAttributes(typeof(DisplayAttribute), true)
                                            .Cast<DisplayAttribute>()
                                            .SingleOrDefault();
            if (display == null)
            {
                return string.Empty;
            }

            return display.GetDisplay("Name");
        }

        /// <summary>
        /// 取得物件屬性
        /// </summary>
        /// <typeparam name="T">這個委派所封裝之方法的參數類型。</typeparam>
        /// <typeparam name="TResult">這個委派所封裝之方法的傳回值之類型。</typeparam>
        /// <param name="instance">物件實體</param>
        /// <param name="selector">Lambda selector</param>
        /// <returns>取得成員的屬性相關資訊，並提供成員中繼資料的存取。如果找不到會回傳null</returns>
        public static MemberInfo GetMember<T, TResult>(this T instance, Expression<Func<T, TResult>> selector)
        {
            if (selector.Body is MemberExpression member)
            {
                return member.Member;
            }
            return null;
        }

        /// <summary>
        /// 取得物件屬性上的 Attribute
        /// </summary>
        /// <typeparam name="T">The type of Attribute</typeparam>
        /// <param name="memberInfo">物件屬性</param>
        /// <returns>Attribute</returns>
        public static T GetAttribute<T>(this MemberInfo memberInfo) where T : Attribute
        {
            return memberInfo.GetCustomAttributes(typeof(T)).FirstOrDefault() as T;
        }
    }
}
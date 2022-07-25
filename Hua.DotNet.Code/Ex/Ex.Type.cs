using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Hua.DotNet.Code.Ex
{
    public static partial class Ex
    {
        /// <summary>
        /// 获取属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static List<PropertyInfo>? GetProperties<T>(this T t)
        {
            if (t == null)
            {
                return null;
            }

            var propertyInfoList =
                t
                    .GetType()
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .ToList();

            return propertyInfoList;
        }
    }
}
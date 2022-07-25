using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Hua.DotNet.Code.Ex
{
    /// <summary>
    /// 枚举拓展
    /// </summary>
    public static partial class Ex
    {
        /// <summary>
        /// 将枚举转换为字典
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static Dictionary<int, string> Dictionary(this Type enumType)
        {
            var dictionary = new Dictionary<int, string>();
            var typeDescription = typeof(DescriptionAttribute);
            var fields = enumType.GetFields();
            foreach (var field in fields)
            {
                if (!field.FieldType.IsEnum) continue;
                var sValue = ((int)enumType.InvokeMember(field.Name, BindingFlags.GetField, null, null, null));
                object[] arr = field.GetCustomAttributes(typeDescription, true);
                string sText;
                if (arr.Length > 0)
                {
                    var da = (DescriptionAttribute)arr[0];
                    sText = da.Description;
                }
                else
                {
                    sText = field.Name;
                }

                dictionary.Add(sValue, sText);
            }

            return dictionary;
        }
    }
}
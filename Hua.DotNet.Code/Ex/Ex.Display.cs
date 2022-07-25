using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Hua.DotNet.Code.Ex
{
    /// <summary>
    /// DisplayName 拓展
    /// </summary>
    public static partial class Ex
    {
        /// <summary>
        ///  获取 展示名称
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string DisplayName(this object obj)
        {
            var type = obj.GetType();
            if (type.IsEnum)
            {
                return type.GetFields().First(m => m.Name == Enum.GetName(type, obj))
                           .GetCustomAttribute<DisplayAttribute>()?.Name
                       ?? type.Name;
            }

            return
                type.GetCustomAttribute<DisplayAttribute>()?.Name ??
                type.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ??
                type.Name;
        }

        /// <summary>
        /// 获取 展示名称
        /// </summary>
        /// <param name="value">枚举</param>
        /// <returns></returns>
        public static string DisplayName(this PropertyInfo value)
        {
            return
                value.GetCustomAttribute<DisplayAttribute>()?.Name ??
                value.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ??
                value.Name;
        }
    }
}
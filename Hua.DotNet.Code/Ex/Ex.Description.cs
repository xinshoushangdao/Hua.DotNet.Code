using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Hua.DotNet.Code.Ex
{
    /// <summary>
    /// Description 拓展
    /// </summary>
    public static partial class Ex
    {
        /// <summary>
        ///  获取 展示名称
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Description(this object obj)
        {
            var type = obj.GetType();
            if (type.IsEnum)
            {
                return
                    type.GetFields().First(m => m.Name == Enum.GetName(type, obj))
                        .GetCustomAttribute<DescriptionAttribute>()?.Description
                    ?? type.Name;
            }

            return
                type.GetCustomAttribute<DescriptionAttribute>()?.Description
                ?? type.Name;
        }

        /// <summary>
        /// 获取 描述
        /// </summary>
        /// <param name="value">枚举</param>
        /// <returns></returns>
        public static string Description(this PropertyInfo value)
        {
            return
                value.GetCustomAttribute<DescriptionAttribute>()?.Description ??
                value.Name;
        }
    }
}
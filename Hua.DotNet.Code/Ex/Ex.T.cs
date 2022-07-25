using System;

namespace Hua.DotNet.Code.Ex
{
    public partial class Ex
    {
        /// <summary>
        /// 判断对象是否为空，为空返回true
        /// </summary>
        /// <typeparam name="T">要验证的对象的类型</typeparam>
        /// <param name="t">要验证的对象</param>        
        public static bool IsNullOrEmpty<T>(T t)
        {
            //如果为null
            if (t == null)
            {
                return true;
            }

            //如果为""
            if (t is not string) return t is DBNull;
            if (string.IsNullOrEmpty(t.ToString().Trim()))
            {
                return true;
            }

            //如果为DBNull
            return t is DBNull;
        }
    }
}
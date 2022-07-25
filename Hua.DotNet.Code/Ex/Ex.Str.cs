using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Hua.DotNet.Code.Ex
{
    /// <summary>
    /// 字符串操作类
    /// </summary>
    public static partial class Ex
    {
        #region 字符串操作

        /// <summary>
        /// 0.删除最前面一个字符之后的字符
        /// </summary>
        /// <param name="s"></param>
        /// <param name="searchStr"></param>
        /// <returns></returns>
        public static string TrimStartStr(this string s, string searchStr)
        {
            var result = s;
            try
            {
                if (string.IsNullOrEmpty(result))
                {
                    return result;
                }

                if (s.Length < searchStr.Length)
                {
                    return result;
                }

                if (s.IndexOf(searchStr, 0, searchStr.Length, StringComparison.Ordinal) > -1)
                {
                    result = s.Substring(searchStr.Length, s.Length - searchStr.Length);
                }

                return result;
            }
            catch (Exception)
            {
                return result;
            }
        }

        /// <summary>
        /// 1.删除最后一个字符之后的字符
        /// </summary>
        public static string TrimEndStr(this string srcStr, string desStr)
        {
            return srcStr.Substring(0, srcStr.Length - desStr.Length);
        }

        /// <summary>
        /// 注意：如果替换的旧值中有特殊符号，替换将会失败，解决办法 例如特殊符号是“(”：  要在调用本方法前加oldValue=oldValue.Replace("(","//(");
        /// </summary>
        /// <param name="input"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public static string ReplaceFirstStr(this string input, string oldValue, string? newValue)
        {
            var regEx = new Regex(oldValue, RegexOptions.Multiline);
            return regEx.Replace(input, newValue ?? "", 1);
        }

        /// <summary>
        /// 4. 把字符串按照指定分隔符装成 List 去除重复
        /// </summary>
        /// <param name="oStr"></param>
        /// <param name="split"></param>
        /// <returns></returns>
        public static List<string> GetSubStringList(this string oStr, char split)
        {
            string[] ss = oStr.Split(split);

            return ss.Where(s => !string.IsNullOrEmpty(s) && s != split.ToString()).ToList();
        }

        /// <summary>
        /// 5.分割字符串
        /// </summary>
        /// <param name="str">要被分割的字符串</param>
        /// <param name="splitStr">间隔字符串</param>
        /// <returns></returns>
        public static string[]? SplitMulti(this string str, string splitStr)
        {
            string[]? strArray = null;
            if (!string.IsNullOrEmpty(str))
            {
                strArray = new Regex(splitStr).Split(str);
            }

            return strArray;
        }

        /// <summary>
        /// 截取指定字符串中间内容
        /// </summary>
        /// <param name="src"></param>
        /// <param name="startStr"></param>
        /// <param name="endStr"></param>
        /// <returns></returns>
        public static string SubstringBetween(this string src, string startStr, string endStr)
        {
            var result = string.Empty;
            try
            {
                var startIndex = src.IndexOf(startStr, StringComparison.Ordinal);
                if (startIndex == -1)
                    return result;
                var substring = src.Substring(startIndex + startStr.Length);
                var endIndex = substring.IndexOf(endStr, StringComparison.Ordinal);
                if (endIndex == -1)
                    return result;
                result = substring.Remove(endIndex);
            }
            catch (Exception ex)
            {
                Console.WriteLine("MidStrEx Err:" + ex.Message);
            }

            return result;
        }

        #endregion

        #region 判断

        /// <summary>
        /// 验证一个字符串是否符合指定的正则表达式
        /// </summary>
        /// <param name="express">正则表达式的内容。</param>
        /// <param name="str">需验证的字符串。</param>
        /// <returns>是否合法的bool值。</returns>
        public static bool IsMatch(this string str, string express)
        {
            if (string.IsNullOrEmpty(str)) return false;
            var myRegex = new Regex(express);
            return str.Length != 0 && myRegex.IsMatch(str);
        }


        /// <summary>
        /// 判断是否为空
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }


        /// <summary>
        /// 判断对象是否为空，为空返回true
        /// </summary>
        /// <param name="data">要验证的对象</param>
        public static bool IsNullOrEmpty(object? data)
        {
            switch (data)
            {
                //如果为null
                case null:
                //如果为""
                case string when string.IsNullOrEmpty(data.ToString().Trim()):
                    return true;
                default:
                    //如果为DBNull
                    return data is DBNull;
            }
        }

        #endregion

        #region 计数

        /// <summary>
        /// 大写字母个数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int CountUpper(this string str)
        {
            int count1 = 0;
            char[] chars = str.ToCharArray();
            foreach (char num in chars)
            {
                if (num >= 'A' && num <= 'Z')
                {
                    count1++;
                }
                //else if (num >= 'a' && num <= 'z')
                //{
                //    count2++;
                //}
            }

            return count1;
        }

        #endregion
    }
}
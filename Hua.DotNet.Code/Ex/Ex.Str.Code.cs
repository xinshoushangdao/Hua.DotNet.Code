using System.Linq;
using System.Text;

namespace Hua.DotNet.Code.Ex
{
    public static partial class Ex
    {
        /// <summary>
        /// 骆驼峰转下划线
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string ToSmallCamelCase(string name)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(name.Substring(0, 1).ToLower());

            for (var i = 0; i < name.Length; i++)
            {
                if (i == 0)
                {
                    stringBuilder.Append(name.Substring(0, 1).ToLower());
                }
                else
                {
                    if (name[i] >= 'A' && name[i] <= 'Z')
                    {
                        stringBuilder.Append($"_{name.Substring(i, 1).ToLower()}");
                    }
                    else
                    {
                        stringBuilder.Append(name[i]);
                    }
                }
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// 下划线命名转驼峰命名
        /// </summary>
        /// <param name="underscore"></param>
        /// <returns></returns>
        public static string UnderScoreToCamelCase(this string underscore)
        {
            var ss = underscore.Split('_');
            if (ss.Length == 1)
            {
                return underscore;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(ss[0]);
            for (var i = 1; i < ss.Length; i++)
            {
                sb.Append(ss[i].FirstCharToUp());
            }

            return sb.ToString();
        }

        /// <summary>
        /// 首字母转大写
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FirstCharToUp(this string str)
        {
            return
                string.IsNullOrEmpty(str) ? str : str.Substring(0, 1).ToUpper() + str.Substring(1, str.Length - 1);
        }

        /// <summary>
        /// 首字母转小写
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FirstCharToLower(this string str)
        {
            return
                string.IsNullOrEmpty(str) ? str : str.Substring(0, 1).ToLower() + str.Substring(1, str.Length - 1);
        }


        /// <summary>
        /// 转换为Pascal风格-每一个单词的首字母大写
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <param name="fieldDelimiter">分隔符</param>
        /// <returns></returns>
        public static string ConvertToPascal(this string fieldName, string fieldDelimiter)
        {
            var result = string.Empty;
            if (fieldName.Contains(fieldDelimiter))
            {
                //全部小写
                var array = fieldName.ToLower().Split(fieldDelimiter.ToCharArray());
                result = array.Aggregate(result,
                    (current, t) => current + (t.Substring(0, 1).ToUpper() + t.Substring(1, t.Length - 1)));
            }
            else if (string.IsNullOrWhiteSpace(fieldName))
            {
                result = fieldName;
            }
            else if (fieldName.Length == 1)
            {
                result = fieldName.ToUpper();
            }
            else if (fieldName.Length == CountUpper(fieldName))
            {
                result = fieldName.Substring(0, 1).ToUpper() + fieldName.Substring(1, fieldName.Length - 1).ToLower();
            }
            else
            {
                result = fieldName.Substring(0, 1).ToUpper() + fieldName.Substring(1, fieldName.Length - 1);
            }

            return result;
        }

        /// <summary>
        /// 转换为Camel风格-第一个单词小写，其后每个单词首字母大写
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <param name="fieldDelimiter">分隔符</param>
        /// <returns></returns>
        public static string ConvertToCamel(this string fieldName, string fieldDelimiter)
        {
            //先Pascal
            var result = ConvertToPascal(fieldName, fieldDelimiter);
            //然后首字母小写
            if (result.Length == 1)
            {
                result = result.ToLower();
            }
            else
            {
                result = result.Substring(0, 1).ToLower() + result.Substring(1, result.Length - 1);
            }

            return result;
        }
    }
}
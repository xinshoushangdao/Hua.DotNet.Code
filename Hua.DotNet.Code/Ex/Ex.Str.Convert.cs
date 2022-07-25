using System.Linq;
using System.Text.RegularExpressions;

namespace Hua.DotNet.Code.Ex
{
    public partial class Ex
    {

        /// <summary>
        /// 2. 转全角的函数(SBC case)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToSbc(this string input)
        {
            //半角转全角：
            var c = input.ToCharArray();
            for (var i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }

                if (c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }

            return new string(c);
        }

        /// <summary>
        ///  3. 转半角的函数(SBC case)
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns></returns>
        public static string ToDbc(this string input)
        {
            var c = input.ToCharArray();
            for (var i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }

                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }

            return new string(c);
        }


        /// <summary>
        /// HTML转行成TEXT
        /// </summary>
        /// <param name="strHtml"></param>
        /// <returns></returns>
        public static string HtmlToTxt(this string strHtml)
        {
            string[] aryReg =
            {
                @"<script[^>]*?>.*?</script>",
                @"<(\/\srcStr*)?!?((\w+:)?\w+)(\w+(\srcStr*=?\srcStr*(([""'])(\\[""'tbnr]|[^\7])*?\7|\w+)|.{0})|\srcStr)*?(\/\srcStr*)?>",
                @"([\r\n])[\srcStr]+",
                @"&(quot|#34);",
                @"&(amp|#38);",
                @"&(lt|#60);",
                @"&(gt|#62);",
                @"&(nbsp|#160);",
                @"&(iexcl|#161);",
                @"&(cent|#162);",
                @"&(pound|#163);",
                @"&(copy|#169);",
                @"&#(\d+);",
                @"-->",
                @"<!--.*\n"
            };

            var newReg = aryReg[0];
            var strOutput = aryReg.Select(t => new Regex(t, RegexOptions.IgnoreCase)).Aggregate(strHtml, (current, regex) => regex.Replace(current, string.Empty));

            strOutput = strOutput.Replace("<", "");
            strOutput = strOutput.Replace(">", "");
            strOutput = strOutput.Replace("\r\n", "");

            return strOutput;
        }

    }
}

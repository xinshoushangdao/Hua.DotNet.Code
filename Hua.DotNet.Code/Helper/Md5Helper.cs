using System;
using System.Linq;
using System.Security.Cryptography;

namespace Hua.DotNet.Code.Helper
{
    /// <summary>
    /// Md5加密
    /// </summary>
    public static class Md5Helper
    {
        #region MD5加密

        /// <summary>
        /// 128位MD5算法加密字符串
        /// </summary>
        /// <param name="text">要加密的字符串</param>    
        public static string EnCode(string text)
        {
            //如果字符串为空，则返回
            return string.IsNullOrEmpty(text) ? "" : EnCode(System.Text.Encoding.Unicode.GetBytes(text));
            //返回MD5值的字符串表示             
        }

        /// <summary>
        /// 128位MD5算法加密Byte数组
        /// </summary>
        /// <param name="data">要加密的Byte数组</param>    
        public static string EnCode(byte[] data)
        {
            //如果Byte数组为空，则返回
            if (data.Length == 0)
            {
                return "";
            }

            try
            {
                //创建MD5密码服务提供程序
                var md5 = new MD5CryptoServiceProvider();

                //计算传入的字节数组的哈希值
                var result = md5.ComputeHash(data);

                //释放资源
                md5.Clear();

                //返回MD5值的字符串表示
                return Convert.ToBase64String(result);
            }
            catch
            {
                //LogHelper.WriteTraceLog(TraceLogLevel.Error, ex.Message);
                return "";
            }
        }

        public static string Md5Sum(string text)
        {
            //如果字符串为空，则返回
            return string.IsNullOrEmpty(text) ? "" : Md5Sum(System.Text.Encoding.UTF8.GetBytes(text));
            //返回MD5值的字符串表示             
        }


        public static string Md5Sum(byte[] bs)
        {
            // 创建md5 对象  
            var md5 = MD5.Create();

            // 生成16位的二进制校验码  
            var hashBytes = md5.ComputeHash(bs);

            // 转为32位字符串  
            var hashString = hashBytes.Aggregate("",
                (current, t) => current + System.Convert.ToString(t, 16).PadLeft(2, '0'));

            return hashString.PadLeft(32, '0');
        }

        #endregion
    }
}
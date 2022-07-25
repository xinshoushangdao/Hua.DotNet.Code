using System;
using System.Security.Cryptography;
using System.Text;

namespace Hua.DotNet.Code.Helper
{
    /// <summary>
    /// RSA 帮助类
    /// </summary>
    public class RsaHelper
    {
        private static RSA? _Rsa = null;

        /// <summary>
        /// 初始化Rsa
        /// </summary>
        private static void InitRsa()
        {
            _Rsa ??= RSA.Create();
        }

        #region 加解密

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Encrypt(string str)
        {
            InitRsa();
            return
                Convert.ToBase64String(_Rsa.Encrypt(Encoding.UTF8.GetBytes(str), RSAEncryptionPadding.Pkcs1));
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="enStr"></param>
        /// <returns></returns>
        public static string Decrypt(string enStr)
        {
            InitRsa();
            return
                Encoding.UTF8.GetString(_Rsa.Decrypt(Convert.FromBase64String(enStr), RSAEncryptionPadding.Pkcs1));
        }

        #endregion

        #region 获取公钥

        /// <summary>
        /// 公钥
        /// </summary>
        /// <returns></returns>
        public static string PublicKey()
        {
            InitRsa();
            return _Rsa.ToXmlString(false);
        }

        #endregion

        #region 私有方法

        // /// <summary>
        // /// xml private key -> base64 private key string
        // /// </summary>
        // /// <param name="xmlPrivateKey"></param>
        // /// <returns></returns>
        // public static string FromXmlPrivateKey(string xmlPrivateKey)
        // {
        //     var result = string.Empty;
        //     using var rsa = new RSACryptoServiceProvider();
        //     rsa.FromXmlString(xmlPrivateKey);
        //     var param = rsa.ExportParameters(true);
        //
        //     RsaPrivateCrtKeyParameters privateKeyParam = new RsaPrivateCrtKeyParameters(
        //         new BigInteger(1, param.Modulus), new BigInteger(1, param.Exponent),
        //         new BigInteger(1, param.D), new BigInteger(1, param.P),
        //         new BigInteger(1, param.Q), new BigInteger(1, param.DP),
        //         new BigInteger(1, param.DQ), new BigInteger(1, param.InverseQ));
        //     PrivateKeyInfo privateKey = PrivateKeyInfoFactory.CreatePrivateKeyInfo(privateKeyParam);
        //
        //     result = Convert.ToBase64String(privateKey.ToAsn1Object().GetEncoded());
        //
        //     return result;
        // }

        // /// <summary>
        // /// xml public key -> base64 public key string
        // /// </summary>
        // /// <param name="xmlPublicKey"></param>
        // /// <returns></returns>
        // public static string FromXmlPublicKey(string xmlPublicKey)
        // {
        //     string result = string.Empty;
        //     using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
        //     {
        //         rsa.FromXmlString(xmlPublicKey);
        //         RSAParameters p = rsa.ExportParameters(false);
        //         RsaKeyParameters keyParams = new RsaKeyParameters(
        //             false, new BigInteger(1, p.Modulus), new BigInteger(1, p.Exponent));
        //         SubjectPublicKeyInfo publicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(keyParams);
        //         result = Convert.ToBase64String(publicKeyInfo.ToAsn1Object().GetEncoded());
        //     }
        //
        //     return result;
        // }

        #endregion
    }
}


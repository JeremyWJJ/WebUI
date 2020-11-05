using SpdCommon.Modules;
using System;
using System.Security.Cryptography;
using System.Text;

namespace SpdCommon.Utils
{
    public class SHA1Helper
    {
        /// <summary>
        /// SHA1 单向加密
        /// </summary>

        private static Encoding encoding = Encoding.UTF8;

        /// <summary>
        /// 获得一个字符串的加密密文
        /// </summary>
        /// <param name="plainTextString"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string GenerateSaltedSHA1Password(string plainTextString, string salt)
        {
            if (string.IsNullOrEmpty(plainTextString)) throw new CoreException("密码不能为空.");
            if (string.IsNullOrEmpty(salt)) throw new CoreException("混淆密钥不能为空.");

            byte[] passwordBytes = encoding.GetBytes(plainTextString);
            byte[] saltBytes = strToToHexByte(salt);

            byte[] buffer = new byte[passwordBytes.Length + saltBytes.Length];

            Buffer.BlockCopy(saltBytes, 0, buffer, 0, saltBytes.Length);
            Buffer.BlockCopy(passwordBytes, 0, buffer, saltBytes.Length, passwordBytes.Length);

            var hashAlgorithm = HashAlgorithm.Create("SHA1");
            var s = hashAlgorithm.ComputeHash(buffer);

            //byte[] saltedSHA1Bytes = s;
            //for (int i = 1; i < 1024; i++)
            //{
            //    saltedSHA1Bytes = hashAlgorithm.ComputeHash(saltedSHA1Bytes);
            //}
            return byteToHexStr(s);
        }

        private static byte[] strToToHexByte(string hexString)
        {
            int NumberChars = hexString.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
            }
            return bytes;
        }

        private static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("x2");
                }
            }
            return returnStr;
        }
    }
}
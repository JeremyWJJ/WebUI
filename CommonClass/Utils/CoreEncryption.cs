using System.Security.Cryptography;
using System.Text;

namespace Common.Utils
{
    // 加密
    public class CoreEncryption
    {
        /// <summary>
        /// 获得一个字符串的加密密文 此密文为单向加密(32位大写)，即不可逆(解密)密文
        /// </summary>
        /// <param name="plainText">待加密明文</param>
        /// <returns>已加密密文</returns>
        public static string Md5_32(string plainText = "")
        {
            // 返回值
            string encryptText = "";
            if (string.IsNullOrEmpty(plainText)) return encryptText;

            // 创建一个md5的默认实例
            MD5 md5 = MD5.Create();
            // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择
            byte[] vs = md5.ComputeHash(Encoding.UTF8.GetBytes(plainText));

            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            for (int i = 0; i < vs.Length; i++)
            {
                // X代表十六进制，x大写返回的大写十六进制，x小写代表
                string btos = vs[i].ToString("X2");

                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符
                encryptText = encryptText + btos;
            }
            return encryptText;
        }
    }
}
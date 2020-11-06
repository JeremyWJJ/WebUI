using Common.Utils.Extensions;
using System.Text;

namespace Common.Utils
{
    public class StringHelper
    {
        /// <summary>
        /// 超过指定长度使用指定省略符代替
        /// </summary>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <param name="ellipsis"></param>
        /// <returns></returns>
        public static string Ellipsis(string value, int length, string ellipsis = "...")
        {
            if (!value.IsLengthLessEqual(length))
            {
                value = value.Substring(0, length);
                value += ellipsis;
            }
            return value;
        }

        /// <summary>
        /// 将驼峰字符串转为下划线字符串
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string UnderscoreName(string name)
        {
            StringBuilder result = new StringBuilder();
            if (name != null && name.Length > 0)
            {
                // 将第一个字符处理成大写
                result.Append(name.Substring(0, 1).ToUpper());
                // 循环处理其余字符
                for (int i = 1; i < name.Length; i++)
                {
                    string s = name.Substring(i, i + 1);
                    // 在大写字母前添加下划线
                    if (s.Equals(s.ToUpper()) && !char.IsDigit(s[0]))
                    {
                        result.Append("_");
                    }
                    // 其他字符直接转成大写
                    result.Append(s.ToUpper());
                }
            }
            return result.ToString();
        }

        /// <summary>
        /// 将下划线字符串转为驼峰字符串
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string CamelName(string name)
        {
            StringBuilder result = new StringBuilder();
            // 快速检查
            if (name == null || name.IsEmpty())
            {
                // 没必要转换
                return "";
            }
            else if (!name.Contains("_"))
            {
                // 不含下划线，仅将首字母小写
                return name.Substring(0, 1).ToLower() + name.Substring(1);
            }
            // 用下划线将原始字符串分割
            string[] camels = name.Split('_');
            foreach (string camel in camels)
            {
                // 跳过原始字符串中开头、结尾的下换线或双重下划线
                if (camel.IsEmpty())
                {
                    continue;
                }
                // 处理真正的驼峰片段
                if (result.Length == 0)
                {
                    // 第一个驼峰片段，全部字母都小写
                    result.Append(camel.ToLower());
                }
                else
                {
                    // 其他的驼峰片段，首字母大写
                    result.Append(camel.Substring(0, 1).ToUpper());
                    result.Append(camel.Substring(1).ToLower());
                }
            }
            return result.ToString();
        }
    }
}
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace SpdCommon.Utils.Extensions
{
    public static class Validator
    {
        /// <summary>
        /// 是否为空字符串或者null
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static bool IsEmpty(this string src)
        {
            return string.IsNullOrEmpty(src);
        }

        /// <summary>
        /// 是否为double类型
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static bool IsDouble(this string src)
        {
            return double.TryParse(src, out _);
        }

        /// <summary>
        /// 是否为int
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static bool IsInt(this string src)
        {
            return int.TryParse(src, out _);
        }

        /// <summary>
        /// 是否为long
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static bool IsLong(this string src)
        {
            return long.TryParse(src, out _);
        }

        /// <summary>
        /// 是否为decimal
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static bool IsDecimal(this string src)
        {
            return decimal.TryParse(src, out _);
        }

        /// <summary>
        /// 是否小于等于指定长度
        /// </summary>
        /// <param name="src"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static bool IsLengthLessEqual(this string src, int length)
        {
            if (src.IsEmpty()) return false;
            return src.Length <= length;
        }

        /// <summary>
        /// 是否是下划线或者字母开头
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static bool IsStartWithLetterUnderLine(this string src)
        {
            if (src.IsEmpty()) return false;
            return Regex.IsMatch(src, "^[_a-zA-Z]+$");
        }

        /// <summary>
        /// 是否都是字母
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static bool IsAllLetter(this string src)
        {
            if (src.IsEmpty()) return false;
            return Regex.Match(src, "^[a-zA-Z]+$").Success;
        }

        /// <summary>
        /// 是否含有字母
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static bool HasLetter(this string src)
        {
            if (src.IsEmpty()) return false;
            return Regex.IsMatch(src, "[a-zA-Z]+");
        }

        /// <summary>
        /// 是否是无符号数值
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static bool IsNumberic(this string src)
        {
            if (src.IsEmpty()) return false;
            return Regex.Match(src, "^([0]|([1-9]+\\d{0,}?), .[\\d]+)?$").Success;
        }

        /// <summary>
        /// 是否包含无符号数值
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static bool HasNumberic(this string src)
        {
            if (src.IsEmpty()) return false;
            return Regex.IsMatch(src, "([0]|([1-9]+\\d{0,}?), .[\\d]+)?");
        }

        /// <summary>
        /// 是否是有符号数值
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static bool IsNumbericSign(this string src)
        {
            if (src.IsEmpty()) return false;
            return Regex.Match(src, "^-?\\d+$|^(-?\\d+)(\\.\\d+)?$").Success;
        }

        /// <summary>
        /// 是否包含有符号数值
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static bool HasNumbericSign(this string src)
        {
            if (src.IsEmpty()) return false;
            return Regex.IsMatch(src, "-?\\d+$|^(-?\\d+)(\\.\\d+)?");
        }

        /// <summary>
        /// 是否都是数字或者字母
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static bool IsAllNumberLetter(this string src)
        {
            if (src.IsEmpty()) return false;
            return Regex.Match(src, "^[0-9a-zA-Z]+$").Success;
        }

        /// <summary>
        /// 是否含有数字或者字母
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static bool HasNumberLetter(this string src)
        {
            if (src.IsEmpty()) return false;
            return Regex.IsMatch(src, "[0-9a-zA-Z]+");
        }

        /// <summary>
        /// 是否都是下划线数字和字母
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static bool IsAllNumberLetterUnderLine(this string src)
        {
            if (src.IsEmpty()) return false;
            return Regex.Match(src, "^[_0-9a-zA-Z]+$").Success;
        }

        /// <summary>
        /// 是否含有下划线数字和字母
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static bool HasNumberLetterUnderLine(this string src)
        {
            if (src.IsEmpty()) return false;
            return Regex.IsMatch(src, "[_0-9a-zA-Z]+");
        }

        /// <summary>
        /// 是否是url
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static bool IsUrl(this string src)
        {
            if (src.IsEmpty()) return false;
            return Regex.Match(src, "^(https?|ftp|file)://[-A-Za-z0-9+&@#/%?=~_|!:,.;]+[-A-Za-z0-9+&@#/%=~_|]$").Success;
        }

        /// <summary>
        /// 是否包含url
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static bool HasUrl(this string src)
        {
            if (src.IsEmpty()) return false;
            return Regex.IsMatch(src, "(https?|ftp|file)://[-A-Za-z0-9+&@#/%?=~_|!:,.;]+[-A-Za-z0-9+&@#/%=~_|]");
        }

        /// <summary>
        /// 是否是email
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static bool IsEmail(this string src)
        {
            if (src.IsEmpty()) return false;
            return Regex.Match(src, @"^[A-Za-z0-9\u4e00-\u9fa5]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+$").Success;
        }

        /// <summary>
        /// 是否包含email
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static bool HasEmail(this string src)
        {
            if (src.IsEmpty()) return false;
            return Regex.IsMatch(src, @"[A-Za-z0-9\u4e00-\u9fa5]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+");
        }

        /// <summary>
        /// 是否是ip地址
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static bool IsIPAddress(this string src)
        {
            if (src.IsEmpty()) return false;
            return Regex.Match(src, @"^(([2][0-5][0-5]\.)|([1][0-9][0-9]\.)|([1-9]\d?\.)){3}(([2][0-5][0-5])|([1][0-9][0-9])|([1-9]\d?))$").Success;
        }

        /// <summary>
        /// 是否包含ip地址
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static bool HasIPAddress(this string src)
        {
            if (src.IsEmpty()) return false;
            return Regex.IsMatch(src, @"(([2][0-5][0-5]\.)|([1][0-9][0-9]\.)|([1-9]\d?\.)){3}(([2][0-5][0-5])|([1][0-9][0-9])|([1-9]\d?))");
        }

        /// <summary>
        /// 是否是中国手机号
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static bool IsPhoneNumberCN(this string src)
        {
            if (src.IsEmpty()) return false;
            return Regex.Match(src, @"^1[3-8]\d{9}$").Success;
        }

        /// <summary>
        /// 是否包含中国手机号
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static bool HasPhoneNumberCN(this string src)
        {
            if (src.IsEmpty()) return false;
            return Regex.IsMatch(src, @"1[3-9]\d{9}");
        }

        /// <summary>
        /// 是否是中国电话号
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static bool IsTelCN(this string src)
        {
            if (src.IsEmpty()) return false;
            return Regex.Match(src, @"^(0[0-9]{2,3}\-)?([2-9][0-9]{6,7})+(\-[0-9]{1,4})?$", RegexOptions.IgnoreCase).Success;
        }

        /// <summary>
        /// 是否包含中国电话号
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static bool HasTelCN(this string src)
        {
            if (src.IsEmpty()) return false;
            return Regex.IsMatch(src, @"(0[0-9]{2,3}\-)?([2-9][0-9]{6,7})+(\-[0-9]{1,4})?", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 是否是有效中国身份证号
        /// </summary>
        /// <param name="str">输入字符</param>
        /// <returns>返回一个bool类型的值</returns>
        public static bool IsIDCardCN(this string src)
        {
            if (src.IsEmpty()) return false;
            switch (src.Length)
            {
                case 18:
                    {
                        return src.IsIDCardCN18();
                    }
                case 15:
                    {
                        return src.IsIDCardCN15();
                    }
                default:
                    return false;
            }
        }

        /// <summary>
        /// 验证输入字符串为中国18位的身份证号码
        /// </summary>
        /// <param name="src">输入字符</param>
        /// <returns>返回一个bool类型的值</returns>
        public static bool IsIDCardCN18(this string src)
        {
            if (src.IsEmpty()) return false;
            long n = 0;
            if (long.TryParse(src.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(src.Replace('x', '0').Replace('X', '0'), out n) == false)
            {
                return false;//数字验证
            }
            const string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(src.Remove(2), StringComparison.Ordinal) == -1)
            {
                return false;//省份验证
            }
            string birth = src.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime time;
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证
            }
            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] ai = src.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                sum += int.Parse(wi[i]) * int.Parse(ai[i].ToString());
            }
            int y = -1;
            Math.DivRem(sum, 11, out y);
            return arrVarifyCode[y] == src.Substring(17, 1).ToLower();
        }

        /// <summary>
        /// 验证输入字符串为中国15位的身份证号码
        /// </summary>
        /// <param name="src">输入字符</param>
        /// <returns>返回一个bool类型的值</returns>
        public static bool IsIDCardCN15(this string src)
        {
            if (src.IsEmpty()) return false;
            long n = 0;
            if (long.TryParse(src, out n) == false || n < Math.Pow(10, 14))
            {
                return false;//数字验证
            }
            const string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(src.Remove(2), StringComparison.Ordinal) == -1)
            {
                return false;
            }
            string birth = src.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            DateTime time;
            return DateTime.TryParse(birth, out time) != false;
        }

        /// <summary>
        /// 是否都是汉字
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static bool IsAllHanzi(this string src)
        {
            if (src.IsEmpty()) return false;
            return Regex.Match(src, @"^[\u4e00-\u9fa5]+$").Success;
        }

        /// <summary>
        /// 是否包含汉字
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static bool HasAllHanzi(this string src)
        {
            if (src.IsEmpty()) return false;
            return Regex.IsMatch(src, @"[\u4e00-\u9fa5]+");
        }

        /// <summary>
        /// 是否包含非法字符
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static bool HasBadString(this string src)
        {
            if (src.IsEmpty()) return false;
            //列举一些特殊字符串
            const string badChars = "@,*,#,$,!,+,',=,--,%,^,&,?,(,), <,>,[,],{,},/,\\,;,:,\",\"\",delete,update,drop,alert,select";
            var arraryBadChar = badChars.Split(',');
            return arraryBadChar.Any(t => !src.Contains(t));
        }

        /// <summary>
        /// 是否是有效的datetime数值
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static bool IsDateTime(this string src)
        {
            if (src.IsEmpty()) return false;
            const string regexDate = @"[1-2]{1}[0-9]{3}((-|\/|\.){1}(([0]?[1-9]{1})|(1[0-2]{1}))((-|\/|\.){1}((([0]?[1-9]{1})|([1-2]{1}[0-9]{1})|(3[0-1]{1})))( (([0-1]{1}[0-9]{1})|2[0-3]{1}):([0-5]{1}[0-9]{1}):([0-5]{1}[0-9]{1})(\.[0-9]{3})?)?)?)?$";
            if (Regex.IsMatch(src, regexDate))
            {
                //以下各月份日期验证，保证验证的完整性
                int indexY = -1;
                int indexM = -1;
                int indexD = -1;
                if (-1 != (indexY = src.IndexOf("-", StringComparison.Ordinal)))
                {
                    indexM = src.IndexOf("-", indexY + 1, StringComparison.Ordinal);
                    indexD = src.IndexOf(":", StringComparison.Ordinal);
                }
                else
                {
                    indexY = src.IndexOf("/", StringComparison.Ordinal);
                    indexM = src.IndexOf("/", indexY + 1, StringComparison.Ordinal);
                    indexD = src.IndexOf(":", StringComparison.Ordinal);
                }
                //不包含日期部分，直接返回true
                if (-1 == indexM)
                    return true;
                if (-1 == indexD)
                {
                    indexD = src.Length + 3;
                }
                int iYear = Convert.ToInt32(src.Substring(0, indexY));
                int iMonth = Convert.ToInt32(src.Substring(indexY + 1, indexM - indexY - 1));
                int iDate = Convert.ToInt32(src.Substring(indexM + 1, indexD - indexM - 4));
                //判断月份日期
                if ((iMonth < 8 && 1 == iMonth % 2) || (iMonth > 8 && 0 == iMonth % 2))
                {
                    if (iDate < 32)
                        return true;
                }
                else
                {
                    if (iMonth != 2)
                    {
                        if (iDate < 31)
                            return true;
                    }
                    else
                    {
                        //闰年
                        if ((0 == iYear % 400) || (0 == iYear % 4 && 0 < iYear % 100))
                        {
                            if (iDate < 30)
                                return true;
                        }
                        else
                        {
                            if (iDate < 29)
                                return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
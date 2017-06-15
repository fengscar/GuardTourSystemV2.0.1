using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Utils
{
    public class ByteConverter
    {
        private const string LOG = "ByteConverter";

        #region 时间转换
        public static DateTime GetTimeOfMinute(byte[] Datas)
        {
            var year = Convert.ToInt32(Datas[0]);
            var mon = Convert.ToInt32(Datas[1]);
            var day = Convert.ToInt32(Datas[2]);
            var hour = Convert.ToInt32(Datas[3]);
            var min = Convert.ToInt32(Datas[4]);
            return new DateTime(year + 2000, mon, day, hour, min, 0);
        }

        public static DateTime GetTimeOfSecond(byte[] Datas)
        {
            var res = GetTimeOfMinute(Datas);
            return res.AddSeconds(Convert.ToInt32(Datas[5]));
        }

        public static byte[] GetTimeOfSecond(DateTime time)
        {
            var year = Convert.ToByte(time.Year - 2000);
            var mon = Convert.ToByte(time.Month);
            var day = Convert.ToByte(time.Day);
            var hour = Convert.ToByte(time.Hour);
            var min = Convert.ToByte(time.Minute);
            var sec = Convert.ToByte(time.Second);

            return new byte[] { year, mon, day, hour, min, sec };
        }

        public static byte[] GetTimeOfMinute(DateTime time)
        {
            var year = Convert.ToByte(time.Year - 2000);
            var mon = Convert.ToByte(time.Month);
            var day = Convert.ToByte(time.Day);
            var hour = Convert.ToByte(time.Hour);
            var min = Convert.ToByte(time.Minute);

            return new byte[] { year, mon, day, hour, min, 0 };
        }
        #endregion

        #region Byte[] String 互转
        public static string ByteToString(byte[] InBytes, string divide)
        {
            if (InBytes == null)
            {
                return null;
            }
            string StringOut = "";
            foreach (byte InByte in InBytes)
            {
                StringOut = StringOut + String.Format("{0:X2}" + divide, InByte);
            }
            return StringOut;
        }
        public static string ByteToString(byte[] InBytes)
        {
            return ByteToString(InBytes, " ");
        }

        //将2个字节的钮号转换成 byte[2] 格式 
        public static byte[] GetCardBytes(string hexString)
        {
            if (hexString == null || hexString.Length != 4)
            {
                DBug.w(LOG, "获取16进制钮号出错,当前钮号并不是4位");
                return new byte[] { 0xFF, 0xFF };
            }
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }
        #endregion




        #region 中文到GBK的转码
        /// <summary>
        /// 得到 巡检员姓名的 byte[], 固定为10个字节,不足10字节的用0xFF填充
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static byte[] GetNameBytes(string name)
        {
            return GetGbkCode(name, 10);
        }
        /// <summary>
        /// 得到 地点名称的 GBK byte[], 固定为32个字节,不足的使用 0xFF填充
        /// </summary>
        /// <param name="place"></param>
        /// <returns></returns>
        public static byte[] GetPointBytes(string point)
        {
            return GetGbkCode(point, 32);
        }

        /// <summary>
        ///  得到 字符串 str 的GBK 编码( 英文将转成国际扩展码格式的2个字节)
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="strLength">返回的byte数组的最大长度,超过则忽略,不足则用0xFF补齐</param>
        /// <returns></returns>
        public static byte[] GetGbkCode(string str, int maxLength)
        {
            byte[] result = new byte[maxLength];
            for (int i = 0; i < maxLength / 2; i++)
            {
                // 如果字符串中 还有未转码的 
                if (str.Length > i)
                {
                    // 不论中英文,都获取2个字节的 GBK code
                    byte[] gbkCode = GetGbkCode(str[i]);
                    result[i * 2] = gbkCode[0];
                    result[i * 2 + 1] = gbkCode[1];
                }
                else
                {
                    // 不足10字节的用 0xFF填充
                    result[i * 2] = 0xFF;
                    result[i * 2 + 1] = 0xFF;
                }
            }
            return result;
        }

        //返回 2个字节的 GBK码 .
        // 如果是中文, 直接转码
        // 如果是英文, 使用8*16国际扩展字符
        private static byte[] GetGbkCode(char c)
        {
            var code = Encoding.GetEncoding("GBK").GetBytes(new char[] { c });
            if (code.Length == 2)
            { // 是中文...直接返回
                return code;
            }
            // 是英文.. 
            else
            {
                var res = new byte[2];
                res[0] = 0xAA;
                // 得到连经理发来的国际扩展表中的字符'0' 和ASCII码表中字符'0' 的差值..( 两张表字符顺序一样)
                int offset = 0xB0 - (int)'0';

                int charASCIIvalue = (int)c;
                // 如果无法识别,转换为 '?'
                if (charASCIIvalue > (int)'~' || charASCIIvalue < (int)' ')
                {
                    charASCIIvalue = (int)'?';
                }
                res[1] = (byte)(charASCIIvalue + offset);
                return res;
            }
        }

        /// <summary>
        ///  将 从巡检机读上来的数据 转成姓名..
        /// </summary>
        /// <param name="nameBytes"></param>
        /// <returns></returns>
        public static string GetNameString(byte[] nameBytes)
        {
            return GetGbkString(nameBytes, 5);
        }
        public static string GetPointString(byte[] pointBytes)
        {
            return GetGbkString(pointBytes,16);
        }

        public static string GetGbkString(byte[] codes, int strLength)
        {
            if (codes == null || codes.Length != strLength*2)
            {
                return null;
            }
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < strLength; i++)
            {
                var b1 = codes[i * 2];
                var b2 = codes[i * 2 + 1];
                if (b1 == 0xFF && b2 == 0xFF) //表示是填充字符 , 直接返回
                {
                    break;
                }
                var str = GetGbkString(new byte[] { b1, b2 });
                sb.Append(str);
            }
            return sb.ToString();
        }


        //将2个字节 转成 字母或 汉字
        private static string GetGbkString(byte[] codes)
        {
            if (codes[0] == 0xAA && codes[1] >= '<' && codes[1] <= ' ') // 是英文
            {
                int offset = 0xB0 - (int)'0';
                // 获取原始 ASICC中的 英文字符
                var enChar= (char)(codes[1]- offset);
                return new string(new char[]{enChar});
            }
            else //是汉字
            {
                return Encoding.GetEncoding("GBK").GetString(codes);
            }
        }
        #endregion
    }
}

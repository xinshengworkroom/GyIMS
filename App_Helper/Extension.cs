using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace GyIMS.App_Helper
{
    public static class Extension
    {

        public static bool ToBool(this int value)
        {
            bool result = true;
            if (value > 0)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static string Trim2(this string value)
        {
            string result = value;
            if (!value.IsNullOrEmpty())
            {
                result = value.Trim();
            }
            return result;
        }
        /// <summary>
        /// 将日期转化为字符串类型
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultTime"></param>
        /// <returns></returns>
        public static string DateToString(this DateTime value, string defaultTime)
        {
            string result = defaultTime;
            if (value != null && value.IsDaylightSavingTime())
            {
                result = value.ToString();
            }
            return result;
        }
        /// <summary>
        /// 判断时间是否有效
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultTime"></param>
        /// <returns></returns>
        public static DateTime ToDate(this DateTime value, DateTime defaultTime)
        {
            DateTime result = value;
            string a = value.ToString();
            if (a == "0001/1/1 0:00:00")//表示时间为空
            {
                result = defaultTime;
            }
            return result;
        }
        public static string ToString2(this object value)
        {
            string result = "";
            if (value != null)
            {
                result = value.ToString();
            }
            return result;
        }
        public static string ToString2(this object value, string defaultValue)
        {
            string result = defaultValue;
            if (value != null)
            {
                result = value.ToString();
            }
            return result;
        }
        public static int ToInt(this string value)
        {
            return ToInt(value, 0);
        }

        public static int ToInt(this string value, int defaultValue)
        {
            int result = 0;
            bool success = Int32.TryParse(value, out result);
            if (!success)
            {
                result = defaultValue;
            }
            return result;
        }
        public static double ToDouble(this string value)
        {
            return ToDoule(value, 0.0);
        }

        public static double ToDoule(this string value, double defaultValue)
        {
            double result = 0;
            bool success = Double.TryParse(value, out result);
            if (!success)
            {
                result = defaultValue;
            }
            return result;
        }

        public static bool ToBool(this string value)
        {
            bool result = false;
            bool.TryParse(value, out result);
            return result;
        }

        public static DateTime ToDateTime(this string value)
        {
            return ToDateTime(value, DateTime.Now);
        }

        public static DateTime ToDateTime(this string value, DateTime defaultValue)
        {
            DateTime result;
            DateTime.TryParse(value, out result);
            return result == DateTime.MinValue ? defaultValue : result;
        }
        /// <summary>
        /// 转换可空日期
        /// </summary>
        public static DateTime ToDateDefault(this object data)
        {
            if (data == null)
                return DateTime.MinValue;
            DateTime result;
            return DateTime.TryParse(data.ToString(), out result) ? result : DateTime.MinValue;
        }

        /// <summary>
        /// 转换为可空日期
        /// </summary>
        public static DateTime? ToDateOrNull(this object data)
        {
            if (data == null)
                return null;
            DateTime result;
            bool isValid = DateTime.TryParse(data.ToString(), out result);
            if (isValid)
                return result;
            return null;
        }
        /// <summary>
        /// 转换为可空数字
        /// </summary>
        public static Decimal? ToDecimalOrNull(this object data)
        {
            if (data == null)
                return null;
            Decimal result;
            bool isValid = Decimal.TryParse(data.ToString(), out result);
            if (isValid)
                return result;
            return null;
        }
        /// <summary>
        /// 转换为有默认数字
        /// </summary>
        public static Decimal ToDecimalDefault(this object data, decimal fault)
        {
            if (data == null)
                return fault;
            Decimal result;
            return Decimal.TryParse(data.ToString(), out result) ? result : fault;
        }
        /// <summary>
        /// 转换为默认为0
        /// </summary>
        public static Decimal ToDecimalDZero(this object data)
        {
            if (data == null)
                return 0;
            Decimal result;
            return Decimal.TryParse(data.ToString(), out result) ? result : 0;
        }
        public static Decimal ChangeToDecimal(this string strData)
        {
            Decimal dData = 0.0M;
            if (strData.Contains("E"))
            {
                dData = Convert.ToDecimal(Decimal.Parse(strData.ToString(), System.Globalization.NumberStyles.Float));
            }
            else
            {
                dData = Convert.ToDecimal(strData);
            }
            return dData;
        }
        /// <summary>
        /// 检测数字是否正常
        /// </summary>
        public static bool CheakDecimal(this object data)
        {
            if (data == null)
                return false;
            Decimal result;
            return Decimal.TryParse(data.ToString(), out result);
        }
        /// <summary>
        /// 检测日期是否正常
        /// </summary>
        public static bool CheakDateTime(this object data)
        {
            if (data == null)
                return false;
            DateTime result;
            return DateTime.TryParse(data.ToString(), out result);
        }
        public static string ToSHA1(this string value)
        {
            string result = string.Empty;
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] array = sha1.ComputeHash(Encoding.Unicode.GetBytes(value));
            for (int i = 0; i < array.Length; i++)
            {
                result += array[i].ToString("x2");
            }
            return result;
        }



        public static T CopyToEntity<T>(this object entity) where T : new()
        {
            var convertProperties = TypeDescriptor.GetProperties(typeof(T)).Cast<PropertyDescriptor>();
            var entityProperties = TypeDescriptor.GetProperties(entity).Cast<PropertyDescriptor>();

            var convert = new T();

            foreach (var entityProperty in entityProperties)
            {
                var property = entityProperty;
                var convertProperty = convertProperties.FirstOrDefault(prop => prop.Name == property.Name && prop.PropertyType == property.PropertyType);
                if (convertProperty != null)
                {
                    object val = entityProperty.GetValue(entity);
                    if (val != null)
                        convertProperty.SetValue(convert, val);
                    //convertProperty.SetValue(convert, Convert.ChangeType(val, convertProperty.PropertyType));
                }
            }

            return convert;
        }


        //以|分开来取name|code
        public static void get_split(string strSplit, out string code, out string name)
        {
            if (!string.IsNullOrEmpty(strSplit))
            {
                string[] strArray = strSplit.Split('|');
                code = strArray[1];
                name = strArray[0];
            }
            else
            {
                code = "";
                name = "";
            }

        }

        //启禁用
        public static string getEnabled(string enabled)
        {
            string db_enabled = string.Empty;
            if (enabled == "1")
            {
                db_enabled = "0";
            }
            else
            {
                db_enabled = "1";
            }
            return db_enabled;
        }

        /// <summary>
        /// 获得字符串中开始和结束字符串中间的值
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="start">开始</param>
        /// <param name="end">结束</param>
        /// <returns></returns> 
        public static string GetBaseCode(string str) //截取指定文本，和易语言的取文本中间差不多
        {
            string restr = "";
            try //异常捕捉
            {

                if (!string.IsNullOrEmpty(str))
                {
                    str.Replace("（", "(").Replace("）", ")");
                    var kn = str.IndexOf("(");
                    var jn = str.IndexOf(")");
                    return str.Substring(kn + 1, jn - kn - 1);
                }
                return restr;
            }
            catch //如果发现未知的错误，比如上面的代码出错了，就执行下面这句代码
            {
                return restr; //返回空字符串
            }
        }
    }
}
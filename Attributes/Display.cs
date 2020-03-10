using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace GyIMS.Attributes
{
    public class Display : Attribute
    {
        private string _code;
        private string _briefname;
        private string _fullname;

        public Display(string code, string briefname, string fullname)
        {
            this._code = code;
            this._briefname = briefname;
            this._fullname = fullname;
        }

        public string Code
        {
            get
            {
                return _code;
            }
            set
            {
                this._code = value;
            }

        }
        public string BriefName
        {
            get
            {
                return _briefname;
            }
            set
            {
                this._briefname = value;
            }
        }
        public string FullName
        {
            get
            {
                return _fullname;
            }
            set
            {
                this._fullname = value;
            }
        }


        /// <summary>
        /// 获取枚举值的简称
        /// </summary>
        /// <param name="_enum"></param>
        /// <returns></returns>
        public static string GetEnumBrief(System.Enum _enum)
        {

            Type type = _enum.GetType();
            FieldInfo fd = type.GetField(_enum.ToString());
            if (fd == null) return string.Empty;
            object[] attrs = fd.GetCustomAttributes(typeof(Display), false);
            string name = string.Empty;
            foreach (Display attr in attrs)
            {
                name = attr._briefname;
            }
            return name;
        }

    }
}
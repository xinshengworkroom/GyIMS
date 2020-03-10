using GyIMS.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GyIMS.Enums
{
    public enum TypeEnum
    {
        [Display("1", "硬件", "硬件")]
        Hard = 0,
        [Display("2", "网络", "网络")]
        Net = 1,
        [Display("3", "邮件", "邮件")]
        Mail = 2,
        [Display("4", "业务系统", "业务系统")]
        EPR = 3,
        [Display("5", "其它", "其它")]
        Other = 4
    }
}
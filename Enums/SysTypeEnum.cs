using GyIMS.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GyIMS.Enums
{
    public enum SysTypeEnum
    {
        [Display("001", "OA系统", "OA系统")]
        OA系统 = 001,
        [Display("002", "TMS系统", "TMS系统")]
        TMS系统 = 002,
        [Display("003", "WMS系统", "WMS系统")]
        WMS系统 = 003,
        [Display("004", "关务系统", "关务系统")]
        关务系统 = 004,
        [Display("005", "用友NC系统", "用友NC系统")]
        用友NC系统 = 005,
        [Display("006", "瀚宇系统", "瀚宇系统")]
        瀚宇系统 = 006,
        [Display("007", "无问题", "无问题")]
        无问题 = 007
    }
}
using GyIMS.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GyIMS.Enums
{
    public enum RequestSysEnum
    {
        [Display("001", "车队系统", "车队系统")]
        One = 001,
        [Display("002", "瀚宇系统", "瀚宇系统")]
        Two = 002,
        [Display("003", "仓储系统", "仓储系统")]
        Three = 003,
        [Display("004", "TMS系统", "TMS系统")]
        Four = 004,
        [Display("005", "OA系统", "OA系统")]
        Five = 005,
        [Display("006", "关务系统", "关务系统")]
        Six = 006,
        [Display("007", "报关系统", "报关系统")]
        Seven = 007
    }
}
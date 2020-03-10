using GyIMS.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GyIMS.Enums
{
    public enum MaintenanceStatusEnum
    {
        [Display("001", "草稿", "草稿")]
        One = 001,
        [Display("002", "已申请", "已申请")]
        Two = 002,
        [Display("003", "处理中", "处理中")]
        Three = 003,
        [Display("004", "已处理", "已处理")]
        Four = 004,
        [Display("005", "已结案", "已结案")]
        Five = 005,
        [Display("006", "已退回", "已退回")]
        Six = 006,
        [Display("007", "已作废", "已作废")]
        Seven = 007,
        [Display("008", "已挂起", "已挂起")]
        Eight = 008,
        [Display("009", "已接单", "已接单")]
        Nine = 009
    }
}
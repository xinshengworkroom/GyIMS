using GyIMS.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GyIMS.Enums
{
    public enum BizTypeEnum
    {
        [Display("Maintenance", "运维申请单", "运维申请单")]
        Maintenance = 0,

        [Display("RequestForm", "需求申请单", "需求申请单")]
        RequestForm = 1
    }
}
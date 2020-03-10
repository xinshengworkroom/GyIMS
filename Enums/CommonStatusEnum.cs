using GyIMS.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GyIMS.Enums
{
    public enum CommonStatusEnum
    {
        [Display("NO", "无效", "无效")]
        Disabled = 0,

        [Display("YES", "正常", "正常")]
        Able = 1
    }
}
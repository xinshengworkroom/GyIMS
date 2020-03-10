using GyIMS.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GyIMS.Enums
{
    public enum IsITStatusEnum
    {
        [Display("NO", "不是", "不是")]
        Disabled = 0,

        [Display("YES", "是", "是")]
        Able = 1
    }
}
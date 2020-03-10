using GyIMS.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GyIMS.Enums
{
    public enum MessageEnum
    {
        [Display("YES", "已阅", "已阅")]
        Able=1,

        [Display("NO", "未读", "未读")]
        Disabled =0
    }
}
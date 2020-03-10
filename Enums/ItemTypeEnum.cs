using GyIMS.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GyIMS.Enums
{
    public enum ItemTypeEnum
    {
        [Display("000", "主单", "主单")]
        Self = 0,
        [Display("001", "清单", "清单")]
        List = 1,
        [Display("002", "日志", "日志")]
        Log = 2,
        [Display("003", "实付", "实付")]
        Out = 3,
        [Display("004", "实收", "实收")]
        In = 4,
        [Display("005", "附件", "附件")]
        Attachment = 5
    }
}
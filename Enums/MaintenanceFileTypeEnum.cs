using GyIMS.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GyIMS.Enums
{
    public enum MaintenanceFileTypeEnum
    {
        [Display("NO", "问题附件", "问题附件")]
        Disabled = 0,

        [Display("YES", "答疑附件", "答疑附件")]
        Able = 1
    }
}
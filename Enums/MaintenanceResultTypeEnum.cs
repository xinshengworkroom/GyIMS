using GyIMS.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GyIMS.Enums
{
    public enum MaintenanceResultTypeEnum
    {
        [Display("1", "已解决", "已解决")]
        One = 1,
        [Display("2", "搁置", "搁置")]
        Two = 2,
        [Display("3", "持续跟进", "持续跟进")]
        Three = 3,
        [Display("3", "无法解决", "无法解决")]
        Four = 4,
        
    }
}
using GyIMS.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GyIMS.Enums
{
    public enum TaskStatusEnum
    {
        [Display("1", "已指派", "已指派")]
        One = 0,
        [Display("2", "已接受", "已接受")]
        Two = 1,
        [Display("3", "执行中", "执行中")]
        Three = 2,
        [Display("4", "已完成", "已完成")]
        Four = 3,
        [Display("5", "延期", "延期")]
        Five = 4,
        [Display("5", "已取消", "已取消")]
        Six = 5
    }
}
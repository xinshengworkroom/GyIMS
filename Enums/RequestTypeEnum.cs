using GyIMS.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GyIMS.Enums
{
    public enum RequestTypeEnum
    {
        [Display("001", "需求文档", "需求文档")]
        One = 001,
        [Display("002", "设计文档", "设计文档")]
        Two = 002,
        [Display("003", "任务附件", "任务附件")]
        Three = 003,
        [Display("004", "操作手册", "操作手册")]
        Four = 004,
        [Display("005", "SOP说明", "SOP说明")]
        Five = 005,
        [Display("006", "BUG截图", "BUG截图")]
        Six = 006
    }
}
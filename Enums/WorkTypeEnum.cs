using GyIMS.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GyIMS.Enums
{
    public enum WorkTypeEnum
    {
        [Display("001", "需求分析", "需求分析")]
        One = 001,
        [Display("002", "详细设计", "详细设计")]
        Two = 002,
        [Display("003", "功能开发", "功能开发")]
        Three = 003,
        [Display("004", "测试修改", "测试修改")]
        Four = 004,
        [Display("005", "部署实施", "部署实施")]
        Five = 005
        
    }
}
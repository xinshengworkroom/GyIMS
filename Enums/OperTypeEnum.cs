using GyIMS.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GyIMS.Enums
{
    public enum OperTypeEnum
    {
        [Display("001", "新增", "新增")]
        Create = 1,
        [Display("002", "修改", "修改")]
        Update = 2,
        [Display("003", "删除", "删除")]
        Delete = 3,
        [Display("004", "启用", "启用")]
        Able = 4,
        [Display("005", "禁用", "禁用")]
        Disable = 5,

        Default = 0
    }
}
using GyIMS.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GyIMS.Enums
{
    public enum SeqGroupTypeEnum
    {
        [Display("001", "Year", "年")]
        Year = 1,
        [Display("002", "Month", "月")]
        Month = 2,
        [Display("003", "Day", "日")]
        Day = 3
    }
}
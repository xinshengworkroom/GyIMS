using GyIMS.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GyIMS.Enums
{
    public enum RegionEnum
    {
        [Display("000", "事业一处", "事业一处")]
        One = 0,
        [Display("001", "事业二处", "事业二处")]
        Two = 1,
        [Display("002", "事业三处", "事业三处")]
        Three = 2,
        [Display("003", "人事", "人事")]
        Four = 3,
        [Display("004", "行政", "行政")]
        Five = 4,
        [Display("005", "财务", "财务")]
        Six = 5,
        [Display("006", "结算", "结算")]
        Seven = 6,
        [Display("007", "采购", "采购")]
        Eight = 7,
    }
}
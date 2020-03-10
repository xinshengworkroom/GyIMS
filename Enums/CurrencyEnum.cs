using GyIMS.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GyIMS.Enums
{
    public enum CurrencyEnum
    {
        [Display("110", "HKD", "港币")]
        HKD = 110,
        [Display("132", "SGD", "新加坡元")]
        SGD = 132,
        [Display("142", "CNY", "人民币")]
        CNY = 142,
        [Display("300", "EUR", "欧元")]
        EUR = 300,
        [Display("502", "USD", "美元")]
        USD = 502



    }
}
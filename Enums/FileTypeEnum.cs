using GyIMS.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GyIMS.Enums
{
    public enum FileTypeEnum
    {
        [Display("001", "GIF", "GIF")]
        GIF = 1,
        [Display("002", "JPG", "JPG")]
        JPG = 2,
        [Display("003", "BMP", "BMP")]
        BMP = 3
    }
}
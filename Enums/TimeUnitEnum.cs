using GyIMS.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GyIMS.Enums
{
    public enum TimeUnitEnum
    {
        [Display("001", "人/天", "人/天")]
        One = 001,
        [Display("002", "人/时", "人/时")]
        Two = 002,
        [Display("003", "人/分", "人/分")]
        Three = 003
    }
}
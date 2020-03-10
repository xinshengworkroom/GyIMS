using GyIMS.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GyIMS.Enums
{
    public enum RankEnum
    {
        [Display("001", "1级", "1级")]
        One = 1,
        [Display("002", "2级", "2级")]
        Two = 2,
        [Display("003", "3级", "3级")]
        Three = 3,
        [Display("003", "4级", "4级")]
        Four = 3
    }
}
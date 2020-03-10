using GyIMS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GyIMS.Interfaces
{
    public interface ICommonStatus
    {
        CommonStatusEnum Status { get; }
        DateTime? CreateDate { get; }
        string CreatePerson { get; }
        DateTime? UpdateDate { get; }
        string UpdatePerson { get; }
        string Summary { get; }
    }
}
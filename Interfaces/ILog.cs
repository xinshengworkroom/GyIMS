using GyIMS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GyIMS.Interfaces
{
    public interface ILog : IUnique
    {
        OperTypeEnum OperType { get; }
        DateTime? CreateDate { get; }
        string CreatePerson { get; }
        string Summary { get; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GyIMS.Models
{
    public class PageView
    {
        public int total { get; set; }
        public IQueryable<object> rows { get; set; }
    }
}
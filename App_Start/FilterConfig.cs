using GyIMS.Attributes;
using System.Web;
using System.Web.Mvc;

namespace GyIMS
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new WebAuthorizeAttribute());
        }
    }
}

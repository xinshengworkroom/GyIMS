using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace GyIMS.Attributes
{
    public class WebAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("HttpContext");
            }
            return base.AuthorizeCore(httpContext);
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {

            if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) ||
                filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
            {               
                return;
            }

            if (!WebContext.Current.IsAuthenticated)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())//是否为ajax请求
                {
                    filterContext.Result = new JsonResult()
                    {
                        Data = new { Message = "登录超时"},
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                    return;
                }
                filterContext.Result = new RedirectResult(FormsAuthentication.LoginUrl);
                return;
            }

        }
    }
}
using GyIMS.Models;
using System;
using System.Web;

namespace GyIMS
{
    public class WebContext
    {
        public WebContext(HttpContext context)
        {

        }

        [ThreadStatic]
        private static WebContext _Context;
        public static WebContext Current
        {
            get
            {
                if (_Context == null)
                {
                    _Context = new WebContext(HttpContext.Current);
                }
                return _Context;
            }
        }

        public User SessionUser
        {
            get
            {
                return HttpContext.Current.Session["SessionUser"] as User;
            }
        }

        public void LogIn(User user)
        {
            HttpContext.Current.Session["SessionUser"] = user;
            HttpContext.Current.Session.Timeout = 10;
        }



        public void LogOut()
        {

            HttpContext.Current.Session.Clear();
        }

        public bool IsAuthenticated
        {
            get { return SessionUser != null; }
        }


        

        
    }
}
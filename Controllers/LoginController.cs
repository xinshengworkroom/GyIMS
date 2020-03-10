using GyIMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GyIMS.Controllers
{
    public class LoginController : Controller
    {
        private DBContext db = new DBContext();


        //GET ://LoginIndex
        [AllowAnonymous]
        public ActionResult LoginIndex()
        {
            return View();
        }


        
        [AllowAnonymous]
        public ActionResult CheckLoginUserIsValid()
        {
            string userName = Request["LoginUserName"] != "" ? Request["LoginUserName"] : String.Empty; ;
            string passWord = Request["LoginPassWord"] != "" ? Request["LoginPassWord"] : String.Empty;
            if (!String.IsNullOrEmpty(userName) && !String.IsNullOrEmpty(passWord))
            {
                try
                {
                    User user = db.Users.Where(item => item.Name == userName).Where(item => item.Password == passWord).First();
                    if (!String.IsNullOrEmpty(user.Code))
                    {
                        WebContext.Current.LogIn(user);
                        return this.Json(new { success = true }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return this.Json(new { success = false, err = "账户信息填写错误" }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    return this.Json(new { success = false, err = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return this.Json(new { success = false, err = "账户/密码输入不能为空" }, JsonRequestBehavior.AllowGet);
            }
        }



        /// <summary>
        /// 初始化登录信息
        /// </summary>
        /// <param name="user"></param>
        private void InitLogin(User user)
        {
            Login login = new Login();
            login.LoginUser = user;
            var queryRoles = from role in db.Roles
                             join mapUserRole in db.MapsUserRole on role.ID equals mapUserRole.RoleID
                             where mapUserRole.UserID == user.Code
                             select role;
            //var queryMenus = from menu in db.Menus
            //                 join mapRoleMenu in db.MapsRoleMenu on menu.ID equals mapRoleMenu.MenuID
            //                 join mapUserRole in db.MapsUserRole on mapRoleMenu.RoleID equals mapUserRole.RoleID
            //                 where mapUserRole.UserID == user.ID
            //                 orderby menu.ID ascending
            //                 select menu;

            login.UserRoles = queryRoles.ToList();
            //login.UserMenus = queryMenus.ToList();
            login.IsLogin = true;
            login.LoginInDate = DateTime.Now;
        }


        private ActionResult Login()
        {
            WebContext.Current.LogOut();
            return RedirectToAction("Index", "Menus");
        }
    }
}
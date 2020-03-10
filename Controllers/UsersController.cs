using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using GyIMS.App_Helper;
using GyIMS.Attributes;
using GyIMS.Enums;
using GyIMS.Helper;
using GyIMS.Models;

namespace GyIMS.Controllers
{
    public class UsersController : Controller
    {
        private DBContext db = new DBContext();

        private IUserDal _IUserQuery { get; set; }//= UserContainer.Resolve<IUserDal>();

        public UsersController(IUserDal userDal)
        {
            _IUserQuery = userDal;
        }



        #region 自定义功能函数

        /// <summary>
        /// 获取表头数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetUserData()
        {
            var pageSize = Request["rows"] == "" ? 10 : int.Parse(Request["rows"]);
            var pageNumber = Request["page"] == "" ? 1 : int.Parse(Request["page"]);
            string Name = string.Empty, order = string.Empty;
            if (Request["Name"] != "")
            {
                Name = Request["Name"];
            }
            if (Request["order"] != "")
            {
                order = Request["order"];
            }
            IQueryable<User> users;
            if (!string.IsNullOrEmpty(Name))
            {
                //users = db.Users.OrderBy(x => x.Name).Where(x => x.Name.Contains(Name) && (x.Status == CommonStatusEnum.Able)).Skip((pageNumber - 1) * pageSize).Take(pageSize);
                users = _IUserQuery.GetModelsByPage(pageSize, pageNumber, true, u => u.Name, u => u.Name.Contains(Name) && u.Status == CommonStatusEnum.Able);
            }
            else
            {
                // users = db.Users.OrderBy(x => x.ID).Where(x => x.Status == CommonStatusEnum.Able).Skip((pageNumber - 1) * pageSize).Take(pageSize);
                users = _IUserQuery.GetModelsByPage(pageSize, pageNumber, true, u => u.Name, u => u.Status == CommonStatusEnum.Able);
            }
            var total = users.Count();
            var list = new PageView { rows = users, total = total };
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        
        //GET: //GetUserList
        [HttpGet]
        public ActionResult GetUserList()
        {
            var pageSize = Request["rows"] == "" ? 10 : int.Parse(Request["rows"]);
            var pageNumber = Request["page"] == "" ? 1 : int.Parse(Request["page"]);
            string Name = string.Empty, order = string.Empty;
            if (Request["Name"] != "")
            {
                Name = Request["Name"];
            }
            if (Request["order"] != "")
            {
                order = Request["order"];
            }
            IQueryable<User> users;
            Expression<Func<User, bool>> expression = user => !string.IsNullOrEmpty(user.Name);
            if (!string.IsNullOrEmpty(Name))
            {
                expression = user => user.Name == Name;
            }       

            users = _IUserQuery.GetModelsByPage(pageSize, pageNumber, true, u => u.Name, expression);
            var total = users.Count();
            var list = new PageView { rows = users, total = total };
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 获取用户状态
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetUserStatus()
        {
            List<ComboboxCommon> list = new List<ComboboxCommon>();
            foreach (var item in Enum.GetValues(typeof(CommonStatusEnum)))
            {
                ComboboxCommon com = new ComboboxCommon();
                com.id = (int)item;
                com.text = Display.GetEnumBrief((CommonStatusEnum)item);
                list.Add(com);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取用户IT状态
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetUserITStatus()
        {
            List<ComboboxCommon> list = new List<ComboboxCommon>();
            foreach (var item in Enum.GetValues(typeof(IsITStatusEnum)))
            {
                ComboboxCommon com = new ComboboxCommon();
                com.id = (int)item;
                com.text = Display.GetEnumBrief((IsITStatusEnum)item);
                list.Add(com);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        // GET: /Users/
        public ActionResult Search(string userName)
        {
            List<User> users = _IUserQuery.GetModels(u => u.Name == userName).ToList();

            return View("Index", users);
        }

        /// <summary>
        /// 通过表单提交
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateByForm(User user)
        {
            try
            {

                int result = _IUserQuery.Add(user);
                return Json(new { Code = 1, Message = "保存成功" }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                return Json(new { Code = -1, Message = ex.Message }, JsonRequestBehavior.DenyGet);
            }

        }


        /// <summary>
        /// 通过表单提交
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditByForm(User user)
        {
            try
            {
                //db.Entry(user).State = EntityState.Modified;
                //db.SaveChanges();

                int result = _IUserQuery.Update(user);
                return Json(new { Code = 1, Message = "保存成功" }, JsonRequestBehavior.DenyGet);
            }
            catch(Exception ex)
            {
                return Json(new { Code = -1, Message = ex.Message }, JsonRequestBehavior.DenyGet);
            }

        }


        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public void DeleteByAjax(string id)
        {
            //try
            //{
            //    int result = _IUserQuery.Delete(id);
            //    return Json(new { Code = 1, Message = "删除成功" }, JsonRequestBehavior.DenyGet);
            //}
            //catch (Exception ex)
            //{
            //    return Json(new { Code = -1, Message = ex.Message }, JsonRequestBehavior.DenyGet);
            //}
            User user = db.Users.Find(id);
            user.Status = CommonStatusEnum.Disabled;
            db.SaveChanges();
        }

        #endregion

        #region 用户授权


        // GET: Users
        public ActionResult GrantIndex()
        {
            return View(db.Users.ToList());
        }


        //GET: //GetUserList
        [HttpGet]
        public ActionResult GetUserListGrant()
        {
            var pageSize = Request["rows"] == "" ? 10 : int.Parse(Request["rows"]);
            var pageNumber = Request["page"] == "" ? 1 : int.Parse(Request["page"]);
            string Name = string.Empty, order = string.Empty;
            if (Request["Name"] != "")
            {
                Name = Request["Name"];
            }
            if (Request["order"] != "")
            {
                order = Request["order"];
            }
            IQueryable<User> users;
            if (!string.IsNullOrEmpty(Name))
            {
                users = db.Users.OrderBy(x => x.Name).Where(x => x.Name.Contains(Name) && (x.Status == CommonStatusEnum.Able)).Skip((pageNumber - 1) * pageSize).Take(pageSize);
            }
            else
            {
                users = db.Users.OrderBy(x => x.ID).Where(x => x.Status == CommonStatusEnum.Able).Skip((pageNumber - 1) * pageSize).Take(pageSize);
            }
            var total = users.Count();
            var list = new PageView { rows = users, total = total };
            return Json(list, JsonRequestBehavior.AllowGet);
        }




        /// <summary>
        /// 角色授权界面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult UserGrant(string id)
        {
            var user = db.Users.Where(x => x.ID == id).FirstOrDefault();
            ViewBag.RoleList = GetRoleListByUserID(id);
            ViewBag.UserID = id;
            return View(user);
        }

        //获取角色菜单
        public ActionResult GetUserRoleList()
        {
            var pageSize = Request["rows"] == "" ? 10 : int.Parse(Request["rows"]);
            var pageNumber = Request["page"] == "" ? 1 : int.Parse(Request["page"]);
            string userID = string.Empty;
            if (Request["UserID"] != "")
            {
                userID = Request["UserID"];
            }
            var userrolelist = GetRoleListByUserID(userID);
            var total = userrolelist.Count();
            var list = new PageView { rows = userrolelist.AsQueryable(), total = total };
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 获取菜单角色授权信息
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        private List<CommonUserRole> GetRoleListByUserID(string userID)
        {
            List<CommonUserRole> rolelist = new List<CommonUserRole>();
            foreach (var role in db.Roles.ToList())
            {
                CommonUserRole item = new CommonUserRole();
                item.User = db.Users.Find(userID);
                item.Role = db.Roles.Find(role.ID);
                if (db.MapsUserRole.Where(x => (x.UserID == userID) && (x.RoleID == role.ID)).ToList().Count > 0)
                {
                    item.IsGrant = true;
                }
                else
                {
                    item.IsGrant = false;
                }
                rolelist.Add(item);
            }
            return rolelist;
        }



        /// <summary>
        /// 通过表单提交
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UserGrantByForm(string userID, string roleidlist)
        {
            string[] roleIDs = Regex.Split(roleidlist, ",", RegexOptions.IgnoreCase);
            User user = db.Users.Find(userID);
            foreach (var item in db.MapsUserRole.Where(x => x.UserID == userID))
            {
                db.MapsUserRole.Remove(item);
            }
            foreach (var roleID in roleIDs)
            {
                MapUserRole mapuserrole = new MapUserRole()
                {
                    RoleID = roleID,
                    UserID = userID,
                    CreateDate = DateTime.Now,
                    CreatePerson = user.CreatePerson
                };
                db.MapsUserRole.Add(mapuserrole);
            }
            db.SaveChanges();

            return Json("授权成功", JsonRequestBehavior.AllowGet);
        }



        #region 取得树形菜单 + ActionResult GetTree(string id)
        /// <summary>
        /// 取得树形菜单
        /// </summary>
        /// <param name="id">用户点击tree时，向后台访问父菜单编号</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetTree(string userID)
        {
            try
            {
                var menus = from menu in db.Menus
                            join mapmenu in db.MapsRoleMenu on menu.ID equals mapmenu.MenuID
                            join maprole in db.MapsUserRole on mapmenu.RoleID equals maprole.RoleID
                            where maprole.UserID == userID
                            select menu;
                var list = menus.ToList();
                var json = new
                {
                    total = list.Count,
                    rows = list
                };
                return Json(json, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }



        #endregion

        #endregion


        #region 框架自动生成

        // GET: Users
        public ActionResult Index()
        {
            return View(_IUserQuery.GetModels(u => u.ID != string.Empty).ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = _IUserQuery.GetModels(u=>u.ID == id).FirstOrDefault();
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {

            User user = new User();
            return View(user);
        }

        // POST: Users/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Code,Name,ChineseName,EnglishName,Password,IsIT,Status,CreateDate,CreatePerson,UpdateDate,UpdatePerson,Summary")] User user)
        {
            if (ModelState.IsValid)
            {
                _IUserQuery.Add(user);
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = _IUserQuery.GetModels(u => u.ID == id).FirstOrDefault();
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Code,Name,ChineseName,EnglishName,Password,IsIT,Status,CreateDate,CreatePerson,UpdateDate,UpdatePerson,Summary")] User user)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(user).State = EntityState.Modified;
                //db.SaveChanges();
                _IUserQuery.Add(user);
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}

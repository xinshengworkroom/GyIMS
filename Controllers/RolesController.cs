using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using GyIMS.App_Helper;
using GyIMS.Attributes;
using GyIMS.Enums;
using GyIMS.Helper;
using GyIMS.Helper.Container;
using GyIMS.Models;

namespace GyIMS.Controllers
{
    public class RolesController : Controller
    {
        private DBContext db = new DBContext();

       // private IRoleDal _IRoleQuery { get; set; }//= UserContainer.Resolve<IUserDal>();
        private IRoleDal _IRoleQuery = BaseContainer.Resolve<IRoleDal, RoleDal>();
        public RolesController(IRoleDal roleDal)
        {
            _IRoleQuery = roleDal;
        }

        #region 自定义功能函数

        //GET: //GetRoleList
        [HttpGet]
        public ActionResult GetRoleList()
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
            IQueryable<Role> roles;
            if (!string.IsNullOrEmpty(Name))
            {
                //users = db.Users.OrderBy(x => x.Name).Where(x => x.Name.Contains(Name) && (x.Status == CommonStatusEnum.Able)).Skip((pageNumber - 1) * pageSize).Take(pageSize);
                roles = _IRoleQuery.GetModelsByPage(pageSize, pageNumber, true, u => u.Name, u => u.Name.Contains(Name) && u.Status == CommonStatusEnum.Able);
            }
            else
            {
                // users = db.Users.OrderBy(x => x.ID).Where(x => x.Status == CommonStatusEnum.Able).Skip((pageNumber - 1) * pageSize).Take(pageSize);
                roles = _IRoleQuery.GetModelsByPage(pageSize, pageNumber, true, u => u.Name, u => u.Status == CommonStatusEnum.Able);
            }
            var total = roles.Count();
            var list = new PageView { rows = roles, total = total };
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        

        /// <summary>
        /// 获取角色状态
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetRoleStatus()
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

        // GET: /Roles/
        public ActionResult Search(string roleName)
        {
            List<Role> roles = _IRoleQuery.GetModels(u => u.Name == roleName).ToList();

            //List<Role> roles = db.Roles.Where(item => item.Name == roleName).Where(item => item.Status == CommonStatusEnum.Able).ToList();
            return View("Index", roles);
        }

        /// <summary>
        /// 通过表单提交
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateByForm(Role role)
        {
            try
            {

                int result = _IRoleQuery.Add(role);
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
        public ActionResult EditByForm(Role role)
        {
            try
            {
                int result = _IRoleQuery.Update(role);
                return Json(new { Code = 1, Message = "保存成功" }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
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
            Role role = db.Roles.Find(id);
            role.Status = CommonStatusEnum.Disabled;
            db.SaveChanges();
        }


        /// <summary>
        /// 角色授权界面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult RoleGrant(string id)
        {
            var role = db.Roles.Where(x => x.ID == id).FirstOrDefault();
            ViewBag.MenuList = GetMenuListByRoleID(id);
            ViewBag.RoleID = id;
            return View(role);
        }

        //获取角色菜单
        public ActionResult GetRoleMenuList()
        {
            var pageSize = Request["rows"] == "" ? 10 : int.Parse(Request["rows"]);
            var pageNumber = Request["page"] == "" ? 1 : int.Parse(Request["page"]);
            string roleID = string.Empty;
            if (Request["RoleID"] != "")
            {
                roleID = Request["RoleID"];
            }
            var rolemenulist = GetMenuListByRoleID(roleID);
            var total = rolemenulist.Count();
            var list = new PageView { rows = rolemenulist.AsQueryable(), total = total };
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 获取菜单角色授权信息
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        private List<CommonRoleMenu> GetMenuListByRoleID(string roleID)
        {
            List<CommonRoleMenu> menulist = new List<CommonRoleMenu>();
            foreach (var menu in db.Menus.ToList())
            {
                CommonRoleMenu item = new CommonRoleMenu();
                item.Role = db.Roles.Find(roleID);
                item.Menu = db.Menus.Find(menu.ID);
                if (db.MapsRoleMenu.Where(x => (x.RoleID == roleID) && (x.MenuID == menu.ID)).ToList().Count > 0)
                {
                    item.IsGrant = true;
                }
                else
                {
                    item.IsGrant = false;
                }
                menulist.Add(item);
            }
            return menulist;
        }



        /// <summary>
        /// 通过表单提交
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RoleGrantByForm(string roleID, string menuidlist)
        {
            string[] menuIDs = Regex.Split(menuidlist, ",", RegexOptions.IgnoreCase);
            Role role = db.Roles.Find(roleID);
            foreach (var item in db.MapsRoleMenu.Where(x => x.RoleID == roleID))
            {
                db.MapsRoleMenu.Remove(item);
            }
            foreach (var menuID in menuIDs)
            {
                MapRoleMenu maprolemenu = new MapRoleMenu()
                {
                    RoleID = roleID,
                    MenuID = menuID,
                    CreateDate = DateTime.Now,
                    CreatePerson = role.CreatePerson
                };
                db.MapsRoleMenu.Add(maprolemenu);
            }
            db.SaveChanges();

            return Json("授权成功", JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region 框架自动生成
        // GET: Roles
        public ActionResult Index()
        {
            return View(_IRoleQuery.GetModels(u => u.ID != string.Empty).ToList());
        }

        // GET: Roles/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role role = _IRoleQuery.GetModels(u => u.ID == id).FirstOrDefault();
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        // GET: Roles/Create
        public ActionResult Create()
        {
            Role role = new Role();
            return View(role);
        }

        // POST: Roles/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Code,Name,ChineseName,EnglishName,Status,CreateDate,CreatePerson,UpdateDate,UpdatePerson,Summary")] Role role)
        {
            if (ModelState.IsValid)
            {
                _IRoleQuery.Add(role);
                return RedirectToAction("Index");
            }

            return View(role);
        }

        // GET: Roles/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role role = _IRoleQuery.GetModels(u => u.ID == id).FirstOrDefault();
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        // POST: Roles/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Code,Name,ChineseName,EnglishName,Status,CreateDate,CreatePerson,UpdateDate,UpdatePerson,Summary")] Role role)
        {
            if (ModelState.IsValid)
            {
                _IRoleQuery.Update(role);
                return RedirectToAction("Index");
            }
            return View(role);
        }

        // GET: Roles/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role role = db.Roles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Role role = db.Roles.Find(id);
            db.Roles.Remove(role);
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

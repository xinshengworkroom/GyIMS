using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GyIMS.App_Helper;
using GyIMS.Attributes;
using GyIMS.Enums;
using GyIMS.Helper;
using GyIMS.Models;

namespace GyIMS.Controllers
{
    public class MenusController : Controller
    {
        private DBContext db = new DBContext();
        private IMenuDal _IMenuQuery { get; set; }//= UserContainer.Resolve<IUserDal>();

        public MenusController(IMenuDal menuDal)
        {
            _IMenuQuery = menuDal;
        }

        #region 自定义功能函数

        //GET: //GetMenuList
        [HttpGet]
        public ActionResult GetMenuList()
        {
            var pageSize = Request["rows"] == "" ? 10 : int.Parse(Request["rows"]);
            var pageNumber = Request["page"] == "" ? 1 : int.Parse(Request["page"]);
            string Name = string.Empty, order = string.Empty;
            if (Request["Name"] != "")
            {
                Name = Request["Name"];
            }
            IQueryable<Menu> menus;
            if (!string.IsNullOrEmpty(Name))
            {
                menus = _IMenuQuery.GetModelsByPage(pageSize, pageNumber, true, u => u.Name, u => u.Name.Contains(Name) && u.Status == CommonStatusEnum.Able);
            }
            else
            {
                menus = _IMenuQuery.GetModelsByPage(pageSize, pageNumber, true, u => u.Name, u => u.Status == CommonStatusEnum.Able);
            }
            var total = _IMenuQuery.GetModels(u => true).Count();
            var list = new PageView { rows = menus, total = total };
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 获取菜单等级
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetMenuRank()
        {
            List<ComboboxCommon> list = new List<ComboboxCommon>();
            foreach (var item in Enum.GetValues(typeof(MenuRankEnum)))
            {
                ComboboxCommon com = new ComboboxCommon();
                com.id = (int)item;
                com.text = Display.GetEnumBrief((MenuRankEnum)item);
                list.Add(com);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 获取菜单状态
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetMenuStatus()
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

        // GET: /Menus/
        public ActionResult Search(string menuName)
        {
            List<Menu> menus = _IMenuQuery.GetModels(u => u.Name == menuName).ToList();
            return View("Index", menus);
        }

        /// <summary>
        /// 通过表单提交
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateByForm(Menu menu)
        {
            try
            {

                int result = _IMenuQuery.Add(menu);
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
        public ActionResult EditByForm(Menu menu)
        {
            try
            {

                int result = _IMenuQuery.Update(menu);
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
            Menu menu = db.Menus.Find(id);
            menu.Status = CommonStatusEnum.Disabled;
            db.SaveChanges();
        }


        #endregion

        #region 框架自动生成

        // GET: Menus
        public ActionResult Index()
        {
            return View(_IMenuQuery.GetModels(u => u.ID != string.Empty).ToList());
        }

        // GET: Menus/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = _IMenuQuery.GetModels(x => x.ID == id).FirstOrDefault();
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        // GET: Menus/Create
        public ActionResult Create()
        {
            Menu menu = new Menu();
            ViewBag.CreatePerson = new SelectList(db.Users, "ID", "Code");
            ViewBag.UpdatePerson = new SelectList(db.Users, "ID", "Code");
            return View(menu);
        }

        // POST: Menus/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ParentID,Rank,Name,ChineseName,EnglishName,Url,ControllerName,ActionName,Status,CreateDate,CreatePerson,UpdateDate,UpdatePerson,Summary")] Menu menu)
        {
            if (ModelState.IsValid)
            {
                _IMenuQuery.Add(menu);
                return RedirectToAction("Index");
            }

            ViewBag.CreatePerson = new SelectList(db.Users, "ID", "Code", menu.CreatePerson);
            ViewBag.UpdatePerson = new SelectList(db.Users, "ID", "Code", menu.UpdatePerson);
            return View(menu);
        }

        // GET: Menus/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = _IMenuQuery.GetModels(x => x.ID==id).FirstOrDefault();
            if (menu == null)
            {
                return HttpNotFound();
            }
            ViewBag.CreatePerson = new SelectList(db.Users, "ID", "Code", menu.CreatePerson);
            ViewBag.UpdatePerson = new SelectList(db.Users, "ID", "Code", menu.UpdatePerson);
            return View(menu);
        }

        // POST: Menus/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ParentID,Rank,Name,ChineseName,EnglishName,Url,ControllerName,ActionName,Status,CreateDate,CreatePerson,UpdateDate,UpdatePerson,Summary")] Menu menu)
        {
            if (ModelState.IsValid)
            {
                _IMenuQuery.Update(menu);
                return RedirectToAction("Index");
            }
            ViewBag.CreatePerson = new SelectList(db.Users, "ID", "Code", menu.CreatePerson);
            ViewBag.UpdatePerson = new SelectList(db.Users, "ID", "Code", menu.UpdatePerson);
            return View(menu);
        }

        // GET: Menus/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = db.Menus.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        // POST: Menus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Menu menu = db.Menus.Find(id);
            db.Menus.Remove(menu);
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

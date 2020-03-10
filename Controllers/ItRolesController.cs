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
    public class ItRolesController : Controller
    {
        private DBContext db = new DBContext();
        private IItRoleDal _IItRoleQuery { get; set; }//= UserContainer.Resolve<IUserDal>();

        public ItRolesController(IItRoleDal itRoleDal)
        {
            _IItRoleQuery = itRoleDal;
        }

        #region 自定义功能函数

        //GET: //GetItRoleList
        [HttpGet]
        public ActionResult GetItRoleList()
        {
            var pageSize = Request["rows"] == "" ? 10 : int.Parse(Request["rows"]);
            var pageNumber = Request["page"] == "" ? 1 : int.Parse(Request["page"]);
            string name = string.Empty;
            if (Request["Name"] != "")
            {
                name = Request["Name"];
            }
            IQueryable<ItRole> itRoles;
            if (!string.IsNullOrEmpty(name))
            {
                itRoles = _IItRoleQuery.GetModelsByPage(pageSize, pageNumber, true, u => u.Name, u => u.Name.Contains(name) && u.Status == CommonStatusEnum.Able);
            }
            else
            {
                itRoles = _IItRoleQuery.GetModelsByPage(pageSize, pageNumber, true, u => u.Name, u => u.Status == CommonStatusEnum.Able);
            }
            var total = _IItRoleQuery.GetModels(u => true).Count();
            var list = new PageView { rows = itRoles, total = total };
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 获取角色状态
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetItRoleStatus()
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


        


        // GET: /Contacts/
        public ActionResult Search(string itroleName)
        {
            List<ItRole> itRoles = _IItRoleQuery.GetModels(u => u.Name == itroleName).ToList();
            return View("Index", itRoles);
        }

        /// <summary>
        /// 通过表单提交
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateByForm(ItRole itRole)
        {
            try
            {

                int result = _IItRoleQuery.Add(itRole);
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
        public ActionResult EditByForm(ItRole itRole)
        {
            try
            {

                int result = _IItRoleQuery.Update(itRole);
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
            ItRole itRole = db.ItRoles.Find(id);
            itRole.Status = CommonStatusEnum.Disabled;
            db.SaveChanges();
        }


        #endregion

        #region 框架自动生成
        // GET: ItRoles
        public ActionResult Index()
        {
            return View(_IItRoleQuery.GetModels(u => u.ID != string.Empty).ToList());
        }

        // GET: ItRoles/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItRole itRole = _IItRoleQuery.GetModels(U => U.Code == id).FirstOrDefault();
            if (itRole == null)
            {
                return HttpNotFound();
            }
            return View(itRole);
        }

        // GET: ItRoles/Create
        public ActionResult Create()
        {
            ItRole itRole = new ItRole();
            return View(itRole);
        }

        // POST: ItRoles/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Code,Name,DayCost,HourCost,MinuteCost,NumCost,Status")] ItRole itRole)
        {
            if (ModelState.IsValid)
            {
                _IItRoleQuery.Add(itRole);
                return RedirectToAction("Index");
            }

            return View(itRole);
        }

        // GET: ItRoles/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItRole itRole = _IItRoleQuery.GetModels(U => U.ID == id).FirstOrDefault();
            if (itRole == null)
            {
                return HttpNotFound();
            }
            return View(itRole);
        }

        // POST: ItRoles/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Code,Name,DayCost,HourCost,MinuteCost,NumCost,Status")] ItRole itRole)
        {
            if (ModelState.IsValid)
            {
                _IItRoleQuery.Update(itRole);
                return RedirectToAction("Index");
            }
            return View(itRole);
        }

        // GET: ItRoles/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItRole itRole = db.ItRoles.Find(id);
            if (itRole == null)
            {
                return HttpNotFound();
            }
            return View(itRole);
        }

        // POST: ItRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ItRole itRole = db.ItRoles.Find(id);
            db.ItRoles.Remove(itRole);
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

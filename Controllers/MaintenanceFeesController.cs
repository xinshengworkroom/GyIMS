using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GyIMS.App_Helper;
using GyIMS.Enums;
using GyIMS.Helper;
using GyIMS.Models;

namespace GyIMS.Controllers
{
    public class MaintenanceFeesController : Controller
    {
        private DBContext db = new DBContext();

        private IMaintenanceFeeDal _IMaintenanceFeeQuery { get; set; }//= UserContainer.Resolve<IUserDal>();

        public MaintenanceFeesController(IMaintenanceFeeDal maintenanceFeeDal)
        {
            _IMaintenanceFeeQuery = maintenanceFeeDal;
        }

        #region 自定义功能函数
        #region 查询界面

        //GET: //GetMaintenanceFeeList
        [HttpGet]
        public ActionResult GetMaintenanceFeeList()
        {
            var pageSize = Request["rows"] == "" ? 10 : int.Parse(Request["rows"]);
            var pageNumber = Request["page"] == "" ? 1 : int.Parse(Request["page"]);
            string name = string.Empty;
            if (Request["MaintenanceName"] != "")
            {
                name = Request["MaintenanceName"];
            }
            IQueryable<MaintenanceFee> maintenanceFees;
            if (!string.IsNullOrEmpty(name))
            {
                maintenanceFees = _IMaintenanceFeeQuery.GetModelsByPage(pageSize, pageNumber, true, u => u.Handler, u => u.Handler.Contains(name) && u.Status == CommonStatusEnum.Able);
            }
            else
            {
                maintenanceFees = _IMaintenanceFeeQuery.GetModelsByPage(pageSize, pageNumber, true, u => u.Handler, u => u.Status == CommonStatusEnum.Able);
            }
            var total = maintenanceFees.Count();
            var list = new PageView { rows = maintenanceFees, total = total };
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region 新增界面

        /// <summary>
        /// 通过表单提交
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateByForm(MaintenanceFee maintenanceFee)
        {
            try
            {

                int result = _IMaintenanceFeeQuery.Add(maintenanceFee);
                return Json(new { Code = 1, Message = "保存成功" }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                return Json(new { Code = -1, Message = ex.Message }, JsonRequestBehavior.DenyGet);
            }

        }

        #endregion


        #region 修改界面


        /// <summary>
        /// 通过表单提交
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditByForm(MaintenanceFee maintenanceFee)
        {
            try
            {

                int result = _IMaintenanceFeeQuery.Update(maintenanceFee);
                return Json(new { Code = 1, Message = "保存成功" }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                return Json(new { Code = -1, Message = ex.Message }, JsonRequestBehavior.DenyGet);
            }

        }


        #endregion


        #region 删除逻辑


        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public void DeleteByAjax(string id)
        {
            MaintenanceFee maintenanceFee = db.MaintenanceFees.Find(id);
            maintenanceFee.Status = CommonStatusEnum.Disabled;
            db.SaveChanges();
        }


        #endregion

        #region 公共函数

        /// <summary>
        /// 获取状态
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetStatus()
        {
            List<ComboboxCommon> list = CommonHelper.GetCommonStatuses();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取运维单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetMaintenances()
        {
            List<ComboboxCommon> list = CommonHelper.GetMaintenances();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取IT角色
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetITRoles()
        {
            List<ComboboxCommon> list = CommonHelper.GetITRoles();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #endregion

        #region 框架自动生成
        // GET: MaintenanceFees
        public ActionResult Index()
        {
            //var maintenanceFees = db.MaintenanceFees.Include(m => m.ItRole).Include(m => m.Maintenance).Include(m => m.UserCreate).Include(m => m.UserUpdate);
            //var maintenanceFees = db.MaintenanceFees.Where(x => x.Status == CommonStatusEnum.Able).ToList();
            //return View(maintenanceFees.ToList());
            return View(_IMaintenanceFeeQuery.GetModels(u => u.ID != string.Empty).ToList());

        }

        // GET: MaintenanceFees/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MaintenanceFee maintenanceFee = _IMaintenanceFeeQuery.GetModels(u => u.ID ==id).FirstOrDefault();
            if (maintenanceFee == null)
            {
                return HttpNotFound();
            }
            return View(maintenanceFee);
        }

        // GET: MaintenanceFees/Create
        public ActionResult Create()
        {
            ViewBag.ItRoleID = new SelectList(db.ItRoles, "ID", "Code");
            ViewBag.MaintenanceID = new SelectList(db.Maintenances, "ID", "Code");
            ViewBag.CreatePerson = new SelectList(db.Users, "ID", "Code");
            ViewBag.UpdatePerson = new SelectList(db.Users, "ID", "Code");
            MaintenanceFee maintenanceFee = new MaintenanceFee();
            return View(maintenanceFee);
        }

        // POST: MaintenanceFees/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,MaintenanceID,Handler,ItRoleID,BeginTime,EndTime,SpendTime,Status,CreateDate,CreatePerson,UpdateDate,UpdatePerson,Summary")] MaintenanceFee maintenanceFee)
        {
            if (ModelState.IsValid)
            {
                _IMaintenanceFeeQuery.Add(maintenanceFee);
                return RedirectToAction("Index");
            }

            ViewBag.ItRoleID = new SelectList(db.ItRoles, "ID", "Code", maintenanceFee.ItRoleID);
            ViewBag.MaintenanceID = new SelectList(db.Maintenances, "ID", "Code", maintenanceFee.MaintenanceID);
            ViewBag.CreatePerson = new SelectList(db.Users, "ID", "Code", maintenanceFee.CreatePerson);
            ViewBag.UpdatePerson = new SelectList(db.Users, "ID", "Code", maintenanceFee.UpdatePerson);
            return View(maintenanceFee);
        }

        // GET: MaintenanceFees/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MaintenanceFee maintenanceFee = _IMaintenanceFeeQuery.GetModels(u => u.ID == id).FirstOrDefault();
            if (maintenanceFee == null)
            {
                return HttpNotFound();
            }
            ViewBag.ItRoleID = new SelectList(db.ItRoles, "ID", "Code", maintenanceFee.ItRoleID);
            ViewBag.MaintenanceID = new SelectList(db.Maintenances, "ID", "Code", maintenanceFee.MaintenanceID);
            ViewBag.CreatePerson = new SelectList(db.Users, "ID", "Code", maintenanceFee.CreatePerson);
            ViewBag.UpdatePerson = new SelectList(db.Users, "ID", "Code", maintenanceFee.UpdatePerson);
            return View(maintenanceFee);
        }

        // POST: MaintenanceFees/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,MaintenanceID,Handler,ItRoleID,BeginTime,EndTime,SpendTime,Status,CreateDate,CreatePerson,UpdateDate,UpdatePerson,Summary")] MaintenanceFee maintenanceFee)
        {
            if (ModelState.IsValid)
            {
                _IMaintenanceFeeQuery.Update(maintenanceFee);
                return RedirectToAction("Index");
            }
            ViewBag.ItRoleID = new SelectList(db.ItRoles, "ID", "Code", maintenanceFee.ItRoleID);
            ViewBag.MaintenanceID = new SelectList(db.Maintenances, "ID", "Code", maintenanceFee.MaintenanceID);
            ViewBag.CreatePerson = new SelectList(db.Users, "ID", "Code", maintenanceFee.CreatePerson);
            ViewBag.UpdatePerson = new SelectList(db.Users, "ID", "Code", maintenanceFee.UpdatePerson);
            return View(maintenanceFee);
        }

        // GET: MaintenanceFees/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MaintenanceFee maintenanceFee = db.MaintenanceFees.Find(id);
            if (maintenanceFee == null)
            {
                return HttpNotFound();
            }
            return View(maintenanceFee);
        }

        // POST: MaintenanceFees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            MaintenanceFee maintenanceFee = db.MaintenanceFees.Find(id);
            db.MaintenanceFees.Remove(maintenanceFee);
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

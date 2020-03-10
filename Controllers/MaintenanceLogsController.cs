using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GyIMS.Common;
using GyIMS.Helper;
using GyIMS.Helper.Container;
using GyIMS.Models;

namespace GyIMS.Controllers
{
    public class MaintenanceLogsController : Controller
    {
        private DBContext db = new DBContext();
        private IMaintenanceLogDal _IMaintenanceLogDal = BaseContainer.Resolve<IMaintenanceLogDal, MaintenanceLogDal>();


        //GET: //GetMaintenanceList
        [HttpGet]
        public ActionResult GetMaintenanceList(MaintenanceLog maintenanceLog)
        {
            var pageSize = Request["rows"] == "" ? 10 : int.Parse(Request["rows"]);
            var pageNumber = Request["page"] == "" ? 1 : int.Parse(Request["page"]);
            string MaintenanceID = string.Empty;
            if (Request["Name"] != "")
            {
                MaintenanceID = Request["Name"];
            }
            IQueryable<MaintenanceLog> maintenances;
            if (!string.IsNullOrEmpty(MaintenanceID))
            {
                maintenances = _IMaintenanceLogDal.GetModelsByPage(
                    pageSize, pageNumber, true, u => u.ID, u => u.MaintenanceID.Contains(MaintenanceID)).OrderBy(u=>u.MaintenanceID);
            }
            else
            {
                maintenances = _IMaintenanceLogDal.GetModelsByPage(
                    pageSize, pageNumber, true, u => u.ID, u => true).OrderBy(u=>u.MaintenanceID);
            }
            var total = _IMaintenanceLogDal.GetModels(u => true).Count();
            var list = new PageView { rows = maintenances, total = total };
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        public void Export()
        {
            var pageSize = string.IsNullOrEmpty(Request["rows"]) ? 10 : int.Parse(Request["rows"]);
            var pageNumber = string.IsNullOrEmpty(Request["page"]) ? 1 : int.Parse(Request["page"]);
            string MaintenanceID = string.Empty;
            if (Request["Name"] != "")
            {
                MaintenanceID = Request["Name"];
            }

            IQueryable<MaintenanceLog> maintenances;
            if (!string.IsNullOrEmpty(MaintenanceID))
            {
                maintenances = _IMaintenanceLogDal.GetModelsByPage(
                    pageSize, pageNumber, true, u => u.ID, u => u.MaintenanceID.Contains(MaintenanceID)).OrderBy(u => u.MaintenanceID);
            }
            else
            {
                maintenances = _IMaintenanceLogDal.GetModelsByPage(
                    pageSize, pageNumber, true, u => u.ID, u => true).OrderBy(u => u.MaintenanceID);
            }


            ExcelHelper.CreateExcel<MaintenanceLog>(maintenances.ToList(), "运维申请清单信息列表");

        }



        #region 条件查找

        // GET: /MaintenanceLogs/
        public ActionResult Search(string maintenance)
        {
            List<MaintenanceLog> maintenanceLogs = db.MaintenanceLogs.Where(item => item.MaintenanceID == maintenance).ToList();
            return View("Index", maintenanceLogs);
        }

        #endregion


        // GET: MaintenanceLogs
        public ActionResult Index()
        {
            return View(_IMaintenanceLogDal.GetModels(u=>true).ToList());
        }

        // GET: MaintenanceLogs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MaintenanceLog maintenanceLog = db.MaintenanceLogs.Find(id);
            if (maintenanceLog == null)
            {
                return HttpNotFound();
            }
            return View(maintenanceLog);
        }

        // GET: MaintenanceLogs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MaintenanceLogs/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,MaintenanceID,OperType,CreateDate,CreatePerson,Summary")] MaintenanceLog maintenanceLog)
        {
            if (ModelState.IsValid)
            {
                db.MaintenanceLogs.Add(maintenanceLog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(maintenanceLog);
        }

        // GET: MaintenanceLogs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MaintenanceLog maintenanceLog = db.MaintenanceLogs.Find(id);
            if (maintenanceLog == null)
            {
                return HttpNotFound();
            }
            return View(maintenanceLog);
        }

        // POST: MaintenanceLogs/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,MaintenanceID,OperType,CreateDate,CreatePerson,Summary")] MaintenanceLog maintenanceLog)
        {
            if (ModelState.IsValid)
            {
                db.Entry(maintenanceLog).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(maintenanceLog);
        }

        // GET: MaintenanceLogs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MaintenanceLog maintenanceLog = db.MaintenanceLogs.Find(id);
            if (maintenanceLog == null)
            {
                return HttpNotFound();
            }
            return View(maintenanceLog);
        }

        // POST: MaintenanceLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            MaintenanceLog maintenanceLog = db.MaintenanceLogs.Find(id);
            db.MaintenanceLogs.Remove(maintenanceLog);
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
    }
}

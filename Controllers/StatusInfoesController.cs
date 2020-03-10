using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GyIMS.App_Helper;
using GyIMS.Helper;
using GyIMS.Helper.Container;
using GyIMS.Models;

namespace GyIMS.Controllers
{
    public class StatusInfoesController : Controller
    {
        private DBContext db = new DBContext();

        private IStatusInfoDal _IStatusInfoDal= BaseContainer.Resolve<IStatusInfoDal,StatusInfoDal>();

        public StatusInfoesController(IStatusInfoDal IStatusInfoDal)
        {
            _IStatusInfoDal = IStatusInfoDal;
        }
        public ActionResult Index()
        {
            return View(_IStatusInfoDal.GetModels(u => u.ID != string.Empty).ToList());
        }

        #region 通用方法,Jqcombox.js中调用
        public JsonResult GetComboxTypes(StatusInfo _StatusInfoS)
        {
            var list = _IStatusInfoDal.GetModels(u => u.TableName == _StatusInfoS.TableName
                                                   && u.StatusDesc == _StatusInfoS.StatusDesc).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTabStatusID(StatusInfo _StatusInfoS)
        {
            var list = _IStatusInfoDal.GetModels(u => u.TableName  == _StatusInfoS.TableName &&
                                                      u.StatusDesc == _StatusInfoS.StatusDesc &&
                                                      u.StatusNo   == _StatusInfoS.StatusNo).ToList();
            return this.Json(new { success = true, err = list[0].ID }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTabStatusNo(StatusInfo _StatusInfoS)
        {
            try
            {
                if (string.IsNullOrEmpty(_StatusInfoS.ID))
                {
                    return this.Json(new { success = true, err = "" }, JsonRequestBehavior.AllowGet);
                }

                var list = _IStatusInfoDal.GetModels(u => u.ID == _StatusInfoS.ID).ToList();

                return this.Json(new { success = true, err = list[0].StatusNo }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return this.Json(new { success = true, err =ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion 


        public ActionResult GetStatusInfo()
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
            IQueryable<StatusInfo> stattsinfo;
            Expression<Func<StatusInfo, bool>> expression = user => !string.IsNullOrEmpty(user.TableName);
            if (!string.IsNullOrEmpty(Name))
            {
                expression = user => user.TableName == Name;
            }

            stattsinfo = _IStatusInfoDal.GetModelsByPage(pageSize, pageNumber, true, u => u.TableName, expression);
            var total = stattsinfo.Count();
            var list = new PageView { rows = stattsinfo, total = total };
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CreateByForm(StatusInfo statusInfo)
        {
            try
            {
                int result = _IStatusInfoDal.Add(statusInfo);
                return Json(new { Code = 1, Message = "保存成功" }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                return Json(new { Code = -1, Message = ex.Message }, JsonRequestBehavior.DenyGet);
            }
        }



        // GET: StatusInfoes/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StatusInfo statusInfo = db.StatusInfoes.Find(id);
            if (statusInfo == null)
            {
                return HttpNotFound();
            }
            return View(statusInfo);
        }

        // GET: StatusInfoes/Create
        public ActionResult Create()
        {           
            StatusInfo _StatusInfo = new StatusInfo();
            return View(_StatusInfo);
        }

        // POST: StatusInfoes/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,TableName,StatusNo,StatusNameCH,StatusNameEN,StatusDesc,CreateUserId,UpdateUserId,CreateDate,UpdateDate")] StatusInfo statusInfo)
        {
            if (ModelState.IsValid)
            {
                db.StatusInfoes.Add(statusInfo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(statusInfo);
        }

        // GET: StatusInfoes/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StatusInfo statusInfo = db.StatusInfoes.Find(id);
            if (statusInfo == null)
            {
                return HttpNotFound();
            }
            return View(statusInfo);
        }

        // POST: StatusInfoes/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,TableName,StatusNo,StatusNameCH,StatusNameEN,StatusDesc,CreateUserId,UpdateUserId,CreateDate,UpdateDate")] StatusInfo statusInfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(statusInfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(statusInfo);
        }

        // GET: StatusInfoes/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StatusInfo statusInfo = db.StatusInfoes.Find(id);
            if (statusInfo == null)
            {
                return HttpNotFound();
            }
            return View(statusInfo);
        }

        // POST: StatusInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            StatusInfo statusInfo = db.StatusInfoes.Find(id);
            db.StatusInfoes.Remove(statusInfo);
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

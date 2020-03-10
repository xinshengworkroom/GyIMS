using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GyIMS.App_Helper;
using GyIMS.Common;
using GyIMS.Enums;
using GyIMS.Helper;
using GyIMS.Models;

namespace GyIMS.Controllers
{
    public class MaintenanceResultsController : Controller
    {
        private DBContext db = new DBContext();
        private IMaintenanceResultDal _IMaintenanceResultQuery { get; set; }//= UserContainer.Resolve<IUserDal>();

        public MaintenanceResultsController(IMaintenanceResultDal maintenanceResultDal)
        {
            _IMaintenanceResultQuery = maintenanceResultDal;
        }

        #region 自定义功能函数

        #region 查询界面

        //GET: //GetMaintenanceResultList
        [HttpGet]
        public ActionResult GetMaintenanceResultList()
        {
            var pageSize = Request["rows"] == "" ? 10 : int.Parse(Request["rows"]);
            var pageNumber = Request["page"] == "" ? 1 : int.Parse(Request["page"]);
            string name = string.Empty, TxtStatus = string.Empty;
            if (Request["MaintenanceName"] != "")
            {
                name = Request["MaintenanceName"];
            }
            if (Request["txtStatus"] != "") { TxtStatus = Request["txtStatus"]; }

            IQueryable<MaintenanceResult> maintenanceResults;
            if (!string.IsNullOrEmpty(name))
            {
                maintenanceResults = _IMaintenanceResultQuery.GetModelsByPage(pageSize, pageNumber, true, u => u.ID, u => u.MaintenanceID.Contains(name) && u.Status == CommonStatusEnum.Able);
            }
            else
            {
                maintenanceResults = _IMaintenanceResultQuery.GetModelsByPage(pageSize, pageNumber, true, u => u.ID, u => u.Status == CommonStatusEnum.Able);
            }

            
            var total = maintenanceResults.Count();
            var list = new PageView { rows = maintenanceResults.Skip((pageNumber - 1) * pageSize).Take(pageSize), total = total };
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion
        public void Export()
        {
            var pageSize = string.IsNullOrEmpty(Request["rows"]) ? 10 : int.Parse(Request["rows"]);
            var pageNumber = string.IsNullOrEmpty(Request["page"]) ? 1 : int.Parse(Request["page"]);
            string name = string.Empty;
            if (Request["MaintenanceName"] != "")
            {
                name = Request["MaintenanceName"];
            }

            IQueryable<MaintenanceResult> maintenanceResults;
            if (!string.IsNullOrEmpty(name))
            {
                maintenanceResults = _IMaintenanceResultQuery.GetModelsByPage(pageSize, pageNumber, true, u => u.ID, u => u.MaintenanceID.Contains(name) && u.Status == CommonStatusEnum.Able);
            }
            else
            {
                maintenanceResults = _IMaintenanceResultQuery.GetModelsByPage(pageSize, pageNumber, true, u => u.ID, u => u.Status == CommonStatusEnum.Able);
            }


            ExcelHelper.CreateExcel<MaintenanceResult>(maintenanceResults.ToList(), "运维结果信息列表");

        }

        #region 新增界面

        /// <summary>
        /// 通过表单提交
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateByForm(MaintenanceResult maintenanceResult)
        {
            try
            {

                int result = _IMaintenanceResultQuery.Add(maintenanceResult);
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
        public ActionResult EditByForm(MaintenanceResult maintenanceResult)
        {
            try
            {

                int result = _IMaintenanceResultQuery.Update(maintenanceResult);
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
            MaintenanceResult maintenanceResult = db.MaintenanceResults.Find(id);
            maintenanceResult.Status = CommonStatusEnum.Disabled;
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
        /// 获取运维结果类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetResultTypes()
        {
            List<ComboboxCommon> list = CommonHelper.GetResultTypes();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 文件上传 下载

        //[HttpPost]
        //public ActionResult UploadFile(HttpPostedFileBase file)
        //{
        //    var fileName = file.FileName;
        //    var filePath = Server.MapPath(string.Format("~/{0}", "File"));
        //    file.SaveAs(Path.Combine(filePath, fileName));
        //    return View();
        //}

        public ActionResult File(HttpPostedFileBase file)
        {
            string path = Server.MapPath("~/File");
            string filename = Path.Combine(path, file.FileName);
            file.SaveAs(filename);
            //return View(Index);
            return RedirectToAction("Create");
        }
        
        


        public ActionResult DownloadFile()
        {
            //下载文件
            string path = Server.MapPath("~/File/运维答疑附件.docx");
            FileStream fs = new FileStream(path, FileMode.Open);
            return File(fs, "word/docx", "运维答疑附件.docx");



        }

        #endregion

        #region 报表查询

        public ActionResult Search(string id)
        {
            List<MaintenanceResult> maintenanceResults = _IMaintenanceResultQuery.GetModels(u => u.ID == id).ToList();

            return View("Index", maintenanceResults);
        }

        // GET: MaintenanceResults
        public ActionResult SearchIndex()
        {

            return View(_IMaintenanceResultQuery.GetModels(u => u.ID != string.Empty).ToList());

        }

        #endregion

        #endregion


        #region 框架自动生成
        // GET: MaintenanceResults
        public ActionResult Index()
        {
            //var maintenanceResults = db.MaintenanceResults.Include(m => m.Maintenance).Include(m => m.UserCreate).Include(m => m.UserUpdate);
            //return View(maintenanceResults.ToList());
            return View(_IMaintenanceResultQuery.GetModels(u => u.ID != string.Empty).ToList());

        }

        // GET: MaintenanceResults/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MaintenanceResult maintenanceResult = _IMaintenanceResultQuery.GetModels(u => u.ID==id).FirstOrDefault();
            if (maintenanceResult == null)
            {
                return HttpNotFound();
            }
            return View(maintenanceResult);
        }

        // GET: MaintenanceResults/Create
        public ActionResult Create()
        {
            ViewBag.MaintenanceID = new SelectList(db.Maintenances, "ID", "Code");
            ViewBag.CreatePerson = new SelectList(db.Users, "ID", "Code");
            ViewBag.UpdatePerson = new SelectList(db.Users, "ID", "Code");
            MaintenanceResult maintenanceResult = new MaintenanceResult();
            return View(maintenanceResult);
        }

        // POST: MaintenanceResults/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,MaintenanceID,Type,Description,Status,CreateDate,CreatePerson,UpdateDate,UpdatePerson,Summary")] MaintenanceResult maintenanceResult)
        {
            if (ModelState.IsValid)
            {
                _IMaintenanceResultQuery.Add(maintenanceResult);
                return RedirectToAction("Index");
            }

            ViewBag.MaintenanceID = new SelectList(db.Maintenances, "ID", "Code", maintenanceResult.MaintenanceID);
            ViewBag.CreatePerson = new SelectList(db.Users, "ID", "Code", maintenanceResult.CreatePerson);
            ViewBag.UpdatePerson = new SelectList(db.Users, "ID", "Code", maintenanceResult.UpdatePerson);
            return View(maintenanceResult);
        }

        // GET: MaintenanceResults/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MaintenanceResult maintenanceResult = _IMaintenanceResultQuery.GetModels(u => u.ID == id).FirstOrDefault();
            if (maintenanceResult == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaintenanceID = new SelectList(db.Maintenances, "ID", "Code", maintenanceResult.MaintenanceID);
            ViewBag.CreatePerson = new SelectList(db.Users, "ID", "Code", maintenanceResult.CreatePerson);
            ViewBag.UpdatePerson = new SelectList(db.Users, "ID", "Code", maintenanceResult.UpdatePerson);
            return View(maintenanceResult);
        }

        // POST: MaintenanceResults/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,MaintenanceID,Type,Description,Status,CreateDate,CreatePerson,UpdateDate,UpdatePerson,Summary")] MaintenanceResult maintenanceResult)
        {
            if (ModelState.IsValid)
            {
                _IMaintenanceResultQuery.Update(maintenanceResult);
                return RedirectToAction("Index");
            }
            ViewBag.MaintenanceID = new SelectList(db.Maintenances, "ID", "Code", maintenanceResult.MaintenanceID);
            ViewBag.CreatePerson = new SelectList(db.Users, "ID", "Code", maintenanceResult.CreatePerson);
            ViewBag.UpdatePerson = new SelectList(db.Users, "ID", "Code", maintenanceResult.UpdatePerson);
            return View(maintenanceResult);
        }

        // GET: MaintenanceResults/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MaintenanceResult maintenanceResult = db.MaintenanceResults.Find(id);
            if (maintenanceResult == null)
            {
                return HttpNotFound();
            }
            return View(maintenanceResult);
        }

        // POST: MaintenanceResults/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            MaintenanceResult maintenanceResult = db.MaintenanceResults.Find(id);
            db.MaintenanceResults.Remove(maintenanceResult);
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

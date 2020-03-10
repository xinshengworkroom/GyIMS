using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
using GyIMS.App_Helper;
using GyIMS.Enums;
using GyIMS.Models;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace GyIMS.Controllers
{
    public class MaintenanceFilesController : Controller
    {
        private DBContext db = new DBContext();

        #region 自定义功能函数

        #region 查询界面

        //GET: //GetMaintenanceFileList
        [HttpGet]
        public ActionResult GetMaintenanceFileList()
        {
            var pageSize = Request["rows"] == "" ? 10 : int.Parse(Request["rows"]);
            var pageNumber = Request["page"] == "" ? 1 : int.Parse(Request["page"]);
            string name = string.Empty;
            if (Request["MaintenanceName"] != "")
            {
                name = Request["MaintenanceName"];
            }
            IQueryable<MaintenanceFile> maintenanceFiles;
            if (!string.IsNullOrEmpty(name))
            {
                maintenanceFiles = db.MaintenanceFiles.OrderBy(x => x.ID).Where(x => x.Maintenance.Name.Contains(name) && (x.Status == CommonStatusEnum.Able)).Skip((pageNumber - 1) * pageSize).Take(pageSize);
            }
            else
            {
                maintenanceFiles = db.MaintenanceFiles.OrderBy(x => x.ID).Where(x => x.Status == CommonStatusEnum.Able).Skip((pageNumber - 1) * pageSize).Take(pageSize);
            }
            var total = maintenanceFiles.Count();
            var list = new PageView { rows = maintenanceFiles, total = total };
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
        public ActionResult CreateByForm(MaintenanceFile maintenanceFile)
        {
            try
            {
                db.MaintenanceFiles.Add(maintenanceFile);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View(maintenanceFile);
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
        public ActionResult EditByForm(MaintenanceFile maintenanceFile)
        {
            try
            {
                db.Entry(maintenanceFile).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View(maintenanceFile);
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
            MaintenanceFile maintenanceFile = db.MaintenanceFiles.Find(id);
            maintenanceFile.Status = CommonStatusEnum.Disabled;
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
        /// 获取运维信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetMaintenances()
        {
            List<ComboboxCommon> list = CommonHelper.GetMaintenances();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取附件类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetTypes()
        {
            List<ComboboxCommon> list = CommonHelper.GetTypes();
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
            return RedirectToAction("Index");
        }
        public ActionResult FileDownload()
        {
            //下载文件
            string path = Server.MapPath("~/File/运维问题附件.docx");
            FileStream fs = new FileStream(path, FileMode.Open);
            return File(fs, "word/docx", "运维问题附件.docx");

            

        }


        public ActionResult DownloadFile()
        {
            //下载文件
            string path = Server.MapPath("~/File/运维答疑附件.docx");
            FileStream fs = new FileStream(path, FileMode.Open);
            return File(fs, "word/docx", "运维答疑附件.docx");



        }

        #endregion      

        #endregion

        #region 框架自动生成

        // GET: MaintenanceFiles
        public ActionResult Index()
        {
            //var maintenanceFiles = db.MaintenanceFiles.Include(m => m.Maintenance).Include(m => m.UserCreate).Include(m => m.UserUpdate);
            var maintenanceFiles = db.MaintenanceFiles.Where(x => x.Status == CommonStatusEnum.Able).ToList();
            return View(maintenanceFiles.ToList());
        }

        // GET: MaintenanceFiles/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MaintenanceFile maintenanceFile = db.MaintenanceFiles.Find(id);
            if (maintenanceFile == null)
            {
                return HttpNotFound();
            }
            return View(maintenanceFile);
        }

        // GET: MaintenanceFiles/Create
        public ActionResult Create()
        {
            ViewBag.MaintenanceID = new SelectList(db.Maintenances, "ID", "Code");
            ViewBag.CreatePerson = new SelectList(db.Users, "ID", "Code");
            ViewBag.UpdatePerson = new SelectList(db.Users, "ID", "Code");
            MaintenanceFile maintenanceFile = new MaintenanceFile();
            return View(maintenanceFile);
        }

        // POST: MaintenanceFiles/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,MaintenanceID,Type,FileName,FilePath,Status,CreateDate,CreatePerson,UpdateDate,UpdatePerson,Summary")] MaintenanceFile maintenanceFile)
        {
            if (ModelState.IsValid)
            {
                db.MaintenanceFiles.Add(maintenanceFile);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaintenanceID = new SelectList(db.Maintenances, "ID", "Code", maintenanceFile.MaintenanceID);
            ViewBag.CreatePerson = new SelectList(db.Users, "ID", "Code", maintenanceFile.CreatePerson);
            ViewBag.UpdatePerson = new SelectList(db.Users, "ID", "Code", maintenanceFile.UpdatePerson);
            return View(maintenanceFile);
        }

        // GET: MaintenanceFiles/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MaintenanceFile maintenanceFile = db.MaintenanceFiles.Find(id);
            if (maintenanceFile == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaintenanceID = new SelectList(db.Maintenances, "ID", "Code", maintenanceFile.MaintenanceID);
            ViewBag.CreatePerson = new SelectList(db.Users, "ID", "Code", maintenanceFile.CreatePerson);
            ViewBag.UpdatePerson = new SelectList(db.Users, "ID", "Code", maintenanceFile.UpdatePerson);
            return View(maintenanceFile);
        }

        // POST: MaintenanceFiles/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,MaintenanceID,Type,FileName,FilePath,Status,CreateDate,CreatePerson,UpdateDate,UpdatePerson,Summary")] MaintenanceFile maintenanceFile)
        {
            if (ModelState.IsValid)
            {
                db.Entry(maintenanceFile).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaintenanceID = new SelectList(db.Maintenances, "ID", "Code", maintenanceFile.MaintenanceID);
            ViewBag.CreatePerson = new SelectList(db.Users, "ID", "Code", maintenanceFile.CreatePerson);
            ViewBag.UpdatePerson = new SelectList(db.Users, "ID", "Code", maintenanceFile.UpdatePerson);
            return View(maintenanceFile);
        }

        // GET: MaintenanceFiles/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MaintenanceFile maintenanceFile = db.MaintenanceFiles.Find(id);
            if (maintenanceFile == null)
            {
                return HttpNotFound();
            }
            return View(maintenanceFile);
        }

        // POST: MaintenanceFiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            MaintenanceFile maintenanceFile = db.MaintenanceFiles.Find(id);
            db.MaintenanceFiles.Remove(maintenanceFile);
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

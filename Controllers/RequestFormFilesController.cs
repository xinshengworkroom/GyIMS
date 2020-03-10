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
using GyIMS.Enums;
using GyIMS.Models;

namespace GyIMS.Controllers
{
    public class RequestFormFilesController : Controller
    {
        private DBContext db = new DBContext();

        #region 自定义功能函数

        #region 查询界面

        //GET: //GetRequestFormFileList
        [HttpGet]
        public ActionResult GetRequestFormFileList()
        {
            var pageSize = Request["rows"] == "" ? 10 : int.Parse(Request["rows"]);
            var pageNumber = Request["page"] == "" ? 1 : int.Parse(Request["page"]);
            string name = string.Empty;
            if (Request["RequestFormName"] != "")
            {
                name = Request["RequestFormName"];
            }
            IQueryable<RequestFormFile> requestFormFiles;
            if (!string.IsNullOrEmpty(name))
            {
                requestFormFiles = db.RequestFormFiles.OrderBy(x => x.ID).Where(x => x.RequestForm.Name.Contains(name) && (x.Status == CommonStatusEnum.Able)).Skip((pageNumber - 1) * pageSize).Take(pageSize);
            }
            else
            {
                requestFormFiles = db.RequestFormFiles.OrderBy(x => x.ID).Where(x => x.Status == CommonStatusEnum.Able).Skip((pageNumber - 1) * pageSize).Take(pageSize);
            }
            var total = requestFormFiles.Count();
            var list = new PageView { rows = requestFormFiles, total = total };
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
        public ActionResult CreateByForm(RequestFormFile requestFormFile)
        {
            try
            {
                db.RequestFormFiles.Add(requestFormFile);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View(requestFormFile);
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
        public ActionResult EditByForm(RequestFormFile requestFormFile)
        {
            try
            {
                db.Entry(requestFormFile).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View(requestFormFile);
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
            RequestFormFile requestFormFile = db.RequestFormFiles.Find(id);
            requestFormFile.Status = CommonStatusEnum.Disabled;
            db.SaveChanges();
        }


        #endregion

        #region 文件上传 下载
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
            string path = Server.MapPath("~/File/需求问题附件.docx");
            FileStream fs = new FileStream(path, FileMode.Open);
            return File(fs, "word/docx", "需求问题附件.docx");



        }


        public ActionResult DownloadFile()
        {
            //下载文件
            string path = Server.MapPath("~/File/需求答疑附件.docx");
            FileStream fs = new FileStream(path, FileMode.Open);
            return File(fs, "word/docx", "需求答疑附件.docx");



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
            List<ComboboxCommon> list = RequestHelper.GetCommonStatuses();
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 获取需求申请单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetRequestForms()
        {
            List<ComboboxCommon> list = RequestHelper.GetRequestForms();
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 获取需求任务
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetRequestFormTasks()
        {
            List<ComboboxCommon> list = RequestHelper.GetRequestFormTasks();
            return Json(list, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// 获取需求任务明细
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetRequestFormTaskLists()
        {
            List<ComboboxCommon> list = RequestHelper.GetRequestFormTaskLists();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取需求文档类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetRequestTypeStatus()
        {
            List<ComboboxCommon> list = RequestHelper.GetRequestTypeStatus();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion

        #region 框架自动生成
        // GET: RequestFormFiles
        public ActionResult Index()
        {
            //var requestFormFiles = db.RequestFormFiles.Include(r => r.RequestForm).Include(r => r.RequestFormTask).Include(r => r.RequestFormTaskList).Include(r => r.UserCreate).Include(r => r.UserUpdate);
            var requestFormFiles = db.RequestFormFiles.Where(x => x.Status == CommonStatusEnum.Able).ToList();
            return View(requestFormFiles.ToList());
        }

        // GET: RequestFormFiles/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestFormFile requestFormFile = db.RequestFormFiles.Find(id);
            if (requestFormFile == null)
            {
                return HttpNotFound();
            }
            return View(requestFormFile);
        }

        // GET: RequestFormFiles/Create
        public ActionResult Create()
        {
            ViewBag.RequestFormID = new SelectList(db.RequestForms, "ID", "Code");
            ViewBag.RequestFormTaskID = new SelectList(db.RequestFormTasks, "ID", "RequestFormID");
            ViewBag.RequestFormTaskListID = new SelectList(db.RequestFormTaskLists, "ID", "RequestFormID");
            ViewBag.CreatePerson = new SelectList(db.Users, "ID", "Code");
            ViewBag.UpdatePerson = new SelectList(db.Users, "ID", "Code");
            RequestFormFile requestFormFile = new RequestFormFile();
            return View(requestFormFile);
        }

        // POST: RequestFormFiles/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,RequestFormID,RequestFormTaskID,RequestFormTaskListID,Type,FileName,FilePath,Status,CreateDate,CreatePerson,UpdateDate,UpdatePerson,Summary")] RequestFormFile requestFormFile)
        {
            if (ModelState.IsValid)
            {
                db.RequestFormFiles.Add(requestFormFile);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RequestFormID = new SelectList(db.RequestForms, "ID", "Code", requestFormFile.RequestFormID);
            ViewBag.RequestFormTaskID = new SelectList(db.RequestFormTasks, "ID", "RequestFormID", requestFormFile.RequestFormTaskID);
            ViewBag.RequestFormTaskListID = new SelectList(db.RequestFormTaskLists, "ID", "RequestFormID", requestFormFile.RequestFormTaskListID);
            ViewBag.CreatePerson = new SelectList(db.Users, "ID", "Code", requestFormFile.CreatePerson);
            ViewBag.UpdatePerson = new SelectList(db.Users, "ID", "Code", requestFormFile.UpdatePerson);
            return View(requestFormFile);
        }

        // GET: RequestFormFiles/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestFormFile requestFormFile = db.RequestFormFiles.Find(id);
            if (requestFormFile == null)
            {
                return HttpNotFound();
            }
            ViewBag.RequestFormID = new SelectList(db.RequestForms, "ID", "Code", requestFormFile.RequestFormID);
            ViewBag.RequestFormTaskID = new SelectList(db.RequestFormTasks, "ID", "RequestFormID", requestFormFile.RequestFormTaskID);
            ViewBag.RequestFormTaskListID = new SelectList(db.RequestFormTaskLists, "ID", "RequestFormID", requestFormFile.RequestFormTaskListID);
            ViewBag.CreatePerson = new SelectList(db.Users, "ID", "Code", requestFormFile.CreatePerson);
            ViewBag.UpdatePerson = new SelectList(db.Users, "ID", "Code", requestFormFile.UpdatePerson);
            return View(requestFormFile);
        }

        // POST: RequestFormFiles/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,RequestFormID,RequestFormTaskID,RequestFormTaskListID,Type,FileName,FilePath,Status,CreateDate,CreatePerson,UpdateDate,UpdatePerson,Summary")] RequestFormFile requestFormFile)
        {
            if (ModelState.IsValid)
            {
                db.Entry(requestFormFile).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RequestFormID = new SelectList(db.RequestForms, "ID", "Code", requestFormFile.RequestFormID);
            ViewBag.RequestFormTaskID = new SelectList(db.RequestFormTasks, "ID", "RequestFormID", requestFormFile.RequestFormTaskID);
            ViewBag.RequestFormTaskListID = new SelectList(db.RequestFormTaskLists, "ID", "RequestFormID", requestFormFile.RequestFormTaskListID);
            ViewBag.CreatePerson = new SelectList(db.Users, "ID", "Code", requestFormFile.CreatePerson);
            ViewBag.UpdatePerson = new SelectList(db.Users, "ID", "Code", requestFormFile.UpdatePerson);
            return View(requestFormFile);
        }

        // GET: RequestFormFiles/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestFormFile requestFormFile = db.RequestFormFiles.Find(id);
            if (requestFormFile == null)
            {
                return HttpNotFound();
            }
            return View(requestFormFile);
        }

        // POST: RequestFormFiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            RequestFormFile requestFormFile = db.RequestFormFiles.Find(id);
            db.RequestFormFiles.Remove(requestFormFile);
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

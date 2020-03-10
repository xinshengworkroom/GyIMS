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
using GyIMS.Helper;
using GyIMS.Helper.Container;
using GyIMS.Models;

namespace GyIMS.Controllers
{
    public class RequestFormTasksController : Controller
    {
        private DBContext db = new DBContext();
        private IRequestFormTaskDal _IRequestFormTaskQuery { get; set; }//= UserContainer.Resolve<IUserDal>();
        private IRequestFormDal _IRequestFormQuery = BaseContainer.Resolve<IRequestFormDal, RequestFormDal>();
        public RequestFormTasksController(IRequestFormTaskDal requestFormTaskDal)
        {
            _IRequestFormTaskQuery = requestFormTaskDal;
        }

        #region 自定义功能函数

        #region 查询界面

        //GET: //GetRequestFormTaskList
        [HttpGet]
        public ActionResult GetRequestFormTaskList(RequestForm requestForm)
        {
            var pageSize = Request["rows"] == "" ? 10 : int.Parse(Request["rows"]);
            var pageNumber = Request["page"] == "" ? 1 : int.Parse(Request["page"]);
            string name = string.Empty;
            if (Request["RequestFormName"] != "")
            {
                name = Request["RequestFormName"];
            }
            IQueryable<RequestFormTask> requestFormTasks;
            //requestForm = _IRequestFormQuery.GetModels(u => u.RequestStatus == "SI20200055").ToList()[0];
            if (!string.IsNullOrEmpty(name))
            {
                requestFormTasks = _IRequestFormTaskQuery.GetModelsByPage(pageSize, pageNumber, true, u => u.ID, u => u.Assigner.Contains(name) );
            }
            else
            {
                requestFormTasks = _IRequestFormTaskQuery.GetModelsByPage(pageSize, pageNumber, true, u => u.ID);
            }
            var total = requestFormTasks.Count();
            var list = new PageView { rows = requestFormTasks, total = total };
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
        public ActionResult CreateByForm(RequestFormTask requestFormTask)
        {
            try
            {

                int result = _IRequestFormTaskQuery.Add(requestFormTask);
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
        public ActionResult EditByForm(RequestFormTask requestFormTask)
        {
            try
            {

                int result = _IRequestFormTaskQuery.Update(requestFormTask);
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
            RequestFormTask requestFormTask = db.RequestFormTasks.Find(id);
            requestFormTask.Status = CommonStatusEnum.Disabled;
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
            List<ComboboxCommon> list = RequestHelper.GetCommonStatuses();
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 获取IT角色
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetITRoles()
        {
            List<ComboboxCommon> list = RequestHelper.GetITRoles();
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
        /// 获取任务状态
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetTaskStatus()
        {
            List<ComboboxCommon> list = RequestHelper.GetTaskStatus();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 任务状态

        #region 已指派

        /// <summary>
        /// 任务已指派
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public void AppointByAjax(RequestFormTask requestFormTask)
        {
            RequestFormTask _requestFormTask = _IRequestFormTaskQuery.GetModels(u => u.ID == requestFormTask.ID).ToList()[0];
            _requestFormTask.TaskStatus = requestFormTask.TaskStatus;
            _IRequestFormTaskQuery.Update(_requestFormTask);
        }

        
        #endregion

        #region 已接受

        /// <summary>
        /// 任务已接受
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public void AcceptByAjax(RequestFormTask requestFormTask)
        {
            RequestFormTask _requestFormTask = _IRequestFormTaskQuery.GetModels(u => u.ID == requestFormTask.ID).ToList()[0];
            _requestFormTask.TaskStatus = requestFormTask.TaskStatus;
            _IRequestFormTaskQuery.Update(_requestFormTask);
        }

        
        #endregion

        #region 执行中

        /// <summary>
        /// 任务执行中
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public void ExecuteByAjax(RequestFormTask requestFormTask)
        {
            RequestFormTask _requestFormTask = _IRequestFormTaskQuery.GetModels(u => u.ID == requestFormTask.ID).ToList()[0];
            _requestFormTask.TaskStatus = requestFormTask.TaskStatus;
            _IRequestFormTaskQuery.Update(_requestFormTask);
        }


        #endregion

        #region 已完成

        /// <summary>
        /// 任务已完成
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public void FinishByAjax(RequestFormTask requestFormTask)
        {
            RequestFormTask _requestFormTask = _IRequestFormTaskQuery.GetModels(u => u.ID == requestFormTask.ID).ToList()[0];
            _requestFormTask.TaskStatus = requestFormTask.TaskStatus;
            _IRequestFormTaskQuery.Update(_requestFormTask);
        }
        
        #endregion

        #region 延期

        /// <summary>
        /// 任务延期
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public void DelayByAjax(RequestFormTask requestFormTask)
        {
            RequestFormTask _requestFormTask = _IRequestFormTaskQuery.GetModels(u => u.ID == requestFormTask.ID).ToList()[0];
            _requestFormTask.TaskStatus = requestFormTask.TaskStatus;
            _IRequestFormTaskQuery.Update(_requestFormTask);
        }
        
        #endregion

        #region 已取消

        /// <summary>
        /// 任务已取消
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public void CancelByAjax(RequestFormTask requestFormTask)
        {
            RequestFormTask _requestFormTask = _IRequestFormTaskQuery.GetModels(u => u.ID == requestFormTask.ID).ToList()[0];
            _requestFormTask.TaskStatus = requestFormTask.TaskStatus;
            _IRequestFormTaskQuery.Update(_requestFormTask);
        }        

        #endregion

        #endregion

        #region 文件上传 下载
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
            string path = Server.MapPath("~/File/需求答疑附件.docx");
            FileStream fs = new FileStream(path, FileMode.Open);
            return File(fs, "word/docx", "需求答疑附件.docx");



        }

        #endregion

        #region 状态查询

        // GET: RequestFormTasks
        public ActionResult SearchIndex()
        {
            //var requestFormTasks = db.RequestFormTasks.Include(r => r.ItRole).Include(r => r.RequestForm).Include(r => r.UserCreate).Include(r => r.UserUpdate);
            //return View(requestFormTasks.ToList());
            return View(_IRequestFormTaskQuery.GetModels(u => u.ID != string.Empty).ToList());

        }

        #endregion

        #endregion

        #region 框架自动生成
        // GET: RequestFormTasks
        public ActionResult Index()
        {
            //var requestFormTasks = db.RequestFormTasks.Include(r => r.ItRole).Include(r => r.RequestForm).Include(r => r.UserCreate).Include(r => r.UserUpdate);
            //return View(requestFormTasks.ToList());
            return View(_IRequestFormTaskQuery.GetModels(u => true).ToList());

        }

        // GET: RequestFormTasks/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestFormTask requestFormTask = _IRequestFormTaskQuery.GetModels(u => u.ID==id).FirstOrDefault();
            if (requestFormTask == null)
            {
                return HttpNotFound();
            }
            return View(requestFormTask);
        }

        // GET: RequestFormTasks/Create
        public ActionResult Create()
        {
            ViewBag.ItRoleID = new SelectList(db.ItRoles, "ID", "Code");
            ViewBag.RequestFormID = new SelectList(db.RequestForms, "ID", "Code");
            ViewBag.CreatePerson = new SelectList(db.Users, "ID", "Code");
            ViewBag.UpdatePerson = new SelectList(db.Users, "ID", "Code");
            RequestFormTask requestFormTask = new RequestFormTask();
            return View(requestFormTask);
        }

        // POST: RequestFormTasks/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,RequestFormID,Assigner,ItRoleID,Handler,FromDate,ToDate,PercentOfRequest,Description,FileName,TaskStatus,Status,CreateDate,CreatePerson,UpdateDate,UpdatePerson,Summary")] RequestFormTask requestFormTask)
        {
            if (ModelState.IsValid)
            {
                _IRequestFormTaskQuery.Add(requestFormTask);
                return RedirectToAction("Index");
            }

            ViewBag.ItRoleID = new SelectList(db.ItRoles, "ID", "Code", requestFormTask.ItRoleID);
            ViewBag.RequestFormID = new SelectList(db.RequestForms, "ID", "Code", requestFormTask.RequestFormID);
            ViewBag.CreatePerson = new SelectList(db.Users, "ID", "Code", requestFormTask.CreatePerson);
            ViewBag.UpdatePerson = new SelectList(db.Users, "ID", "Code", requestFormTask.UpdatePerson);
            return View(requestFormTask);
        }

        // GET: RequestFormTasks/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestFormTask requestFormTask = _IRequestFormTaskQuery.GetModels(u => u.ID == id).FirstOrDefault();
            if (requestFormTask == null)
            {
                return HttpNotFound();
            }
            ViewBag.ItRoleID = new SelectList(db.ItRoles, "ID", "Code", requestFormTask.ItRoleID);
            ViewBag.RequestFormID = new SelectList(db.RequestForms, "ID", "Code", requestFormTask.RequestFormID);
            ViewBag.CreatePerson = new SelectList(db.Users, "ID", "Code", requestFormTask.CreatePerson);
            ViewBag.UpdatePerson = new SelectList(db.Users, "ID", "Code", requestFormTask.UpdatePerson);
            return View(requestFormTask);
        }

        // POST: RequestFormTasks/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,RequestFormID,Assigner,ItRoleID,Handler,FromDate,ToDate,PercentOfRequest,Description,FileName,TaskStatus,Status,CreateDate,CreatePerson,UpdateDate,UpdatePerson,Summary")] RequestFormTask requestFormTask)
        {
            if (ModelState.IsValid)
            {
                _IRequestFormTaskQuery.Update(requestFormTask);
                return RedirectToAction("Index");
            }
            ViewBag.ItRoleID = new SelectList(db.ItRoles, "ID", "Code", requestFormTask.ItRoleID);
            ViewBag.RequestFormID = new SelectList(db.RequestForms, "ID", "Code", requestFormTask.RequestFormID);
            ViewBag.CreatePerson = new SelectList(db.Users, "ID", "Code", requestFormTask.CreatePerson);
            ViewBag.UpdatePerson = new SelectList(db.Users, "ID", "Code", requestFormTask.UpdatePerson);
            return View(requestFormTask);
        }

        // GET: RequestFormTasks/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestFormTask requestFormTask = db.RequestFormTasks.Find(id);
            if (requestFormTask == null)
            {
                return HttpNotFound();
            }
            return View(requestFormTask);
        }

        // POST: RequestFormTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            RequestFormTask requestFormTask = db.RequestFormTasks.Find(id);
            db.RequestFormTasks.Remove(requestFormTask);
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

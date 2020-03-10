using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
    public class RequestFormBugsController : Controller
    {
        private DBContext db = new DBContext();
        private IRequestFormBugDal _IRequestFormBugQuery { get; set; }//= UserContainer.Resolve<IUserDal>();

        public RequestFormBugsController(IRequestFormBugDal requestFormBugDal)
        {
            _IRequestFormBugQuery = requestFormBugDal;
        }

        #region 自定义功能函数

        #region 查询界面

        //GET: //GetRequestFormBugList
        [HttpGet]
        public ActionResult GetRequestFormBugList()
        {
            var pageSize = Request["rows"] == "" ? 10 : int.Parse(Request["rows"]);
            var pageNumber = Request["page"] == "" ? 1 : int.Parse(Request["page"]);
            string RequestFormID = string.Empty;
            if (Request["RequestFormName"] != "")
            {
                RequestFormID = Request["RequestFormName"];
            }
            IQueryable<RequestFormBug> requestFormBugs;
            if (!string.IsNullOrEmpty(RequestFormID))
            {
                requestFormBugs = _IRequestFormBugQuery.GetModelsByPage(pageSize, pageNumber, true, u => u.ID, u => u.RequestFormID.Contains(RequestFormID)).OrderBy(u =>u.RequestFormID);
            }
            else
            {
                requestFormBugs = _IRequestFormBugQuery.GetModelsByPage(pageSize, pageNumber, true, u => u.RequestFormID == RequestFormID).OrderBy(u=>u.RequestFormID);
            }
            var total = _IRequestFormBugQuery.GetModels(u => true).Count();
            var list = new PageView { rows = requestFormBugs, total = total };
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        public void Export()
        {
            var pageSize = string.IsNullOrEmpty(Request["rows"]) ? 10 : int.Parse(Request["rows"]);
            var pageNumber = string.IsNullOrEmpty(Request["page"]) ? 1 : int.Parse(Request["page"]);
            string RequestFormID = string.Empty;
            if (Request["RequestFormName"] != "")
            {
                RequestFormID = Request["RequestFormName"];
            }

            IQueryable<RequestFormBug> requestFormBugs;
            if (!string.IsNullOrEmpty(RequestFormID))
            {
                requestFormBugs = _IRequestFormBugQuery.GetModelsByPage(pageSize, pageNumber, true, u => u.ID, u => u.RequestFormID.Contains(RequestFormID)).OrderBy(u => u.RequestFormID);
            }
            else
            {
                requestFormBugs = _IRequestFormBugQuery.GetModelsByPage(pageSize, pageNumber, true, u => u.RequestFormID == RequestFormID).OrderBy(u => u.RequestFormID);
            }


            ExcelHelper.CreateExcel<RequestFormBug>(requestFormBugs.ToList(), "需求日志信息列表");

        }

        #endregion


        #region 新增界面

        /// <summary>
        /// 通过表单提交
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateByForm(RequestFormBug requestFormBug)
        {
            try
            {

                int result = _IRequestFormBugQuery.Add(requestFormBug);
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
        public ActionResult EditByForm(RequestFormBug requestFormBug)
        {
            try
            {

                int result = _IRequestFormBugQuery.Update(requestFormBug);
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
            RequestFormBug requestFormBug = db.RequestFormBugs.Find(id);
            requestFormBug.Status = CommonStatusEnum.Disabled;
            db.SaveChanges();
        }


        #endregion

        #region 条件查找

        // GET: /RequestFormBugs/
        public ActionResult Search(string requestform)
        {
            List<RequestFormBug> requestforms = db.RequestFormBugs.Where(item => item.RequestFormID == requestform).ToList();
            return View("Index", requestforms);
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
        /// 获取BUG等级 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetRankEnums()
        {
            List<ComboboxCommon> list = RequestHelper.GetRankEnums();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #endregion


        #region 框架自动生成
        // GET: RequestFormBugs
        public ActionResult Index()
        {
            //var requestFormBugs = db.RequestFormBugs.Include(r => r.RequestForm).Include(r => r.RequestFormTask).Include(r => r.RequestFormTaskList).Include(r => r.UserCreate).Include(r => r.UserUpdate);
            //var requestFormBugs = db.RequestFormBugs.Where(x => x.Status == CommonStatusEnum.Able).ToList();
            //return View(requestFormBugs.ToList());
            return View(_IRequestFormBugQuery.GetModels(u =>true).ToList());

        }

        // GET: RequestFormBugs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestFormBug requestFormBug = _IRequestFormBugQuery.GetModels(u =>u.ID==id).FirstOrDefault();
            if (requestFormBug == null)
            {
                return HttpNotFound();
            }
            return View(requestFormBug);
        }

        // GET: RequestFormBugs/Create
        public ActionResult Create()
        {
            ViewBag.RequestFormID = new SelectList(db.RequestForms, "ID", "Code");
            ViewBag.RequestFormTaskID = new SelectList(db.RequestFormTasks, "ID", "RequestFormID");
            ViewBag.RequestFormTaskListID = new SelectList(db.RequestFormTaskLists, "ID", "RequestFormID");
            ViewBag.CreatePerson = new SelectList(db.Users, "ID", "Code");
            ViewBag.UpdatePerson = new SelectList(db.Users, "ID", "Code");
            RequestFormBug requestFormBug = new RequestFormBug();
            return View(requestFormBug);
        }

        // POST: RequestFormBugs/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,RequestFormID,RequestFormTaskID,RequestFormTaskListID,Handler,Rank,Description,Status,CreateDate,CreatePerson,UpdateDate,UpdatePerson,Summary")] RequestFormBug requestFormBug)
        {
            if (ModelState.IsValid)
            {
                _IRequestFormBugQuery.Add(requestFormBug);
                return RedirectToAction("Index");
            }

            ViewBag.RequestFormID = new SelectList(db.RequestForms, "ID", "Code", requestFormBug.RequestFormID);
            ViewBag.RequestFormTaskID = new SelectList(db.RequestFormTasks, "ID", "RequestFormID", requestFormBug.RequestFormTaskID);
            ViewBag.RequestFormTaskListID = new SelectList(db.RequestFormTaskLists, "ID", "RequestFormID", requestFormBug.RequestFormTaskListID);
            ViewBag.CreatePerson = new SelectList(db.Users, "ID", "Code", requestFormBug.CreatePerson);
            ViewBag.UpdatePerson = new SelectList(db.Users, "ID", "Code", requestFormBug.UpdatePerson);
            return View(requestFormBug);
        }

        // GET: RequestFormBugs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestFormBug requestFormBug = _IRequestFormBugQuery.GetModels(u => u.ID == id).FirstOrDefault();
            if (requestFormBug == null)
            {
                return HttpNotFound();
            }
            ViewBag.RequestFormID = new SelectList(db.RequestForms, "ID", "Code", requestFormBug.RequestFormID);
            ViewBag.RequestFormTaskID = new SelectList(db.RequestFormTasks, "ID", "RequestFormID", requestFormBug.RequestFormTaskID);
            ViewBag.RequestFormTaskListID = new SelectList(db.RequestFormTaskLists, "ID", "RequestFormID", requestFormBug.RequestFormTaskListID);
            ViewBag.CreatePerson = new SelectList(db.Users, "ID", "Code", requestFormBug.CreatePerson);
            ViewBag.UpdatePerson = new SelectList(db.Users, "ID", "Code", requestFormBug.UpdatePerson);
            return View(requestFormBug);
        }

        // POST: RequestFormBugs/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,RequestFormID,RequestFormTaskID,RequestFormTaskListID,Handler,Rank,Description,Status,CreateDate,CreatePerson,UpdateDate,UpdatePerson,Summary")] RequestFormBug requestFormBug)
        {
            if (ModelState.IsValid)
            {
                _IRequestFormBugQuery.Update(requestFormBug);
                return RedirectToAction("Index");
            }
            ViewBag.RequestFormID = new SelectList(db.RequestForms, "ID", "Code", requestFormBug.RequestFormID);
            ViewBag.RequestFormTaskID = new SelectList(db.RequestFormTasks, "ID", "RequestFormID", requestFormBug.RequestFormTaskID);
            ViewBag.RequestFormTaskListID = new SelectList(db.RequestFormTaskLists, "ID", "RequestFormID", requestFormBug.RequestFormTaskListID);
            ViewBag.CreatePerson = new SelectList(db.Users, "ID", "Code", requestFormBug.CreatePerson);
            ViewBag.UpdatePerson = new SelectList(db.Users, "ID", "Code", requestFormBug.UpdatePerson);
            return View(requestFormBug);
        }

        // GET: RequestFormBugs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestFormBug requestFormBug = db.RequestFormBugs.Find(id);
            if (requestFormBug == null)
            {
                return HttpNotFound();
            }
            return View(requestFormBug);
        }

        // POST: RequestFormBugs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            RequestFormBug requestFormBug = db.RequestFormBugs.Find(id);
            db.RequestFormBugs.Remove(requestFormBug);
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

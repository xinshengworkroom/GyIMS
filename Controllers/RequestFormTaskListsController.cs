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
    public class RequestFormTaskListsController : Controller
    {
        private DBContext db = new DBContext();
        private IRequestFormTaskListDal _IRequestFormTaskListQuery { get; set; }//= UserContainer.Resolve<IUserDal>();

        public RequestFormTaskListsController(IRequestFormTaskListDal requestFormTaskListDal)
        {
            _IRequestFormTaskListQuery = requestFormTaskListDal;
        }

        #region 自定义功能函数

        #region 查询界面

        //GET: //GetRequestFormTaskListList
        [HttpGet]
        public ActionResult GetRequestFormTaskListList()
        {
            var pageSize = Request["rows"] == "" ? 10 : int.Parse(Request["rows"]);
            var pageNumber = Request["page"] == "" ? 1 : int.Parse(Request["page"]);
            string name = string.Empty, requestFormTaskID = string.Empty;
            if (Request["RequestFormName"] != "")
            {
                name = Request["RequestFormName"];
            }
            requestFormTaskID = Request["RequestFormTaskID"];

            IQueryable<RequestFormTaskList> requestFormTaskLists;
            if (!string.IsNullOrEmpty(name))
            {
                requestFormTaskLists = _IRequestFormTaskListQuery.GetModelsByPage(pageSize, pageNumber, true, u => u.ID, u => u.RequestFormTaskID.Contains(requestFormTaskID) && u.Status == CommonStatusEnum.Able);
            }
            else
            {
                requestFormTaskLists = _IRequestFormTaskListQuery.GetModelsByPage(pageSize, pageNumber, true, u => u.ID, u => u.Status == CommonStatusEnum.Able);
            }
            var total = requestFormTaskLists.Count();
            var list = new PageView { rows = requestFormTaskLists, total = total };
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
        public ActionResult CreateByForm(RequestFormTaskList requestFormTaskList)
        {
            try
            {

                int result = _IRequestFormTaskListQuery.Add(requestFormTaskList);
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
        public ActionResult EditByForm(RequestFormTaskList requestFormTaskList)
        {
            try
            {

                int result = _IRequestFormTaskListQuery.Update(requestFormTaskList);
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
            RequestFormTaskList requestFormTaskList = db.RequestFormTaskLists.Find(id);
            requestFormTaskList.Status = CommonStatusEnum.Disabled;
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

        #endregion

        #endregion

        #region 框架自动生成
        // GET: RequestFormTaskLists
        public ActionResult Index(string requestFormTaskID)
        {
            //var requestFormTaskLists = db.RequestFormTaskLists.Include(r => r.RequestForm).Include(r => r.RequestFormTask).Include(r => r.UserCreate).Include(r => r.UserUpdate);
            //var requestFormTaskLists = db.RequestFormTaskLists.Where(x => x.Status == CommonStatusEnum.Able).ToList();
            //return View(requestFormTaskLists.ToList());
            ViewBag.RequestFormTaskID = requestFormTaskID;
            //var requestFormTaskList = db.RequestFormTaskLists.Where(u => u.RequestFormTaskID == requestFormTaskID).ToList();
            //return View(requestFormTaskList);
            return View(_IRequestFormTaskListQuery.GetModels(u => u.RequestFormTaskID == requestFormTaskID).ToList());

        }

        // GET: RequestFormTaskLists/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestFormTaskList requestFormTaskList = _IRequestFormTaskListQuery.GetModels(u => u.ID == id).FirstOrDefault();
            if (requestFormTaskList == null)
            {
                return HttpNotFound();
            }
            return View(requestFormTaskList);
        }

        // GET: RequestFormTaskLists/Create
        public ActionResult Create(string requestFormTaskID)
        {
            ViewBag.RequestFormID = new SelectList(db.RequestForms, "ID", "Code");
            ViewBag.RequestFormTaskID = new SelectList(db.RequestFormTasks, "ID", "RequestFormID");
            ViewBag.CreatePerson = new SelectList(db.Users, "ID", "Code");
            ViewBag.UpdatePerson = new SelectList(db.Users, "ID", "Code");
            ViewBag.RequestFormTaskID = requestFormTaskID;

            RequestFormTaskList requestFormTaskList = new RequestFormTaskList(requestFormTaskID);
            return View(requestFormTaskList);
        }

        // POST: RequestFormTaskLists/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,RequestFormID,RequestFormTaskID,TaskSeqNo,Handler,HandleDate,FromTime,ToTime,RateOfProgress,Status,CreateDate,CreatePerson,UpdateDate,UpdatePerson,Summary")] RequestFormTaskList requestFormTaskList)
        {
            if (ModelState.IsValid)
            {
                _IRequestFormTaskListQuery.Add(requestFormTaskList);
                return RedirectToAction("Index");
            }

            ViewBag.RequestFormID = new SelectList(db.RequestForms, "ID", "Code", requestFormTaskList.RequestFormID);
            ViewBag.RequestFormTaskID = new SelectList(db.RequestFormTasks, "ID", "RequestFormID", requestFormTaskList.RequestFormTaskID);
            ViewBag.CreatePerson = new SelectList(db.Users, "ID", "Code", requestFormTaskList.CreatePerson);
            ViewBag.UpdatePerson = new SelectList(db.Users, "ID", "Code", requestFormTaskList.UpdatePerson);
            ViewBag.RequestFormTaskID = requestFormTaskList.RequestFormTaskID;

            return View(requestFormTaskList);
        }

        // GET: RequestFormTaskLists/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestFormTaskList requestFormTaskList = _IRequestFormTaskListQuery.GetModels(u => u.ID == id).FirstOrDefault();
            if (requestFormTaskList == null)
            {
                return HttpNotFound();
            }
            ViewBag.RequestFormID = new SelectList(db.RequestForms, "ID", "Code", requestFormTaskList.RequestFormID);
            ViewBag.RequestFormTaskID = new SelectList(db.RequestFormTasks, "ID", "RequestFormID", requestFormTaskList.RequestFormTaskID);
            ViewBag.CreatePerson = new SelectList(db.Users, "ID", "Code", requestFormTaskList.CreatePerson);
            ViewBag.UpdatePerson = new SelectList(db.Users, "ID", "Code", requestFormTaskList.UpdatePerson);
            ViewBag.RequestFormTaskID = requestFormTaskList.RequestFormTaskID;
            return View(requestFormTaskList);
        }

        // POST: RequestFormTaskLists/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,RequestFormID,RequestFormTaskID,TaskSeqNo,Handler,HandleDate,FromTime,ToTime,RateOfProgress,Status,CreateDate,CreatePerson,UpdateDate,UpdatePerson,Summary")] RequestFormTaskList requestFormTaskList)
        {
            if (ModelState.IsValid)
            {
                _IRequestFormTaskListQuery.Update(requestFormTaskList);
                return RedirectToAction("Index");
            }
            ViewBag.RequestFormID = new SelectList(db.RequestForms, "ID", "Code", requestFormTaskList.RequestFormID);
            ViewBag.RequestFormTaskID = new SelectList(db.RequestFormTasks, "ID", "RequestFormID", requestFormTaskList.RequestFormTaskID);
            ViewBag.CreatePerson = new SelectList(db.Users, "ID", "Code", requestFormTaskList.CreatePerson);
            ViewBag.UpdatePerson = new SelectList(db.Users, "ID", "Code", requestFormTaskList.UpdatePerson);
            ViewBag.RequestFormTaskID = requestFormTaskList.RequestFormTaskID;

            return View(requestFormTaskList);
        }

        // GET: RequestFormTaskLists/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestFormTaskList requestFormTaskList = db.RequestFormTaskLists.Find(id);
            if (requestFormTaskList == null)
            {
                return HttpNotFound();
            }
            return View(requestFormTaskList);
        }

        // POST: RequestFormTaskLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            RequestFormTaskList requestFormTaskList = db.RequestFormTaskLists.Find(id);
            db.RequestFormTaskLists.Remove(requestFormTaskList);
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

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
using GyIMS.Helper.Container;
using GyIMS.Models;

namespace GyIMS.Controllers
{
    public class RequestFormAssessmentsController : Controller
    {
        private DBContext db = new DBContext();
        private IRequestFormAssessmentDal _IRequestFormAssessmentQuery { get; set; }//= UserContainer.Resolve<IUserDal>();
        private IRequestFormDal _IRequestFormQuery = BaseContainer.Resolve<IRequestFormDal, RequestFormDal>();
        private IStatusInfoDal _IStatusInfoDal = BaseContainer.Resolve<IStatusInfoDal, StatusInfoDal>();
        public RequestFormAssessmentsController(IRequestFormAssessmentDal requestFormAssessmentDal)
        {
            _IRequestFormAssessmentQuery = requestFormAssessmentDal;
        }

        #region 自定义功能函数

        #region 查询界面

        //GET: //GetRequestFormAssessmentList
        [HttpGet]
        public ActionResult GetRequestFormAssessmentList(RequestForm requestForm)
        {
            var pageSize = Request["rows"] == "" ? 10 : int.Parse(Request["rows"]);
            var pageNumber = Request["page"] == "" ? 1 : int.Parse(Request["page"]);
            string name = string.Empty;
            if (Request["RequestFormName"] != "")
            {
                name = Request["RequestName"];
            }
            IQueryable<RequestFormAssessment> requestFormAssessments;
            
            if (!string.IsNullOrEmpty(name))
            {
                requestFormAssessments = _IRequestFormAssessmentQuery.GetModelsByPage(pageSize, pageNumber, true, u => u.Status == CommonStatusEnum.Able, u => u.Offerer.Contains(name) && u.RequestStatus == "SI20209529").OrderBy(u=>u.RequestStatus);
            }
            else
            {
                requestFormAssessments = _IRequestFormAssessmentQuery.GetModelsByPage(pageSize, pageNumber, true, u => u.Status == CommonStatusEnum.Able && u.RequestStatus == "SI20209529").OrderBy(u=>u.RequestStatus);
            }
            var total = _IRequestFormAssessmentQuery.GetModels(u => true).Count();
            var list = new PageView { rows = requestFormAssessments, total = total };
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
        public ActionResult CreateByForm(RequestFormAssessment requestFormAssessments)
        {
            try
            {

                int result = _IRequestFormAssessmentQuery.Add(requestFormAssessments);
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
        public ActionResult EditByForm(RequestFormAssessment requestFormAssessments)
        {
            try
            {
                int result = _IRequestFormAssessmentQuery.Update(requestFormAssessments);
                return Json(new { Code = 1, Message = "保存成功" }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                return Json(new { Code = -1, Message = ex.Message }, JsonRequestBehavior.DenyGet);
            }


        }


        #endregion

        #region 需求申请报价

        public void PriceByAjax(RequestFormAssessment requestFormAssessment)
        {
            RequestFormAssessment request = _IRequestFormAssessmentQuery.GetModels(u => u.ID == requestFormAssessment.ID).ToList()[0];
            request.RequestStatus = "SI20209531";
            RequestForm requestForm = _IRequestFormQuery.GetModels(u=>u.ID == request.RequestFormID).ToList()[0];
            requestForm.RequestStatus = "SI20200043";
            _IRequestFormQuery.Update(requestForm);
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
            RequestFormAssessment requestFormAssessments = db.RequestFormAssessments.Find(id);
            requestFormAssessments.Status = CommonStatusEnum.Disabled;
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
        /// 获取需求状态
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetRequestForms()
        {
            List<ComboboxCommon> list = RequestHelper.GetRequestForms();
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
        /// 获取工作类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetWorkType()
        {
            List<ComboboxCommon> list = RequestHelper.GetWorkType();
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 获取时间单位
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetTimeUnit()
        {
            List<ComboboxCommon> list = RequestHelper.GetTimeUnit();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion

        #region 框架自动生成
        // GET: RequestFormAssessments
        public ActionResult Index()
        {
            var requestform = db.RequestFormAssessments.Where(u => u.RequestStatus == "SI20209529").ToList();
            return View(requestform);
           

        }

        // GET: RequestFormAssessments/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestFormAssessment requestFormAssessment = _IRequestFormAssessmentQuery.GetModels(u => u.ID==id).FirstOrDefault();
            if (requestFormAssessment == null)
            {
                return HttpNotFound();
            }
            return View(requestFormAssessment);
        }

        // GET: RequestFormAssessments/Create
        public ActionResult Create()
        {
            ViewBag.ItRoleID = new SelectList(db.ItRoles, "ID", "Code");
            ViewBag.RequestFormID = new SelectList(db.RequestForms, "ID", "Code");
            ViewBag.CreatePerson = new SelectList(db.Users, "ID", "Code");
            ViewBag.UpdatePerson = new SelectList(db.Users, "ID", "Code");
            RequestFormAssessment requestFormAssessment = new RequestFormAssessment();
            return View(requestFormAssessment);
        }

        // POST: RequestFormAssessments/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,RequestFormID,Assessor,AssessDate,FromDate,ToDate,Offerer,OfferDate,ItRoleID,WorkType,TimeUnit,SpendTime,Status,CreateDate,CreatePerson,UpdateDate,UpdatePerson,Summary")] RequestFormAssessment requestFormAssessment)
        {
            if (ModelState.IsValid)
            {
                _IRequestFormAssessmentQuery.Add(requestFormAssessment);
                return RedirectToAction("Index");
            }

            ViewBag.ItRoleID = new SelectList(db.ItRoles, "ID", "Code", requestFormAssessment.ItRoleID);
            ViewBag.RequestFormID = new SelectList(db.RequestForms, "ID", "Code", requestFormAssessment.RequestFormID);
            ViewBag.CreatePerson = new SelectList(db.Users, "ID", "Code", requestFormAssessment.CreatePerson);
            ViewBag.UpdatePerson = new SelectList(db.Users, "ID", "Code", requestFormAssessment.UpdatePerson);
            return View(requestFormAssessment);
        }

        // GET: RequestFormAssessments/Edit/5
        public ActionResult Edit(string id)
        {
            RequestFormAssessment requestFormAssessment = _IRequestFormAssessmentQuery.GetModels(u => u.ID == id).FirstOrDefault();

            RequestFormAssessment request = _IRequestFormAssessmentQuery.GetModels(u => u.ID == requestFormAssessment.ID).ToList()[0];
            request.RequestStatus = "SI20209531";
            RequestForm requestForm = _IRequestFormQuery.GetModels(u => u.ID == request.RequestFormID).ToList()[0];
            requestForm.RequestStatus = "SI20200043";
            _IRequestFormQuery.Update(requestForm);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (requestFormAssessment == null)
            {
                return HttpNotFound();
            }
            ViewBag.ItRoleID = new SelectList(db.ItRoles, "ID", "Code", requestFormAssessment.ItRoleID);
            ViewBag.RequestFormID = new SelectList(db.RequestForms, "ID", "Code", requestFormAssessment.RequestFormID);
            ViewBag.CreatePerson = new SelectList(db.Users, "ID", "Code", requestFormAssessment.CreatePerson);
            ViewBag.UpdatePerson = new SelectList(db.Users, "ID", "Code", requestFormAssessment.UpdatePerson);
            return View(requestFormAssessment);
        }

        // POST: RequestFormAssessments/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,RequestFormID,Assessor,AssessDate,FromDate,ToDate,Offerer,OfferDate,ItRoleID,WorkType,TimeUnit,SpendTime,Status,CreateDate,CreatePerson,UpdateDate,UpdatePerson,Summary")] RequestFormAssessment requestFormAssessment)
        {
            if (ModelState.IsValid)
            {
                _IRequestFormAssessmentQuery.Update(requestFormAssessment);
                return RedirectToAction("Index");
            }
            ViewBag.ItRoleID = new SelectList(db.ItRoles, "ID", "Code", requestFormAssessment.ItRoleID);
            ViewBag.RequestFormID = new SelectList(db.RequestForms, "ID", "Code", requestFormAssessment.RequestFormID);
            ViewBag.CreatePerson = new SelectList(db.Users, "ID", "Code", requestFormAssessment.CreatePerson);
            ViewBag.UpdatePerson = new SelectList(db.Users, "ID", "Code", requestFormAssessment.UpdatePerson);
            return View(requestFormAssessment);
        }

        // GET: RequestFormAssessments/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestFormAssessment requestFormAssessment = db.RequestFormAssessments.Find(id);
            if (requestFormAssessment == null)
            {
                return HttpNotFound();
            }
            return View(requestFormAssessment);
        }

        // POST: RequestFormAssessments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            RequestFormAssessment requestFormAssessment = db.RequestFormAssessments.Find(id);
            db.RequestFormAssessments.Remove(requestFormAssessment);
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

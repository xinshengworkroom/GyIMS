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
using GyIMS.Helper.Container;
using GyIMS.Models;

namespace GyIMS.Controllers
{
    public class RequestFormsController : Controller
    {
        private DBContext db = new DBContext();

        private IRequestFormDal _IRequestFormQuery= BaseContainer.Resolve<IRequestFormDal, RequestFormDal>(); 
        private IRequestFormAssessmentDal _IRequestFormAssessmentQuery = BaseContainer.Resolve<IRequestFormAssessmentDal, RequestFormAssessmentDal>();
        private IRequestFormTaskDal _requestFormTaskDal = BaseContainer.Resolve<IRequestFormTaskDal, RequestFormTaskDal>();
        private IRequestFormBugDal _requestFormBugDal = BaseContainer.Resolve<IRequestFormBugDal, RequestFormBugDal>();
        public RequestFormsController(IRequestFormDal requestFormDal)
        {
            _IRequestFormQuery = requestFormDal;
        }


        #region  需求申请

        /// <summary>
        /// 需求申请
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public void ApplyByAjax(RequestForm requestForm)
        {
            RequestForm _requestForm = _IRequestFormQuery.GetModels(u => u.ID == requestForm.ID).ToList()[0];
            _requestForm.RequestStatus = requestForm.RequestStatus;
            _IRequestFormQuery.Update(_requestForm); //更新需求
            
            RequestFormAssessment _requestFormAssessment = new RequestFormAssessment();
            _requestFormAssessment.RequestFormID = requestForm.ID;
            _requestFormAssessment.RequestStatus = "SI20209529";
            _IRequestFormAssessmentQuery.Add(_requestFormAssessment); //更新至估计Tablessss

            RequestFormBug requestFormBug = new RequestFormBug();
            requestFormBug.RequestFormID = _requestForm.ID;
            requestFormBug.Name = _requestForm.Name;
            requestFormBug.SysType = _requestForm.SysType;
            requestFormBug.Applier = _requestForm.Applier;
            requestFormBug.RequestStatus = _requestForm.RequestStatus;
            _requestFormBugDal.Add(requestFormBug);

        }

        #endregion


        #region 新增界面确认

        /// <summary>
        /// 通过表单提交
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateByForm(RequestForm requestForm)
        {
            try
            {
                int result = _IRequestFormQuery.Add(requestForm);
                if (result > 0)
                {
                    return Json(new { Code = 1, Message = "保存成功" }, JsonRequestBehavior.DenyGet);
                }
                return Json(new { Code = 0, Message = "保存失败" }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                return Json(new { Code = -1, Message = ex.Message }, JsonRequestBehavior.DenyGet);
            }

        }

        #endregion





        #region 自定义功能函数

        #region 查询界面

        //GET: //GetRequestFormList
        [HttpGet]
        public ActionResult GetRequestFormList()
        {
            var pageSize = Request["rows"] == "" ? 10 : int.Parse(Request["rows"]);
            var pageNumber = Request["page"] == "" ? 1 : int.Parse(Request["page"]);
            string ID = string.Empty;
            if (Request["RequestFormName"] != "")
            {
                ID = Request["RequestFormName"];
            }
            IQueryable<RequestForm> requestForms;
            if (!string.IsNullOrEmpty(ID))
            {
                requestForms = _IRequestFormQuery.GetModelsByPage(pageSize, pageNumber, true, u => u.ID, u => u.ID.Contains(ID) && 
                                                                                                              u.Status == CommonStatusEnum.Able &&
                                                                                                             (u.RequestStatus == "SI20200039" ||
                                                                                                              u.RequestStatus == "SI20200041" ||
                                                                                                              u.RequestStatus == "SI20200047" ||
                                                                                                              u.RequestStatus == "SI20200051"));
            }
            else
            {
                requestForms = _IRequestFormQuery.GetModelsByPage(pageSize, pageNumber, true, u => u.Name, u => u.Status == CommonStatusEnum.Able &&
                                                                                                               (u.RequestStatus == "SI20200039" ||
                                                                                                                u.RequestStatus == "SI20200041" ||
                                                                                                                u.RequestStatus == "SI20200047" ||
                                                                                                                u.RequestStatus == "SI20200051"));
            }
            var total = _IRequestFormQuery.GetModels(u => true).Count(); 
            var list = new PageView { rows = requestForms, total = total };
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion

        public void Export()
        {
            var pageSize = string.IsNullOrEmpty(Request["rows"]) ? 10 : int.Parse(Request["rows"]);
            var pageNumber = string.IsNullOrEmpty(Request["page"]) ? 1 : int.Parse(Request["page"]);
            string name = string.Empty;
            if (Request["RequestFormName"] != "")
            {
                name = Request["RequestFormName"];
            }

            IQueryable<RequestForm> requestForms;
            if (!string.IsNullOrEmpty(name))
            {
                requestForms = _IRequestFormQuery.GetModelsByPage(pageSize, pageNumber, true, u => u.ID, u => u.Name.Contains(name) && u.Status == CommonStatusEnum.Able);
            }
            else
            {
                requestForms = _IRequestFormQuery.GetModelsByPage(pageSize, pageNumber, true, u => u.Name, u => u.Status == CommonStatusEnum.Able);
            }

            ExcelHelper.CreateExcel<RequestForm>(requestForms.ToList(), "需求申请信息列表");

        }

        #region 修改界面


        /// <summary>
        /// 通过表单提交
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditByForm(RequestForm requestForm)
        {
            try
            {

                int result = _IRequestFormQuery.Update(requestForm);
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
            RequestForm requestForm = db.RequestForms.Find(id);
            requestForm.Status = CommonStatusEnum.Disabled;
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
        /// 获取需求类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetRequstFormSys()
        {
            List<ComboboxCommon> list = RequestHelper.GetRequstFormSys();
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 获取需求状态
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetRequestStatus()
        {
            List<ComboboxCommon> list = RequestHelper.GetRequestStatus();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取申请人
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetPerson()
        {
            List<ComboboxCommon> list = CommonHelper.GetPerson();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region 需求状态

        #region 需求申请

    

        // GET: RequestForms
        public ActionResult ApplyIndex()
        {
            //var requestForms = db.RequestForms.Include(r => r.UserCreate).Include(r => r.UserUpdate);
            var requestForms = db.RequestForms.Where(x => x.Status == CommonStatusEnum.Able).ToList();

           // return View(db.RequestForms.Where(x => x.RequestStatus == RequestStatusEnum.Two).ToList());
            return View();
        }

        //GET: //GetRequestFormList
        [HttpGet]
        public ActionResult GetRequestFormApplyList()
        {
            //var pageSize = Request["rows"] == "" ? 10 : int.Parse(Request["rows"]);
            //var pageNumber = Request["page"] == "" ? 1 : int.Parse(Request["page"]);
            //string name = string.Empty;
            //if (Request["RequestFormName"] != "")
            //{
            //    name = Request["RequestFormName"];
            //}
            //IQueryable<RequestForm> requestForms;
            //if (!string.IsNullOrEmpty(name))
            //{
            //    requestForms = db.RequestForms.OrderBy(x => x.ID).Where(x => x.Name.Contains(name) && (x.RequestStatus == RequestStatusEnum.Two)).Skip((pageNumber - 1) * pageSize).Take(pageSize);
            //}
            //else
            //{
            //    requestForms = db.RequestForms.OrderBy(x => x.ID).Where(x => x.RequestStatus == RequestStatusEnum.Two).Skip((pageNumber - 1) * pageSize).Take(pageSize);
            //}
            //var total = requestForms.Count();
            //var list = new PageView { rows = requestForms, total = total };
            //return Json(list, JsonRequestBehavior.AllowGet);

            return View();
        }

        #endregion

        #region 需求申请报价

        /// <summary>
        /// 需求申请报价
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public void PriceByAjax(RequestForm requestForm,RequestFormAssessment requestFormAssessment)
        {
            
        }


        #endregion

        #region 需求申请确认/拒绝报价

        /// <summary>
        /// 需求申请确认报价
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public void SureByAjax(RequestForm requestForm)
        {
            RequestForm _requestForm = _IRequestFormQuery.GetModels(u => u.ID == requestForm.ID).FirstOrDefault();
            _requestForm.RequestStatus = requestForm.RequestStatus;
            _IRequestFormQuery.Update(_requestForm); //更新需求

            RequestFormBug requestFormBug = new RequestFormBug();
            requestFormBug.RequestFormID = _requestForm.ID;
            requestFormBug.Name = _requestForm.Name;
            requestFormBug.SysType = _requestForm.SysType;
            requestFormBug.Applier = _requestForm.Applier;
            requestFormBug.RequestStatus = _requestForm.RequestStatus;
            _requestFormBugDal.Add(requestFormBug);
        }

        /// <summary>
        /// 需求申请拒绝报价
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public void RefuseByAjax(RequestForm requestForm)
        {
            RequestForm _requestForm = _IRequestFormQuery.GetModels(u => u.ID == requestForm.ID).FirstOrDefault();
            _requestForm.RequestStatus = requestForm.RequestStatus;
            _IRequestFormQuery.Update(_requestForm); //更新需求

            RequestFormBug requestFormBug = new RequestFormBug();
            requestFormBug.RequestFormID = _requestForm.ID;
            requestFormBug.Name = _requestForm.Name;
            requestFormBug.SysType = _requestForm.SysType;
            requestFormBug.Applier = _requestForm.Applier;
            requestFormBug.RequestStatus = _requestForm.RequestStatus;
            _requestFormBugDal.Add(requestFormBug);
        }

        // GET: RequestForms
        public ActionResult SureIndex()
        {
            //var requestForms = db.RequestForms.Include(r => r.UserCreate).Include(r => r.UserUpdate);
            var requestForms = db.RequestForms.Where(x => x.RequestStatus == "SI20200043").ToList();

            return View(requestForms);
        }

        //GET: //GetRequestFormList
        [HttpGet]
        public ActionResult GetRequestFormSureList()
        {
            var pageSize = Request["rows"] == "" ? 10 : int.Parse(Request["rows"]);
            var pageNumber = Request["page"] == "" ? 1 : int.Parse(Request["page"]);
            string name = string.Empty;
            if (Request["RequestFormName"] != "")
            {
                name = Request["RequestFormName"];
            }
            IQueryable<RequestForm> requestForms;
            if (!string.IsNullOrEmpty(name))
            {
                requestForms = db.RequestForms.OrderBy(x => x.ID).Where(x => x.Name.Contains(name) && (x.RequestStatus == "SI20200043")).Skip((pageNumber - 1) * pageSize).Take(pageSize);
            }
            else
            {
                requestForms = db.RequestForms.OrderBy(x => x.ID).Where(x => x.RequestStatus == "SI20200043").Skip((pageNumber - 1) * pageSize).Take(pageSize);
            }
            var total = requestForms.Count();
            var list = new PageView { rows = requestForms, total = total };
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion

        

        #region 需求申请待开发

        /// <summary>
        /// 需求申请接单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public void OrderByAjax(RequestForm requestForm)
        {
            RequestForm _requestForm = _IRequestFormQuery.GetModels(u => u.ID == requestForm.ID).FirstOrDefault();
            _requestForm.RequestStatus = requestForm.RequestStatus;
            _IRequestFormQuery.Update(_requestForm);

            RequestFormBug requestFormBug = new RequestFormBug();
            requestFormBug.RequestFormID = _requestForm.ID;
            requestFormBug.Name = _requestForm.Name;
            requestFormBug.SysType = _requestForm.SysType;
            requestFormBug.Applier = _requestForm.Applier;
            requestFormBug.RequestStatus = _requestForm.RequestStatus;
            _requestFormBugDal.Add(requestFormBug);
        }

        // GET: RequestForms
        public ActionResult OrderIndex()
        {
            //var requestForms = db.RequestForms.Include(r => r.UserCreate).Include(r => r.UserUpdate);
            var requestForms = db.RequestForms.Where(x => x.RequestStatus == "SI20200045").ToList();

            return View(requestForms);
        }

        //GET: //GetRequestFormList
        [HttpGet]
        public ActionResult GetRequestFormOrderList()
        {
            var pageSize = Request["rows"] == "" ? 10 : int.Parse(Request["rows"]);
            var pageNumber = Request["page"] == "" ? 1 : int.Parse(Request["page"]);
            string name = string.Empty;
            if (Request["RequestFormName"] != "")
            {
                name = Request["RequestFormName"];
            }
            IQueryable<RequestForm> requestForms;
            if (!string.IsNullOrEmpty(name))
            {
                requestForms = db.RequestForms.OrderBy(x => x.ID).Where(x => x.Name.Contains(name) && (x.RequestStatus == "SI20200045")).Skip((pageNumber - 1) * pageSize).Take(pageSize);
            }
            else
            {
                requestForms = db.RequestForms.OrderBy(x => x.ID).Where(x => x.RequestStatus == "SI20200045").Skip((pageNumber - 1) * pageSize).Take(pageSize);
            }
            var total = requestForms.Count();
            var list = new PageView { rows = requestForms, total = total };
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region 需求申请开发中

        /// <summary>
        /// 需求申请测试
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public void TestByAjax(RequestForm requestForm)
        {
            RequestForm _requestForm = _IRequestFormQuery.GetModels(u => u.ID == requestForm.ID).FirstOrDefault();
            _requestForm.RequestStatus = requestForm.RequestStatus;
            _IRequestFormQuery.Update(_requestForm);

            RequestFormBug requestFormBug = new RequestFormBug();
            requestFormBug.RequestFormID = _requestForm.ID;
            requestFormBug.Name = _requestForm.Name;
            requestFormBug.SysType = _requestForm.SysType;
            requestFormBug.Applier = _requestForm.Applier;
            requestFormBug.RequestStatus = _requestForm.RequestStatus;
            _requestFormBugDal.Add(requestFormBug);
        }

        /// <summary>
        /// 需求申请延期
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public void BackByAjax(RequestForm requestForm)
        {
            RequestForm _requestForm = _IRequestFormQuery.GetModels(u => u.ID == requestForm.ID).FirstOrDefault();
            _requestForm.RequestStatus = requestForm.RequestStatus;
            _IRequestFormQuery.Update(_requestForm);

            RequestFormBug requestFormBug = new RequestFormBug();
            requestFormBug.RequestFormID = _requestForm.ID;
            requestFormBug.Name = _requestForm.Name;
            requestFormBug.SysType = _requestForm.SysType;
            requestFormBug.Applier = _requestForm.Applier;
            requestFormBug.RequestStatus = _requestForm.RequestStatus;
            _requestFormBugDal.Add(requestFormBug);
        }

        /// <summary>
        /// 需求申请指派
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public void PointByAjax(RequestForm requestForm)
        {
            RequestFormTask requestFormTask = new RequestFormTask();
            requestFormTask.RequestFormID = requestForm.ID;
            _requestFormTaskDal.Add(requestFormTask); //更新至估计Tablessss

        }

        // GET: RequestForms
        public ActionResult DevelopIndex()
        {
            //var requestForms = db.RequestForms.Include(r => r.UserCreate).Include(r => r.UserUpdate);
            var requestForms = db.RequestForms.Where(x => x.RequestStatus == "SI20200055" ||
                                                          x.RequestStatus == "SI20200155").ToList();

            return View(requestForms);
        }

        //GET: //GetRequestFormList
        [HttpGet]
        public ActionResult GetRequestFormDevelopList()
        {
            var pageSize = Request["rows"] == "" ? 10 : int.Parse(Request["rows"]);
            var pageNumber = Request["page"] == "" ? 1 : int.Parse(Request["page"]);
            string name = string.Empty;
            if (Request["RequestFormName"] != "")
            {
                name = Request["RequestFormName"];
            }
            IQueryable<RequestForm> requestForms;
            if (!string.IsNullOrEmpty(name))
            {
                requestForms = db.RequestForms.OrderBy(x => x.ID).Where(x => x.Name.Contains(name) && (x.RequestStatus == "SI20200055" || x.RequestStatus == "SI20200155")).Skip((pageNumber - 1) * pageSize).Take(pageSize);
            }
            else
            {
                requestForms = db.RequestForms.OrderBy(x => x.ID).Where(x => x.RequestStatus == "SI20200055" || x.RequestStatus == "SI20200155").Skip((pageNumber - 1) * pageSize).Take(pageSize);
            }
            var total = requestForms.Count();
            var list = new PageView { rows = requestForms, total = total };
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 需求申请待验收

        /// <summary>
        /// 需求申请验收
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public void CheckByAjax(RequestForm requestForm)
        {
            RequestForm _requestForm = _IRequestFormQuery.GetModels(u => u.ID == requestForm.ID).FirstOrDefault();
            _requestForm.RequestStatus = requestForm.RequestStatus;
            _IRequestFormQuery.Update(_requestForm);

            RequestFormBug requestFormBug = new RequestFormBug();
            requestFormBug.RequestFormID = _requestForm.ID;
            requestFormBug.Name = _requestForm.Name;
            requestFormBug.SysType = _requestForm.SysType;
            requestFormBug.Applier = _requestForm.Applier;
            requestFormBug.RequestStatus = _requestForm.RequestStatus;
            _requestFormBugDal.Add(requestFormBug);
        }

        // GET: RequestForms
        public ActionResult CheckIndex()
        {
            var requestForms = db.RequestForms.Where(x => x.RequestStatus == "SI20200057" || x.RequestStatus == "SI20200059").ToList();
            return View(requestForms);
        }

        //GET: //GetRequestFormList
        [HttpGet]
        public ActionResult GetRequestFormCheckList()
        {
            var pageSize = Request["rows"] == "" ? 10 : int.Parse(Request["rows"]);
            var pageNumber = Request["page"] == "" ? 1 : int.Parse(Request["page"]);
            string name = string.Empty;
            if (Request["RequestFormName"] != "")
            {
                name = Request["RequestFormName"];
            }
            IQueryable<RequestForm> requestForms;
            if (!string.IsNullOrEmpty(name))
            {
                requestForms = db.RequestForms.OrderBy(x => x.ID).Where(x => x.Name.Contains(name) && (x.RequestStatus == "SI20200057" || x.RequestStatus == "SI20200059")).Skip((pageNumber - 1) * pageSize).Take(pageSize);
            }
            else
            {
                requestForms = db.RequestForms.OrderBy(x => x.ID).Where(x => x.RequestStatus == "SI20200057" || x.RequestStatus == "SI20200059").Skip((pageNumber - 1) * pageSize).Take(pageSize);
            }
            var total = requestForms.Count();
            var list = new PageView { rows = requestForms, total = total };
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion

        #region 需求查询

        // GET: RequestForms
        public ActionResult Search(RequestForm requestForm )
        {
            //var requestForms = db.RequestForms.Include(r => r.UserCreate).Include(r => r.UserUpdate);
            List<RequestForm> requestforms = db.RequestForms.Where(item => item.ID == requestForm.ID).ToList();
            return View("Index",requestforms);
        }

       
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
        public ActionResult FileDownload()
        {
            //下载文件
            string path = Server.MapPath("~/File/需求问题附件.docx");
            FileStream fs = new FileStream(path, FileMode.Open);
            return File(fs, "word/docx", "需求问题附件.docx");



        }


        

        #endregion


        #endregion

        #region 框架自动生成
        // GET: RequestForms
        public ActionResult Index()
        {
            //var requestForms = db.RequestForms.Include(r => r.User);
            //return View(requestForms.ToList());
            return View(_IRequestFormQuery.GetModels(
                  u => true
                ).ToList());

        }

        // GET: RequestForms/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestForm requestForm = _IRequestFormQuery.GetModels(u => u.ID==id).FirstOrDefault();
            if (requestForm == null)
            {
                return HttpNotFound();
            }
            return View(requestForm);
        }

        // GET: RequestForms/Create
        public ActionResult Create()
        {
            ViewBag.Applier = new SelectList(db.Users, "ID", "Code");
            RequestForm requestForm = new RequestForm();
            return View(requestForm);
        }

        // POST: RequestForms/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Code,Name,SysType,Description,FileName,Applier,RequestStatus,Status,CreateDate,CreatePerson,UpdateDate,UpdatePerson,Summary")] RequestForm requestForm)
        {
            if (ModelState.IsValid)
            {
                _IRequestFormQuery.Add(requestForm);
                return RedirectToAction("Index");
            }

            ViewBag.Applier = new SelectList(db.Users, "ID", "Code", requestForm.Applier);
            return View(requestForm);
        }

        // GET: RequestForms/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestForm requestForm = _IRequestFormQuery.GetModels(u => u.ID == id).FirstOrDefault();
            if (requestForm == null)
            {
                return HttpNotFound();
            }
            ViewBag.Applier = new SelectList(db.Users, "ID", "Code", requestForm.Applier);
            return View(requestForm);
        }

        // POST: RequestForms/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Code,Name,SysType,Description,FileName,Applier,RequestStatus,Status,CreateDate,CreatePerson,UpdateDate,UpdatePerson,Summary")] RequestForm requestForm)
        {
            if (ModelState.IsValid)
            {
                _IRequestFormQuery.Update(requestForm);
                return RedirectToAction("Index");
            }
            ViewBag.Applier = new SelectList(db.Users, "ID", "Code", requestForm.Applier);
            return View(requestForm);
        }

        // GET: RequestForms/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestForm requestForm = db.RequestForms.Find(id);
            if (requestForm == null)
            {
                return HttpNotFound();
            }
            return View(requestForm);
        }

        // POST: RequestForms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            RequestForm requestForm = db.RequestForms.Find(id);
            db.RequestForms.Remove(requestForm);
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

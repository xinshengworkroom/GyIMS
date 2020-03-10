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
using GyIMS.Attributes;
using GyIMS.Enums;
using GyIMS.Helper;
using GyIMS.Helper.Container;
using GyIMS.Helper.Currency;
using GyIMS.Models;

namespace GyIMS.Controllers
{
    public class MaintenancesController : Controller
    {
        private DBContext db = new DBContext();

        private IMaintenanceDal _IMaintenanceQuery { get; set; }//= UserContainer.Resolve<IUserDal>();
        private IMaintenanceFeeDal _IMaintenanceFeeDal = BaseContainer.Resolve<IMaintenanceFeeDal,MaintenanceFeeDal>();
        private IMaintenanceDal _IMaintenanceDal = BaseContainer.Resolve<IMaintenanceDal, MaintenanceDal>();
        private IMaintenanceLogDal _IMaintenanceLogDal = BaseContainer.Resolve<IMaintenanceLogDal, MaintenanceLogDal>();
        private IMaintenanceHandlerDal _IMaintenanceHandlerDal = BaseContainer.Resolve<IMaintenanceHandlerDal, MaintenanceHandlerDal>();

        public MaintenancesController(IMaintenanceDal maintenanceDal)
        {
            _IMaintenanceQuery = maintenanceDal;
        }

        MaintenanceFee _MaintenanceFee = new MaintenanceFee();
        MaintenanceFee _UpdateMaintenanceFee = new MaintenanceFee();
        BaseIDT _BaseIDT = new BaseIDT();
        IBaseIDT _IBaseIDT = new IDTMaintenanceFees();

        #region 自定义功能函数

        #region 查询界面

        //GET: //GetMaintenanceList
        [HttpGet]
        public ActionResult GetMaintenanceList(Maintenance maintenance)
        {
            var pageSize = Request["rows"] == "" ? 10 : int.Parse(Request["rows"]);
            var pageNumber = Request["page"] == "" ? 1 : int.Parse(Request["page"]);
            string name = string.Empty;
            if (Request["Name"] != "")
            {
                name = Request["Name"];
            }
            IQueryable<Maintenance> maintenances;
            if (!string.IsNullOrEmpty(name))
            {
                maintenances = _IMaintenanceQuery.GetModelsByPage(
                    pageSize, pageNumber, true, u => u.Name, u => u.Name.Contains(name) &&
                    (u.MaintenanceStatus == "SI20200097" ||
                     u.MaintenanceStatus == "SI20200099" ||
                     u.MaintenanceStatus == "SI20200109")
            );
            }
            else
            {
                maintenances = _IMaintenanceQuery.GetModelsByPage(
                    pageSize, pageNumber, true, u => u.Name, u => u.Status == CommonStatusEnum.Able &&
                    (u.MaintenanceStatus == "SI20200097" ||
                     u.MaintenanceStatus == "SI20200099" ||
                     u.MaintenanceStatus == "SI20200109")
                    ) ;
            }
            var total = _IMaintenanceQuery.GetModels(u => true).Count();
            var list = new PageView { rows = maintenances, total = total };
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
        public ActionResult CreateByForm(Maintenance maintenance)
        {
            try
            {

                int result = _IMaintenanceQuery.Add(maintenance);
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
        public ActionResult EditByForm(Maintenance maintenance)
        {
            try
            {
                int result = _IMaintenanceQuery.Update(maintenance);
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
            Maintenance maintenance = db.Maintenances.Find(id);
            maintenance.Status = CommonStatusEnum.Disabled;
            db.SaveChanges();
        }


        #endregion

        #region 条件查找

        // GET: /Maintenances/
        public ActionResult Search(string id)
        {
            List<Maintenance> maintenances = _IMaintenanceQuery.GetModels(u => u.Name == id).ToList();
            return View("Index", maintenances);
        }

        #endregion

        #region 运维状态

        #region 运维申请

        /// <summary>
        /// 运维申请
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public void ApplyByAjax(Maintenance maintenance)
        {
            Maintenance _maintenance = _IMaintenanceDal.GetModels(u => u.ID == maintenance.ID).FirstOrDefault();
            _maintenance.MaintenanceStatus = maintenance.MaintenanceStatus;
            _IMaintenanceQuery.Update(_maintenance); //更新需求

            _BaseIDT.SetCon(_IBaseIDT);
            _MaintenanceFee.MaintenanceID = maintenance.ID;
            _MaintenanceFee.UserBeginTime = DateTime.Now;
            _BaseIDT.Connstring(_MaintenanceFee);
            _BaseIDT.Add();

            MaintenanceLog maintenanceLog = new MaintenanceLog();
            maintenanceLog.MaintenanceID = _maintenance.ID;
            maintenanceLog.OperType = _maintenance.MaintenanceStatus;
            maintenanceLog.Applier = _maintenance.Applier;
            maintenanceLog.Region = _maintenance.Region;
            maintenanceLog.Type = _maintenance.Type;
            maintenanceLog.SysType = _maintenance.SysType;
            maintenanceLog.HSysType = _maintenance.HSysType;
            maintenanceLog.Name = _maintenance.Name;
            _IMaintenanceLogDal.Add(maintenanceLog);
        }

        /// <summary>
        /// 运维申请作废
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public void CancelByAjax(Maintenance maintenance)
        {
            Maintenance _maintenance = _IMaintenanceQuery.GetModels(u => u.ID == maintenance.ID).FirstOrDefault();
            _maintenance.MaintenanceStatus = maintenance.MaintenanceStatus;
            _IMaintenanceQuery.Update(_maintenance);

            MaintenanceLog maintenanceLog = new MaintenanceLog();
            maintenanceLog.MaintenanceID = _maintenance.ID;
            maintenanceLog.OperType = _maintenance.MaintenanceStatus;
            maintenanceLog.Applier = _maintenance.Applier;
            maintenanceLog.Region = _maintenance.Region;
            maintenanceLog.Type = _maintenance.Type;
            maintenanceLog.SysType = _maintenance.SysType;
            maintenanceLog.HSysType = _maintenance.HSysType;
            maintenanceLog.Name = _maintenance.Name;
            _IMaintenanceLogDal.Add(maintenanceLog);
        }

        #endregion

        #region 运维待接单

        /// <summary>
        /// 运维申请接单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public void AcceptByAjax(Maintenance maintenance)
        {
            Maintenance _maintenance = _IMaintenanceDal.GetModels(u => u.ID == maintenance.ID).FirstOrDefault();
            _maintenance.MaintenanceStatus = maintenance.MaintenanceStatus;
            _IMaintenanceQuery.Update(_maintenance);

            _BaseIDT.SetCon(_IBaseIDT);
            _MaintenanceFee.ID = _IMaintenanceFeeDal.GetModels(u => u.MaintenanceID == maintenance.ID).FirstOrDefault().ID;
            _MaintenanceFee = _IMaintenanceFeeDal.GetModels(u => u.MaintenanceID == maintenance.ID).ToList()[0];
            _MaintenanceFee.BeginTime = DateTime.Now;
            _BaseIDT.Connstring(_MaintenanceFee);
            _BaseIDT.Update();

            MaintenanceLog maintenanceLog = new MaintenanceLog();
            maintenanceLog.MaintenanceID = _maintenance.ID;
            maintenanceLog.OperType = _maintenance.MaintenanceStatus;
            maintenanceLog.Applier = _maintenance.Applier;
            maintenanceLog.Region = _maintenance.Region;
            maintenanceLog.Type = _maintenance.Type;
            maintenanceLog.SysType = _maintenance.SysType;
            maintenanceLog.HSysType = _maintenance.HSysType;
            maintenanceLog.Name = _maintenance.Name;
            _IMaintenanceLogDal.Add(maintenanceLog);

        }



        /// <summary>
        /// 运维申请退回
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public void ReturnByAjax(Maintenance maintenance)
        {
            Maintenance _maintenance = _IMaintenanceDal.GetModels(u => u.ID == maintenance.ID).FirstOrDefault();
            _maintenance.MaintenanceStatus = maintenance.MaintenanceStatus;
            _IMaintenanceQuery.Update(_maintenance);

            MaintenanceLog maintenanceLog = new MaintenanceLog();
            maintenanceLog.MaintenanceID = _maintenance.ID;
            maintenanceLog.OperType = _maintenance.MaintenanceStatus;
            maintenanceLog.Applier = _maintenance.Applier;
            maintenanceLog.Region = _maintenance.Region;
            maintenanceLog.Type = _maintenance.Type;
            maintenanceLog.SysType = _maintenance.SysType;
            maintenanceLog.HSysType = _maintenance.HSysType;
            maintenanceLog.Name = _maintenance.Name;
            _IMaintenanceLogDal.Add(maintenanceLog);
        }

        // GET: Maintenances
        public ActionResult ApplyIndex()
        {
            //var maintenances = db.Maintenances.Include(m => m.UserCreate).Include(m => m.UserUpdate);
            var maintenances = db.Maintenances.Where(x => x.MaintenanceStatus == "SI20200099").ToList();
            return View(maintenances);
        }

        //GET: //GetMaintenanceList
        [HttpGet]
        public ActionResult GetMaintenanceApplyList()
        {
            var pageSize = Request["rows"] == "" ? 10 : int.Parse(Request["rows"]);
            var pageNumber = Request["page"] == "" ? 1 : int.Parse(Request["page"]);
            string name = string.Empty;
            if (Request["Name"] != "")
            {
                name = Request["Name"];
            }
            IQueryable<Maintenance> maintenances;
            if (!string.IsNullOrEmpty(name))
            {
                maintenances = db.Maintenances.OrderBy(x => x.Name).Where(x => x.Name.Contains(name) && (x.MaintenanceStatus == "SI20200099")).Skip((pageNumber - 1) * pageSize).Take(pageSize);
            }
            else
            {
                maintenances = db.Maintenances.OrderBy(x => x.ID).Where(x => x.MaintenanceStatus == "SI20200099").Skip((pageNumber - 1) * pageSize).Take(pageSize);
            }
            var total = maintenances.Count();
            var list = new PageView { rows = maintenances, total = total };
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 运维待处理

        /// <summary>
        /// 运维处理
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public void PlayByAjax(Maintenance maintenance)
        {
            Maintenance _maintenance = _IMaintenanceDal.GetModels(u => u.ID == maintenance.ID).FirstOrDefault();
            _maintenance.MaintenanceStatus = maintenance.MaintenanceStatus;
            _IMaintenanceQuery.Update(_maintenance);

            MaintenanceLog maintenanceLog = new MaintenanceLog();
            maintenanceLog.MaintenanceID = _maintenance.ID;
            maintenanceLog.OperType = _maintenance.MaintenanceStatus;
            maintenanceLog.Applier = _maintenance.Applier;
            maintenanceLog.Region = _maintenance.Region;
            maintenanceLog.Type = _maintenance.Type;
            maintenanceLog.SysType = _maintenance.SysType;
            maintenanceLog.HSysType = _maintenance.HSysType;
            maintenanceLog.Name = _maintenance.Name;
            _IMaintenanceLogDal.Add(maintenanceLog);
        }

        // GET: Maintenances
        public ActionResult AcceptIndex()
        {
            //var maintenances = db.Maintenances.Include(m => m.UserCreate).Include(m => m.UserUpdate);
            //var maintenances = db.Maintenances.Where(x => x.MaintenanceStatus == MaintenanceStatusEnum.Two).ToList();
            return View(db.Maintenances.Where(x => x.MaintenanceStatus == "SI20200101").ToList());
        }

        //GET: //GetMaintenanceList
        [HttpGet]
        public ActionResult GetMaintenanceAcceptList()
        {
            var pageSize = Request["rows"] == "" ? 10 : int.Parse(Request["rows"]);
            var pageNumber = Request["page"] == "" ? 1 : int.Parse(Request["page"]);
            string name = string.Empty;
            if (Request["Name"] != "")
            {
                name = Request["Name"];
            }
            IQueryable<Maintenance> maintenances;
            if (!string.IsNullOrEmpty(name))
            {
                maintenances = db.Maintenances.OrderBy(x => x.Name).Where(x => x.Name.Contains(name) && (x.MaintenanceStatus == "SI20200101")).Skip((pageNumber - 1) * pageSize).Take(pageSize);
            }
            else
            {
                maintenances = db.Maintenances.OrderBy(x => x.ID).Where(x => x.MaintenanceStatus == "SI20200101").Skip((pageNumber - 1) * pageSize).Take(pageSize);
            }
            var total = maintenances.Count();
            var list = new PageView { rows = maintenances, total = total };
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 运维处理中

        /// <summary>
        /// 运维解决
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public void HandleByAjax(Maintenance maintenance)
        {
            Maintenance _maintenance = _IMaintenanceDal.GetModels(u => u.ID == maintenance.ID).FirstOrDefault();
            _maintenance.MaintenanceStatus = maintenance.MaintenanceStatus;
            _IMaintenanceQuery.Update(_maintenance);

            _BaseIDT.SetCon(_IBaseIDT);
            _MaintenanceFee.ID = _IMaintenanceFeeDal.GetModels(u => u.MaintenanceID == maintenance.ID).FirstOrDefault().ID;
            _MaintenanceFee = _IMaintenanceFeeDal.GetModels(u => u.MaintenanceID == maintenance.ID).ToList()[0];
            _MaintenanceFee.EndTime = DateTime.Now;
            _BaseIDT.Connstring(_MaintenanceFee);
            _BaseIDT.Update();

            MaintenanceLog maintenanceLog = new MaintenanceLog();
            maintenanceLog.MaintenanceID = _maintenance.ID;
            maintenanceLog.OperType = _maintenance.MaintenanceStatus;
            maintenanceLog.Applier = _maintenance.Applier;
            maintenanceLog.Region = _maintenance.Region;
            maintenanceLog.Type = _maintenance.Type;
            maintenanceLog.SysType = _maintenance.SysType;
            maintenanceLog.HSysType = _maintenance.HSysType;
            maintenanceLog.Name = _maintenance.Name;
            _IMaintenanceLogDal.Add(maintenanceLog);
        }

        /// <summary>
        /// 运维申请挂起
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public void Hang_upByAjax(Maintenance maintenance)
        {
            Maintenance _maintenance = _IMaintenanceDal.GetModels(u => u.ID == maintenance.ID).FirstOrDefault();
            _maintenance.MaintenanceStatus = maintenance.MaintenanceStatus;
            _IMaintenanceQuery.Update(_maintenance);

            _BaseIDT.SetCon(_IBaseIDT);
            _MaintenanceFee.ID = _IMaintenanceFeeDal.GetModels(u => u.MaintenanceID == maintenance.ID).FirstOrDefault().ID;
            _MaintenanceFee = _IMaintenanceFeeDal.GetModels(u => u.MaintenanceID == maintenance.ID).ToList()[0];
            _MaintenanceFee.DelayTime = DateTime.Now;
            _BaseIDT.Connstring(_MaintenanceFee);
            _BaseIDT.Update();

            MaintenanceLog maintenanceLog = new MaintenanceLog();
            maintenanceLog.MaintenanceID = _maintenance.ID;
            maintenanceLog.OperType = _maintenance.MaintenanceStatus;
            maintenanceLog.Applier = _maintenance.Applier;
            maintenanceLog.Region = _maintenance.Region;
            maintenanceLog.Type = _maintenance.Type;
            maintenanceLog.SysType = _maintenance.SysType;
            maintenanceLog.HSysType = _maintenance.HSysType;
            maintenanceLog.Name = _maintenance.Name;
            _IMaintenanceLogDal.Add(maintenanceLog);
        }

        // GET: Maintenances
        public ActionResult HandleIndex()
        {
            //var maintenances = db.Maintenances.Include(m => m.UserCreate).Include(m => m.UserUpdate);
            //var maintenances = db.Maintenances.Where(x => x.MaintenanceStatus == MaintenanceStatusEnum.Two).ToList();
            return View(db.Maintenances.Where(x => x.MaintenanceStatus == "SI20200103" || 
                                                   x.MaintenanceStatus == "SI20200113"
                                                   ).ToList());
        }

        //GET: //GetMaintenanceList
        [HttpGet]
        public ActionResult GetMaintenanceHandleList()
        {
            var pageSize = Request["rows"] == "" ? 10 : int.Parse(Request["rows"]);
            var pageNumber = Request["page"] == "" ? 1 : int.Parse(Request["page"]);
            string name = string.Empty;
            if (Request["Name"] != "")
            {
                name = Request["Name"];
            }
            IQueryable<Maintenance> maintenances;
            if (!string.IsNullOrEmpty(name))
            {
                maintenances = db.Maintenances.OrderBy(x => x.Name).Where(x => x.Name.Contains(name) && 
                                                                              (x.MaintenanceStatus == "SI20200103" || 
                                                                               x.MaintenanceStatus == "SI20200113"))
                                                                               .Skip((pageNumber - 1) * pageSize).Take(pageSize);
            }
            else
            {
                maintenances = db.Maintenances.OrderBy(x => x.ID).Where(x => x.MaintenanceStatus == "SI20200103" || 
                                                                             x.MaintenanceStatus == "SI20200113").Skip((pageNumber - 1) * pageSize).Take(pageSize);
            }
            var total = maintenances.Count();
            var list = new PageView { rows = maintenances, total = total };
            return Json(list, JsonRequestBehavior.AllowGet);
        }





        #endregion

        #region 运维待结案

        /// <summary>
        /// 运维申请结案
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public void EndByAjax(Maintenance maintenance)
        {
            Maintenance _maintenance = _IMaintenanceDal.GetModels(u => u.ID == maintenance.ID).FirstOrDefault();
            _maintenance.MaintenanceStatus = maintenance.MaintenanceStatus;
            _IMaintenanceQuery.Update(_maintenance);

            _BaseIDT.SetCon(_IBaseIDT);
            _MaintenanceFee.ID = _IMaintenanceFeeDal.GetModels(u => u.MaintenanceID == maintenance.ID).FirstOrDefault().ID;
            _MaintenanceFee = _IMaintenanceFeeDal.GetModels(u => u.MaintenanceID == maintenance.ID).ToList()[0];
            _MaintenanceFee.UserEndTime = DateTime.Now;
            _BaseIDT.Connstring(_MaintenanceFee);
            _BaseIDT.Update();

            MaintenanceLog maintenanceLog = new MaintenanceLog();
            maintenanceLog.MaintenanceID = _maintenance.ID;
            maintenanceLog.OperType = _maintenance.MaintenanceStatus;
            maintenanceLog.Applier = _maintenance.Applier;
            maintenanceLog.Region = _maintenance.Region;
            maintenanceLog.Type = _maintenance.Type;
            maintenanceLog.SysType = _maintenance.SysType;
            maintenanceLog.HSysType = _maintenance.HSysType;
            maintenanceLog.Name = _maintenance.Name;
            _IMaintenanceLogDal.Add(maintenanceLog);
        }

        // GET: Maintenances
        public ActionResult EndIndex()
        {
            //var maintenances = db.Maintenances.Include(m => m.UserCreate).Include(m => m.UserUpdate);
            return View(_IMaintenanceQuery.GetModels(u => u.MaintenanceStatus == "SI20200105" || 
                                                          u.MaintenanceStatus == "SI20200107"

            ).ToList());
        }

        //GET: //GetMaintenanceList
        [HttpGet]
        public ActionResult GetMaintenanceEndList()
        {
            var pageSize = Request["rows"] == "" ? 10 : int.Parse(Request["rows"]);
            var pageNumber = Request["page"] == "" ? 1 : int.Parse(Request["page"]);
            string name = string.Empty;
            if (Request["Name"] != "")
            {
                name = Request["Name"];
            }
            IQueryable<Maintenance> maintenances;
            if (!string.IsNullOrEmpty(name))
            {
                maintenances = _IMaintenanceQuery.GetModelsByPage(
                    pageSize, pageNumber, true, u => u.Name, u => u.Name.Contains(name)

                    && (u.MaintenanceStatus == "SI20200105" || 
                        u.MaintenanceStatus == "SI20200107"
                    )
            );
            }
            else
            {
                maintenances = _IMaintenanceQuery.GetModelsByPage(
                    pageSize, pageNumber, true, u => u.Name,
                    u => u.MaintenanceStatus == "SI20200105" || 
                         u.MaintenanceStatus == "SI20200107"


                    );
            }
            var total = maintenances.Count();
            var list = new PageView { rows = maintenances, total = total };
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 运维移交

        [HttpPost]
        public void TranByAjax(Maintenance maintenance)
        {
            Maintenance _maintenance = _IMaintenanceDal.GetModels(u => u.ID == maintenance.ID).FirstOrDefault();
           
            MaintenanceHandler maintenanceHandler = new MaintenanceHandler();
            maintenanceHandler.MaintenanceID = _maintenance.ID;
            _IMaintenanceHandlerDal.Add(maintenanceHandler);
        }

        #endregion

        #endregion

        #region 运维查询



        // GET: Maintenances
        public ActionResult SearchIndex()
        {
            //var maintenances = db.Maintenances.Include(m => m.UserCreate).Include(m => m.UserUpdate);
            //var maintenances = db.Maintenances.Where(x => x.MaintenanceStatus == MaintenanceStatusEnum.Two).ToList();
            return View(_IMaintenanceQuery.GetModels(u => u.Status == CommonStatusEnum.Able).ToList());
        }

        //GET: //GetMaintenanceList
        [HttpGet]
        public ActionResult GetMaintenanceSearchList()
        {
            var pageSize = Request["rows"] == "" ? 10 : int.Parse(Request["rows"]);
            var pageNumber = Request["page"] == "" ? 1 : int.Parse(Request["page"]);
            string name = string.Empty;
            if (Request["Name"] != "")
            {
                name = Request["Name"];
            }
            IQueryable<Maintenance> maintenances;
            if (!string.IsNullOrEmpty(name))
            {
                maintenances = _IMaintenanceQuery.GetModelsByPage(pageSize, pageNumber, true, u => u.Name, u => u.Code.Contains(name)&& u.Status == CommonStatusEnum.Able );
            }
            else
            {
                maintenances = _IMaintenanceQuery.GetModelsByPage(pageSize, pageNumber, true, u => u.Name, u => u.Status == CommonStatusEnum.Able);
            }
            var total = maintenances.Count();
            var list = new PageView { rows = maintenances, total = total };
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 公共函数


        /// <summary>
        /// 获取状态
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetCommonStatuses()
        {
            List<ComboboxCommon> list = CommonHelper.GetCommonStatuses();
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 获取问题类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetMTypes()
        {
            List<ComboboxCommon> list = CommonHelper.GetMTypes();
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 获取软件系统类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetSysTypes()
        {
            List<ComboboxCommon> list = CommonHelper.GetSysTypes();
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 获取硬件系统类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetHSysTypes()
        {
            List<ComboboxCommon> list = CommonHelper.GetHSysTypes();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取运维状态
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetMaintenanceStatus()
        {
            List<ComboboxCommon> list = CommonHelper.GetMaintenanceStatus();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取申请区域
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetRegions()
        {
            List<ComboboxCommon> list = CommonHelper.GetRegions();
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

        #region 文件上传 下载

        //[HttpPost]
        //public ActionResult UploadFile(HttpPostedFileBase file)
        //{
        //    var fileName = file.FileName;
        //    var filePath = Server.MapPath(string.Format("~/{0}", "File"));
        //    file.SaveAs(Path.Combine(filePath, fileName));
        //    return View();
        //}

        //public ActionResult File(HttpPostedFileBase file)
        //{
        //    string path = Server.MapPath("~/File");
        //    string filename = Path.Combine(path, file.FileName);
        //    file.SaveAs(filename);
        //    //return View(Index);
        //    return RedirectToAction("Create");
        //}
        //public ActionResult FileDownload()
        //{
        //    //下载文件
        //    string path = Server.MapPath("~/File/运维问题附件.docx");
        //    FileStream fs = new FileStream(path, FileMode.Open);
        //    return File(fs, "word/docx", "运维问题附件.docx");



        //}

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public ActionResult FileUpload()
        {
            List<string> result = new List<string>();
            HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
            if (files != null && files.Count > 0)
            {
                string path = Server.MapPath("~/UploadFile");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFile file = files[i];
                    if (!string.IsNullOrEmpty(file.FileName))
                    {
                        string extName = Path.GetExtension(file.FileName);
                        string fname = Path.GetFileNameWithoutExtension(file.FileName);
                        string savaName = string.Concat(fname, DateTime.Now.ToString("_yyyyMMddHHmmss"), extName);
                        string filename = Path.Combine(path, savaName);
                        file.SaveAs(filename);
                        result.Add(savaName);
                    }
                }

                //foreach (HttpPostedFile file in files)
                //{
                //    string extName = Path.GetExtension(file.FileName);
                //    string fname = Path.GetFileNameWithoutExtension(file.FileName);
                //    string savaName = string.Concat(fname, DateTime.Now.ToString("yyyyMMddHHmmss"), extName);
                //    string filename = Path.Combine(path, savaName);
                //    file.SaveAs(filename);
                //    result.Add(savaName);
                //}
            }
            else
            {
                return Content("请先选择文件");
            }
            //return RedirectToAction("Create", new { filename = file.FileName });
            return Json(new { FileNames = result},JsonRequestBehavior.DenyGet);
        }

        /// <summary>
        /// 文件下载
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public ActionResult FileDownload(string filename)
        {
            //下载文件
            string path = Server.MapPath("~/UploadFile/" + filename);
            if (System.IO.File.Exists(path))
            {
                FileStream fs = new FileStream(path, FileMode.Open);
                return File(fs, "application/octet-stream", filename);
            }
            else
            {
                return Content("文件不存在");
            }
        }

        #endregion

        #endregion

        #region 框架自动生成
        // GET: Maintenances
        public ActionResult Index()
        {
            var maintenance = db.Maintenances.Where(u => u.MaintenanceStatus == "SI20200097" ||
                                                         u.MaintenanceStatus == "SI20200099" ||
                                                         u.MaintenanceStatus == "SI20200109").ToList();
            return View(maintenance);
            

        }

        // GET: Maintenances/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Maintenance maintenance = _IMaintenanceQuery.GetModels(e =>e.ID ==id).FirstOrDefault();
            if (maintenance == null)
            {
                return HttpNotFound();
            }
            return View(maintenance);
        }

        // GET: Maintenances/Create
        public ActionResult Create()
        {
            ViewBag.CreatePerson = new SelectList(db.Users, "ID", "Code");
            ViewBag.Applier = new SelectList(db.Users, "ID", "Code");
            ViewBag.UpdatePerson = new SelectList(db.Users, "ID", "Code");
            Maintenance maintenance = new Maintenance();
            return View(maintenance);
        }

        // POST: Maintenances/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Code,Applier,Region,Type,SysType,HSysType,Name,Description,FileName,MaintenanceStatus,Status,CreateDate,CreatePerson,UpdateDate,UpdatePerson,Summary")] Maintenance maintenance)
        {
            if (ModelState.IsValid)
            {
                _IMaintenanceQuery.Add(maintenance);
                return RedirectToAction("Index");
            }

            ViewBag.CreatePerson = new SelectList(db.Users, "ID", "Code", maintenance.CreatePerson);
            ViewBag.Applier = new SelectList(db.Users, "ID", "Code", maintenance.Applier);
            ViewBag.UpdatePerson = new SelectList(db.Users, "ID", "Code", maintenance.UpdatePerson);
            return View(maintenance);
        }

        // GET: Maintenances/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Maintenance maintenance = _IMaintenanceQuery.GetModels(e => e.ID == id).FirstOrDefault();
            if (maintenance == null)
            {
                return HttpNotFound();
            }
            ViewBag.CreatePerson = new SelectList(db.Users, "ID", "Code", maintenance.CreatePerson);
            ViewBag.Applier = new SelectList(db.Users, "ID", "Code", maintenance.Applier);
            ViewBag.UpdatePerson = new SelectList(db.Users, "ID", "Code", maintenance.UpdatePerson);
            return View(maintenance);
        }

        // POST: Maintenances/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Code,Applier,Region,Type,SysType,HSysType,Name,Description,FileName,MaintenanceStatus,Status,CreateDate,CreatePerson,UpdateDate,UpdatePerson,Summary")] Maintenance maintenance)
        {
            if (ModelState.IsValid)
            {
                _IMaintenanceQuery.Update(maintenance);
                return RedirectToAction("Index");
            }
            ViewBag.CreatePerson = new SelectList(db.Users, "ID", "Code", maintenance.CreatePerson);
            ViewBag.Applier = new SelectList(db.Users, "ID", "Code", maintenance.Applier);
            ViewBag.UpdatePerson = new SelectList(db.Users, "ID", "Code", maintenance.UpdatePerson);
            return View(maintenance);
        }

        // GET: Maintenances/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Maintenance maintenance = db.Maintenances.Find(id);
            if (maintenance == null)
            {
                return HttpNotFound();
            }
            return View(maintenance);
        }

        // POST: Maintenances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Maintenance maintenance = db.Maintenances.Find(id);
            db.Maintenances.Remove(maintenance);
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

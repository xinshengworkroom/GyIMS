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
    public class MaintenanceHandlersController : Controller
    {
        private DBContext db = new DBContext();
        private IMaintenanceHandlerDal _IMaintenanceHandlerQuery { get; set; }//= UserContainer.Resolve<IUserDal>();

        public MaintenanceHandlersController(IMaintenanceHandlerDal maintenanceHandlerDal)
        {
            _IMaintenanceHandlerQuery = maintenanceHandlerDal;
        }

        #region 自定义功能函数

        #region 查询界面

        //GET: //GetMaintenanceHandlerList
        [HttpGet]
        public ActionResult GetMaintenanceHandlerList()
        {
            var pageSize = Request["rows"] == "" ? 10 : int.Parse(Request["rows"]);
            var pageNumber = Request["page"] == "" ? 1 : int.Parse(Request["page"]);
            string name = string.Empty;
            if (Request["MaintenanceName"] != "")
            {
                name = Request["MaintenanceName"];
            }
            IQueryable<MaintenanceHandler> maintenanceHandlers;
            if (!string.IsNullOrEmpty(name))
            {
                maintenanceHandlers = _IMaintenanceHandlerQuery.GetModelsByPage(pageSize, pageNumber, true, u => u.Handler, u => u.ID.Contains(name) && u.Status == CommonStatusEnum.Able);
            }
            else
            {
                maintenanceHandlers = _IMaintenanceHandlerQuery.GetModelsByPage(pageSize, pageNumber, true, u => u.ID, u => u.Status == CommonStatusEnum.Able);
            }
            var total = maintenanceHandlers.Count();
            var list = new PageView { rows = maintenanceHandlers, total = total };
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
        public ActionResult CreateByForm(MaintenanceHandler maintenanceHandler)
        {
            try
            {

                int result = _IMaintenanceHandlerQuery.Add(maintenanceHandler);
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
        public ActionResult EditByForm(MaintenanceHandler maintenanceHandler)
        {
            try
            {

                int result = _IMaintenanceHandlerQuery.Update(maintenanceHandler);
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
            MaintenanceHandler maintenanceHandler = db.MaintenanceHandlers.Find(id);
            maintenanceHandler.Status = CommonStatusEnum.Disabled;
            db.SaveChanges();
        }


        #endregion

        #region 转移界面

        /// <summary>
        /// 通过表单提交
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult TranByForm(MaintenanceHandler maintenanceHandler)
        {
            try
            {

                int result = _IMaintenanceHandlerQuery.Update(maintenanceHandler);
                return Json(new { Code = 1, Message = "保存成功" }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                return Json(new { Code = -1, Message = ex.Message }, JsonRequestBehavior.DenyGet);
            }

        }

        // GET: MaintenanceHandlers/Edit/5
        public ActionResult Tran(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MaintenanceHandler maintenanceHandler = _IMaintenanceHandlerQuery.GetModels(u => u.ID == id).FirstOrDefault();
            if (maintenanceHandler == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaintenanceID = new SelectList(db.Maintenances, "ID", "Code", maintenanceHandler.MaintenanceID);
            ViewBag.FromHandler = new SelectList(db.MaintenanceFees, "ID", "MaintenanceID", maintenanceHandler.FromHandler);
            ViewBag.CreatePerson = new SelectList(db.Users, "ID", "Code", maintenanceHandler.CreatePerson);
            return View(maintenanceHandler);
        }

        // POST: MaintenanceHandlers/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Tran([Bind(Include = "ID,MaintenanceID,FromHandler,Handler,Status,CreateDate,CreatePerson")] MaintenanceHandler maintenanceHandler)
        {
            if (ModelState.IsValid)
            {
                _IMaintenanceHandlerQuery.Update(maintenanceHandler);
                return RedirectToAction("Index");
            }
            ViewBag.MaintenanceID = new SelectList(db.Maintenances, "ID", "Code", maintenanceHandler.MaintenanceID);
            ViewBag.FromHandler = new SelectList(db.MaintenanceFees, "ID", "MaintenanceID", maintenanceHandler.FromHandler);
            ViewBag.CreatePerson = new SelectList(db.Users, "ID", "Code", maintenanceHandler.CreatePerson);
            return View(maintenanceHandler);
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
        /// 获取运维单号
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetMaintenances()
        {
            List<ComboboxCommon> list = CommonHelper.GetMaintenances();
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        
        #endregion

        #endregion

        #region 框架自动生成

        // GET: MaintenanceHandlers
        public ActionResult Index()
        {
            //var maintenanceHandlers = db.MaintenanceHandlers.Include(m => m.Maintenance).Include(m => m.MaintenanceFee).Include(m => m.UserCreate);
            //return View(maintenanceHandlers.ToList());
            return View(_IMaintenanceHandlerQuery.GetModels(u => u.ID != string.Empty).ToList());

        }

        // GET: MaintenanceHandlers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MaintenanceHandler maintenanceHandler = _IMaintenanceHandlerQuery.GetModels(u =>u.ID==id).FirstOrDefault();
            if (maintenanceHandler == null)
            {
                return HttpNotFound();
            }
            return View(maintenanceHandler);
        }

        // GET: MaintenanceHandlers/Create
        public ActionResult Create()
        {
            ViewBag.MaintenanceID = new SelectList(db.Maintenances, "ID", "Code");
            ViewBag.FromHandler = new SelectList(db.MaintenanceFees, "ID", "MaintenanceID");
            ViewBag.CreatePerson = new SelectList(db.Users, "ID", "Code");
            MaintenanceHandler maintenanceHandler = new MaintenanceHandler();
            return View(maintenanceHandler);
        }

        // POST: MaintenanceHandlers/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,MaintenanceID,FromHandler,Handler,Status,CreateDate,CreatePerson")] MaintenanceHandler maintenanceHandler)
        {
            if (ModelState.IsValid)
            {
                _IMaintenanceHandlerQuery.Add(maintenanceHandler);
                return RedirectToAction("Index");
            }

            ViewBag.MaintenanceID = new SelectList(db.Maintenances, "ID", "Code", maintenanceHandler.MaintenanceID);
            ViewBag.FromHandler = new SelectList(db.MaintenanceFees, "ID", "MaintenanceID", maintenanceHandler.FromHandler);
            ViewBag.CreatePerson = new SelectList(db.Users, "ID", "Code", maintenanceHandler.CreatePerson);
            return View(maintenanceHandler);
        }

        // GET: MaintenanceHandlers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MaintenanceHandler maintenanceHandler = _IMaintenanceHandlerQuery.GetModels(u => u.ID == id).FirstOrDefault();
            if (maintenanceHandler == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaintenanceID = new SelectList(db.Maintenances, "ID", "Code", maintenanceHandler.MaintenanceID);
            ViewBag.FromHandler = new SelectList(db.MaintenanceFees, "ID", "MaintenanceID", maintenanceHandler.FromHandler);
            ViewBag.CreatePerson = new SelectList(db.Users, "ID", "Code", maintenanceHandler.CreatePerson);
            return View(maintenanceHandler);
        }

        // POST: MaintenanceHandlers/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,MaintenanceID,FromHandler,Handler,Status,CreateDate,CreatePerson")] MaintenanceHandler maintenanceHandler)
        {
            if (ModelState.IsValid)
            {
                _IMaintenanceHandlerQuery.Update(maintenanceHandler);
                return RedirectToAction("Index");
            }
            ViewBag.MaintenanceID = new SelectList(db.Maintenances, "ID", "Code", maintenanceHandler.MaintenanceID);
            ViewBag.FromHandler = new SelectList(db.MaintenanceFees, "ID", "MaintenanceID", maintenanceHandler.FromHandler);
            ViewBag.CreatePerson = new SelectList(db.Users, "ID", "Code", maintenanceHandler.CreatePerson);
            return View(maintenanceHandler);
        }

        // GET: MaintenanceHandlers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MaintenanceHandler maintenanceHandler = db.MaintenanceHandlers.Find(id);
            if (maintenanceHandler == null)
            {
                return HttpNotFound();
            }
            return View(maintenanceHandler);
        }

        // POST: MaintenanceHandlers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            MaintenanceHandler maintenanceHandler = db.MaintenanceHandlers.Find(id);
            db.MaintenanceHandlers.Remove(maintenanceHandler);
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

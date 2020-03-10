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
using GyIMS.Models;

namespace GyIMS.Controllers
{
    public class RequestFormPriceListsController : Controller
    {
        private DBContext db = new DBContext();

        #region 自定义功能函数

        #region 查询界面

        //GET: //GetRequestFormPriceListList
        [HttpGet]
        public ActionResult GetRequestFormPriceListList()
        {
            var pageSize = Request["rows"] == "" ? 10 : int.Parse(Request["rows"]);
            var pageNumber = Request["page"] == "" ? 1 : int.Parse(Request["page"]);
            string name = string.Empty;
            if (Request["RequestFormName"] != "")
            {
                name = Request["RequestFormName"];
            }
            IQueryable<RequestFormPriceList> requestFormPriceLists;
            if (!string.IsNullOrEmpty(name))
            {
                requestFormPriceLists = db.RequestFormPriceLists.OrderBy(x => x.ID).Where(x => x.RequestForm.Name.Contains(name) && (x.Status == CommonStatusEnum.Able)).Skip((pageNumber - 1) * pageSize).Take(pageSize);
            }
            else
            {
                requestFormPriceLists = db.RequestFormPriceLists.OrderBy(x => x.ID).Where(x => x.Status == CommonStatusEnum.Able).Skip((pageNumber - 1) * pageSize).Take(pageSize);
            }
            var total = requestFormPriceLists.Count();
            var list = new PageView { rows = requestFormPriceLists, total = total };
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
        public ActionResult CreateByForm(RequestFormPriceList requestFormPriceList)
        {
            try
            {
                db.RequestFormPriceLists.Add(requestFormPriceList);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View(requestFormPriceList);
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
        public ActionResult EditByForm(RequestFormPriceList requestFormPriceList)
        {
            try
            {
                db.Entry(requestFormPriceList).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View(requestFormPriceList);
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
            RequestFormPriceList requestFormPriceList = db.RequestFormPriceLists.Find(id);
            requestFormPriceList.Status = CommonStatusEnum.Disabled;
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
        /// 获取IT角色
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetITRoles()
        {
            List<ComboboxCommon> list = RequestHelper.GetITRoles();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion


        #region 框架自动生成
        // GET: RequestFormPriceLists
        public ActionResult Index()
        {
            //var requestFormPriceLists = db.RequestFormPriceLists.Include(r => r.ItRole).Include(r => r.RequestForm).Include(r => r.UserCreate).Include(r => r.UserUpdate);
            var requestFormPriceLists = db.RequestFormPriceLists.Where(x => x.Status == CommonStatusEnum.Able).ToList();
            return View(requestFormPriceLists.ToList());
        }

        // GET: RequestFormPriceLists/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestFormPriceList requestFormPriceList = db.RequestFormPriceLists.Find(id);
            if (requestFormPriceList == null)
            {
                return HttpNotFound();
            }
            return View(requestFormPriceList);
        }

        // GET: RequestFormPriceLists/Create
        public ActionResult Create()
        {
            ViewBag.ItRoleID = new SelectList(db.ItRoles, "ID", "Code");
            ViewBag.RequestFormID = new SelectList(db.RequestForms, "ID", "Code");
            ViewBag.CreatePerson = new SelectList(db.Users, "ID", "Code");
            ViewBag.UpdatePerson = new SelectList(db.Users, "ID", "Code");
            RequestFormPriceList requestFormPriceList = new RequestFormPriceList();
            return View(requestFormPriceList);
        }

        // POST: RequestFormPriceLists/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,RequestFormID,Offerer,OfferDate,ItRoleID,WorkType,TimeUnit,SpendTime,Status,CreateDate,CreatePerson,UpdateDate,UpdatePerson,Summary")] RequestFormPriceList requestFormPriceList)
        {
            if (ModelState.IsValid)
            {
                db.RequestFormPriceLists.Add(requestFormPriceList);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ItRoleID = new SelectList(db.ItRoles, "ID", "Code", requestFormPriceList.ItRoleID);
            ViewBag.RequestFormID = new SelectList(db.RequestForms, "ID", "Code", requestFormPriceList.RequestFormID);
            ViewBag.CreatePerson = new SelectList(db.Users, "ID", "Code", requestFormPriceList.CreatePerson);
            ViewBag.UpdatePerson = new SelectList(db.Users, "ID", "Code", requestFormPriceList.UpdatePerson);
            return View(requestFormPriceList);
        }

        // GET: RequestFormPriceLists/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestFormPriceList requestFormPriceList = db.RequestFormPriceLists.Find(id);
            if (requestFormPriceList == null)
            {
                return HttpNotFound();
            }
            ViewBag.ItRoleID = new SelectList(db.ItRoles, "ID", "Code", requestFormPriceList.ItRoleID);
            ViewBag.RequestFormID = new SelectList(db.RequestForms, "ID", "Code", requestFormPriceList.RequestFormID);
            ViewBag.CreatePerson = new SelectList(db.Users, "ID", "Code", requestFormPriceList.CreatePerson);
            ViewBag.UpdatePerson = new SelectList(db.Users, "ID", "Code", requestFormPriceList.UpdatePerson);
            return View(requestFormPriceList);
        }

        // POST: RequestFormPriceLists/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,RequestFormID,Offerer,OfferDate,ItRoleID,WorkType,TimeUnit,SpendTime,Status,CreateDate,CreatePerson,UpdateDate,UpdatePerson,Summary")] RequestFormPriceList requestFormPriceList)
        {
            if (ModelState.IsValid)
            {
                db.Entry(requestFormPriceList).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ItRoleID = new SelectList(db.ItRoles, "ID", "Code", requestFormPriceList.ItRoleID);
            ViewBag.RequestFormID = new SelectList(db.RequestForms, "ID", "Code", requestFormPriceList.RequestFormID);
            ViewBag.CreatePerson = new SelectList(db.Users, "ID", "Code", requestFormPriceList.CreatePerson);
            ViewBag.UpdatePerson = new SelectList(db.Users, "ID", "Code", requestFormPriceList.UpdatePerson);
            return View(requestFormPriceList);
        }

        // GET: RequestFormPriceLists/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestFormPriceList requestFormPriceList = db.RequestFormPriceLists.Find(id);
            if (requestFormPriceList == null)
            {
                return HttpNotFound();
            }
            return View(requestFormPriceList);
        }

        // POST: RequestFormPriceLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            RequestFormPriceList requestFormPriceList = db.RequestFormPriceLists.Find(id);
            db.RequestFormPriceLists.Remove(requestFormPriceList);
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

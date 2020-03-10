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
    public class MessagesController : Controller
    {
        private DBContext db = new DBContext();

        private IMessageDal _IMessageQuery { get; set; }//= UserContainer.Resolve<IUserDal>();

        public MessagesController(IMessageDal messageDal)
        {
            _IMessageQuery = messageDal;
        }

        #region 自定义功能函数

        #region 查询界面

        //GET: //GetMessageList
        [HttpGet]
        public ActionResult GetMessageList()
        {
            var pageSize = Request["rows"] == "" ? 10 : int.Parse(Request["rows"]);
            var pageNumber = Request["page"] == "" ? 1 : int.Parse(Request["page"]);
            string name = string.Empty;
            if (Request["Name"] != "")
            {
                name = Request["Name"];
            }
            IQueryable<Message> messages;
            if (!string.IsNullOrEmpty(name))
            {
                messages = db.Messages.Where(x => x.BizName.Contains(name) && (x.Status == MessageEnum.Disabled)).OrderBy(x => x.ID).Skip((pageNumber - 1) * pageSize).Take(pageSize);
            }
            else
            {
                try
                {
                    messages = db.Messages.Where(x => x.Status == MessageEnum.Disabled).OrderBy(x => x.ID).Skip((pageNumber - 1) * pageSize).Take(pageSize);
                }
                catch (Exception ex) {

                    messages = db.Messages;
                }
            }
            var total = messages.ToList<Message>();
            var data = new
            {
                error = 0,
                total = total,
                rows = messages.ToList<Message>()
            };
            return Json(data, JsonRequestBehavior.AllowGet);

        }

        #endregion


        #region 新增界面

        /// <summary>
        /// 通过表单提交
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateByForm(Message message)
        {
            try
            {
                db.Messages.Add(message);
                //db.Entry<Message>(message).State = EntityState.Added;
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();
                db.Configuration.ValidateOnSaveEnabled = true;
                return RedirectToAction("Index");
            }
            catch
            {
                return View(message);
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
        public ActionResult EditByForm(Message message)
        {
            try
            {
                db.Entry(message).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View(message);
            }

        }


        #endregion


        #region 查询逻辑


        /// <summary>
        /// 查看消息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public void SearchByAjax(string id)
        {
            Message message = db.Messages.Find(id);
            message.Status = MessageEnum.Able;
            db.SaveChanges();
        }


        #endregion

        #region 公共函数

        /// <summary>
        /// 获取状态
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetMessageStatus()
        {
            List<ComboboxCommon> list = CommonHelper.GetMessageStatus();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取运维单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetMaintenances()
        {
            List<ComboboxCommon> list = CommonHelper.GetMaintenances();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取需求单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetRequestForms()
        {
            List<ComboboxCommon> list = RequestHelper.GetRequestForms();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetBizTypes()
        {
            List<ComboboxCommon> list = CommonHelper.GetBizTypes();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 消息已读

        public ActionResult Read(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = _IMessageQuery.GetModels(m => m.ID == id).SingleOrDefault();
            if (message == null)
            {
                return HttpNotFound();
            }

            message.Status = MessageEnum.Able;
            message.ConsultDate = DateTime.Now;
            _IMessageQuery.Update(message);

            return View(message);
        }

        #endregion

        #endregion

        #region 框架自动生成
        // GET: Messages
        public ActionResult Index()
        {
            //var messages = db.Messages.Include(m => m.Maintenance).Include(m => m.RequestForm);
            var messages = db.Messages.Where(x => x.Status == MessageEnum.Disabled).ToList();
            return View(messages);
        }

        // GET: Messages/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // GET: Messages/Create
        public ActionResult Create()
        {
            ViewBag.BizNo1 = new SelectList(db.Maintenances, "ID", "Code");
            ViewBag.BizNo2 = new SelectList(db.RequestForms, "ID", "Code");
            Message message = new Message();
            return View(message);
        }

        // POST: Messages/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,BizType,BizNo1,BizNo2,BizName,NotifyParty,Status,CreateDate,ConsultDate")] Message message)
        {
            if (ModelState.IsValid)
            {
                db.Messages.Add(message);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BizNo1 = new SelectList(db.Maintenances, "ID", "Code", message.BizNo1);
            ViewBag.BizNo2 = new SelectList(db.RequestForms, "ID", "Code", message.BizNo2);
            return View(message);
        }

        // GET: Messages/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            ViewBag.BizNo1 = new SelectList(db.Maintenances, "ID", "Code", message.BizNo1);
            ViewBag.BizNo2 = new SelectList(db.RequestForms, "ID", "Code", message.BizNo2);
            return View(message);
        }

        // POST: Messages/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,BizType,BizNo1,BizNo2,BizName,NotifyParty,Status,CreateDate,ConsultDate")] Message message)
        {
            if (ModelState.IsValid)
            {
                db.Entry(message).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BizNo1 = new SelectList(db.Maintenances, "ID", "Code", message.BizNo1);
            ViewBag.BizNo2 = new SelectList(db.RequestForms, "ID", "Code", message.BizNo2);
            return View(message);
        }

        // GET: Messages/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // POST: Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Message message = db.Messages.Find(id);
            db.Messages.Remove(message);
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

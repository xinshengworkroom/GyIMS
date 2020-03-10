using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GyIMS.App_Helper;
using GyIMS.Attributes;
using GyIMS.Enums;
using GyIMS.Helper;
using GyIMS.Models;

namespace GyIMS.Controllers
{
    public class ContactsController : Controller
    {
        private DBContext db = new DBContext();
        private IContactDal _IContactQuery { get; set; }//= UserContainer.Resolve<IUserDal>();

        public ContactsController(IContactDal contactDal)
        {
            _IContactQuery = contactDal;
        }

        #region 自定义功能函数
        //GET: //GetContactList
        [HttpGet]
        public ActionResult GetContactList()
        {
            var pageSize = Request["rows"] == "" ? 10 : int.Parse(Request["rows"]);
            var pageNumber = Request["page"] == "" ? 1 : int.Parse(Request["page"]);
            string name = string.Empty;
            if (Request["Name"] != "")
            {
                name = Request["Name"];
            }
            IQueryable<Contact> contacts;
            if (!string.IsNullOrEmpty(name))
            {
                contacts = _IContactQuery.GetModelsByPage(pageSize, pageNumber, true, u => u.Name, u => u.Name.Contains(name) && u.Status == CommonStatusEnum.Able);
            }
            else
            {
                contacts = _IContactQuery.GetModelsByPage(pageSize, pageNumber, true, u => u.Name, u => u.Status == CommonStatusEnum.Able);
            }
            var total = _IContactQuery.GetModels(u => true).Count();
            var list = new PageView { rows = contacts, total = total };
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 获取银行状态
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetContactStatus()
        {
            List<ComboboxCommon> list = new List<ComboboxCommon>();
            foreach (var item in Enum.GetValues(typeof(CommonStatusEnum)))
            {
                ComboboxCommon com = new ComboboxCommon();
                com.id = (int)item;
                com.text = Display.GetEnumBrief((CommonStatusEnum)item);
                list.Add(com);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 获取人员信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetUsers()
        {
            List<ComboboxCommon> list = new List<ComboboxCommon>();
            foreach (var item in db.Users.ToList())
            {
                ComboboxCommon com = new ComboboxCommon();
                com.id = item.ID;
                com.text = item.Name;
                list.Add(com);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        // GET: /Contacts/
        public ActionResult Search(string contactName)
        {
            List<Contact> contacts = _IContactQuery.GetModels(u => u.Name == contactName).ToList();
            return View("Index", contacts);
        }

        /// <summary>
        /// 通过表单提交
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateByForm(Contact contact)
        {
            try
            {

                int result = _IContactQuery.Add(contact);
                return Json(new { Code = 1, Message = "保存成功" }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                return Json(new { Code = -1, Message = ex.Message }, JsonRequestBehavior.DenyGet);
            }

        }


        /// <summary>
        /// 通过表单提交
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditByForm(Contact contact)
        {
            try
            {

                int result = _IContactQuery.Update(contact);
                return Json(new { Code = 1, Message = "保存成功" }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                return Json(new { Code = -1, Message = ex.Message }, JsonRequestBehavior.DenyGet);
            }

        }


        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public void DeleteByAjax(string id)
        {
            Contact contact = db.Contacts.Find(id);
            contact.Status = CommonStatusEnum.Disabled;
            db.SaveChanges();
        }

        #endregion

        

        #region 框架自动生成
        // GET: Contacts
        public ActionResult Index()
        {
            return View(_IContactQuery.GetModels(u => true).ToList());
        }

        // GET: Contacts/Details/5
        public ActionResult Details(int id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            Contact contact = _IContactQuery.GetModels(u => u.ID == id).FirstOrDefault();
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // GET: Contacts/Create
        public ActionResult Create()
        {
            Contact contact = new Contact();

            return View(contact);
        }

        // POST: Contacts/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Code,Name,Tel,Mobile,Email,Fax,PostCode,Address,Status,CreateDate,CreatePerson,UpdateDate,UpdatePerson,Summary")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                _IContactQuery.Add(contact);
                return RedirectToAction("Index");
            }

            return View(contact);
        }

        // GET: Contacts/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = _IContactQuery.GetModels(u => u.ID == id).FirstOrDefault();
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // POST: Contacts/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Code,Name,Tel,Mobile,Email,Fax,PostCode,Address,Status,CreateDate,CreatePerson,UpdateDate,UpdatePerson,Summary")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                _IContactQuery.Update(contact);
                return RedirectToAction("Index");
            }
            return View(contact);
        }

        // GET: Contacts/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Contact contact = db.Contacts.Find(id);
            db.Contacts.Remove(contact);
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

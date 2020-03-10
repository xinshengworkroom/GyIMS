using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GyIMS.Helper.ViewModel.MenuTitles;
using GyIMS.Helper;
using GyIMS.Helper.Container;
using GyIMS.Enums;
using GyIMS.Models;
using Newtonsoft.Json;
using System.Reflection;
using System.Web.Script.Serialization;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Json;

namespace GyIMS.Controllers
{
    public class MenuTitleController : Controller
    {
        private IMenuTitleDal _IMenuTitleDal = BaseContainer.Resolve<IMenuTitleDal, MenuTitleDal>();
        private IMapsMenuTitleDal _IMapMenuTitleDal = BaseContainer.Resolve<IMapsMenuTitleDal, MapsMenuTitleDal>();
        private IStatusInfoDal _IStatusInfoDal = BaseContainer.Resolve<IStatusInfoDal, StatusInfoDal>();
        ViewMenuTitle _ViewMenuTitle = new ViewMenuTitle();
        linqViewMenuTitle _linqViewMenuTitle = new linqViewMenuTitle();


        #region  动态获取Tree
        DataTable dt = new DataTable();
        public ActionResult GetTreeMenu(string Userid)
        {
            var fristmodelid = _IStatusInfoDal.GetModels(u => u.TableName == "Menus" && u.StatusDesc == "MenuRank" && u.StatusNo == "模块菜单").ToList()[0].ID;
            var status = _IStatusInfoDal.GetModels(s => s.TableName == "ALL" && s.StatusDesc == "STATUS" && s.StatusNo == "可用").ToList()[0].ID;
            var data = _IMenuTitleDal.GetModels(u => u.Rank == fristmodelid && u.Status == status).OrderBy(u=>u.SN).ToList();
            dt = GenTree.ToDataTable(data);
            List<JsonTree> list = initTree(dt);
            var json = JsonConvert.SerializeObject(list);
            return Content(json);
        }

        public List<JsonTree> initTree(DataTable dt)
        {
            List<JsonTree> rootNode = new List<JsonTree>();
            foreach (DataRow dr in dt.Rows)
            {
                JsonTree jt = new JsonTree();
                jt.id = dr["ID"].ToString();
                jt.text = dr["TitleName"].ToString();
                jt.state = dr["Status"].ToString();
                jt.attributes = CreateUrl(dt, jt);  // 第一节点设定
                jt.children = CreateChildTree(jt);
                rootNode.Add(jt);
            }
            return rootNode;
        }

        private List<JsonTree> CreateChildTree(JsonTree jt)
        {
            string keyid = jt.id;   //根节点ID
            var data = (from n in _IMapMenuTitleDal.GetModels(k => k.MapsTitleID == keyid).ToList()
                        join k in _IMenuTitleDal.GetModels(k => true).ToList() on n.HierID equals k.HierID
                        join s in _IStatusInfoDal.GetModels(s=>s.TableName=="ALL"&&s.StatusDesc== "STATUS"&& s.StatusNo=="可用").ToList() 
                        on  k.Status equals s.ID
                        select new { k.ID, k.TitleName,k.Status,k.LoginUrl,k.SN }).OrderBy(k=>k.SN).ToList();
            dt = GenTree.ToDataTable(data);
            List<JsonTree> nodeList = new List<JsonTree>();
            foreach (DataRow dr in dt.Rows)
            {
                JsonTree node = new JsonTree();
                node.id = dr["ID"].ToString();
                node.text = dr["TitleName"].ToString();
                node.state = dr["Status"].ToString();
                node.url = dr["LoginUrl"].ToString();
                node.attributes = CreateUrl(dt, node);
                node.children = CreateChildTree(node);
                nodeList.Add(node);
            }
            return nodeList;
        }

        private static Dictionary<string, string> CreateUrl(DataTable dt, JsonTree jt)    //把Url属性添加到attribute中
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            string keyid = jt.id;
            DataRow[] urlList = dt.Select("id='" + keyid + "'");
            string url = "";
            try
            {
                url = urlList[0]["Url"].ToString();
            }
            catch
            {

            }
            dic.Add("url", url);
            return dic;
        }
        #endregion 


        public ActionResult Index()
        {
            return View(_ViewMenuTitle._MenuTitle);
        }
        public ActionResult Create()
        {
            return View(_ViewMenuTitle._MenuTitle);
        }
        public ActionResult CreateMenu()
        {
            return View(_ViewMenuTitle);
        }
        public ActionResult Edit(string id)
        {
            _ViewMenuTitle._MenuTitle = _IMenuTitleDal.GetModels(u => u.ID == id).ToList()[0];
            var fristmodelid = _IStatusInfoDal.GetModels(u => u.TableName == "Menus" && u.StatusDesc == "MenuRank" && u.StatusNo == "模块菜单").ToList()[0].ID;
            if (_ViewMenuTitle._MenuTitle.Rank == fristmodelid)
            {
                _ViewMenuTitle._MapsMenuTitle.HierID = "N/A";
                return View(_ViewMenuTitle);
            }
            if (_ViewMenuTitle._MenuTitle.HierID != null)
            {
                _ViewMenuTitle._MapsMenuTitle = _IMapMenuTitleDal.GetModels(u => u.HierID == _ViewMenuTitle._MenuTitle.HierID).ToList()[0];
            }
            return View(_ViewMenuTitle);
        }
        public ActionResult EditMenuTitle(string id)
        {
            return View(_ViewMenuTitle);
        }
        public ActionResult GetMenuTitleList(MenuTitle menutitle)
        {
            try
            {
                var pageSize = Request["rows"] == "" ? 10 : int.Parse(Request["rows"]);
                var pageNumber = Request["page"] == "" ? 1 : int.Parse(Request["page"]);
                IQueryable<MenuTitle> _MenuTitle;
                _MenuTitle = _IMenuTitleDal.GetModelsByPage(pageSize, pageNumber, true, u => true);
                if (menutitle.TitleName != null)
                {
                    _MenuTitle = _IMenuTitleDal.GetModelsByPage(pageSize, pageNumber, true, u => true,u=>u.TitleName.Contains(menutitle.TitleName));
                }
                var total = _IMenuTitleDal.GetModels(u => true).Count();
                var list = new PageView { rows = _MenuTitle, total = total };
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult CreateByForm(MenuTitle menutitle)
        {
            try
            {
                int result = _IMenuTitleDal.Add(menutitle);
                return Json(new { result = 1, Message = "保存成功" }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                return Json(new { Code = -1, Message = ex.Message }, JsonRequestBehavior.DenyGet);
            }

        }
        public ActionResult CreateMenuByForm(MapsMenuTitle mapsMenuTitle)
        {
            try
            {
                int result = _IMapMenuTitleDal.Add(mapsMenuTitle);
                return Json(new { Code = 1, Message = "保存成功" }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                return Json(new { Code = -1, Message = ex.Message }, JsonRequestBehavior.DenyGet);
            }

        }
        public ActionResult EditByForm(MenuTitle menutitle)
        {
            try
            {
                int result = _IMenuTitleDal.Update(menutitle);
                return Json(new { Code = 1, Message = "保存成功" }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                return Json(new { Code = -1, Message = ex.Message }, JsonRequestBehavior.DenyGet);
            }

        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _IMenuTitleDal.Dispose();
                _IMapMenuTitleDal.Dispose();
                _IStatusInfoDal.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}


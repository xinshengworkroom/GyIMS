using GyIMS.Enums;
using GyIMS.Helper;
using GyIMS.Models;
using System;
using System.Web.Mvc;

namespace GyIMS.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class LogAttribute : ActionFilterAttribute
    {
        IMaintenanceLogDal MaintenanceLogDal = new MaintenanceLogDal();


        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);


            //操作人
            string userName = WebContext.Current.SessionUser.ID;
           
            //操作结果

            string resultData = "";
            if (filterContext.Result is JsonResult)
            {
                resultData = (filterContext.Result as JsonResult).Data.ToString();
            }
            else if (filterContext.Result is ContentResult)
            {
                resultData = (filterContext.Result as ContentResult).Content;
            }

            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string actionName = filterContext.ActionDescriptor.ActionName;

            OperTypeEnum operType = OperTypeEnum.Default;
            string summary = "";

            switch (controllerName)
            {
                case "":
                    break;
                default:
                    summary += controllerName;
                    break;
            }

            switch (actionName)
            {
                case "Create":
                    operType = OperTypeEnum.Create;
                    summary += "新增";
                    break;
                case "Update":
                    operType = OperTypeEnum.Update;
                    summary += "修改";
                    break;
                case "Delete":
                    operType = OperTypeEnum.Delete;
                    summary += "删除";
                    break;
                case "Able":
                    operType = OperTypeEnum.Able;
                    summary += "启用";
                    break;
                case "Disable":
                    operType = OperTypeEnum.Disable;
                    summary += "禁用";
                    break;
                default:
                    summary +=  "/" + actionName;
                    break;
            }
           


            MaintenanceLog log = new MaintenanceLog
            {
                OperType = "",
                CreateDate = DateTime.Now,
                CreatePerson = userName,
                Summary = summary
            };
            //记录Log
           

            MaintenanceLogDal.Add(log);

        }


    }


}
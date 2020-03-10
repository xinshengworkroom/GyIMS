using GyIMS.Attributes;
using GyIMS.Enums;
using GyIMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GyIMS.App_Helper
{
    public static class RequestHelper
    {
        private static DBContext db = new DBContext();

        /// <summary>
        /// 获取通用状态
        /// </summary>
        /// <returns></returns>
        public static List<ComboboxCommon> GetCommonStatuses()
        {
            List<ComboboxCommon> list = new List<ComboboxCommon>();
            foreach (var item in Enum.GetValues(typeof(CommonStatusEnum)))
            {
                ComboboxCommon com = new ComboboxCommon();
                com.id = (int)item;
                com.text = Display.GetEnumBrief((CommonStatusEnum)item);
                list.Add(com);
            }
            return list;
        }

        /// <summary>
        /// 获取需求类型
        /// </summary>
        /// <returns></returns>
        public static List<ComboboxCommon> GetRequstFormSys()
        {
            List<ComboboxCommon> list = new List<ComboboxCommon>();
            foreach (var item in Enum.GetValues(typeof(RequestSysEnum)))
            {
                ComboboxCommon com = new ComboboxCommon();
                com.id = (int)item;
                com.text = Display.GetEnumBrief((RequestSysEnum)item);
                list.Add(com);
            }
            return list;
        }

        /// <summary>
        /// 获取需求状态
        /// </summary>
        /// <returns></returns>
        public static List<ComboboxCommon> GetRequestStatus()
        {
            List<ComboboxCommon> list = new List<ComboboxCommon>();
            foreach (var item in Enum.GetValues(typeof(RequestStatusEnum)))
            {
                ComboboxCommon com = new ComboboxCommon();
                com.id = (int)item;
                com.text = Display.GetEnumBrief((RequestStatusEnum)item);
                list.Add(com);
            }
            return list;
        }

        /// <summary>
        /// 获取需求信息
        /// </summary>
        /// <returns></returns>
        public static List<ComboboxCommon> GetRequestForms()
        {
            List<ComboboxCommon> list = new List<ComboboxCommon>();
            foreach (var item in db.RequestForms.Where(x => x.Status == CommonStatusEnum.Able).ToList())
            {
                ComboboxCommon com = new ComboboxCommon();
                com.id = item.ID;
               // com.text = "系统:" + item.SysTypeName + "|申请人:" + item.ApplierName + "|需求名称:" + item.Name;
                list.Add(com);
            }
            return list;
        }

        /// <summary>
        /// 获取IT角色
        /// </summary>
        /// <returns></returns>
        public static List<ComboboxCommon> GetITRoles()
        {
            List<ComboboxCommon> list = new List<ComboboxCommon>();
            foreach (var item in db.ItRoles.Where(x => x.Status == CommonStatusEnum.Able).ToList())
            {
                ComboboxCommon com = new ComboboxCommon();
                com.id = item.ID;
                com.text = item.Name;
                list.Add(com);
            }
            return list;
        }

        /// <summary>
        /// 获取工作类型
        /// </summary>
        /// <returns></returns>
        public static List<ComboboxCommon> GetWorkType()
        {
            List<ComboboxCommon> list = new List<ComboboxCommon>();
            foreach (var item in Enum.GetValues(typeof(WorkTypeEnum)))
            {
                ComboboxCommon com = new ComboboxCommon();
                com.id = (int)item;
                com.text = Display.GetEnumBrief((WorkTypeEnum)item);
                list.Add(com);
            }
            return list;
        }

        /// <summary>
        /// 获取时间单位
        /// </summary>
        /// <returns></returns>
        public static List<ComboboxCommon> GetTimeUnit()
        {
            List<ComboboxCommon> list = new List<ComboboxCommon>();
            foreach (var item in Enum.GetValues(typeof(TimeUnitEnum)))
            {
                ComboboxCommon com = new ComboboxCommon();
                com.id = (int)item;
                com.text = Display.GetEnumBrief((TimeUnitEnum)item);
                list.Add(com);
            }
            return list;
        }

        /// <summary>
        /// 获取任务状态
        /// </summary>
        /// <returns></returns>
        public static List<ComboboxCommon> GetTaskStatus()
        {
            List<ComboboxCommon> list = new List<ComboboxCommon>();
            foreach (var item in Enum.GetValues(typeof(TaskStatusEnum)))
            {
                ComboboxCommon com = new ComboboxCommon();
                com.id = (int)item;
                com.text = Display.GetEnumBrief((TaskStatusEnum)item);
                list.Add(com);
            }
            return list;
        }

        /// <summary>
        /// 获取需求任务
        /// </summary>
        /// <returns></returns>
        public static List<ComboboxCommon> GetRequestFormTasks()
        {
            List<ComboboxCommon> list = new List<ComboboxCommon>();
            foreach (var item in db.RequestFormTasks.Where(x => x.Status == CommonStatusEnum.Able).ToList())
            {
                ComboboxCommon com = new ComboboxCommon();
                com.id = item.ID;
                com.text = "任务指派人:" + item.Assigner + "|处理人:" + item.Handler + "|任务状态：" + item.TaskStatus;
                list.Add(com);
            }
            return list;
        }

        /// <summary>
        /// 获取需求任务明细
        /// </summary>
        /// <returns></returns>
        public static List<ComboboxCommon> GetRequestFormTaskLists()
        {
            List<ComboboxCommon> list = new List<ComboboxCommon>();
            foreach (var item in db.RequestFormTaskLists.Where(x => x.Status == CommonStatusEnum.Able).ToList())
            {
                ComboboxCommon com = new ComboboxCommon();
                com.id = item.ID;
                com.text = "处理人:" + item.Handler;
                list.Add(com);
            }
            return list;
        }

        /// <summary>
        /// 获取需求文档类型
        /// </summary>
        /// <returns></returns>
        public static List<ComboboxCommon> GetRequestTypeStatus()
        {
            List<ComboboxCommon> list = new List<ComboboxCommon>();
            foreach (var item in Enum.GetValues(typeof(RequestTypeEnum)))
            {
                ComboboxCommon com = new ComboboxCommon();
                com.id = (int)item;
                com.text = Display.GetEnumBrief((RequestTypeEnum)item);
                list.Add(com);
            }
            return list;
        }

        /// <summary>
        /// 获取Bug类型
        /// </summary>
        /// <returns></returns>
        public static List<ComboboxCommon> GetRankEnums()
        {
            List<ComboboxCommon> list = new List<ComboboxCommon>();
            foreach (var item in Enum.GetValues(typeof(RankEnum)))
            {
                ComboboxCommon com = new ComboboxCommon();
                com.id = (int)item;
                com.text = Display.GetEnumBrief((RankEnum)item);
                list.Add(com);
            }
            return list;
        }
    }
}
using GyIMS.Attributes;
using GyIMS.Enums;
using GyIMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GyIMS.App_Helper
{
    public static class CommonHelper
    {
        private static DBContext db = new DBContext();

        /// <summary>
        /// 获取运维信息
        /// </summary>
        /// <returns></returns>
        public static List<ComboboxCommon> GetMaintenances()
        {
            List<ComboboxCommon> list = new List<ComboboxCommon>();
            foreach (var item in db.Maintenances.Where(x => x.Status == CommonStatusEnum.Able).ToList())
            {
                ComboboxCommon com = new ComboboxCommon();
                com.id = item.ID;
               // com.text = "区域:" + item.RegionName + "|申请人:" + item.ApplierName + "|申请状态：" + item.MaintenanceStatusName;
                list.Add(com);
            }
            return list;
        }

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
        /// 获取运维附件类型
        /// </summary>
        /// <returns></returns>
        public static List<ComboboxCommon> GetTypes()
        {
            List<ComboboxCommon> list = new List<ComboboxCommon>();
            foreach (var item in Enum.GetValues(typeof(MaintenanceFileTypeEnum)))
            {
                ComboboxCommon com = new ComboboxCommon();
                com.id = (int)item;
                com.text = Display.GetEnumBrief((MaintenanceFileTypeEnum)item);
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
        /// 获取运维结果类型
        /// </summary>
        /// <returns></returns>
        public static List<ComboboxCommon> GetResultTypes()
        {
            List<ComboboxCommon> list = new List<ComboboxCommon>();
            foreach (var item in Enum.GetValues(typeof(MaintenanceResultTypeEnum)))
            {
                ComboboxCommon com = new ComboboxCommon();
                com.id = (int)item;
                com.text = Display.GetEnumBrief((MaintenanceResultTypeEnum)item);
                list.Add(com);
            }
            return list;
        }

        /// <summary>
        /// 获取消息状态
        /// </summary>
        /// <returns></returns>
        public static List<ComboboxCommon> GetMessageStatus()
        {
            List<ComboboxCommon> list = new List<ComboboxCommon>();
            foreach (var item in Enum.GetValues(typeof(MessageEnum)))
            {
                ComboboxCommon com = new ComboboxCommon();
                com.id = (int)item;
                com.text = Display.GetEnumBrief((MessageEnum)item);
                list.Add(com);
            }
            return list;
        }

        /// <summary>
        /// 获取消息类型
        /// </summary>
        /// <returns></returns>
        public static List<ComboboxCommon> GetBizTypes()
        {
            List<ComboboxCommon> list = new List<ComboboxCommon>();
            foreach (var item in Enum.GetValues(typeof(BizTypeEnum)))
            {
                ComboboxCommon com = new ComboboxCommon();
                com.id = (int)item;
                com.text = Display.GetEnumBrief((BizTypeEnum)item);
                list.Add(com);
            }
            return list;
        }

        /// <summary>
        /// 获取申请人
        /// </summary>
        /// <returns></returns>
        public static List<ComboboxCommon> GetPerson()
        {
            List<ComboboxCommon> list = new List<ComboboxCommon>();
            foreach (var item in db.Users.Where(x => x.Status == CommonStatusEnum.Able).ToList())
            {
                ComboboxCommon com = new ComboboxCommon();
                com.id = item.ID;
                com.text = "员工工号:" + item.ID + "|姓名:" + item.ChineseName ;
                list.Add(com);
            }
            return list;
        }

        /// <summary>
        /// 获取申请区域
        /// </summary>
        /// <returns></returns>
        public static List<ComboboxCommon> GetRegions()
        {
            List<ComboboxCommon> list = new List<ComboboxCommon>();
            foreach (var item in Enum.GetValues(typeof(RegionEnum)))
            {
                ComboboxCommon com = new ComboboxCommon();
                com.id = (int)item;
                com.text = Display.GetEnumBrief((RegionEnum)item);
                list.Add(com);
            }
            return list;
        }

        /// <summary>
        /// 获取运维状态
        /// </summary>
        /// <returns></returns>
        public static List<ComboboxCommon> GetMaintenanceStatus()
        {
            List<ComboboxCommon> list = new List<ComboboxCommon>();
            foreach (var item in Enum.GetValues(typeof(MaintenanceStatusEnum)))
            {
                ComboboxCommon com = new ComboboxCommon();
                com.id = (int)item;
                com.text = Display.GetEnumBrief((MaintenanceStatusEnum)item);
                list.Add(com);
            }
            return list;
        }

        /// <summary>
        /// 获取硬件系统类型
        /// </summary>
        /// <returns></returns>
        public static List<ComboboxCommon> GetHSysTypes()
        {
            List<ComboboxCommon> list = new List<ComboboxCommon>();
            foreach (var item in Enum.GetValues(typeof(HSysTypeEnum)))
            {
                ComboboxCommon com = new ComboboxCommon();
                com.id = (int)item;
                com.text = Display.GetEnumBrief((HSysTypeEnum)item);
                list.Add(com);
            }
            return list;
        }

        /// <summary>
        /// 获取软件系统类型
        /// </summary>
        /// <returns></returns>
        public static List<ComboboxCommon> GetSysTypes()
        {
            List<ComboboxCommon> list = new List<ComboboxCommon>();
            foreach (var item in Enum.GetValues(typeof(SysTypeEnum)))
            {
                ComboboxCommon com = new ComboboxCommon();
                com.id = (int)item;
                com.text = Display.GetEnumBrief((SysTypeEnum)item);
                list.Add(com);
            }
            return list;
        }

        /// <summary>
        /// 获取问题类型
        /// </summary>
        /// <returns></returns>
        public static List<ComboboxCommon> GetMTypes()
        {
            List<ComboboxCommon> list = new List<ComboboxCommon>();
            foreach (var item in Enum.GetValues(typeof(TypeEnum)))
            {
                ComboboxCommon com = new ComboboxCommon();
                com.id = (int)item;
                com.text = Display.GetEnumBrief((TypeEnum)item);
                list.Add(com);
            }
            return list;
        }
    }
}
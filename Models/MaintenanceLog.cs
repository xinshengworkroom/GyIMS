using GyIMS.App_Helper;
using GyIMS.Enums;
using GyIMS.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GyIMS.Models
{
    public class MaintenanceLog 
    {
        public MaintenanceLog()
        {
            this.CreateDate = DateTime.Now;
        }

        string id = String.Empty;
        [DisplayName("系统主键")]
        [StringLength(50)]
        [Required]
        [Key]
        public string ID
        {
            get
            {
                if (String.IsNullOrEmpty(this.id))
                {
                    this.id = GeneratePrimaryKey.GetrimaryKey("MaintenanceLog");
                }
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }

        [DisplayName("运维单号")]
        [StringLength(50)]
        [Required]
        public string MaintenanceID { get; set; }

        [DisplayName("操作类型")]
        public string OperType { get; set; }

        [DisplayName("申请人")]
        [StringLength(50)]
        public string Applier { get; set; }

        [DisplayName("区域（申请人所在）")]
        [StringLength(500)]
        public string Region { get; set; }

        [DisplayName("问题类型")]
        [StringLength(500)]
        public string Type { get; set; }

        [DisplayName("软件系统类型")]
        [StringLength(500)]

        public string SysType { get; set; }

        [DisplayName("硬件系统类型")]
        [StringLength(500)]
        public string HSysType { get; set; }

        [DisplayName("问题名称")]
        [StringLength(50)]
        public string Name { get; set; }

        [DisplayName("创建时间")]
        [Required]
        public DateTime? CreateDate { get; set; }

        [DisplayName("创建人")]
        [StringLength(50)]
        public string CreatePerson { get; set; }

        [DisplayName("备注")]
        [StringLength(500)]
        public string Summary { get; set; }
    }
}
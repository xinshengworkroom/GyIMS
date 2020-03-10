using GyIMS.App_Helper;
using GyIMS.Attributes;
using GyIMS.Enums;
using GyIMS.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GyIMS.Models
{
    public class Maintenance :  IName, ICommonStatus
    {
        public Maintenance()
        {
            this.CreateDate = DateTime.Now;
            this.UpdateDate = DateTime.Now;
            this.Status = CommonStatusEnum.Able;
        }
        int? id;
        [DisplayName("运维单号")]
        //[StringLength(50)]
        [Required]
        [Key]
        public int ID
        {
            get
            {
                if (this.id != null)
                {
                    //string str = GeneratePrimaryKey.GetrimaryKey("Maintenance");


                    return this.id.Value;
                }
                return 1;
            
            }
            set
            {
                this.id = value;
            }
        }


        [DisplayName("单号")]
        [StringLength(150)]
        [Required]
        public string Code { get; set; }

        [DisplayName("申请人")]
        [StringLength(50)]
        [Required]
        public string Applier { get; set; }

        [DisplayName("申请人姓名")]
        [NotMapped]
        public string ApplierName
        {
            get
            {
                return this.UserID == null ? String.Empty : this.UserID.ChineseName;
            }
        }

        [DisplayName("区域（申请人所在）")]
        [Required]
        [StringLength(500)]
        public string Region { get; set; }

        [DisplayName("问题类型")]
        [StringLength(500)]
        [Required]
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

        [DisplayName("问题描述")]
        [StringLength(500)]
        public string Description { get; set; }


        [DisplayName("附件名称")]
        [StringLength(500)]
        public string FileName { get; set; }

        [DisplayName("运维状态")]
        [Required]
        [StringLength(500)]
        public string MaintenanceStatus { get; set; }

        [DisplayName("状态")]
        [Required]
        public CommonStatusEnum Status { get; set; }

        [DisplayName("状态名称")]
        [NotMapped]
        public string StatusName
        {
            get
            {
                return Display.GetEnumBrief(this.Status);
            }
        }

        [DisplayName("创建时间")]
        public DateTime? CreateDate { get; set; }

        [DisplayName("创建人")]
        [StringLength(50)]
        public string CreatePerson { get; set; }

        [DisplayName("创建人名称")]
        [NotMapped]
        public string CreatePersonName
        {
            get
            {
                return this.UserCreate == null ? String.Empty : this.UserCreate.Name;
            }
        }

        [DisplayName("修改时间")]
        public DateTime? UpdateDate { get; set; }

        [DisplayName("修改人")]
        [StringLength(50)]
        public string UpdatePerson { get; set; }


        [DisplayName("修改人名称")]
        [NotMapped]
        public string UpdatePersonName
        {
            get
            {
                return this.UserUpdate == null ? String.Empty : this.UserUpdate.Name;
            }
        }

        [DisplayName("备注")]
        [StringLength(500)]
        public string Summary { get; set; }


        List<MaintenanceLog> maintenanceLogs { get; set; }

        [ForeignKey("CreatePerson")]
        public virtual User UserCreate { get; set; }

        [ForeignKey("UpdatePerson")]
        public virtual User UserUpdate { get; set; }

        [ForeignKey("Applier")]
        public virtual User UserID { get; set; }



    }
}
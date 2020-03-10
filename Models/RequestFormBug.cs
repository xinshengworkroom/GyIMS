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
    public class RequestFormBug : IUnique, ICommonStatus
    {
        public RequestFormBug()
        {
            this.CreateDate = DateTime.Now;
            this.UpdateDate = DateTime.Now;
            this.Status = CommonStatusEnum.Able;
        }
        string id = String.Empty;
        [DisplayName("需求BUG编号")]
        [StringLength(50)]
        [Required]
        [Key]
        public string ID
        {
            get
            {
                if (String.IsNullOrEmpty(this.id))
                {
                    this.id = GeneratePrimaryKey.GetrimaryKey("RequestFormBug");
                }
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }



        [DisplayName("需求申请单号")]
        [StringLength(50)]
        [Required]
        public string RequestFormID { get; set; }

        [DisplayName("需求名称")]
        [NotMapped]
        public string RequestFormName
        {
            get
            {
                return this.RequestForm == null ? String.Empty : this.RequestForm.Name;
            }
        }

        [DisplayName("需求任务编号")]
        [StringLength(50)]
        public string RequestFormTaskID { get; set; }

        [DisplayName("需求任务指派人")]
        [NotMapped]
        public string AssignerName
        {
            get
            {
                return this.RequestFormTask == null ? String.Empty : this.RequestFormTask.Assigner;
            }
        }

        [DisplayName("任务清单编号")]
        [StringLength(50)]
        public string RequestFormTaskListID { get; set; }

        [DisplayName("处理人")]
        [NotMapped]
        public string HandlerName
        {
            get
            {
                return this.RequestFormTaskList == null ? String.Empty : this.RequestFormTaskList.Handler;
            }
        }

        [DisplayName("指定处理人")]
        [StringLength(50)]
        public string Handler { get; set; }

        [DisplayName("BUG等级")]
        [Required]
        [StringLength(500)]
        public string Rank { get; set; }

        [StringLength(150)]
        [DisplayName("名称")]
        public string Name { get; set; }

        [DisplayName("需求状态")]
        [StringLength(500)]
        public string RequestStatus { get; set; }

        [DisplayName("系统类型")]
        [StringLength(500)]
        public string SysType { get; set; }

        [StringLength(50)]
        [DisplayName("需求申请人")]
        [Required]
        public string Applier { get; set; }

        [DisplayName("描述")]
        [StringLength(500)]
        public string Description { get; set; }

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
        [Required]
        public DateTime? CreateDate { get; set; }

        [DisplayName("创建人")]
        [StringLength(50)]
        [Required]
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
        [Required]
        public DateTime? UpdateDate { get; set; }

        [DisplayName("修改人")]
        [StringLength(50)]
        [Required]
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




        [ForeignKey("CreatePerson")]
        public virtual User UserCreate { get; set; }

        [ForeignKey("UpdatePerson")]
        public virtual User UserUpdate { get; set; }

        [ForeignKey("RequestFormID")]
        public virtual RequestForm RequestForm { get; set; }

        [ForeignKey("RequestFormTaskID")]
        public virtual RequestFormTask RequestFormTask { get; set; }

        [ForeignKey("RequestFormTaskListID")]
        public virtual RequestFormTaskList RequestFormTaskList { get; set; }
    }
}
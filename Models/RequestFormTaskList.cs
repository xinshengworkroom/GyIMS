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
    public class RequestFormTaskList :IUnique, ICommonStatus
    {
        public RequestFormTaskList()
        {
            this.CreateDate = DateTime.Now;
            this.UpdateDate = DateTime.Now;
            this.Status = CommonStatusEnum.Able;
        }
        public RequestFormTaskList(string requestFormTaskID)
        {
            this.RequestFormTaskID = requestFormTaskID;
            this.CreateDate = DateTime.Now;
            this.UpdateDate = DateTime.Now;
            this.Status = CommonStatusEnum.Able;
        }
        string id = String.Empty;
        [DisplayName("需求任务明细编号")]
        [StringLength(50)]
        [Required]
        [Key]
        public string ID
        {
            get
            {
                if (String.IsNullOrEmpty(this.id))
                {
                    this.id = GeneratePrimaryKey.GetrimaryKey("RequestFormTaskList");
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
        [Required]
        public string RequestFormTaskID { get; set; }

        [DisplayName("需求任务指派人")]
        [NotMapped]
        public string Assigner
        {
            get
            {
                return this.RequestFormTask == null ? String.Empty : this.RequestFormTask.Assigner;
            }
        }

        [DisplayName("任务序号（从1开始）")]
        [StringLength(50)]
        [Required]
        public string TaskSeqNo { get; set; }

        [DisplayName("处理人")]
        [Required]
        public string Handler { get; set; }

        [DisplayName("处理日期")]
        public DateTime? HandleDate { get; set; }

        [DisplayName("起始时间")]
        public DateTime? FromTime { get; set; }

        [DisplayName("结束时间")]
        public DateTime? ToTime { get; set; }

        [DisplayName("完成比例（0-100）")]
        public decimal? RateOfProgress { get; set; }

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




        [ForeignKey("CreatePerson")]
        public virtual User UserCreate { get; set; }

        [ForeignKey("UpdatePerson")]
        public virtual User UserUpdate { get; set; }

        [ForeignKey("RequestFormID")]
        public virtual RequestForm RequestForm { get; set; }

        [ForeignKey("RequestFormTaskID")]
        public virtual RequestFormTask RequestFormTask { get; set; }
    }
}
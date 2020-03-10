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
    public class RequestFormTask : IUnique, ICommonStatus
    {
        public RequestFormTask()
        {
            this.CreateDate = DateTime.Now;
            this.UpdateDate = DateTime.Now;
            this.Status = CommonStatusEnum.Able;
        }
        string id = String.Empty;
        [DisplayName("需求任务编号")]
        [StringLength(50)]
        [Required]
        [Key]
        public string ID
        {
            get
            {
                if (String.IsNullOrEmpty(this.id))
                {
                    this.id = GeneratePrimaryKey.GetrimaryKey("RequestFormTask");
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

        

        [DisplayName("任务指派人")]
        [StringLength(50)]
        public string Assigner { get; set; }

        
        

        [DisplayName("IT角色")]
        [StringLength(50)]
        public string ItRoleID { get; set; }

        [DisplayName("IT角色")]
        [NotMapped]
        public string ItRoleName
        {
            get
            {
                return this.ItRole == null ? String.Empty : this.ItRole.Name;
            }
        }

        [DisplayName("处理人")]
        [StringLength(50)]
        public string Handler { get; set; }

        [DisplayName("任务开始日期")]
        public DateTime? FromDate { get; set; }

        [DisplayName("任务结束日期")]
        public DateTime? ToDate { get; set; }

        [DisplayName("需求占比")]
        public decimal? PercentOfRequest { get; set; }

        [DisplayName("任务描述")]
        [StringLength(500)]
        public string Description { get; set; }

        [DisplayName("上传文件名称")]
        [StringLength(500)]
        public string FileName { get; set; }

        [DisplayName("任务状态")]
        [StringLength(500)]
        public string TaskStatus { get; set; }

        

        [DisplayName("状态")]
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

        [ForeignKey("ItRoleID")]
        public virtual ItRole ItRole { get; set; }
    }
}
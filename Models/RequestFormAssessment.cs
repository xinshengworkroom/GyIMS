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
    public class RequestFormAssessment : IUnique, ICommonStatus
    {
        public RequestFormAssessment()
        {
            this.CreateDate = DateTime.Now;
            this.UpdateDate = DateTime.Now;
            this.Status = CommonStatusEnum.Able;
        }
        string id = String.Empty;
        [DisplayName("需求评估单号")]
        [StringLength(50)]
        [Required]
        [Key]
        public string ID
        {
            get
            {
                if (String.IsNullOrEmpty(this.id))
                {
                    this.id = GeneratePrimaryKey.GetrimaryKey("RequestFormAssessment");
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

        [DisplayName("评估人")]
        [StringLength(50)]
        public string Assessor { get; set; }

        [DisplayName("需求状态")]
        [StringLength(50)]
        public string RequestStatus { get; set; }

        [DisplayName("评估日期")]
        public DateTime? AssessDate { get; set; }


        [DisplayName("需求预计开始日期")]
        public DateTime? FromDate { get; set; }


        [DisplayName("需求预计结束日期")]
        public DateTime? ToDate { get; set; }

        [DisplayName("报价人")]
        [StringLength(50)]
        public string Offerer { get; set; }

        [DisplayName("报价日期")]
        public DateTime? OfferDate { get; set; }

        [DisplayName("IT角色")]
        public string ItRoleID { get; set; }

        [DisplayName("IT角色名称")]
        [NotMapped]
        public string ItRoleName
        {
            get
            {
                return this.ItRole == null ? String.Empty : this.ItRole.Name;
            }
        }

        [DisplayName("工作类型")]
        [StringLength(500)]
        public string WorkType { get; set; }


        

        [DisplayName("时间单位")]
        [StringLength(500)]
        public string TimeUnit { get; set; }


        

        [DisplayName("工期")]
        public decimal SpendTime { get; set; }

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
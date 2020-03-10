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
    public class RequestForm :IUnique, IName, ICommonStatus
    {
        public RequestForm()
        {
            this.CreateDate = DateTime.Now;
            this.UpdateDate = DateTime.Now;
            this.Status = CommonStatusEnum.Able;
        }

        string id = String.Empty;
        [DisplayName("需求申请单号")]
        [StringLength(50)]
        [Required]
        [Key]
        public string ID
        {
            get
            {
                if (String.IsNullOrEmpty(this.id))
                {
                    this.id = GeneratePrimaryKey.GetrimaryKey("RequestForm");
                }
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }



        [DisplayName("单号")]
        [StringLength(50)]
        public string Code { get; set; }

        
        [StringLength(150)]
        [DisplayName("需求名称")]
        [Required]
        public string Name { get; set; }

        

        [DisplayName("系统类型")]
        [Required]
        [StringLength(500)]
        public string SysType { get; set; }

        

        [StringLength(500)]
        [DisplayName("需求描述")]
        public string Description { get; set; }

        [StringLength(50)]
        [DisplayName("上传文件名称")]
        public string FileName { get; set; }

        [StringLength(50)]
        [DisplayName("需求申请人")]
        [Required]
        public string Applier { get; set; }

        [DisplayName("申请人名称")]
        [NotMapped]
        public string ApplierName
        {
            get
            {
                return this.User == null ? String.Empty : this.User.ChineseName;
            }
        }

        [DisplayName("需求状态")]
        [Required]
        [StringLength(500)]
        public string RequestStatus { get; set; }

        //[DisplayName("需求状态名称")]
        //[NotMapped]
        //public string RequestStatusName
        //{
        //    get
        //    {
        //        return Display.GetEnumBrief(this.RequestStatus);
        //    }
        //}

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

        //[DisplayName("创建人名称")]
        //[NotMapped]
        //public string CreatePersonName
        //{
        //    get
        //    {
        //        return this.UserCreate == null ? String.Empty : this.UserCreate.Name;
        //    }
        //}

        [DisplayName("修改时间")]
        public DateTime? UpdateDate { get; set; }

        [DisplayName("修改人")]
        [StringLength(50)]
        public string UpdatePerson { get; set; }


        //[DisplayName("修改人名称")]
        //[NotMapped]
        //public string UpdatePersonName
        //{
        //    get
        //    {
        //        return this.UserUpdate == null ? String.Empty : this.UserUpdate.Name;
        //    }
        //}

        [DisplayName("备注")]
        [StringLength(500)]
        public string Summary { get; set; }




        [ForeignKey("Applier")]
        public virtual User User { get; set; }

        

        
    }
}
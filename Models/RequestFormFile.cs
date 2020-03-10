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
    public class RequestFormFile :IUnique, ICommonStatus
    {
        public RequestFormFile()
        {
            this.CreateDate = DateTime.Now;
            this.UpdateDate = DateTime.Now;
            this.Status = CommonStatusEnum.Able;
        }
        string id = String.Empty;
        [DisplayName("需求附件编号")]
        [StringLength(50)]
        [Required]
        [Key]
        public string ID
        {
            get
            {
                if (String.IsNullOrEmpty(this.id))
                {
                    this.id = GeneratePrimaryKey.GetrimaryKey("RequestFormFile");
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

        [DisplayName("文档类型")]
        [Required]
        public RequestTypeEnum Type { get; set; }

        [DisplayName("文档类型")]
        [NotMapped]
        public string TypeName
        {
            get
            {
                return Display.GetEnumBrief(this.Type);
            }
        }

        [DisplayName("附件名称")]
        [StringLength(500)]
        public string FileName { get; set; }

        [DisplayName("附件类型")]
        [StringLength(500)]
        public string FilePath { get; set; }


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

        [ForeignKey("RequestFormTaskListID")]
        public virtual RequestFormTaskList RequestFormTaskList { get; set; }
    }
}
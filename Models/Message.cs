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
    public class Message : IUnique
    {
        public Message()
        {
            this.CreateDate = DateTime.Now;
            this.Status = MessageEnum.Disabled;
        }

        string id = String.Empty;
        [DisplayName("消息编号")]
        [StringLength(50)]
        [Required]
        [Key]
        public string ID
        {
            get
            {
                if (String.IsNullOrEmpty(this.id))
                {
                    this.id = GeneratePrimaryKey.GetrimaryKey("Message");
                }
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }

        [DisplayName("通知类型")]
        [Required]
        public BizTypeEnum BizType { get; set; }

        [DisplayName("通知类型")]
        [NotMapped]
        public string BizTypeName
        {
            get
            {
                return Display.GetEnumBrief(this.BizType);
            }
        }

        [DisplayName("运维业务单号")]
        //[StringLength(50)]
        public int BizNo1 { get; set; }

        [DisplayName("问题名称")]
        [NotMapped]
        public string MaintenanceName
        {
            get
            {
                return this.Maintenance == null ? String.Empty : this.Maintenance.Name;
            }
        }

        [DisplayName("需求业务单号")]
        [StringLength(50)]
        public string BizNo2 { get; set; }

        [DisplayName("需求名称")]
        [NotMapped]
        public string RequestFormName
        {
            get
            {
                return this.RequestForm == null ? String.Empty : this.RequestForm.Name;
            }
        }

        [DisplayName("业务简述")]
        [StringLength(500)]
        [Required]
        public string BizName { get; set; }

        [DisplayName("被通知人")]
        [StringLength(50)]
        [Required]
        public string NotifyParty { get; set; }

        [DisplayName("状态")]
        [Required]
        public MessageEnum Status { get; set; }

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

        [DisplayName("查阅时间")]
        public DateTime? ConsultDate { get; set; }

        [ForeignKey("BizNo1")]
        public virtual Maintenance Maintenance { get; set; }

        [ForeignKey("BizNo2")]
        public virtual RequestForm RequestForm { get; set; }
    }
}
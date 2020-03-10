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
    public class StatusInfo 
    {

        public StatusInfo()
        {
            this.CreateDate = DateTime.Now;
            this.UpdateDate = DateTime.Now;
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
                    this.id = GeneratePrimaryKey.GetrimaryKey("StatusInfoes");
                }
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }

        [StringLength(50)]
        [Required]
        public string TableName { get; set; }

        [StringLength(50)]
        [Required]
        public string StatusNo{ get; set; }


        [StringLength(50)]
        public string StatusNameCH { get; set; }


        [StringLength(50)]
        public string StatusNameEN { get; set; }

        [StringLength(50)]
        public string StatusDesc { get; set; }


        [StringLength(50)]
        public string CreateUserId { get; set; }

        [StringLength(50)]
        public string UpdateUserId { get; set; }

        [DisplayName("创建时间")]
        public DateTime? CreateDate { get; set; }

        [DisplayName("修改时间")]
        public DateTime? UpdateDate { get; set; }
    }
}
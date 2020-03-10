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
    public class MenuLog : ILog
    {
        [DisplayName("系统主键")]
        [StringLength(50)]
        [Required]
        [Key]
        public string ID { get; set; }

        [DisplayName("菜单编号")]
        [StringLength(50)]
        [Required]
        public string MenuID { get; set; }

        [DisplayName("操作类型")]
        [Required]
        public OperTypeEnum OperType { get; set; }

        [DisplayName("创建时间")]
        [Required]
        public DateTime? CreateDate { get; set; }

        [DisplayName("创建人")]
        [StringLength(50)]
        [Required]
        public string CreatePerson { get; set; }

        [DisplayName("备注")]
        [StringLength(500)]
        public string Summary { get; set; }
    }
}
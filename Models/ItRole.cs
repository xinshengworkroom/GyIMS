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
    public class ItRole : IUnique, IName
    {
        string id = String.Empty;
        [DisplayName("角色编号")]
        [StringLength(50)]
        [Required]
        [Key]
        public string ID
        {
            get
            {
                if (String.IsNullOrEmpty(this.id))
                {
                    this.id = GeneratePrimaryKey.GetrimaryKey("ItRole");
                }
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }

        [DisplayName("角色编码")]
        [StringLength(150)]
        public string Code { get; set; }

        [DisplayName("角色名称")]
        [StringLength(150)]
        public string Name { get; set; }

        [DisplayName("按天成本（每人每天单价）")]
        
        public decimal DayCost { get; set; }

        [DisplayName("按时成本（每人每小时单价）")]
        
        public decimal HourCost { get; set; }

        [DisplayName("按分成本（每人每分单价）")]
        public decimal MinuteCost { get; set; }


        [DisplayName("按次计费价格")]
        public decimal NumCost { get; set; }

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
    }
}
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
    public class MaintenanceFee : IUnique, ICommonStatus
    {
        public MaintenanceFee()
        {
            this.CreateDate = DateTime.Now;
            this.UpdateDate = DateTime.Now;
            this.Status = CommonStatusEnum.Able;
        }
        string id = String.Empty;
        [DisplayName("运维费用编号")]
        [StringLength(50)]
        [Required]
        [Key]
        public string ID
        {
            get
            {
                if (String.IsNullOrEmpty(this.id))
                {
                    this.id = GeneratePrimaryKey.GetrimaryKey("MaintenanceFee");
                }
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }


        [DisplayName("运维单号")]
        [StringLength(150)]
        [Required]
        public string MaintenanceID { get; set; }

        [DisplayName("问题名称")]
        [NotMapped]
        public string MaintenanceName
        {
            get
            {
                return this.Maintenance == null ? String.Empty : this.Maintenance.Name;
            }
        }

        [DisplayName("处理人")]
        [StringLength(50)]
        public string Handler { get; set; }


        [DisplayName("IT角色编号")]
        [StringLength(50)]
        public string ItRoleID { get; set; }

        [DisplayName("IT角色")]
        [NotMapped]
        public string ITRoleName
        {
            get
            {
                return this.ItRole == null ? String.Empty : this.ItRole.Name;
            }
        }

        [DisplayName("开始时间")]
        public DateTime? UserBeginTime { get; set; }



        [DisplayName("结束时间")]

        public DateTime? UserEndTime { get; set; }



        [DisplayName("开始时间")]
        public DateTime? BeginTime { get; set; }


        [DisplayName("开始时间")]
        public DateTime? DelayTime { get; set; }



        [DisplayName("结束时间")]
        
        public DateTime? EndTime { get; set; }

        


        [DisplayName("花费时间（分）")]
        
        public decimal? SpendTime { get; set; }

        


        


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



        [ForeignKey("MaintenanceID")]
        public virtual Maintenance Maintenance { get; set; }


        [ForeignKey("ItRoleID")]
        public virtual ItRole ItRole { get; set; }

        


    }
}
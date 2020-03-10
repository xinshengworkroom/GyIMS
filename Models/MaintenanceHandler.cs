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
    public class MaintenanceHandler : IUnique
    {
        public MaintenanceHandler()
        {
            this.CreateDate = DateTime.Now;
            this.Status = CommonStatusEnum.Able;
        }

        string id = String.Empty;
        [DisplayName("运维处理编号")]
        [StringLength(50)]
        [Required]
        [Key]
        public string ID
        {
            get
            {
                if (String.IsNullOrEmpty(this.id))
                {
                    this.id = GeneratePrimaryKey.GetrimaryKey("MaintenanceHandler");
                }
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }



        [DisplayName("运维单号")]
        [StringLength(50)]
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

        

        [DisplayName("前序处理人")]
        public string FromHandler { get; set; }

        [DisplayName("处理人名称")]
        [NotMapped]
        public string HandlerName
        {
            get
            {
                return this.MaintenanceFee == null ? String.Empty : this.MaintenanceFee.Handler;
            }
        }

        [DisplayName("处理人")]
        [Required]
        public string Handler { get; set; }

        


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

        




        [ForeignKey("CreatePerson")]
        public virtual User UserCreate { get; set; }

        

        [ForeignKey("MaintenanceID")]
        public virtual Maintenance Maintenance { get; set; }

        [ForeignKey("FromHandler")]
        public virtual MaintenanceFee MaintenanceFee { get; set; }

    }   
}
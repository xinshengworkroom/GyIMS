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
    public class User : IName, ICommonStatus
    {


        public User()
        {
            this.CreateDate = DateTime.Now;
            this.UpdateDate = DateTime.Now;
            this.Status = CommonStatusEnum.Able;
        }


        string id = String.Empty;

        [DisplayName("用户编号")]
        [Required]
        [Key]
        public string ID
        {
            get
            {
                if (String.IsNullOrEmpty(this.id))
                {
                    this.id = GeneratePrimaryKey.GetrimaryKey("User");
                }
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }
        

        [DisplayName("员工工号")]
        [StringLength(150)]
        [Required]
        public string Code { get; set; }
     

        [DisplayName("用户名")]
        [StringLength(150)]
        [Required]
        public string Name { get; set; }

        [DisplayName("中文名称")]
        [StringLength(150)]
        [Required]
        public string ChineseName { get; set; }

        [DisplayName("英文名称")]
        [StringLength(150)]
        public string EnglishName { get; set; }

        [DisplayName("登录密码")]
        [StringLength(150)]
        [Required]
        public string Password { get; set; }

        [DisplayName("IT标志")]
        [Required]
        public IsITStatusEnum IsIT { get; set; }

        [DisplayName("IT名称")]
        [NotMapped]
        public string IsITName
        {
            get
            {
                return Display.GetEnumBrief(this.IsIT);
            }
        }

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

  


        [DisplayName("修改时间")]
        public DateTime? UpdateDate { get; set; }

        [DisplayName("修改人")]
        [StringLength(50)]
        public string UpdatePerson { get; set; }

       

        [DisplayName("备注")]
        [StringLength(500)]
        public string Summary { get; set; }


        List<UserLog> userLogs { get; set; }

        
    }
}
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
    public class Contact : IName, ICommonStatus
    {
        public Contact()
        {
            this.CreateDate = DateTime.Now;
            this.UpdateDate = DateTime.Now;
            this.Status = CommonStatusEnum.Able;
        }

        int? id;
        [DisplayName("联系人编号")]
        //[StringLength(50)]
        [Required]
        [Key]
        public int ID
        {
            get
            {
                if (this.id == null)
                {
                    this.id = int.Parse(GeneratePrimaryKey.GetrimaryKey("Contact"));
                }
                return this.id.Value;
            }
            set
            {
                this.id = value;
            }


        }

        [DisplayName("编码")]
        [StringLength(150)]
        [Required]
        public string Code { get; set; }
        

        [DisplayName("联系人名称")]
        [StringLength(150)]
        [Required]
        public string Name { get; set; }

        [DisplayName("固定电话")]
        [StringLength(50)]
        public string Tel { get; set; }

        [DisplayName("移动电话")]
        [StringLength(50)]
        public string Mobile { get; set; }

        [DisplayName("电子邮箱")]
        [StringLength(50)]
        public string Email { get; set; }

        [DisplayName("传真")]
        [StringLength(50)]
        public string Fax { get; set; }

        [DisplayName("邮政编码")]
        [StringLength(50)]
        public string PostCode { get; set; }

        [DisplayName("地址")]
        [StringLength(500)]
        [Required]
        public string Address { get; set; }


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
        //        return this.UserCreate.Name;
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
        //        return this.UserUpdate.Name;
        //    }
        //}


        [DisplayName("备注")]
        [StringLength(500)]
        public string Summary { get; set; }


        List<ContactLog> contactLogs { get; set; }



        //[ForeignKey("CreatePerson")]
        //public virtual User UserCreate { get; set; }

        //[ForeignKey("UpdatePerson")]
        //public virtual User UserUpdate { get; set; }
    }
}
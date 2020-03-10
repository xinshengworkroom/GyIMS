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

    /// <summary>
    /// 菜单标题
    /// </summary>
    public class MenuTitle
    {
        public MenuTitle()
        {
            this.CreateDate = DateTime.Now;
            this.UpdateDate = DateTime.Now;
        }

        string id = String.Empty;

        [DisplayName("ID")]
        [StringLength(50)]
        [Required]
        [Key]

        public string ID
        {
            get
            {
                if (String.IsNullOrEmpty(this.id))
                {
                    this.id = GeneratePrimaryKey.GetrimaryKey("MenuTitle");
                }
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }

        [DisplayName("名称")]
        [StringLength(50)]
        public string TitleName { get; set; }

        [DisplayName("状态")]
        [StringLength(50)]
        public string Status { get; set; }

        [DisplayName("序号")]
        public int SN { get; set; }

        [DisplayName("映射ID")]
        [StringLength(50)]
        public string HierID{ get ; set; }

        [DisplayName("等级")]
        [StringLength(50)]
        public string Rank { get; set; }

        [DisplayName("URL")]
        [StringLength(100)]
        public string LoginUrl { get; set; }

        [DisplayName("创建时间")]
        public DateTime? CreateDate { get; set; }

        [DisplayName("修改时间")]
        public DateTime? UpdateDate { get; set; }

    }

    public class MapsMenuTitle
    {

        public MapsMenuTitle()
        {
            this.CreateDate = DateTime.Now;
        }
        string id = String.Empty;
        [DisplayName("ID")]
        [StringLength(50)]
        [Required]
        [Key]

        public string HierID
        {
            get
            {
                if (String.IsNullOrEmpty(this.id))
                {
                    this.id = GeneratePrimaryKey.GetrimaryKey("MapsMenuTitle");
                }
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }

        [DisplayName("菜单编号")]
        [StringLength(50)]
        public string MapsTitleID { get; set; }



        [DisplayName("创建时间")]
        public DateTime? CreateDate { get; set; }

        [DisplayName("创建人")]
        [StringLength(50)]
        public string CreatePerson { get; set; }
    }

    public class Menu : IName, IUnique, ICommonStatus
    {

        public Menu()
        {
            this.CreateDate = DateTime.Now;
            this.UpdateDate = DateTime.Now;
            this.Status = CommonStatusEnum.Able;
        }

        string id = String.Empty;

        [DisplayName("菜单编号")]
        [StringLength(50)]
        [Required]
        [Key]
        public string ID
        {
            get
            {
                if (String.IsNullOrEmpty(this.id))
                {
                    this.id = GeneratePrimaryKey.GetrimaryKey("Menu");
                }
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }

        [DisplayName("父级菜单编号")]
        [StringLength(50)]
        public string ParentID { get; set; }

        [DisplayName("菜单层级")]
        [Required]
        public MenuRankEnum Rank { get; set; }

        [DisplayName("菜单层级名称")]
        [NotMapped]
        public string RankName
        {
            get
            {
                return Display.GetEnumBrief(this.Rank);
            }
        }

        [DisplayName("菜单名称")]
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


        [DisplayName("菜单地址")]
        [StringLength(150)]
        public string Url { get; set; }



        [DisplayName("菜单控制器名称")]
        [StringLength(150)]
        public string ControllerName { get; set; }



        [DisplayName("菜单操作名称")]
        [StringLength(150)]
        public string ActionName { get; set; }


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

        [DisplayName("创建人名称")]
        [NotMapped]
        public string CreatePersonName
        {
            get
            {
                return this.UserCreate == null ? String.Empty : this.UserCreate.Name;
            }
        }


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


       
        List<MenuLog> menuLogs { get; set; }


        [ForeignKey("CreatePerson")]
        public virtual User UserCreate { get; set; }


        [ForeignKey("UpdatePerson")]
        public virtual User UserUpdate { get; set; }
    }
}
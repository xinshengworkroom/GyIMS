using GyIMS.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GyIMS.Models
{
    public class PrimaryKey
    {
        [DisplayName("业务类型")]
        [StringLength(50)]
        [Required]
        [Key]
        public string BizType { get; set; }


        [DisplayName("起始字符")]
        [StringLength(10)]
        [Required]
        public string FromChar { get; set; }

        [DisplayName("当前索引")]
        [Required]
        public int CurrentIndex { get; set; }

        [DisplayName("序号分组类型")]
        [Required]
        public SeqGroupTypeEnum GroupType { get; set; }


        [DisplayName("流水号长度")]
        [Required]
        public int SerialLenth { get; set; }

        [DisplayName("状态")]
        [Required]
        public CommonStatusEnum Status { get; set; }

        [DisplayName("创建时间")]
        [Required]
        public DateTime? CreateDate { get; set; }

        [DisplayName("修改时间")]
        [Required]
        public DateTime? UpdateDate { get; set; }

        [DisplayName("备注")]
        [StringLength(500)]
        public string Summary { get; set; }

    }
}
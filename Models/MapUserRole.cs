﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GyIMS.Models
{
    public class MapUserRole
    {
        [Key]
        [DisplayName("角色编号")]
        [StringLength(50)]
        [Required]
        [Column(Order = 2)]
        public string RoleID { get; set; }

        [Key]
        [DisplayName("人员编号")]
        [StringLength(50)]
        [Required]
        [Column(Order = 1)]
        public string UserID { get; set; }

        [DisplayName("创建时间")]
        [Required]
        public DateTime? CreateDate { get; set; }

        [DisplayName("创建人")]
        [StringLength(50)]
        [Required]
        public string CreatePerson { get; set; }
    }
}
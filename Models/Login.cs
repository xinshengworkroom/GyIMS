using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace GyIMS.Models
{
    public class Login
    {
        [DisplayName("登录用户")]
        public User LoginUser { get; set; }

        [DisplayName("登录用户角色列表")]
        public List<Role> UserRoles { get; set; }

        public List<Menu> UserMenus { get; set; }

        [DisplayName("登录状态")]
        public bool IsLogin { get; set; }

        [DisplayName("登入时间")]
        public DateTime? LoginInDate { get; set; }
    }
}
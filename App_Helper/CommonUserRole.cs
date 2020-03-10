using GyIMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GyIMS.App_Helper
{
    public class CommonUserRole
    {
        public Role Role { get; set; }

        public User User { get; set; }

        public bool IsGrant { get; set; }

        public string UserID
        {
            get
            {
                return this.User == null ? String.Empty : this.User.Code;
            }
        }

        public string UserName
        {
            get
            {
                return this.User == null ? String.Empty : this.User.Name;
            }
        }


        public string ChineseName
        {
            get
            {
                return this.Role == null ? String.Empty : this.Role.ChineseName;
            }
        }


        public string EnglishName
        {
            get
            {
                return this.Role == null ? String.Empty : this.Role.EnglishName;
            }
        }


        public string RoleName
        {
            get
            {
                return this.Role == null ? String.Empty : this.Role.Name;
            }
        }


        public string RoleID
        {
            get
            {
                return this.Role == null ? String.Empty : this.Role.ID;
            }
        }
    }
}
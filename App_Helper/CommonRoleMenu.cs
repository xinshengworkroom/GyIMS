using GyIMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GyIMS.App_Helper
{
    public class CommonRoleMenu
    {

        public Role Role { get; set; }

        public Menu Menu { get; set; }

        public bool IsGrant { get; set; }

        public string MenuID
        {
            get
            {
                return this.Menu == null ? String.Empty : this.Menu.ID;
            }
        }

        public string MenuName
        {
            get
            {
                return this.Menu == null ? String.Empty : this.Menu.Name;
            }
        }


        public string ControllerName
        {
            get
            {
                return this.Menu == null ? String.Empty : this.Menu.ControllerName;
            }
        }


        public string ActionName
        {
            get
            {
                return this.Menu == null ? String.Empty : this.Menu.ActionName;
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
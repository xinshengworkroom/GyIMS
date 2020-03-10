using GyIMS.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GyIMS.Enums
{
    public enum MenuRankEnum
    {
        [Display("000", "登录界面", "登录界面")]
        Login = 0,
        [Display("001", "模块菜单", "模块菜单")]
        LevelOne = 1,
        [Display("002", "二级菜单", "二级菜单")]
        LevelTwo = 2,
        [Display("003", "三级菜单", "三级菜单")]
        LevelThree = 3,
        [Display("004", "四级菜单", "四级菜单")]
        LevelFour = 4
    }
}
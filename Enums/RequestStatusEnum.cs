using GyIMS.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GyIMS.Enums
{
    public enum RequestStatusEnum
    {
        [Display("001", "创建", "创建")]
        One = 001,
        [Display("002", "已申请", "已申请")]
        Two = 002,
        [Display("003", "报价", "报价")]
        Three = 003,
        [Display("004", "已确认", "已确认")]
        Four = 004,
        [Display("005", "已退回", "已退回")]
        Five = 005,
        [Display("006", "已取消", "已取消")]
        Six = 006,
        [Display("007", "已接单", "已接单")]
        Seven = 007,
        [Display("008", "分解任务", "分解任务")]
        Eight = 008,
        [Display("009", "开发中", "开发中")]
        Nine = 009,
        [Display("010", "已测试", "已测试")]
        Ten = 010,
        [Display("011", "已验收", "已验收")]
        Eleven = 011

    }
}
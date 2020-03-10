using GyIMS.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GyIMS.Enums
{
    public enum HSysTypeEnum
    {
        [Display("001", "台式电脑", "台式电脑")]
        台式电脑 = 001,
        [Display("002", "笔记本电脑", "笔记本电脑")]
        笔记本电脑 = 002,
        [Display("003", "打印机", "打印机")]
        打印机 = 003,
        [Display("004", "固定电话", "固定电话")]
        固定电话 = 004,
        [Display("005", "其他", "其他")]
        其他 = 005,
        [Display("006", "无问题", "无问题")]
        无问题 = 006,

    }
}
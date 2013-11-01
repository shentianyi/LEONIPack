using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.ComponentModel;
using System.Runtime.Serialization;


namespace Brilliantech.Packaging.Store.Data.Enum
{
    public enum PackageStatus
    {

        [Description("新建立")]
        NewCreated = 0,
        //Avilable,end with status 5 or 6

        [Description("正在装箱")]
        Begin = 1,

        [Description("未满箱暂停")]
        Unfull = 2,
        //Avilable, end with status 5 or 6

        [Description("从暂停恢复装箱")]
        Rebegin = 3,

        [Description("从故障恢复装箱")]
        BeginUnexpect = 4,
        //close with status 7

        [Description("正常结束")]
        Close = 5,

        [Description("未满箱强制结束")]
        CloseUnfull = 6,

        [Description("结束，期间有中断")]
        CloseWithException = 7,

        [Description("未满箱暂停，期间有中断")]
        UnfullExpection = 8,

        [Description("已取消")]
        Scraped = 999,


        [Description("返工建立")]
        ReworkNew = 9,

        [Description("返工开始")]
        ReworkBegin = 10,

        [Description("返工暂停")]
        ReworkUnfull = 11,

        [Description("返工从暂停恢复装箱")]
        ReworkRebegin = 12,

        [Description("返工从故障恢复装箱")]
        ReworkBeginUnexpect = 13,

        [Description("返工结束")]
        ReworkClose = 14,

        [Description("返工结束，期间有中断")]
        ReworkCloseWithException = 15,

        [Description("返工未满箱暂停，期间有中断")]
        ReworkUnfullExpection = 16,

        [Description("模板")]
        Template = 99
    }
}
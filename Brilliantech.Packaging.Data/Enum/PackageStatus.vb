Imports System.Reflection
Imports System.ComponentModel
Imports System.Runtime.Serialization
<DataContract()>
Public Enum PackageStatus
    <EnumMember()>
    <Description("新建立")>
    NewCreated = 0   'Avilable,end with status 5 or 6
    <EnumMember()>
    <Description("正在装箱")>
    Begin = 1
    <EnumMember()>
    <Description("未满箱暂停")>
    Unfull = 2   'Avilable, end with status 5 or 6
    <EnumMember()>
    <Description("从暂停恢复装箱")>
    Rebegin = 3
    <EnumMember()>
    <Description("从故障恢复装箱")>
    BeginUnexpect = 4    'close with status 7
    <EnumMember()>
    <Description("正常结束")>
    Close = 5
    <EnumMember()>
    <Description("未满箱强制结束")>
    CloseUnfull = 6
    <EnumMember()>
    <Description("结束，期间有中断")>
    CloseWithException = 7
    <EnumMember()>
    <Description("未满箱暂停，期间有中断")>
    UnfullExpection = 8
    <EnumMember()>
    <Description("已取消")>
    Scraped = 999

    <EnumMember()>
    <Description("返工建立")>
    ReworkNew = 9
    <EnumMember()>
    <Description("返工开始")>
    ReworkBegin = 10
    <EnumMember()>
    <Description("返工暂停")>
    ReworkUnfull = 11
    <EnumMember()>
    <Description("返工从暂停恢复装箱")>
    ReworkRebegin = 12
    <EnumMember()>
    <Description("返工从故障恢复装箱")>
    ReworkBeginUnexpect = 13
    <EnumMember()>
    <Description("返工结束")>
    ReworkClose = 14
    <EnumMember()>
    <Description("返工结束，期间有中断")>
    ReworkCloseWithException = 15
    <EnumMember()>
    <Description("返工未满箱暂停，期间有中断")>
    ReworkUnfullExpection = 16
    <EnumMember()>
    <Description("模板")>
    Template = 99
End Enum

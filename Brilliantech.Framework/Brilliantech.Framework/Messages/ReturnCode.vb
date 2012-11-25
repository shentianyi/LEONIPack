Imports System.Runtime.Serialization
Namespace Messages

    <DataContract()>
    Public Enum ReturnCode
        <EnumMember()>
        OK = 1
        <EnumMember()>
        Warning = 2
        <EnumMember()>
        [Error] = 3
        <EnumMember()>
        Fail = 4
        <EnumMember()>
        Unset = 0
    End Enum
End Namespace
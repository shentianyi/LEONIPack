Imports System.Runtime.Serialization

<DataContract()>
Public Class ReportGenConfig

    Private pf_template As String
    Private pf_printer As String
    Private pf_copies As Integer = 1

    ''' <summary>
    ''' 打印数量
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property NumberOfCopies As Integer
        Get
            Return pf_copies
        End Get
        Set(value As Integer)
            pf_copies = value
        End Set
    End Property

    ''' <summary>
    ''' 标签模板的绝对地址
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property Template As String
        Get
            Return pf_template
        End Get
        Set(value As String)
            pf_template = value
        End Set
    End Property

    ''' <summary>
    ''' Windows打印机名
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property Printer As String
        Get
            Return pf_printer
        End Get
        Set(value As String)
            pf_printer = value
        End Set
    End Property
End Class

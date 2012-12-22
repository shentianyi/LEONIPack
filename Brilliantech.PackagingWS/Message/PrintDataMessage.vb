Imports Brilliantech.Framework.WCF.Data
Imports Brilliantech.ReportGenConnector

''' <summary>
''' 打印数据传输消息类
''' </summary>
''' <remarks></remarks>
<DataContract()>
Public Class PrintDataMessage
    Inherits ServiceMessage

    Private pf_printTask As List(Of PrintTask)

    ''' <summary>
    ''' 打印数据
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DataMember()>
Public Property PrintTask As List(Of PrintTask)
        Get
            If pf_printTask Is Nothing Then
                pf_printTask = New List(Of PrintTask)
            End If
            Return pf_printTask
        End Get
        Set(ByVal value As List(Of PrintTask))
            pf_printTask = value
        End Set
    End Property
End Class

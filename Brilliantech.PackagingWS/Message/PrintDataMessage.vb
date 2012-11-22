Imports Brilliantech.Framework.WCF.Data
Imports Brilliantech.ReportGenConnector
<DataContract()>
Public Class PrintDataMessage
    Inherits ServiceMessage

    Private pf_printTask As List(Of PrintTask)
    <DataMember()>
    Public Property PrintTask As List(Of PrintTask)
        Get
            If pf_printTask Is Nothing Then
                pf_printTask = New List(Of PrintTask)
            End If
            Return pf_printTask
        End Get
        Set(value As List(Of PrintTask))
            pf_printTask = value
        End Set
    End Property
End Class

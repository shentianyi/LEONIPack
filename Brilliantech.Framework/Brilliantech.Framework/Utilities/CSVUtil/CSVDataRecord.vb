Public Class CSVDataRecord
    Private data_record As List(Of String)

    Public Property DataRecord As List(Of String)
        Get
            If data_record Is Nothing Then
                data_record = New List(Of String)
            End If
            Return data_record
        End Get
        Set(ByVal value As List(Of String))
            data_record = value
        End Set
    End Property
    Public Sub Add(ByVal record As String)
        Me.DataRecord.Add(record)
    End Sub
End Class
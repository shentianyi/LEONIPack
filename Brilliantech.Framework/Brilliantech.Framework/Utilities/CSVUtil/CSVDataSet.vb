Public Class CSVDataSet

    Private data_record As List(Of CSVDataRecord)

    Public Property DataRecord As List(Of CSVDataRecord)
        Get
            If data_record Is Nothing Then
                data_record = New List(Of CSVDataRecord)
            End If
            Return data_record
        End Get
        Set(ByVal value As List(Of CSVDataRecord))
            data_record = value
        End Set
    End Property

    Public Sub Add(ByVal record As CSVDataRecord)
        Me.DataRecord.Add(record)
    End Sub

    Public Sub AddRange(ByVal records As List(Of CSVDataRecord))
        Me.DataRecord.AddRange(records)
    End Sub
End Class
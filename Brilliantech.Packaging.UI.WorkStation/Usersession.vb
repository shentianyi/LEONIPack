Public Class Usersession
    Private Shared _WorkStationNr As String = String.Empty
    Public Shared Property WorkStationNr As String
        Get
            Return _WorkStationNr
        End Get
        Set(ByVal value As String)
            _WorkStationNr = value
        End Set
    End Property

    Public Shared Sub Clear()
        WorkStationNr = String.Empty
    End Sub
End Class

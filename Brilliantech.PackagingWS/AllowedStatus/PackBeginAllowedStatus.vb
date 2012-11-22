Imports Brilliantech.Packaging.Data
Public Class PackBeginAllowedStatus


    Private Shared _AllowedStatus As List(Of PackageStatus)


    Public Shared ReadOnly Property AllowedStatus As List(Of PackageStatus)
        Get
            _AllowedStatus = New List(Of PackageStatus)
            _AllowedStatus.Add(PackageStatus.NewCreated)
            _AllowedStatus.Add(PackageStatus.Unfull)
            _AllowedStatus.Add(PackageStatus.UnfullExpection)
            _AllowedStatus.Add(PackageStatus.ReworkNew)
            _AllowedStatus.Add(PackageStatus.ReworkUnfull)
            _AllowedStatus.Add(PackageStatus.ReworkUnfullExpection)
            Return _AllowedStatus
        End Get
    End Property

    Public Shared Function IsAllowed(stat As PackageStatus) As Boolean
        Return AllowedStatus.Contains(stat)
    End Function

End Class


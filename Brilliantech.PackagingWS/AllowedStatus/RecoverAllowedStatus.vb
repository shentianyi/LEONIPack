Imports Brilliantech.Packaging.Data
Public Class RecoverAllowedStatus


    Private Shared _AllowedStatus As List(Of PackageStatus)


    Public Shared ReadOnly Property AllowedStatus As List(Of PackageStatus)
        Get
            _AllowedStatus = New List(Of PackageStatus)
            _AllowedStatus.Add(PackageStatus.Begin)
            _AllowedStatus.Add(PackageStatus.BeginUnexpect)
            _AllowedStatus.Add(PackageStatus.Rebegin)
            _AllowedStatus.Add(PackageStatus.ReworkBegin)
            _AllowedStatus.Add(PackageStatus.ReworkBeginUnexpect)
            _AllowedStatus.Add(PackageStatus.ReworkRebegin)
            Return _AllowedStatus
        End Get
    End Property

    Public Shared Function IsAllowed(stat As PackageStatus) As Boolean
        Return AllowedStatus.Contains(stat)
    End Function
End Class

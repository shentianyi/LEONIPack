Partial Public Class SinglePackage
    Implements IModify(Of SinglePackage)


    Public Sub Modify(updated As SinglePackage) Implements IModify(Of SinglePackage).Modify
        If updated Is Nothing Then
            Throw New ArgumentNullException("updatedPackage")
        End If
        Me.partNr = updated.partNr
        Me.planedDate = updated.planedDate
        Me.status = updated.status
        Me.wrkstnID = updated.wrkstnID
        Me.capa = updated.capa
    End Sub
End Class

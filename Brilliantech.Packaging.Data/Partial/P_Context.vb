Partial Public Class PackagingDataDataContext
    Implements IUnitofWork


    Public Sub Commit() Implements IUnitofWork.Commit
        Me.SubmitChanges()
    End Sub
End Class

Partial Public Class PartContainerLabel
    Implements IModify(Of PartContainerLabel)

    Public Sub Modify(updated As PartContainerLabel) Implements IModify(Of PartContainerLabel).Modify
        If updated Is Nothing Then
            Throw New ArgumentNullException("updated")
        End If

        Me.labelTemplateName = updated.labelTemplateName
        Me.labelTeplateFile = updated.labelTeplateFile
        Me.labelType = updated.labelType
        Me.partNr = updated.partNr
        Me.piece = updated.piece
    End Sub
End Class

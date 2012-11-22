Imports Brilliantech.Packaging.Data
Public Class ContainerLabelRepo
    Inherits RepoBase
    Implements IContainerLabelRepo



    Public Sub New(context As PackagingDataDataContext)
        MyBase.New(context)
    End Sub

    Public Sub Delete(id As String) Implements IBaseRepo(Of PartContainerLabel).Delete
        Dim toDel As PartContainerLabel = GetByID(id)
        _context.PartContainerLabels.DeleteOnSubmit(toDel)
    End Sub

    Public Function GetAll() As System.Collections.Generic.IEnumerable(Of PartContainerLabel) Implements IBaseRepo(Of PartContainerLabel).GetAll
        Return _context.PartContainerLabels
    End Function

    Public Function GetByID(id As String) As Data.PartContainerLabel Implements IBaseRepo(Of Data.PartContainerLabel).GetByID
        Return _context.PartContainerLabels.Single(Function(c) c.relID = id)
    End Function

    Public Sub Insert(entity As Data.PartContainerLabel) Implements IBaseRepo(Of Data.PartContainerLabel).Insert
        If entity Is Nothing Then
            Throw New ArgumentNullException("entity")
        End If
        _context.PartContainerLabels.InsertOnSubmit(entity)
    End Sub

    Public Sub Modify(entity As Data.PartContainerLabel) Implements IBaseRepo(Of Data.PartContainerLabel).Modify
        If entity Is Nothing Then
            Throw New ArgumentNullException("entity")
        End If
        If String.IsNullOrEmpty(entity.relID) Then
            Throw New ArgumentException("ID不能为空值")
        End If
        Dim toModify As PartContainerLabel = Me.GetByID(entity.relID)
        toModify.Modify(entity)
    End Sub

    Public Function GetByPartNumber(partNr As String) As System.Collections.Generic.IEnumerable(Of Data.PartContainerLabel) Implements IContainerLabelRepo.GetByPartNumber
        Return (From label In _context.PartContainerLabels Where label.partNr = partNr Select label)
    End Function

    Public Function Exists(id As String) As Boolean Implements IBaseRepo(Of Data.PartContainerLabel).Exists

    End Function
End Class

Imports Brilliantech.Packaging.Data
Public Class EntityPartRelRepo
    Inherits RepoBase
    Implements IEntityPartRelRepo

    Public Sub New(ByVal context As PackagingDataDataContext)
        MyBase.New(context)
    End Sub

    Public Sub Delete(ByVal id As String) Implements IBaseRepo(Of Data.PartEntityRel).Delete
        Throw New Exception("not implemented")
    End Sub

    Public Function Exists(ByVal id As String) As Boolean Implements IBaseRepo(Of Data.PartEntityRel).Exists
        Throw New Exception("not implemented")
    End Function

    Public Function GetAll() As System.Collections.Generic.IEnumerable(Of Data.PartEntityRel) Implements IBaseRepo(Of Data.PartEntityRel).GetAll
        Throw New Exception("not implemented")
    End Function

    Public Function GetByID(ByVal id As String) As Data.PartEntityRel Implements IBaseRepo(Of Data.PartEntityRel).GetByID
        Throw New Exception("not implemented")
    End Function

    Public Sub Insert(ByVal entity As Data.PartEntityRel) Implements IBaseRepo(Of Data.PartEntityRel).Insert
        Throw New Exception("not implemented")
    End Sub

    Public Sub Modify(ByVal entity As Data.PartEntityRel) Implements IBaseRepo(Of Data.PartEntityRel).Modify
        Throw New Exception("not implemented")
    End Sub

    Public Function GetByPartNumber(ByVal partNr As String) As System.Collections.Generic.IEnumerable(Of Data.PartEntityRel) Implements IEntityPartRelRepo.GetByPartNumber
        Return (From partEntyRel In _context.PartEntityRels Where String.Compare(partEntyRel.partNr, partNr, True) = 0 Select partEntyRel)
    End Function
End Class

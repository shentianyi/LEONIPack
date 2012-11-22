Imports Brilliantech.Packaging.Data
Public Class PartRepo
    Inherits RepoBase
    Implements IPartRepository






    Public Sub New(context As PackagingDataDataContext)
        MyBase.New(context)
    End Sub


    Public Sub Delete(id As String) Implements IBaseRepo(Of Data.Part).Delete
        Throw New MethodNotImplementException
    End Sub

    Public Function GetAll() As System.Collections.Generic.IEnumerable(Of Data.Part) Implements IBaseRepo(Of Data.Part).GetAll
        Return _context.Parts
    End Function

    Public Function GetByID(id As String) As Data.Part Implements IBaseRepo(Of Data.Part).GetByID
        Return _context.Parts.Single(Function(c) c.partNr = id)
    End Function

    Public Sub Insert(entity As Data.Part) Implements IBaseRepo(Of Data.Part).Insert
        Throw New MethodNotImplementException
    End Sub

    Public Sub Modify(entity As Data.Part) Implements IBaseRepo(Of Data.Part).Modify
        Throw New MethodNotImplementException
    End Sub

    Public Function GetByProjectID(projID As String) As System.Collections.Generic.IEnumerable(Of Data.Part) Implements IPartRepository.GetByProjectID
        Return (From parts In _context.Parts Where parts.projectID = projID Select parts)
    End Function

    Public Function Exists(id As String) As Boolean Implements IBaseRepo(Of Data.Part).Exists
        Throw New MethodNotImplementException
    End Function
End Class

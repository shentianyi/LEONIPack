Imports Brilliantech.Packaging.Data
Public Class ProjectRepo
    Inherits RepoBase
    Implements IProjectRepo


    Public Sub New(context As PackagingDataDataContext)
        MyBase.new(context)
    End Sub

    Public Sub Delete(id As String) Implements IBaseRepo(Of Data.Project).Delete
        Throw New MethodNotImplementException
    End Sub

    Public Function GetAll() As System.Collections.Generic.IEnumerable(Of Data.Project) Implements IBaseRepo(Of Data.Project).GetAll
        Return _context.Projects
    End Function

    Public Function GetByID(id As String) As Data.Project Implements IBaseRepo(Of Data.Project).GetByID
        Return _context.Projects.Single(Function(c) c.projectID = id)
    End Function

    Public Sub Insert(entity As Data.Project) Implements IBaseRepo(Of Data.Project).Insert
        Throw New MethodNotImplementException
    End Sub

    Public Sub Modify(entity As Data.Project) Implements IBaseRepo(Of Data.Project).Modify
        Throw New MethodNotImplementException
    End Sub

    Public Function Exists(id As String) As Boolean Implements IBaseRepo(Of Data.Project).Exists
        Throw New MethodNotImplementException
    End Function
End Class

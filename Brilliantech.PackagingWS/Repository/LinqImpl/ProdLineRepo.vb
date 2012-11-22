Imports Brilliantech.Packaging.Data
Public Class ProdLineRepo
    Inherits RepoBase
    Implements IProdLineRepo




    Public Sub New(context As PackagingDataDataContext)
        MyBase.New(context)
    End Sub


    Public Sub Delete(id As String) Implements IBaseRepo(Of Data.ProdLine).Delete
        Throw New MethodNotImplementException
    End Sub

    Public Function GetAll() As System.Collections.Generic.IEnumerable(Of Data.ProdLine) Implements IBaseRepo(Of Data.ProdLine).GetAll
        Return _context.ProdLines
    End Function

    Public Function GetByID(id As String) As Data.ProdLine Implements IBaseRepo(Of Data.ProdLine).GetByID
        Return _context.ProdLines.Single(Function(c) c.prodLineID = id)
    End Function

    Public Sub Insert(entity As Data.ProdLine) Implements IBaseRepo(Of Data.ProdLine).Insert
        Throw New MethodNotImplementException
    End Sub

    Public Sub Modify(entity As Data.ProdLine) Implements IBaseRepo(Of Data.ProdLine).Modify
        Throw New MethodNotImplementException
    End Sub

    Public Function GetByProjectID(projId As String) As System.Collections.Generic.IEnumerable(Of Data.ProdLine) Implements IProdLineRepo.GetByProjectID
        Return (From prodl In _context.ProdLines Where prodl.projectID = projId Select prodl)
    End Function

    Public Function Exists(id As String) As Boolean Implements IBaseRepo(Of Data.ProdLine).Exists
        Throw New MethodNotImplementException
    End Function
End Class

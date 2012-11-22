Imports Brilliantech.Packaging.Data
Public Class WorkStationRepo
    Inherits RepoBase
    Implements IWorkStationRepo



    Public Sub New(context As PackagingDataDataContext)
        MyBase.New(context)
    End Sub

    Public Sub Delete(id As String) Implements IBaseRepo(Of Data.WorkStation).Delete
        Throw New MethodNotImplementException
    End Sub

    Public Function GetAll() As System.Collections.Generic.IEnumerable(Of Data.WorkStation) Implements IBaseRepo(Of Data.WorkStation).GetAll
        Return _context.WorkStations
    End Function

    Public Function GetByID(id As String) As Data.WorkStation Implements IBaseRepo(Of Data.WorkStation).GetByID
        Return _context.WorkStations.Single(Function(c) c.wrkStnID = id)
    End Function

    Public Sub Insert(entity As Data.WorkStation) Implements IBaseRepo(Of Data.WorkStation).Insert
        Throw New MethodNotImplementException
    End Sub

    Public Sub Modify(entity As Data.WorkStation) Implements IBaseRepo(Of Data.WorkStation).Modify
        Throw New MethodNotImplementException
    End Sub

    Public Function GetByProdLineID(prodLineID As String) As System.Collections.Generic.IEnumerable(Of Data.WorkStation) Implements IWorkStationRepo.GetByProdLineID
        Return (From ws In _context.WorkStations Where ws.prodLineID = prodLineID Select ws)
    End Function

    Public Function GetByProjectId(projectId As String) As System.Collections.Generic.IEnumerable(Of Data.WorkStation) Implements IWorkStationRepo.GetByProjectId
        Return (From ws In _context.WorkStations Where ws.ProdLine.projectID = projectId Select ws)
    End Function

    Public Function Exists(id As String) As Boolean Implements IBaseRepo(Of Data.WorkStation).Exists
        Throw New MethodNotImplementException
    End Function
End Class

Imports Brilliantech.Packaging.Data

''' <summary>
''' Workstation表记录的仓库类
''' </summary>
''' <remarks></remarks>
Public Class WorkStationRepo
    Inherits RepoBase
    Implements IWorkStationRepo


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="context"></param>
    ''' <remarks></remarks>
    Public Sub New(context As PackagingDataDataContext)
        MyBase.New(context)
    End Sub

    ''' <summary>
    ''' 方法未实现
    ''' </summary>
    ''' <param name="id"></param>
    ''' <remarks></remarks>
    Public Sub Delete(id As String) Implements IBaseRepo(Of Data.WorkStation).Delete
        Throw New MethodNotImplementException
    End Sub

    ''' <summary>
    ''' 获得所有的Workstation纪录
    ''' </summary>
    ''' <returns>Workstation的IEnumerable集合</returns>
    ''' <remarks></remarks>
    Public Function GetAll() As System.Collections.Generic.IEnumerable(Of Data.WorkStation) Implements IBaseRepo(Of Data.WorkStation).GetAll
        Return _context.WorkStations
    End Function

    ''' <summary>
    ''' 根据ID号获取Workstation纪录
    ''' </summary>
    ''' <param name="id">Workstation ID号</param>
    ''' <returns>根据ID号查询到的Workstation对象或空值</returns>
    ''' <remarks></remarks>
    Public Function GetByID(id As String) As Data.WorkStation Implements IBaseRepo(Of Data.WorkStation).GetByID
        Return _context.WorkStations.SingleOrDefault(Function(c) c.WrkStnID = id)
    End Function

    ''' <summary>
    ''' 方法未实现
    ''' </summary>
    ''' <param name="entity"></param>
    ''' <remarks></remarks>
    Public Sub Insert(entity As Data.WorkStation) Implements IBaseRepo(Of Data.WorkStation).Insert
        Throw New MethodNotImplementException
    End Sub

    ''' <summary>
    ''' 方法未实现
    ''' </summary>
    ''' <param name="entity"></param>
    ''' <remarks></remarks>
    Public Sub Modify(entity As Data.WorkStation) Implements IBaseRepo(Of Data.WorkStation).Modify
        Throw New MethodNotImplementException
    End Sub

    ''' <summary>
    ''' 根据生产线ID获取所有的Workstation实例
    ''' </summary>
    ''' <param name="prodLineID">生产线ID</param>
    ''' <returns>Workstation的IEnumerable集合</returns>
    ''' <remarks></remarks>
    Public Function GetByProdLineID(prodLineID As String) As System.Collections.Generic.IEnumerable(Of Data.WorkStation) Implements IWorkStationRepo.GetByProdLineID
        Return (From ws In _context.WorkStations Where ws.prodLineID = prodLineID Select ws)
    End Function

    ''' <summary>
    ''' 根据项目ID获取所有的Workstation纪录
    ''' </summary>
    ''' <param name="projectId">项目ID号</param>
    ''' <returns>Workstation的IEnumerable集合</returns>
    ''' <remarks></remarks>
    Public Function GetByProjectId(projectId As String) As System.Collections.Generic.IEnumerable(Of Data.WorkStation) Implements IWorkStationRepo.GetByProjectId
        Return (From ws In _context.WorkStations Where ws.ProdLine.projectID = projectId Select ws)
    End Function

    ''' <summary>
    ''' 方法未实现
    ''' </summary>
    ''' <param name="id"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Exists(id As String) As Boolean Implements IBaseRepo(Of Data.WorkStation).Exists
        Dim work As WorkStation = _context.WorkStations.SingleOrDefault(Function(c) String.Compare(c.WrkStnID, id, True) = 0)
        If work Is Nothing Then
            Return False
        Else
            Return True
        End If
    End Function
End Class

Imports Brilliantech.Packaging.Data

''' <summary>
''' Part表记录的仓库类
''' </summary>
''' <remarks></remarks>
Public Class PartRepo
    Inherits RepoBase
    Implements IPartRepository






    Public Sub New(ByVal context As PackagingDataDataContext)
        MyBase.New(context)
    End Sub

    ''' <summary>
    ''' 方法未实施
    ''' </summary>
    ''' <param name="id"></param>
    ''' <remarks></remarks>
    Public Sub Delete(ByVal id As String) Implements IBaseRepo(Of Data.Part).Delete
        Throw New MethodNotImplementException
    End Sub

    ''' <summary>
    ''' 获取所有的Part记录
    ''' </summary>
    ''' <returns>获取的Part IEnumerable集合</returns>
    ''' <remarks></remarks>
    Public Function GetAll() As System.Collections.Generic.IEnumerable(Of Data.Part) Implements IBaseRepo(Of Data.Part).GetAll
        Return _context.Parts
    End Function

    ''' <summary>
    ''' 根据ID获取一个Part记录
    ''' </summary>
    ''' <param name="id">Part ID</param>
    ''' <returns>Part实例对象或空值</returns>
    ''' <remarks></remarks>
    Public Function GetByID(ByVal id As String) As Data.Part Implements IBaseRepo(Of Data.Part).GetByID
        Return _context.Parts.Single(Function(c) c.PartNr = id)
    End Function

    ''' <summary>
    ''' 方法未实施
    ''' </summary>
    ''' <param name="entity"></param>
    ''' <remarks></remarks>
    Public Sub Insert(ByVal entity As Data.Part) Implements IBaseRepo(Of Data.Part).Insert
        Throw New MethodNotImplementException
    End Sub

    ''' <summary>
    ''' 方法未实施
    ''' </summary>
    ''' <param name="entity"></param>
    ''' <remarks></remarks>
    Public Sub Modify(ByVal entity As Data.Part) Implements IBaseRepo(Of Data.Part).Modify
        Throw New MethodNotImplementException
    End Sub

    ''' <summary>
    ''' 根据项目ID获取零件对象
    ''' </summary>
    ''' <param name="projID">项目ID</param>
    ''' <returns>获取的零件对象列表</returns>
    ''' <remarks></remarks>
    Public Function GetByProjectID(ByVal projID As String) As System.Collections.Generic.IEnumerable(Of Data.Part) Implements IPartRepository.GetByProjectID
        Return (From parts In _context.Parts Where parts.ProjectID = projID Select parts)
    End Function

    ''' <summary>
    ''' 方法未实施
    ''' </summary>
    ''' <param name="id"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Exists(ByVal id As String) As Boolean Implements IBaseRepo(Of Data.Part).Exists
        Throw New MethodNotImplementException
    End Function
End Class

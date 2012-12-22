Imports Brilliantech.Packaging.Data
''' <summary>
''' Project记录仓库类
''' </summary>
''' <remarks></remarks>
Public Class ProjectRepo
    Inherits RepoBase
    Implements IProjectRepo


    Public Sub New(ByVal context As PackagingDataDataContext)
        MyBase.new(context)
    End Sub

    ''' <summary>
    ''' 方法未实施
    ''' </summary>
    ''' <param name="id"></param>
    ''' <remarks></remarks>
    Public Sub Delete(ByVal id As String) Implements IBaseRepo(Of Data.Project).Delete
        Throw New MethodNotImplementException
    End Sub

    ''' <summary>
    ''' 获取所有Project记录
    ''' </summary>
    ''' <returns>返回Project记录的IEnumerable列表</returns>
    ''' <remarks></remarks>
    Public Function GetAll() As System.Collections.Generic.IEnumerable(Of Data.Project) Implements IBaseRepo(Of Data.Project).GetAll
        Return _context.Projects
    End Function

    ''' <summary>
    ''' 根据ID号获取Project记录
    ''' </summary>
    ''' <param name="id">Project ID</param>
    ''' <returns>Project记录或空值</returns>
    ''' <remarks></remarks>
    Public Function GetByID(ByVal id As String) As Data.Project Implements IBaseRepo(Of Data.Project).GetByID
        Return _context.Projects.Single(Function(c) c.ProjectID = id)
    End Function

    ''' <summary>
    ''' 方法未实施
    ''' </summary>
    ''' <param name="entity"></param>
    ''' <remarks></remarks>
    Public Sub Insert(ByVal entity As Data.Project) Implements IBaseRepo(Of Data.Project).Insert
        Throw New MethodNotImplementException
    End Sub

    ''' <summary>
    ''' 方法未实施
    ''' </summary>
    ''' <param name="entity"></param>
    ''' <remarks></remarks>
    Public Sub Modify(ByVal entity As Data.Project) Implements IBaseRepo(Of Data.Project).Modify
        Throw New MethodNotImplementException
    End Sub

    ''' <summary>
    ''' 方法未实施
    ''' </summary>
    ''' <param name="id"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Exists(ByVal id As String) As Boolean Implements IBaseRepo(Of Data.Project).Exists
        Throw New MethodNotImplementException
    End Function
End Class

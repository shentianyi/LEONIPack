Imports Brilliantech.Packaging.Data

''' <summary>
''' Event 表记录的仓库类
''' </summary>
''' <remarks></remarks>
Public Class EventRepo
    Inherits RepoBase
    Implements IEventRepo





    Public Sub New(ByVal context As PackagingDataDataContext)
        MyBase.New(context)
    End Sub

    ''' <summary>
    ''' 方法未实施
    ''' </summary>
    ''' <param name="id"></param>
    ''' <remarks></remarks>
    Public Sub Delete(ByVal id As String) Implements IBaseRepo(Of Data.PackagingEvent).Delete
        Throw New MethodNotImplementException
    End Sub

    ''' <summary>
    ''' 获取所有Event表记录
    ''' </summary>
    ''' <returns>Event类型的IEnumerable集合</returns>
    ''' <remarks></remarks>
    Public Function GetAll() As System.Collections.Generic.IEnumerable(Of Data.PackagingEvent) Implements IBaseRepo(Of Data.PackagingEvent).GetAll
        Return _context.PackagingEvents
    End Function

    ''' <summary>
    ''' 通过ID获取一个Event记录
    ''' </summary>
    ''' <param name="id">Event ID</param>
    ''' <returns>一个Event实例对象或空值</returns>
    ''' <remarks></remarks>
    Public Function GetByID(ByVal id As String) As Data.PackagingEvent Implements IBaseRepo(Of Data.PackagingEvent).GetByID
        Return _context.PackagingEvents.Single(Function(c) c.EventID = id)
    End Function

    ''' <summary>
    ''' 方法未实施
    ''' </summary>
    ''' <param name="entity"></param>
    ''' <remarks></remarks>
    Public Sub Insert(ByVal entity As Data.PackagingEvent) Implements IBaseRepo(Of Data.PackagingEvent).Insert
        Throw New MethodNotImplementException
    End Sub

    ''' <summary>
    ''' 方法未实施
    ''' </summary>
    ''' <param name="entity"></param>
    ''' <remarks></remarks>
    Public Sub Modify(ByVal entity As Data.PackagingEvent) Implements IBaseRepo(Of Data.PackagingEvent).Modify
        Throw New MethodNotImplementException
    End Sub

    ''' <summary>
    ''' 方法未实施
    ''' </summary>
    ''' <param name="id"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Exists(ByVal id As String) As Boolean Implements IBaseRepo(Of Data.PackagingEvent).Exists
        Throw New MethodNotImplementException
    End Function
End Class

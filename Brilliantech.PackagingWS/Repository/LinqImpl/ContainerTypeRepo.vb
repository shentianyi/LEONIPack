Imports Brilliantech.Packaging.Data

''' <summary>
''' ContainerType表记录仓库类
''' </summary>
''' <remarks></remarks>
Public Class ContainerTypeRepo
    Inherits RepoBase
    Implements IContainerTypeRepo



    Public Sub New(ByVal context As PackagingDataDataContext)
        MyBase.New(context)
    End Sub

    ''' <summary>
    ''' 方法未实施
    ''' </summary>
    ''' <param name="id"></param>
    ''' <remarks></remarks>
    Public Sub Delete(ByVal id As String) Implements IBaseRepo(Of ContainerType).Delete
        Throw New MethodNotImplementException
    End Sub

    ''' <summary>
    ''' 获取所有的ContainerType表记录
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAll() As System.Collections.Generic.IEnumerable(Of Data.ContainerType) Implements IBaseRepo(Of Data.ContainerType).GetAll
        Return _context.ContainerTypes

    End Function

    ''' <summary>
    ''' 根据ID号码获取ContainerType记录
    ''' </summary>
    ''' <param name="id"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetByID(ByVal id As String) As Data.ContainerType Implements IBaseRepo(Of Data.ContainerType).GetByID
        Return _context.ContainerTypes.Single(Function(c) c.ContainerID = id)
    End Function


    ''' <summary>
    ''' 方法未实施
    ''' </summary>
    ''' <param name="entity"></param>
    ''' <remarks></remarks>
    Public Sub Insert(ByVal entity As Data.ContainerType) Implements IBaseRepo(Of Data.ContainerType).Insert
        Throw New MethodNotImplementException
    End Sub

    ''' <summary>
    ''' 方法未实施
    ''' </summary>
    ''' <param name="entity"></param>
    ''' <remarks></remarks>
    Public Sub Modify(ByVal entity As Data.ContainerType) Implements IBaseRepo(Of Data.ContainerType).Modify
        Throw New MethodNotImplementException
    End Sub

    ''' <summary>
    ''' 方法未实施
    ''' </summary>
    ''' <param name="id"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Exists(ByVal id As String) As Boolean Implements IBaseRepo(Of Data.ContainerType).Exists
        Throw New MethodNotImplementException
    End Function
End Class

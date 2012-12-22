Imports Brilliantech.Packaging.Data
''' <summary>
''' History表记录的仓库类
''' </summary>
''' <remarks></remarks>
Public Class HistoryRepo
    Inherits RepoBase
    Implements IHistoryRepo




    Public Sub New(ByVal context As PackagingDataDataContext)
        MyBase.New(context)
    End Sub

    ''' <summary>
    ''' 方法未实施
    ''' </summary>
    ''' <param name="id"></param>
    ''' <remarks></remarks>
    Public Sub Delete(ByVal id As String) Implements IBaseRepo(Of Data.History).Delete
        Throw New MethodNotImplementException
    End Sub

    ''' <summary>
    ''' 方法未实施
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAll() As System.Collections.Generic.IEnumerable(Of Data.History) Implements IBaseRepo(Of Data.History).GetAll
        Throw New MethodNotImplementException
    End Function

    ''' <summary>
    ''' 方法未实施
    ''' </summary>
    ''' <param name="id"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetByID(ByVal id As String) As Data.History Implements IBaseRepo(Of Data.History).GetByID
        Throw New MethodNotImplementException
    End Function

    ''' <summary>
    ''' 方法未实施
    ''' </summary>
    ''' <param name="entity"></param>
    ''' <remarks></remarks>
    Public Sub Insert(ByVal entity As Data.History) Implements IBaseRepo(Of Data.History).Insert
        Throw New MethodNotImplementException
    End Sub

    ''' <summary>
    ''' 方法未实施
    ''' </summary>
    ''' <param name="entity"></param>
    ''' <remarks></remarks>
    Public Sub Modify(ByVal entity As Data.History) Implements IBaseRepo(Of Data.History).Modify
        Throw New MethodNotImplementException
    End Sub

    ''' <summary>
    ''' 方法未实施
    ''' </summary>
    ''' <param name="id"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Exists(ByVal id As String) As Boolean Implements IBaseRepo(Of Data.History).Exists
        Throw New MethodNotImplementException
    End Function
End Class

Imports Brilliantech.Packaging.Data
''' <summary>
''' ProdLine的表记录仓库类
''' </summary>
''' <remarks></remarks>
Public Class ProdLineRepo
    Inherits RepoBase
    Implements IProdLineRepo




    Public Sub New(ByVal context As PackagingDataDataContext)
        MyBase.New(context)
    End Sub

    ''' <summary>
    ''' 方法未实施
    ''' </summary>
    ''' <param name="id"></param>
    ''' <remarks></remarks>
    Public Sub Delete(ByVal id As String) Implements IBaseRepo(Of Data.ProdLine).Delete
        Throw New MethodNotImplementException
    End Sub

    ''' <summary>
    ''' 获取所有ProdLine记录
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAll() As System.Collections.Generic.IEnumerable(Of Data.ProdLine) Implements IBaseRepo(Of Data.ProdLine).GetAll
        Return _context.ProdLines
    End Function

    ''' <summary>
    ''' 通过ID号获取一个Prodline记录
    ''' </summary>
    ''' <param name="id">ProdLine ID</param>
    ''' <returns>一个ProdLine实例对象或空值</returns>
    ''' <remarks></remarks>
    Public Function GetByID(ByVal id As String) As Data.ProdLine Implements IBaseRepo(Of Data.ProdLine).GetByID
        Return _context.ProdLines.Single(Function(c) c.ProdLineID = id)
    End Function

    ''' <summary>
    ''' 方法未实施
    ''' </summary>
    ''' <param name="entity"></param>
    ''' <remarks></remarks>
    Public Sub Insert(ByVal entity As Data.ProdLine) Implements IBaseRepo(Of Data.ProdLine).Insert
        Throw New MethodNotImplementException
    End Sub

    ''' <summary>
    ''' 方法未实施
    ''' </summary>
    ''' <param name="entity"></param>
    ''' <remarks></remarks>
    Public Sub Modify(ByVal entity As Data.ProdLine) Implements IBaseRepo(Of Data.ProdLine).Modify
        Throw New MethodNotImplementException
    End Sub

    ''' <summary>
    ''' 通过ProjectID获取ProdLine记录
    ''' </summary>
    ''' <param name="projId">Project ID</param>
    ''' <returns>ProdLine记录集合</returns>
    ''' <remarks></remarks>
    Public Function GetByProjectID(ByVal projId As String) As System.Collections.Generic.IEnumerable(Of Data.ProdLine) Implements IProdLineRepo.GetByProjectID
        Return (From prodl In _context.ProdLines Where prodl.ProjectID = projId Select prodl)
    End Function

    ''' <summary>
    ''' 方法未实施
    ''' </summary>
    ''' <param name="id"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Exists(ByVal id As String) As Boolean Implements IBaseRepo(Of Data.ProdLine).Exists
        Throw New MethodNotImplementException
    End Function
End Class

Imports Brilliantech.Packaging.Data

''' <summary>
''' ContainerLabel表的映射对象的仓库类
''' </summary>
''' <remarks></remarks>
Public Class ContainerLabelRepo
    Inherits RepoBase
    Implements IContainerLabelRepo



    Public Sub New(ByVal context As PackagingDataDataContext)
        MyBase.New(context)
    End Sub

    ''' <summary>
    ''' 删除一个ContainerLabel对象
    ''' </summary>
    ''' <param name="id">ContainerLabel的ID</param>
    ''' <remarks></remarks>
    Public Sub Delete(ByVal id As String) Implements IBaseRepo(Of PartContainerLabel).Delete
        Dim toDel As PartContainerLabel = GetByID(id)
        _context.PartContainerLabels.DeleteOnSubmit(toDel)
    End Sub

    ''' <summary>
    ''' 获取所有的ContainerLabel表纪录
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAll() As System.Collections.Generic.IEnumerable(Of PartContainerLabel) Implements IBaseRepo(Of PartContainerLabel).GetAll
        Return _context.PartContainerLabels
    End Function

    ''' <summary>
    ''' 通过ID号获取一个ContainerLabel对象
    ''' </summary>
    ''' <param name="id">ContainerLabel ID号</param>
    ''' <returns>获取到的对象实例或空值</returns>
    ''' <remarks></remarks>
    Public Function GetByID(ByVal id As String) As Data.PartContainerLabel Implements IBaseRepo(Of Data.PartContainerLabel).GetByID
        Return _context.PartContainerLabels.Single(Function(c) c.RelID = id)
    End Function

    ''' <summary>
    ''' 插入一个ContainerLabel对象
    ''' </summary>
    ''' <param name="entity">待插入的ContainerLabel对象</param>
    ''' <remarks></remarks>
    Public Sub Insert(ByVal entity As Data.PartContainerLabel) Implements IBaseRepo(Of Data.PartContainerLabel).Insert
        If entity Is Nothing Then
            Throw New ArgumentNullException("entity")
        End If
        _context.PartContainerLabels.InsertOnSubmit(entity)
    End Sub

    ''' <summary>
    ''' 修改一个已存在的ContainerLabel记录
    ''' </summary>
    ''' <param name="entity">修改完成的ContainerLabel实例</param>
    ''' <remarks></remarks>
    Public Sub Modify(ByVal entity As Data.PartContainerLabel) Implements IBaseRepo(Of Data.PartContainerLabel).Modify
        If entity Is Nothing Then
            Throw New ArgumentNullException("entity")
        End If
        If String.IsNullOrEmpty(entity.RelID) Then
            Throw New ArgumentException("ID不能为空值")
        End If
        Dim toModify As PartContainerLabel = Me.GetByID(entity.RelID)
        toModify.Modify(entity)
    End Sub

    ''' <summary>
    ''' 通过零件号获得对应的ContainerLabel对象
    ''' </summary>
    ''' <param name="partNr">零件号</param>
    ''' <returns>查找到的ContainerLabel对象</returns>
    ''' <remarks></remarks>
    Public Function GetByPartNumber(ByVal partNr As String) As System.Collections.Generic.IEnumerable(Of Data.PartContainerLabel) Implements IContainerLabelRepo.GetByPartNumber
        Return (From label In _context.PartContainerLabels Where label.PartNr = partNr Select label)
    End Function

    ''' <summary>
    ''' 方法未实施
    ''' </summary>
    ''' <param name="id"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Exists(ByVal id As String) As Boolean Implements IBaseRepo(Of Data.PartContainerLabel).Exists

    End Function
End Class

﻿Imports Brilliantech.Packaging.Data
Imports System.Transactions


''' <summary>
''' SinglePackage表记录的仓库类
''' </summary>
''' <remarks></remarks>
Public Class SinglePackageRepo
    Inherits RepoBase
    Implements ISinglePackageRepo

    Public Sub New(context As PackagingDataDataContext)
        MyBase.New(context)
    End Sub

    ''' <summary>
    ''' 根据ID号删除一条SinglePackage纪录
    ''' </summary>
    ''' <param name="id">SinglePackage的ID</param>
    ''' <remarks></remarks>
    Public Sub Delete(id As String) Implements IBaseRepo(Of Data.SinglePackage).Delete
        Using scope As New TransactionScope
            Try
                Dim packageItems As IQueryable(Of PackageItem) = (From items In _context.PackageItems Where items.PackageID = id)
                _context.PackageItems.DeleteAllOnSubmit(packageItems)
                _context.SubmitChanges()
                Dim toDel As SinglePackage = Me.GetByID(id)
                _context.SinglePackages.DeleteOnSubmit(toDel)
                _context.SubmitChanges()
                scope.Complete()
            Catch ex As Exception
                Throw New Exception("删除包装时出现系统错误", ex)
            End Try

        End Using
     
    End Sub

    ''' <summary>
    ''' 获取所有SinglePackageID
    ''' </summary>
    ''' <returns>SinglePackage的IEnumerable集合</returns>
    ''' <remarks>不建议使用，会消耗大量服务器资源</remarks>
    Public Function GetAll() As System.Collections.Generic.IEnumerable(Of Data.SinglePackage) Implements IBaseRepo(Of Data.SinglePackage).GetAll
        Return _context.SinglePackages
    End Function

    ''' <summary>
    ''' 根据包装箱内任意一个内容的追溯唯一号获取到相应的SinglePackage对象
    ''' </summary>
    ''' <param name="itemId">包装箱内任意一个内容的追溯唯一号</param>
    ''' <returns>SinglePackage对象或空值</returns>
    ''' <remarks></remarks>
    Public Function GetByItemId(itemId As String) As SinglePackage
        Dim item As PackageItem = _context.PackageItems.SingleOrDefault(Function(c) c.TNr.ToLower = itemId.ToLower)
        If item IsNot Nothing Then
            Return item.SinglePackage
        Else
            Return Nothing
        End If
    End Function

    ''' <summary>
    ''' 根据ID号获取唯一的SinglePackage对象
    ''' </summary>
    ''' <param name="id">SinglePackage ID</param>
    ''' <returns>SinglePackage对象</returns>
    ''' <remarks></remarks>
    Public Function GetByID(ByVal id As String) As Data.SinglePackage Implements IBaseRepo(Of Data.SinglePackage).GetByID
        Return _context.SinglePackages.SingleOrDefault(Function(c) c.PackageID = id)
    End Function

    ''' <summary>
    ''' 插入一个新的SinglePackage纪录
    ''' </summary>
    ''' <param name="entity">需要插入的SinglePackage对象</param>
    ''' <remarks></remarks>
    Public Sub Insert(entity As Data.SinglePackage) Implements IBaseRepo(Of Data.SinglePackage).Insert
        If entity Is Nothing Then
            Throw New ArgumentNullException("entity")
        End If
        _context.SinglePackages.InsertOnSubmit(entity)
    End Sub

    ''' <summary>
    ''' 修改一条已有的SinglePackage纪录
    ''' </summary>
    ''' <param name="entity">修改完成的SinglePackage对象</param>
    ''' <remarks></remarks>
    Public Sub Modify(entity As Data.SinglePackage) Implements IBaseRepo(Of Data.SinglePackage).Modify
        If entity Is Nothing Then
            Throw New ArgumentNullException("packageEntity")
        End If
        If Not Exists(entity.packageID) Then
            Throw New ArgumentException("包装号不存在")
        End If

        Dim toModify As SinglePackage = GetByID(entity.packageID)
        toModify.Modify(entity)
        _context.SubmitChanges()
    End Sub


    ''' <summary>
    ''' 为SinglePackage添加一个内容
    ''' </summary>
    ''' <param name="packageID">需要添加的Package ID</param>
    ''' <param name="newItem">需要新添加的Item对象</param>
    ''' <remarks></remarks>
    Public Sub AddItem(packageID As String, newItem As Data.PackageItem) Implements ISinglePackageRepo.AddItem
        If Not Exists(packageID) Then
            Throw New ArgumentException("包装号不存在")
        End If

        If newItem Is Nothing Then
            Throw New ArgumentNullException("newItem")
        End If

        Dim toModifyPackage As SinglePackage = GetByID(packageID)
        toModifyPackage.PackageItems.Add(newItem)
    End Sub

    ''' <summary>
    ''' 检查SinglePackage对象是否存在
    ''' </summary>
    ''' <param name="id">SinglePackage ID</param>
    ''' <returns>布尔值代表相应的SinglePackage对象存在或不存在</returns>
    ''' <remarks></remarks>
    Public Function Exists(id As String) As Boolean Implements IBaseRepo(Of Data.SinglePackage).Exists
        Dim counter As Integer = _context.SinglePackages.Count(Function(c) c.packageID = id)
        If counter > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' 根据包装内容里的Sequence ID来获取一条包装箱内容
    ''' </summary>
    ''' <param name="id">Package ID</param>
    ''' <returns>获取到的PackageItem或抛出异常</returns>
    ''' <remarks></remarks>
    Public Function GetLastItem(id As String) As PackageItem
        Dim items As List(Of PackageItem) = (From _items In _context.PackageItems Where _items.packageID = id).ToList
        Dim i As Integer = 0
        If items IsNot Nothing Then
            If items.Count > 0 Then
                For j As Integer = 0 To items.Count - 1
                    If items(j).itemSeq > i Then
                        i = j
                    End If
                Next
            End If
        End If
        Return items(i)
    End Function


    ''' <summary>
    ''' 统计在一个包装台上某个零件号某个包装箱ID下有多少个未满箱
    ''' </summary>
    ''' <param name="partNr">零件号</param>
    ''' <param name="containerId">包装箱ID</param>
    ''' <param name="wrkStnr">操作台ID</param>
    ''' <returns>整形数字，代表未满箱的数量</returns>
    ''' <remarks></remarks>
    Public Function CountUnfull(partNr As String, containerId As String, wrkStnr As String) As Integer

        Return _context.SinglePackages.Count(Function(c) c.containerID = containerId And c.partNr = partNr And c.wrkstnID = wrkStnr And (c.status = PackageStatus.Begin _
                                                                         Or c.status = PackageStatus.BeginUnexpect _
                                                                         Or c.status = PackageStatus.Rebegin Or c.status = PackageStatus.Unfull _
                                                                         Or c.status = PackageStatus.UnfullExpection))

    End Function

    ''' <summary>
    ''' 获取某个零件在某个包装台的间隔时间对象，间隔时间指两次扫描之前允许的最小时间跨度
    ''' </summary>
    ''' <param name="partNr">零件号</param>
    ''' <param name="wrkstnr">包装台号</param>
    ''' <returns>间隔时间对象，包含了允许的最小时间跨度</returns>
    ''' <remarks></remarks>
    Public Function GetPartTime(partNr As String, wrkstnr As String) As PartAllowedSec
        Return _context.PartAllowedSecs.Single(Function(c) c.partNr = partNr And c.wrkstnr = wrkstnr)
    End Function


    ''' <summary>
    ''' 根据Package Item ID 来检验对应的Item是否存在
    ''' </summary>
    ''' <param name="itemUID">Package Item的UID</param>
    ''' <returns>布尔值，代表ItemID对应的Package Item是否存在</returns>
    ''' <remarks></remarks>
    Public Function HasItem(ByVal itemUID As String) As Boolean
        Dim counter As Integer = _context.PackageItems.Count(Function(c) c.TNr = itemUID)
        If counter > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' 获取一个包装台某种包装方案的第一个未满箱对象ID号
    ''' </summary>
    ''' <param name="partNr">零件号</param>
    ''' <param name="containerId">包装方案号</param>
    ''' <param name="wrkstnr">包装台ID</param>
    ''' <returns>SinglePackage ID或者空字符串</returns>
    ''' <remarks></remarks>
    Public Function GetFirstUnfullPID(partNr As String, containerId As String, wrkstnr As String) As String
        Dim pid As String = ""
        Try
            pid = _context.SinglePackages.First(Function(c) c.containerID = containerId And c.partNr = partNr And c.wrkstnID = wrkstnr And (c.status = PackageStatus.Begin _
                                                                       Or c.status = PackageStatus.BeginUnexpect _
                                                                       Or c.status = PackageStatus.Rebegin Or c.status = PackageStatus.Unfull _
                                                                       Or c.status = PackageStatus.UnfullExpection)).packageID
        Catch ex As Exception

        End Try

        If Not String.IsNullOrEmpty(pid) Then
            Return pid
        Else
            Return String.Empty
        End If


    End Function

    ''' <summary>
    ''' 根据综合查找条件查找符合条件的SinglePackage对象集合
    ''' </summary>
    ''' <param name="pid">PackageID</param>
    ''' <param name="wrkstnr">包装台号</param>
    ''' <param name="tnr">内容的唯一号</param>
    ''' <param name="pnr">零件号</param>
    ''' <param name="packStatus">包装箱状态</param>
    ''' <param name="fromDate">开始时间</param>
    ''' <param name="toDate">结束时间</param>
    ''' <returns>符合条件的SinglePackage对象的List集合</returns>
    ''' <remarks></remarks>
    Public Function FindSinglePackage(ByVal pid As String, _
                                      ByVal wrkstnr As String, _
                                      ByVal tnr As String, _
                                      ByVal pnr As String, _
                                      ByVal packStatus As Integer, _
                                      ByVal fromDate As DateTime, _
                                      ByVal toDate As DateTime) As List(Of FullPackageInfo)

        Dim infos As List(Of FullPackageInfo) = New List(Of FullPackageInfo)
        If String.IsNullOrEmpty(tnr) Then
            Dim result = (From packs In _context.SinglePackages _
               Where IIf(String.IsNullOrEmpty(pid), True, packs.PackageID = pid) _
               Where IIf(String.IsNullOrEmpty(wrkstnr), True, wrkstnr = packs.WrkstnID) _
               Where IIf(packStatus < 0, True, packs.Status = packStatus) _
               Where (packs.PlanedDate <= toDate And packs.PlanedDate >= fromDate) _
               Select packs)
            If result IsNot Nothing Then
                For Each info As SinglePackage In result
                    Dim pack As FullPackageInfo = New FullPackageInfo
                    pack.Capa = info.Capa
                    pack.CustomerPartNr = info.Part.CustomerPartNr
                    pack.PartNr = info.PartNr
                    pack.PId = info.PackageID
                    pack.status = info.Status
                    pack.Wrkst = info.wrkstnID
                    pack.ContainerType = info.containerID
                    infos.Add(pack)
                Next
            End If
        Else
            Dim info As SinglePackage = GetByItemId(tnr)
            If info IsNot Nothing Then
                Dim pack As FullPackageInfo = New FullPackageInfo
                pack.Capa = info.Capa
                pack.CustomerPartNr = info.Part.CustomerPartNr
                pack.PartNr = info.PartNr
                pack.PId = info.PackageID
                pack.status = info.Status
                pack.Wrkst = info.WrkstnID
                infos.Add(pack)
            End If
          
        End If
       
        Return infos
    End Function

    ''' <summary>
    ''' 根据Package ID来查找相对应的Items
    ''' </summary>
    ''' <param name="pid">Package ID</param>
    ''' <returns>PackageItem的IQueryable对象</returns>
    ''' <remarks></remarks>
    Public Function GetItemsByPId(ByVal pid As String) As IQueryable(Of PackageItem)
        Return (From items In _context.PackageItems
                Where items.PackageID = pid
                Select items)
    End Function
End Class

''' <summary>
''' 实现IEqualityComparer的Comparer
''' </summary>
''' <remarks></remarks>
Public Class PackageItemByIdComparer
    Implements IEqualityComparer(Of PackageItem)

    ''' <summary>
    ''' 通过比较TNR来比较两个PackageItem是否一致
    ''' </summary>
    ''' <param name="x">Item 1</param>
    ''' <param name="y">Item 2</param>
    ''' <returns>布尔值,代表两个PackageItem是否一致</returns>
    ''' <remarks></remarks>
    Public Function Equals1(ByVal x As Data.PackageItem, ByVal y As Data.PackageItem) As Boolean Implements System.Collections.Generic.IEqualityComparer(Of Data.PackageItem).Equals
        If String.Compare(x.TNr, y.TNr, True) = 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' 方法未实现
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetHashCode1(ByVal obj As Data.PackageItem) As Integer Implements System.Collections.Generic.IEqualityComparer(Of Data.PackageItem).GetHashCode

    End Function
End Class

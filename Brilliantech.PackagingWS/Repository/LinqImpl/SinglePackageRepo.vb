Imports Brilliantech.Packaging.Data
Imports System.Transactions
Public Class SinglePackageRepo
    Inherits RepoBase
    Implements ISinglePackageRepo

    Public Sub New(context As PackagingDataDataContext)
        MyBase.New(context)
    End Sub

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

    Public Function GetAll() As System.Collections.Generic.IEnumerable(Of Data.SinglePackage) Implements IBaseRepo(Of Data.SinglePackage).GetAll
        Return _context.SinglePackages
    End Function

    Public Function GetByItemId(itemId As String) As SinglePackage
        Dim item As PackageItem = _context.PackageItems.SingleOrDefault(Function(c) c.TNr.ToLower = itemId.ToLower)
        If item IsNot Nothing Then
            Return item.SinglePackage
        Else
            Return Nothing
        End If
    End Function
    Public Function GetByID(id As String) As Data.SinglePackage Implements IBaseRepo(Of Data.SinglePackage).GetByID
        Return _context.SinglePackages.SingleOrDefault(Function(c) c.PackageID = id)
    End Function

    Public Sub Insert(entity As Data.SinglePackage) Implements IBaseRepo(Of Data.SinglePackage).Insert
        If entity Is Nothing Then
            Throw New ArgumentNullException("entity")
        End If
        _context.SinglePackages.InsertOnSubmit(entity)
    End Sub

    Public Sub Modify(entity As Data.SinglePackage) Implements IBaseRepo(Of Data.SinglePackage).Modify
        If entity Is Nothing Then
            Throw New ArgumentNullException("packageEntity")
        End If
        If Not Exists(entity.packageID) Then
            Throw New ArgumentException("包装号不存在")
        End If

        Dim toModify As SinglePackage = GetByID(entity.packageID)
        toModify.Modify(entity)
    End Sub

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

    Public Function Exists(id As String) As Boolean Implements IBaseRepo(Of Data.SinglePackage).Exists
        Dim counter As Integer = _context.SinglePackages.Count(Function(c) c.packageID = id)
        If counter > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

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

    Public Function CountUnfull(partNr As String, containerId As String, wrkStnr As String) As Integer

        Return _context.SinglePackages.Count(Function(c) c.containerID = containerId And c.partNr = partNr And c.wrkstnID = wrkStnr And (c.status = PackageStatus.Begin _
                                                                         Or c.status = PackageStatus.BeginUnexpect _
                                                                         Or c.status = PackageStatus.Rebegin Or c.status = PackageStatus.Unfull _
                                                                         Or c.status = PackageStatus.UnfullExpection))

    End Function

    Public Function GetPartTime(partNr As String, wrkstnr As String) As PartAllowedSec
        Return _context.PartAllowedSecs.Single(Function(c) c.partNr = partNr And c.wrkstnr = wrkstnr)
    End Function

    Public Function HasItem(itemUID As String)
        Dim counter As Integer = _context.PackageItems.Count(Function(c) c.TNr = itemUID)
        If counter > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

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
                    pack.Wrkst = info.WrkstnID
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

    Public Function GetItemsByPId(ByVal pid As String) As IQueryable(Of PackageItem)
        Return (From items In _context.PackageItems
                Where items.PackageID = pid
                Select items)
    End Function
End Class

Public Class PackageItemByIdComparer
    Implements IEqualityComparer(Of PackageItem)

    Public Function Equals1(ByVal x As Data.PackageItem, ByVal y As Data.PackageItem) As Boolean Implements System.Collections.Generic.IEqualityComparer(Of Data.PackageItem).Equals
        If String.Compare(x.TNr, y.TNr, True) = 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function GetHashCode1(ByVal obj As Data.PackageItem) As Integer Implements System.Collections.Generic.IEqualityComparer(Of Data.PackageItem).GetHashCode

    End Function
End Class

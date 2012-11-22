Imports Brilliantech.Packaging.Data
Imports Brilliantech.Framework.WCF.Data
Imports Brilliantech.Packaging.WS.IDService
Imports Brilliantech.Framework.Utilities.EnumUtil
Imports Nini.Config
'Imports log4net
'Imports log4net.Ext.TracableID
Imports System.IO
Public Class PackageUtil
    Private IDClient As NumericIDWSClient


    Private Shared myConfig As IConfigSource = New IniConfigSource _
                                             (Path.Combine(My.Application.Info.DirectoryPath, "WSConfig\WSConfig.ini"))


    Private Shared connStr As String = myConfig.Configs("Connection").Get("connStr")


    ' Private Shared log As ITracableIDLog = TracableIDLogManager.GetLogger("BusinessLogger")

    Public Function ChangeRecoverStatus(ChangeEndStatus As PackageStatus) As PackageStatus
        Select Case ChangeEndStatus
            Case PackageStatus.ReworkBegin
                Return PackageStatus.ReworkBeginUnexpect
            Case PackageStatus.ReworkBeginUnexpect
                Return PackageStatus.ReworkBeginUnexpect
            Case PackageStatus.ReworkRebegin
                Return PackageStatus.ReworkBeginUnexpect
            Case Else
                Return PackageStatus.BeginUnexpect
        End Select

    End Function


    Public Function HasSingleUnfull(packageID As String, containerId As String, wrkStnr As String) As Boolean
        Dim result As Boolean = True

        Try
            Dim unitOfWork As IUnitofWork = New PackagingDataDataContext(connStr)
            Dim packageRepo As SinglePackageRepo = New SinglePackageRepo(unitOfWork)
            Dim package As SinglePackage = packageRepo.GetByID(packageID)

            If packageRepo.CountUnfull(package.partNr, containerId, package.wrkstnID) < 1 Then
                result = False
            End If

        Catch ex As Exception

        End Try

        Return result

    End Function




    Public Function CheckReoverStatus(status As PackageStatus) As ServiceMessage
        Dim result As ServiceMessage = New ServiceMessage With {.ReturnedResult = False}
        If Not RecoverAllowedStatus.IsAllowed(status) Then
            result.ReturnedMessage.Add("包装ID状态 " & EnumContainer.GetEnumContent( _
                                       GetType(PackageStatus), status).Description & " 不需要恢复")
        Else
            result.ReturnedResult = True
        End If
        Return result
    End Function




    Public Function BasicValidatePackage(packageID As String) As ServiceMessage
        Dim result As ServiceMessage = New ServiceMessage With {.ReturnedResult = False}
        If String.IsNullOrEmpty(packageID) Then
            result.ReturnedMessage.Add("包装ID是空值，检查扫描枪，条码完整性后重试")
        Else
            Try
                Dim unitOfWork As IUnitofWork = New PackagingDataDataContext(connStr)
                Dim packageRepo As SinglePackageRepo = New SinglePackageRepo(unitOfWork)
                Dim package As SinglePackage = packageRepo.GetByID(packageID)
                If package Is Nothing Then
                    result.ReturnedMessage.Add("系统出错，找不到包装号信息")
                Else
                    result.ReturnedResult = True
                End If
            Catch ex As Exception
                result.ReturnedMessage.Add("系统出错，不能开始装箱，请联系管理员" & ex.ToString)
            End Try
        End If
        Return result
    End Function




    Public Function ChangeStatusToBegin(status) As PackageStatus
        Select Case status
            Case PackageStatus.NewCreated
                Return PackageStatus.Begin
            Case PackageStatus.Unfull
                Return PackageStatus.Rebegin
            Case PackageStatus.UnfullExpection
                Return PackageStatus.BeginUnexpect
            Case PackageStatus.ReworkNew
                Return PackageStatus.ReworkBegin
            Case PackageStatus.ReworkUnfull
                Return PackageStatus.ReworkRebegin
            Case PackageStatus.ReworkUnfullExpection
                Return PackageStatus.ReworkBeginUnexpect
            Case Else
                Throw New Exception("当前状态不能开始装箱")
        End Select
    End Function




    Public Function CheckPackageStatusforBegin(packageID As String, status As PackageStatus) As ServiceMessage
        Dim result As ServiceMessage = New ServiceMessage With {.ReturnedResult = False}
        If Not PackBeginAllowedStatus.IsAllowed(status) Then
            result.ReturnedMessage.Add("包装ID状态 " & EnumContainer.GetEnumContent( _
                                       GetType(PackageStatus), status).Description & " 不能用来开始装箱")
            If RecoverAllowedStatus.IsAllowed(status) Then
                result.ReturnedMessage.Add("包装曾经意外中断过，系统内的已装箱数量为 " & Me.CountItem(packageID) & "件，请启动中断恢复程序")
            End If
        Else
            result.ReturnedResult = True
        End If
        Return result
    End Function




    Public Function ChangeEndStatus(stat As PackageStatus) As PackageStatus
        Select Case stat
            Case PackageStatus.Begin
                Return PackageStatus.Close
            Case PackageStatus.Rebegin
                Return PackageStatus.Close
            Case PackageStatus.BeginUnexpect
                Return PackageStatus.CloseWithException
            Case PackageStatus.ReworkBegin
                Return PackageStatus.ReworkClose
            Case PackageStatus.ReworkRebegin
                Return PackageStatus.ReworkClose
            Case PackageStatus.ReworkBeginUnexpect
                Return PackageStatus.ReworkCloseWithException
            Case Else
                Throw New Exception("当前状态不能结束包装")
        End Select
    End Function




    Public Function CheckEndStatus(stat As PackageStatus) As ServiceMessage
        Dim result As ServiceMessage = New ServiceMessage With {.ReturnedResult = False}
        If EndAllowedStatus.IsAllowed(stat) = False Then
            result.ReturnedMessage.Add( _
                "包装ID状态 " & EnumContainer.GetEnumContent( _
                    GetType(PackageStatus), stat).Description & "不能用于结束包装")
        Else
            result.ReturnedResult = True
        End If
        Return result
    End Function




    Public Function ChangeSuspendStatus(stat As PackageStatus) As PackageStatus
        Select Case stat
            Case PackageStatus.Begin
                Return PackageStatus.Unfull
            Case PackageStatus.Rebegin
                Return PackageStatus.Unfull
            Case PackageStatus.BeginUnexpect
                Return PackageStatus.UnfullExpection
            Case PackageStatus.ReworkBegin
                Return PackageStatus.ReworkUnfull
            Case PackageStatus.ReworkRebegin
                Return PackageStatus.ReworkUnfull
            Case PackageStatus.ReworkBeginUnexpect
                Return PackageStatus.ReworkUnfullExpection
            Case Else
                Throw New Exception("当前状态不能暂停装箱")
        End Select
    End Function




    Public Function CheckSuspendStatus(stat As PackageStatus) As ServiceMessage
        Dim result As ServiceMessage = New ServiceMessage With {.ReturnedResult = False}
        If PauseAllowedStatus.IsAllowed(stat) = False Then
            result.ReturnedMessage.Add( _
                "包装ID状态 " & EnumContainer.GetEnumContent( _
                    GetType(PackageStatus), stat).Description & "不能用于暂停包装")
        Else
            result.ReturnedResult = True
        End If
        Return result
    End Function



    Public Function GetItemTnr(packageID As String, barcodeStr As String) As String
        Dim unitOfWork As IUnitofWork = New PackagingDataDataContext(connStr)
        Dim packageRepo As SinglePackageRepo = New SinglePackageRepo(unitOfWork)
        Dim package As SinglePackage = packageRepo.GetByID(packageID)
        If package.Part.SourceBarcodes.First.isUniq Then
            Return barcodeStr
        Else
            Return GetTID()
        End If
    End Function



    Public Function _addItem(packId As String, tnr As String) As ServiceMessage
        Dim result As ServiceMessage = New ServiceMessage With {.ReturnedResult = False}
        Try
            Dim unitOfWork As IUnitofWork = New PackagingDataDataContext(connStr)
            Dim packageRepo As SinglePackageRepo = New SinglePackageRepo(unitOfWork)
            Dim package As SinglePackage = packageRepo.GetByID(packId)
            If packageRepo.HasItem(tnr) Then
                result.ReturnedMessage.Add("产品唯一号已经存在，请确认是否重复扫描了同一件产品")
            Else
                package.PackageItems.Add(New PackageItem With _
                                    {.ItemUid = Guid.NewGuid, _
                                        .ItemSeq = package.PackageItems.Count + 1, _
                                     .PackageID = packId, _
                                     .PackagingTime = Now(), .TNr = tnr, .Rowguid = Guid.NewGuid})
                unitOfWork.Commit()
                result.ReturnedResult = True
            End If
           
        Catch ex As Exception
            result.ReturnedMessage.Add("装入单个货物时系统出错，请联系管理员：" & ex.Message)
        End Try
        Return result
    End Function




    Public Function CheckItemContent(packageid As String, barcodeContent As String) As ServiceMessage
        Dim result As ServiceMessage = New ServiceMessage With {.ReturnedResult = False}
        If String.IsNullOrEmpty(barcodeContent) Then
            result.ReturnedMessage.Add("读取的条形码内容为空")
        Else
            Dim unitOfWork As IUnitofWork = New PackagingDataDataContext(connStr)
            Dim packageRepo As SinglePackageRepo = New SinglePackageRepo(unitOfWork)
            Dim package As SinglePackage = packageRepo.GetByID(packageid)
            If package.PackageItems.Count = package.capa Then
                result.ReturnedMessage.Add("包装箱已经到达最大容量")
            Else
                Dim barcodeSetting As SourceBarcode = package.Part.SourceBarcodes.First
                Dim fixedContent As String = barcodeSetting.defaultFixedText
                Dim scanedFix As String = ""
                Try
                    scanedFix = barcodeContent.Substring(barcodeSetting.fromPosition, barcodeSetting.length)
                Catch ex As Exception

                End Try

                If String.Compare(fixedContent, scanedFix, _
                                  myConfig.Configs("BarcodeContent").GetBoolean("IgnoreBarcodeSource")) <> 0 Then
                    result.ReturnedMessage.Add("非指定产品，请检查是否是错误的产品")
                Else
                    result.ReturnedResult = True
                End If
            End If
        End If
        Return result
    End Function




    Public Function CheckPackageIDStatusForAdd(packageID As String) As ServiceMessage
        Dim result As ServiceMessage = New ServiceMessage With {.ReturnedResult = False}
        Dim unitOfWork As IUnitofWork = New PackagingDataDataContext(connStr)
        Dim packageRepo As SinglePackageRepo = New SinglePackageRepo(unitOfWork)
        Dim package As SinglePackage = packageRepo.GetByID(packageID)
        If package Is Nothing Then
            result.ReturnedMessage.Add("没有找到包装号，可能已经被删除或锁定")
        Else
            If Not PackagingAllowedStatus.IsAllowed(package.status) Then
                result.ReturnedMessage.Add( _
                    "包装当前的状态 " & _
                    EnumContainer.GetEnumContent(GetType(PackageStatus), CType(package.status, Integer)).Description & _
                    "不能再装产品。")
            Else
                result.ReturnedResult = True
            End If
        End If
        Return result
    End Function




    Public Function GetNewPackageID(wrkStNr As String) As String
        Return "P" & Now.ToString("yyMMddHHmmssfff")
        'Return GetID(myConfig.Configs("NumericID").Get("PackageID"))
    End Function





    Private Function GetTID() As String
        Return Guid.NewGuid.ToString
        ' Return GetID(myConfig.Configs("NumericID").Get("TID"))
    End Function




    Public Function GetID(numericID As String) As String
        IDClient = New NumericIDWSClient( _
           myConfig.Configs("IDService").Get("EndPointConfig"), myConfig.Configs("IDService").Get("RemoteAddress"))
        Dim result As IDMessage = IDClient.GetID(numericID)
        If result Is Nothing Then
            Throw New IDGenerationException
        Else
            If String.IsNullOrEmpty(result.IDString) Then
                Throw New IDGenerationException
            End If
        End If
        Return result.IDString
    End Function


    Public Function FindByItem(itemTnr As String) As PackageMessage
        Dim result As PackageMessage = New PackageMessage With {.ReturnedResult = False}

        Try
            Dim unitOfWork As IUnitofWork = New PackagingDataDataContext(connStr)
            Dim packageRepo As SinglePackageRepo = New SinglePackageRepo(unitOfWork)
            result.Package = packageRepo.GetByItemId(itemTnr)
            If result.Package Is Nothing Then
                result.ReturnedResult = False
                result.ReturnedMessage.Add("没有找到该唯一号所属的包装箱")
            Else
                result.ReturnedResult = True
            End If
        Catch ex As Exception
            result.ReturnedResult = False
        End Try

        Return result
    End Function

    Public Function FindByID(id As String) As PackageMessage
        Dim result As ServiceMessage = New ServiceMessage With {.ReturnedResult = False}
        Dim result_returned As PackageMessage = New PackageMessage
        result = Me.BasicValidatePackage(id)
        If result.ReturnedResult = True Then
            Try
                Dim unitOfWork As IUnitofWork = New PackagingDataDataContext(connStr)
                Dim packageRepo As SinglePackageRepo = New SinglePackageRepo(unitOfWork)
                result_returned.Package = packageRepo.GetByID(id)
                result.ReturnedResult = True
            Catch ex As Exception
                result.ReturnedResult = False
            End Try
        End If
        result_returned.ReturnedMessage = result.ReturnedMessage
        result_returned.ReturnedResult = result.ReturnedResult

        Return result_returned
    End Function




    Public Function CountItem(id As String) As Integer
        Dim count As Integer = -1
        Dim result As ServiceMessage = BasicValidatePackage(id)
        If result.ReturnedResult = True Then
            Try


                Dim unitOfWork As IUnitofWork = New PackagingDataDataContext(connStr)
                Dim packageRepo As SinglePackageRepo = New SinglePackageRepo(unitOfWork)
                count = packageRepo.GetByID(id).PackageItems.Count
            Catch ex As Exception

            End Try
        End If
        Return count
    End Function


End Class

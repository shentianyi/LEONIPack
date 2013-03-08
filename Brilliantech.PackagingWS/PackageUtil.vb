Imports Brilliantech.Packaging.Data
Imports Brilliantech.Framework.WCF.Data
Imports Brilliantech.Packaging.WS.IDService
Imports Brilliantech.Framework.Utilities.EnumUtil
Imports Nini.Config
'Imports log4net
'Imports log4net.Ext.TracableID
Imports System.IO

''' <summary>
''' 包装服务的工具类
''' </summary>
''' <remarks></remarks>
Public Class PackageUtil
    Private IDClient As NumericIDWSClient

    ''' <summary>
    ''' 配置文件获取和设置对象
    ''' </summary>
    ''' <remarks>需首先初始化，以为下面的变量提供合适的配置</remarks>
    Private Shared myConfig As IConfigSource = New IniConfigSource _
                                             (Path.Combine(My.Application.Info.DirectoryPath, "WSConfig\WSConfig.ini"))

    ''' <summary>
    ''' 主数据库联接字符串
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared connStr As String = myConfig.Configs("Connection").Get("connStr")



    ''' <summary>
    ''' 用于在故障恢复时确定需要改变的目标状态
    ''' </summary>
    ''' <param name="ChangeEndStatus">当前的状态</param>
    ''' <returns>故障恢复后的最终状态</returns>
    ''' <remarks></remarks>
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

    ''' <summary>
    ''' 检查在某个操作台上是否有同包装类型同型号状态为未满的包装箱
    ''' </summary>
    ''' <param name="packageID">包装箱号</param>
    ''' <param name="containerId">包装号</param>
    ''' <param name="wrkStnr">操作台</param>
    ''' <returns>是否存在未满箱</returns>
    ''' <remarks></remarks>
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



    ''' <summary>
    ''' 检查当前状态是否允许进行异常恢复动作
    ''' </summary>
    ''' <param name="status">当前包装箱状态</param>
    ''' <returns>ServiceMessage对象实例，包含一个ReturnedResult的布尔类型属性，指示是否允许异常恢复</returns>
    ''' <remarks></remarks>
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



    ''' <summary>
    ''' 对传入的包装号做基本的验证
    ''' </summary>
    ''' <param name="packageID">待验证的包装号</param>
    ''' <returns>ServiceMessage实例对象，说明验证是否通过并包含可能的错误信息</returns>
    ''' <remarks></remarks>
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



    ''' <summary>
    ''' 修改当前的状态至开始包装状态
    ''' </summary>
    ''' <param name="status">当前状态</param>
    ''' <returns>修改后的包装状态</returns>
    ''' <remarks></remarks>
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



    ''' <summary>
    ''' 检查当前状态是否适合开始包装流程
    ''' </summary>
    ''' <param name="packageID">包装箱ID</param>
    ''' <param name="status">当前状态</param>
    ''' <returns>ServiceMessage实例对象，包含检查结果的布尔值和一个错误信息集合的List</returns>
    ''' <remarks></remarks>
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



    ''' <summary>
    ''' 将当前状态切换为结束状态
    ''' </summary>
    ''' <param name="stat">当前的包装状态</param>
    ''' <returns>修改后的状态</returns>
    ''' <remarks></remarks>
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



    ''' <summary>
    ''' 检查当前状态是否适合结束当前包装
    ''' </summary>
    ''' <param name="stat">当前状态</param>
    ''' <returns>ServiceMessage实例对象，包含一个代表检查结果的布尔值和错误信息列表</returns>
    ''' <remarks></remarks>
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



    ''' <summary>
    ''' 将当前状态修改为未满箱暂停状态
    ''' </summary>
    ''' <param name="stat">当前状态</param>
    ''' <returns>修改后的状态</returns>
    ''' <remarks></remarks>
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



    ''' <summary>
    ''' 检查当前状态能够用于暂停一个包装
    ''' </summary>
    ''' <param name="stat">当前状态</param>
    ''' <returns>ServiceMessage实例，包含一个代表检查结果的布尔值和错误信息列表</returns>
    ''' <remarks></remarks>
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


    ''' <summary>
    ''' 生成包装项的追踪唯一号
    ''' </summary>
    ''' <param name="packageID">包装箱号</param>
    ''' <param name="barcodeStr">输入的包装项条码</param>
    ''' <returns>生成的追踪唯一号</returns>
    ''' <remarks>如果产品条码不唯一，则系统生成一个GUID，如果已经唯一，则直接返回输入的条码内容</remarks>
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


    ''' <summary>
    ''' 添加包装箱的帮助函数
    ''' </summary>
    ''' <param name="packId">包装箱号</param>
    ''' <param name="tnr">产品追溯唯一号</param>
    ''' <returns>ServiceMessage实例对象，包含一个代表操作结果的布尔值和错误信息列表</returns>
    ''' <remarks></remarks>
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
                                    {.itemUid = Guid.NewGuid, _
                                        .itemSeq = package.PackageItems.Count + 1, _
                                     .packageID = packId, _
                                     .packagingTime = Now(), .TNr = tnr})
                unitOfWork.Commit()
                result.ReturnedResult = True
            End If
           
        Catch ex As Exception
            result.ReturnedMessage.Add("装入单个货物时系统出错，请联系管理员：" & ex.Message)
        End Try
        Return result
    End Function



    ''' <summary>
    ''' 扫描一个产品入包装时的主验证方法，用于检查扫描的物品是否适合放入当前包装
    ''' </summary>
    ''' <param name="packageid">包装箱ID</param>
    ''' <param name="barcodeContent">扫描的物品条码内容</param>
    ''' <returns>一个ServiceMessage实例对象，包含代表验证结果的布尔值和错误信息列表</returns>
    ''' <remarks></remarks>
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



    ''' <summary>
    ''' 检查包装箱状态的验证函数，用于检查包装号是否允许再装入产品
    ''' </summary>
    ''' <param name="packageID">包装箱ID</param>
    ''' <returns>ServiceMessage实例对象，包含代表验证结果的布尔值及错误信息列表</returns>
    ''' <remarks></remarks>
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



    ''' <summary>
    ''' 生成新的包装箱ID
    ''' </summary>
    ''' <param name="wrkStNr">操作台ID</param>
    ''' <returns>生成的包装箱号</returns>
    ''' <remarks></remarks>
    Public Function GetNewPackageID(wrkStNr As String) As String
        Return "P" & Now.ToString("yyMMddHHmmssfff")
        'Return GetID(myConfig.Configs("NumericID").Get("PackageID"))
    End Function




    ''' <summary>
    ''' 生成包装项目唯一号
    ''' </summary>
    ''' <returns>生成的包装项目唯一号</returns>
    ''' <remarks></remarks>
    Private Function GetTID() As String
        Return Guid.NewGuid.ToString
        ' Return GetID(myConfig.Configs("NumericID").Get("TID"))
    End Function



    ''' <summary>
    ''' 通过外部的ID生成器来生成ID
    ''' 已经停用
    ''' </summary>
    ''' <param name="numericID">ID生成器ID</param>
    ''' <returns>新生成的ID</returns>
    ''' <remarks>已停用</remarks>
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

    ''' <summary>
    ''' 使用一个包装项目的追溯唯一号来找到相应的包装箱信息
    ''' </summary>
    ''' <param name="itemTnr">包装项目的追溯唯一号</param>
    ''' <returns>一个PackageMessage实例对象，包含找到的包装对象，代表处理结果
    ''' 的布尔值以及错误信息列表</returns>
    ''' <remarks></remarks>
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


    ''' <summary>
    ''' 通过包装箱号找到包装箱对象
    ''' </summary>
    ''' <param name="id">包装箱号</param>
    ''' <returns>一个PackageMessage实例对象，包含找到的包装对象，代表处理结果
    ''' 的布尔值以及错误信息列表</returns>
    ''' <remarks></remarks>
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



    ''' <summary>
    ''' 统计一个包装箱已经装入的货物数量
    ''' </summary>
    ''' <param name="id">包装箱号</param>
    ''' <returns>已经装入的货物数量的整型</returns>
    ''' <remarks></remarks>
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

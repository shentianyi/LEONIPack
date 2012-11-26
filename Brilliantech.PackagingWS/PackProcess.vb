﻿Imports Brilliantech.Packaging.Data

Imports Brilliantech.Framework.WCF.Data

Imports Brilliantech.Packaging.WS.IDService

Imports Brilliantech.Framework.Utilities.EnumUtil

Imports Nini.Config

'Imports log4net

'Imports log4net.Ext.TracableID

Imports System.IO


''' <summary>
''' 该WCF实现类封装完成装箱流程控制的主要功能
''' </summary>
''' <remarks></remarks>
<ServiceBehavior(ConcurrencyMode:=ConcurrencyMode.Reentrant, useSynchronizationContext:=False)>
Public Class PackProcess
    Implements IPackProcess






    ''' <summary>
    ''' Nini ini 配置实例
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared myConfig As IConfigSource = New IniConfigSource _
                                             (Path.Combine(My.Application.Info.DirectoryPath, "WSConfig\WSConfig.ini"))

    ''' <summary>
    ''' 数据库连接字符串
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared connStr As String = myConfig.Configs("Connection").Get("connStr")

    ' ''' <summary>
    ' ''' Log实例，TracableIDLog继承与实现了Log4Net的的ILog
    ' ''' </summary>
    ' ''' <remarks></remarks>
    ' Private Shared log As ITracableIDLog = TracableIDLogManager.GetLogger("BusinessLogger")

    ''' <summary>
    ''' 主要工具类
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared util As PackageUtil = New PackageUtil

    ''' <summary>
    ''' 添加一个新的包装，并返回该新包装的实例
    ''' </summary>
    ''' <param name="package">需要新建的包装实例</param>
    ''' <returns>PackageMessage实例，包含了处理结果，错误信息以及已经插入数据库的实例</returns>
    ''' <remarks></remarks>
    Public Function Create(ByVal package As Data.SinglePackage, ByVal mode As PackagingType) As PackageMessage Implements IPackProcess.Create
        Dim returnedMsg As PackageMessage = New PackageMessage With {.ReturnedResult = False}
        Dim toInsert As SinglePackage = New SinglePackage
        toInsert.Capa = package.Capa
        toInsert.ContainerID = package.ContainerID
        toInsert.PartNr = package.PartNr
        toInsert.PlanedDate = Now.ToString("yyyy-MM-dd")
        Select Case mode
            Case PackagingType.normal
                toInsert.Status = PackageStatus.NewCreated
            Case PackagingType.rework
                toInsert.Status = PackageStatus.ReworkNew
        End Select

        toInsert.WrkstnID = package.WrkstnID
        toInsert.Rowguid = Guid.NewGuid
        Try
            toInsert.PackageID = util.GetNewPackageID(package.WrkstnID)
            Dim unitOfWork As IUnitofWork = New PackagingDataDataContext(connStr)
            Dim packageRepo As SinglePackageRepo = New SinglePackageRepo(unitOfWork)
            packageRepo.Insert(toInsert)
            unitOfWork.Commit()
            returnedMsg.Package = toInsert
            returnedMsg.ReturnedResult = True
        Catch ex As Exception
            returnedMsg.ReturnedMessage.Add("新建时出错了，请联系管理员" & ex.Message)
        End Try
        Return returnedMsg
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="packageId"></param>
    ''' <param name="wrkStnId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetUnfull(ByVal packageId As String, ByVal wrkStnId As String) As String Implements IPackProcess.GetUnfull
        Dim unfullId As String = String.Empty
        Dim unitOfWork As IUnitofWork = New PackagingDataDataContext(connStr)
        Dim packageRepo As SinglePackageRepo = New SinglePackageRepo(unitOfWork)
        Dim package As SinglePackage = packageRepo.GetByID(packageId)

        If package.Status = PackageStatus.Template Then
            If util.HasSingleUnfull(packageId, package.ContainerID, wrkStnId) = True Then
                unfullId = packageRepo.GetFirstUnfullPID(package.PartNr, package.ContainerID, package.WrkstnID)
            End If
        End If
        Return unfullId
    End Function


    ''' <summary>
    ''' 开始一次装箱流程，在确认开始前，需要验证以下几点：
    ''' 1.ID已经非空
    ''' 2.该包装的ID在系统中已经建立并存在
    ''' 3.该包装箱属于输入的操作台
    ''' 4.该操作台上没有相同型号的未满箱
    ''' 5.该包装箱的状态允许开始流程
    ''' </summary>
    ''' <param name="packageID">需要开始的包装箱ID</param>
    ''' <param name="wrkStnID">当前的工作台ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function BeginProcess(ByVal packageID As String, ByVal wrkStnID As String, ByVal mode As PackagingType) As PackageMessage Implements IPackProcess.BeginProcess
        Dim result As ServiceMessage = New ServiceMessage With {.ReturnedResult = False}
        Dim returnedResult As PackageMessage = New PackageMessage With {.ReturnedResult = False}

        result = util.BasicValidatePackage(packageID)
        If result.ReturnedResult = True Then

            result.ReturnedResult = False
            Try
                Dim unitOfWork As IUnitofWork = New PackagingDataDataContext(connStr)
                Dim packageRepo As SinglePackageRepo = New SinglePackageRepo(unitOfWork)
                Dim package As SinglePackage = packageRepo.GetByID(packageID)
                If package Is Nothing Then
                    result.ReturnedResult = False
                    result.ReturnedMessage.Add("没有找到包装箱号为 " & packageID & " 的包装箱")
                    Return result
                End If
                If String.Compare(package.WrkstnID, wrkStnID) <> 0 Then
                    returnedResult.ReturnedMessage.Add("此装箱任务属于操作台 " & package.WrkstnID)
                Else
                    If package.Status = PackageStatus.Template Then
                        If util.HasSingleUnfull(packageID, package.ContainerID, wrkStnID) = True Then
                            returnedResult.ReturnedResult = False
                            returnedResult.ReturnedMessage.Add _
                                ("此操作台已经有一个同型号的未满箱，包装箱号为 " _
                                 & packageRepo.GetFirstUnfullPID(package.PartNr, package.ContainerID, package.WrkstnID) & " ,请装满后再开新箱")
                        Else
                            returnedResult = Create(package, mode)
                            If returnedResult.ReturnedResult = True Then
                                returnedResult.ReturnedResult = False
                                Dim unitOfWork1 As IUnitofWork = New PackagingDataDataContext(connStr)
                                Dim packageRepo1 As SinglePackageRepo = New SinglePackageRepo(unitOfWork1)
                                Dim package1 As SinglePackage = packageRepo1.GetByID(returnedResult.Package.PackageID)
                                package1.Status = util.ChangeStatusToBegin(package1.Status)
                                unitOfWork1.Commit()
                                returnedResult.Package = package1
                                returnedResult.ReturnedResult = True
                            End If
                        End If


                    Else
                        result = util.CheckPackageStatusforBegin(packageID, package.Status)
                        If result.ReturnedResult = True Then
                            returnedResult.ReturnedResult = False
                            package.Status = util.ChangeStatusToBegin(package.Status)
                            unitOfWork.Commit()
                            returnedResult.Package = package
                            returnedResult.ReturnedResult = True
                        Else
                            returnedResult.ReturnedMessage = result.ReturnedMessage
                            returnedResult.ReturnedResult = result.ReturnedResult
                        End If
                    End If
                End If

            Catch ex As Exception
                result.ReturnedMessage.Add("系统出错，不能开始装箱，请联系管理员" & ex.ToString)
            End Try


        Else
            returnedResult.ReturnedMessage = result.ReturnedMessage
            returnedResult.ReturnedResult = result.ReturnedResult
        End If
        Return returnedResult
    End Function


    ''' <summary>
    ''' 该方法用于将包装过程意外中断的包装箱的状态调整为可以继续的状态，在确认开始前，需要验证以下几点：
    ''' 1.ID已经非空
    ''' 2.该包装的ID在系统中已经建立并存在
    ''' 3.该包装的状态允许恢复
    ''' </summary>
    ''' <param name="packageID">需要恢复的包装箱ＩＤ</param>
    ''' <returns>ServiceMessage实例，包含处理结果和消息</returns>
    ''' <remarks></remarks>
    Public Function Recover(ByVal packageID As String) As Framework.WCF.Data.ServiceMessage Implements IPackProcess.Recover
        Dim result As ServiceMessage = New ServiceMessage With {.ReturnedResult = False}
        result = util.BasicValidatePackage(packageID)
        If result.ReturnedResult = True Then
            Try
                Dim unitOfWork As IUnitofWork = New PackagingDataDataContext(connStr)
                Dim packageRepo As SinglePackageRepo = New SinglePackageRepo(unitOfWork)
                Dim package As SinglePackage = packageRepo.GetByID(packageID)
                result = util.CheckReoverStatus(package.Status)
                If result.ReturnedResult = True Then
                    result.ReturnedResult = False
                    package.Status = util.ChangeRecoverStatus(package.Status)
                    unitOfWork.Commit()
                    result.ReturnedResult = True
                End If
            Catch ex As Exception
                result.ReturnedMessage.Add("系统出错，不能恢复包装，请联系管理员" & ex.ToString)
            End Try
        End If
        Return result
    End Function


    ''' <summary>
    ''' 该方法用于在装箱流程中添加一件货物，在此期间需要检查包装箱是否已满，包装箱状态，扫描时间间隔，是否重复扫描
    ''' </summary>
    ''' <param name="packageID">包装箱号</param>
    ''' <param name="barcodeStr">扫描的原始标签条码内容</param>
    ''' <returns>ServiceMessage实例，包含处理结果和错误信息</returns>
    ''' <remarks></remarks>
    Public Function AddItem(ByVal packageID As String, ByVal barcodeStr As String) As Framework.WCF.Data.ServiceMessage Implements IPackProcess.AddItem
        Dim result As ServiceMessage = New ServiceMessage With {.ReturnedResult = False}
        If String.IsNullOrEmpty(packageID) Or String.IsNullOrEmpty(barcodeStr) Then
            result.ReturnedMessage.Add("没有提供包装ID或条码内容")
            Return result
        End If

        Try
            result = util.CheckPackageIDStatusForAdd(packageID)
            If result.ReturnedResult = True Then
                result = util.CheckItemContent(packageID, barcodeStr)
                If result.ReturnedResult = True Then
                    result = CEP(packageID)
                    If result.ReturnedResult = True Then
                        result.ReturnedResult = False
                        result = util._addItem(packageID, util.GetItemTnr(packageID, barcodeStr))
                    End If
                End If
            End If
        Catch ex As Exception
            result.ReturnedResult = False
            result.ReturnedMessage.Add(ex.Message)
        End Try
        Return result
    End Function

    ''' <summary>
    ''' 复杂事件处理程序，用于通过用户某些复杂事件来判断是否可以继续，该方法仅判断两次扫描间的时间间隔
    ''' </summary>
    ''' <param name="packageID">需要判断的包装箱号</param>
    ''' <returns>ServiceMessage实例，包含处理结果和错误信息</returns>
    ''' <remarks></remarks>
    Private Function CEP(ByVal packageID) As ServiceMessage
        Dim result As ServiceMessage = New ServiceMessage With {.ReturnedResult = False}
        Try
            Dim unitOfWork As IUnitofWork = New PackagingDataDataContext(connStr)
            Dim packageRepo As SinglePackageRepo = New SinglePackageRepo(unitOfWork)
            Dim package As SinglePackage = packageRepo.GetByID(packageID)
            If package.PackageItems.Count = 0 Then
                result.ReturnedResult = True
            Else
                Dim lastItem As PackageItem = packageRepo.GetLastItem(packageID)
                Dim allowedTime As PartAllowedSec = packageRepo.GetPartTime(package.PartNr, package.WrkstnID)

                Dim timeSpan As TimeSpan = Now - lastItem.PackagingTime
                If timeSpan.TotalSeconds < allowedTime.Sec Then
                    result.ReturnedMessage.Add("扫描间隔时间过短，请检查扫描枪灵敏度")
                Else
                    result.ReturnedResult = True

                End If
            End If

        Catch ex As Exception
            result.ReturnedMessage.Add(ex.Message)
        End Try

        Return result
    End Function





    ''' <summary>
    ''' 正常结束一次包装流程，在确认开始前，需要验证以下几点：
    ''' 1.ID已经非空
    ''' 2.该包装的ID在系统中已经建立并存在
    ''' 3.该包装的状态符合条件
    ''' 4.该包装箱已经装满
    ''' </summary>
    ''' <param name="packageID">需要结束的包装箱号</param>
    ''' <returns>ServiceMessage实例，包含处理结果和错误信息</returns>
    ''' <remarks></remarks>
    Public Function EndProcess(ByVal packageID As String) As Framework.WCF.Data.ServiceMessage Implements IPackProcess.EndProcess
        Dim result As ServiceMessage = New ServiceMessage With {.ReturnedResult = False}
        result = util.BasicValidatePackage(packageID)
        If result.ReturnedResult = True Then
            Try
                Dim unitOfWork As IUnitofWork = New PackagingDataDataContext(connStr)
                Dim packageRepo As SinglePackageRepo = New SinglePackageRepo(unitOfWork)
                Dim package As SinglePackage = packageRepo.GetByID(packageID)
                result = util.CheckEndStatus(package.Status)
                If result.ReturnedResult = True Then
                    result.ReturnedResult = False
                    If package.PackageItems.Count < package.Capa Then
                        result.ReturnedMessage.Add("包装未达到预定容量，不能关闭")
                    Else
                        package.Status = util.ChangeEndStatus(package.Status)
                        package.PlanedDate = Now.ToString("yyyy-MM-dd")
                        unitOfWork.Commit()
                        result.ReturnedResult = True
                    End If
                End If
            Catch ex As Exception
                result.ReturnedMessage.Add("系统出错，不能关闭包装，请联系管理员")
            End Try
        End If
        Return result
    End Function





    ''' <summary>
    ''' 报废一个包装箱
    ''' </summary>
    ''' <param name="packageID"></param>
    ''' <returns></returns>
    ''' <remarks>方法未实现</remarks>
    Public Function Scrap(ByVal packageID As String) As Framework.WCF.Data.ServiceMessage Implements IPackProcess.Scrap
        Dim result As ServiceMessage = New ServiceMessage With {.ReturnedResult = False}
        result.ReturnedMessage.Add("该功能未开通")
        Return result
    End Function


    ''' <summary>
    ''' 暂停一次装箱过程，在确认开始前，需要验证以下几点：
    ''' 1.ID已经非空
    ''' 2.该包装的ID在系统中已经建立并存在
    ''' 3.该包装的状态符合条件
    ''' </summary>
    ''' <param name="packageID">需要暂停的包装箱ＩＤ</param>
    ''' <returns>ServiceMessage实例，包含处理结果和错误信息</returns>
    ''' <remarks></remarks>
    Public Function SuspendProcess(ByVal packageID As String) As Framework.WCF.Data.ServiceMessage Implements IPackProcess.SuspendProcess
        Dim result As ServiceMessage = New ServiceMessage With {.ReturnedResult = False}
        result = util.BasicValidatePackage(packageID)
        If result.ReturnedResult = True Then
            Try

                Dim unitOfWork As IUnitofWork = New PackagingDataDataContext(connStr)
                Dim packageRepo As SinglePackageRepo = New SinglePackageRepo(unitOfWork)
                Dim package As SinglePackage = packageRepo.GetByID(packageID)
                result = util.CheckSuspendStatus(package.Status)
                If result.ReturnedResult = True Then
                    result.ReturnedResult = False
                    package.Status = util.ChangeSuspendStatus(package.Status)
                    unitOfWork.Commit()
                    result.ReturnedResult = True
                End If
            Catch ex As Exception
                result.ReturnedMessage.Add("系统错误，不能暂停装箱，请联系管理员")
            End Try

        End If
        Return result
    End Function

    ''' <summary>
    ''' 获取一个包装箱实例
    ''' </summary>
    ''' <param name="id">需要查找的包装箱ＩＤ</param>
    ''' <returns>一个PackageMessage的实例，包含处理结果，错误信息和返回的包装箱实例</returns>
    ''' <remarks></remarks>
    Public Function FindByID(ByVal id As String) As PackageMessage Implements IPackProcess.FindByID
        Dim result As ServiceMessage = New ServiceMessage With {.ReturnedResult = False}
        Dim result_returned As PackageMessage = New PackageMessage
        result = util.BasicValidatePackage(id)
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
    ''' 计数某个包装箱已经装箱的数量
    ''' </summary>
    ''' <param name="id">包装箱ＩＤ</param>
    ''' <returns>包装箱已装箱的数量</returns>
    ''' <remarks></remarks>
    Public Function CountItem(ByVal id As String) As Integer Implements IPackProcess.CountItem
        Return util.CountItem(id)
    End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="id"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FindByItem(ByVal id As String) As PackageMessage Implements IPackProcess.FindByItem
        Return util.FindByItem(id)
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="packageId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CancelPackaging(ByVal packageId As String) As Framework.WCF.Data.ServiceMessage Implements IPackProcess.CancelPackaging
        Dim result As ServiceMessage = New ServiceMessage
        result = util.BasicValidatePackage(packageId)
        If result.ReturnedResult = True Then
            result = New ServiceMessage
            Dim toDel As PackageMessage = util.FindByID(packageId)
            Try
                If toDel.Package.Status = PackageStatus.Template Then
                    result.ReturnedMessage.Add("不能删除一个启动标签")
                Else
                    Try
                        Dim unitOfWork As IUnitofWork = New PackagingDataDataContext(connStr)
                        Dim packageRepo As SinglePackageRepo = New SinglePackageRepo(unitOfWork)
                        packageRepo.Delete(packageId)
                        result.ReturnedResult = True

                    Catch ex As Exception
                        result.ReturnedMessage.Add("删除包装箱时出现未知错误")
                    End Try
                End If
            Catch ex As Exception
                result.ReturnedMessage.Add("系统发生未知错误，这可能是没有找到该包装箱号引起")
            End Try
        End If
        Return result
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="pId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetItemsByPackageId(ByVal pId As String) As System.Collections.Generic.List(Of Data.PackageItem) Implements IPackProcess.GetItemsByPackageId
        Dim unitOfWork As IUnitofWork = New PackagingDataDataContext(connStr)
        Dim packageRepo As SinglePackageRepo = New SinglePackageRepo(unitOfWork)
        Return packageRepo.GetItemsByPId(pId).ToList
    End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="pid"></param>
    ''' <param name="wrkstnr"></param>
    ''' <param name="tnr"></param>
    ''' <param name="partnr"></param>
    ''' <param name="status"></param>
    ''' <param name="fromDate"></param>
    ''' <param name="toDate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetPackageInfos(ByVal pid As String, ByVal wrkstnr As String, ByVal tnr As String, ByVal partnr As String, ByVal status As Integer, ByVal fromDate As Date, ByVal toDate As Date) As List(Of Data.FullPackageInfo) Implements IPackProcess.GetPackageInfos
        Dim unitOfWork As IUnitofWork = New PackagingDataDataContext(connStr)
        Dim packageRepo As SinglePackageRepo = New SinglePackageRepo(unitOfWork)
        Return packageRepo.FindSinglePackage(pid, wrkstnr, tnr, partnr, status, fromDate, toDate)
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>

    Public Function GetPackageStatus() As System.Collections.Generic.List(Of EnumObject) Implements IPackProcess.GetPackageStatus
        Return EnumObject.TryParse(GetType(PackageStatus))
    End Function
End Class
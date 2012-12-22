Imports Brilliantech.Packaging.Data
Imports Brilliantech.Framework.WCF.Data
Imports Brilliantech.Packaging.WS.IDService
Imports Brilliantech.Framework.Utilities.EnumUtil
Imports Brilliantech.ReportGenConnector
Imports Nini.Config
'Imports log4net
'Imports log4net.Ext.TracableID
Imports System.IO
<ServiceBehavior(ConcurrencyMode:=ConcurrencyMode.Reentrant, useSynchronizationContext:=False)>
Public Class PrintService
    Implements IPrintService


    ''' <summary>
    ''' 配置集合的实例对象
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared myConfig As IConfigSource = New IniConfigSource _
                                         (Path.Combine(My.Application.Info.DirectoryPath, "WSConfig\WSConfig.ini"))

    ''' <summary>
    ''' 数据库联接字符串
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared connStr As String = myConfig.Configs("Connection").Get("connStr")


    'Private Shared log As ITracableIDLog = TracableIDLogManager.GetLogger("BusinessLogger")
    ''' <summary>
    ''' 工具类实例对象
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared util As PackageUtil = New PackageUtil


    ''' <summary>
    ''' 获取结束标签所需要的数据
    ''' </summary>
    ''' <param name="packageID">包装箱号</param>
    ''' <returns>一个PrintDataMessag，包含获取到的打印数据，代表获取状态的布尔值以及错误信息列表</returns>
    ''' <remarks></remarks>
    Public Function PrintCloseLabel(packageID As String) As PrintDataMessage Implements IPrintService.PrintCloseLabel
        Dim result As PrintDataMessage = New PrintDataMessage With {.ReturnedResult = False}
        Dim serviceresult As ServiceMessage
        serviceresult = util.BasicValidatePackage(packageID)
        If serviceresult.ReturnedResult = True Then
            result.ReturnedResult = False
            Try
                Dim package As SinglePackage = util.FindByID(packageID).Package
                Dim piece As Integer = 1
                Try
                    piece = package.Part.PartContainerLabels.First(Function(c) c.LabelType = LabelType.closeLabel).Piece
                Catch ex As Exception

                End Try
                Dim LabelData As RecordData = New RecordData
                LabelData.Add("packageID", package.PackageID)
                LabelData.Add("partNr", package.PartNr)
                LabelData.Add("capa", package.Capa)
                LabelData.Add("projectID", package.WorkStation.ProdLine.Project.ProjectName)
                LabelData.Add("offlineTime", Now().ToString("yyyy-MM-dd HH:mm:ss:ffff"))
                LabelData.Add("wrkstnr", package.WrkstnID)
                LabelData.Add("planedDate", DateTime.Parse(package.PlanedDate).ToString("yyyy-MM-dd"))
                LabelData.Add("status", package.Status)
                LabelData.Add("FIFO", Now().ToString("ddMMyy"))
                LabelData.Add("custPartnr", package.Part.CustomerPartNr)
                Dim records As RecordSet = New RecordSet
                records.Add(LabelData)
                Dim template As String = myConfig.Configs("LabelTemplate").Get("CloseLabelFile")
                Dim task As PrintTask = New PrintTask With {.Config = New ReportGenConfig With {.NumberOfCopies = piece, .Template = template}, .DataSet = records}
                Dim tasks As List(Of PrintTask) = New List(Of PrintTask)
                tasks.Add(task)
                result.PrintTask = tasks
                result.ReturnedResult = True
            Catch ex As Exception
                result.ReturnedMessage.Add("生成闭箱打印标签数据时出错" & ex.Message)

            End Try
        Else
            result.ReturnedMessage = serviceresult.ReturnedMessage
        End If
        Return result
    End Function


    ''' <summary>
    ''' 获取开箱标签所需要的数据
    ''' </summary>
    ''' <param name="packageID">包装箱号</param>
    ''' <returns>>一个PrintDataMessag，包含获取到的打印数据，代表获取状态的布尔值以及错误信息列表</returns>
    ''' <remarks></remarks>
    Public Function PrintOpenLabel(packageID As String) As PrintDataMessage Implements IPrintService.PrintOpenLabel
        Dim result As PrintDataMessage = New PrintDataMessage With {.ReturnedResult = False}
        Dim serviceresult As ServiceMessage
        serviceresult = util.BasicValidatePackage(packageID)
        If serviceresult.ReturnedResult = True Then
            result.ReturnedResult = False
            Try
                Dim package As SinglePackage = util.FindByID(packageID).Package


                Dim LabelData As RecordData = New RecordData
                LabelData.Add("packageID", package.PackageID)
                LabelData.Add("partNr", package.PartNr)
                LabelData.Add("capa", package.Capa)
                LabelData.Add("wrkstnr", package.WrkstnID)
                LabelData.Add("containerID", package.ContainerID)
                LabelData.Add("planedTime", package.PlanedDate.ToString("yyyy-MM-dd"))
                Dim records As RecordSet = New RecordSet
                records.Add(LabelData)
                Dim template As String = myConfig.Configs("LabelTemplate").Get("OpenLabelFile")
                Dim task As PrintTask = New PrintTask With {.Config = New ReportGenConfig With {.NumberOfCopies = 1, .Template = template}, .DataSet = records}
                Dim tasks As List(Of PrintTask) = New List(Of PrintTask)
                tasks.Add(task)
                result.PrintTask = tasks
                result.ReturnedResult = True
            Catch ex As Exception
                result.ReturnedMessage.Add("生成开箱标签数据时出错" & ex.Message)

            End Try
        Else
            result.ReturnedMessage = serviceresult.ReturnedMessage
        End If
        Return result
    End Function


    ''' <summary>
    ''' 获取暂停标签的数据
    ''' </summary>
    ''' <param name="packageID">包装箱号</param>
    ''' <returns>>一个PrintDataMessag，包含获取到的打印数据，代表获取状态的布尔值以及错误信息列表</returns>
    ''' <remarks></remarks>
    Public Function PrintUnfullLabel(packageID As String) As PrintDataMessage Implements IPrintService.PrintUnfullLabel
        Dim result As PrintDataMessage = New PrintDataMessage With {.ReturnedResult = False}
        Dim serviceresult As ServiceMessage
        serviceresult = util.BasicValidatePackage(packageID)
        If serviceresult.ReturnedResult = True Then
            result.ReturnedResult = False
            Try
                Dim package As SinglePackage = util.FindByID(packageID).Package
                Dim LabelData As RecordData = New RecordData
                LabelData.Add("packageID", package.PackageID)
                LabelData.Add("partNr", package.PartNr)
                LabelData.Add("wrkstnr", package.WrkstnID)
                LabelData.Add("capa", package.Capa)
                LabelData.Add("custPartnr", package.Part.CustomerPartNr)
                LabelData.Add("pauseTime", Now().ToString("yyyy-MM-dd HH:mm:ss"))
                Dim records As RecordSet = New RecordSet
                records.Add(LabelData)
                Dim template As String = myConfig.Configs("LabelTemplate").Get("UnfullLabelFile")
                Dim task As PrintTask = New PrintTask With {.Config = New ReportGenConfig With {.NumberOfCopies = 1, .Template = template}, .DataSet = records}
                Dim tasks As List(Of PrintTask) = New List(Of PrintTask)
                tasks.Add(task)
                result.PrintTask = tasks
                result.ReturnedResult = True
            Catch ex As Exception
                result.ReturnedMessage.Add("生成未满箱标识数据时出错" & ex.Message)

            End Try
        Else
            result.ReturnedMessage = serviceresult.ReturnedMessage
        End If
        Return result
    End Function


End Class

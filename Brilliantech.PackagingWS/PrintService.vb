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



    Private Shared myConfig As IConfigSource = New IniConfigSource _
                                         (Path.Combine(My.Application.Info.DirectoryPath, "WSConfig\WSConfig.ini"))


    Private Shared connStr As String = myConfig.Configs("Connection").Get("connStr")


    'Private Shared log As ITracableIDLog = TracableIDLogManager.GetLogger("BusinessLogger")

    Private Shared util As PackageUtil = New PackageUtil


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

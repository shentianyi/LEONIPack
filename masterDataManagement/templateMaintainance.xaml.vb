﻿Imports masterDataManagement.packSvc
Imports System.ComponentModel
Imports Brilliantech.Packaging.Data
Imports Brilliantech.ReportGenConnector
Imports System.IO


Public Class templateMaintainance
    Friend WithEvents bw_search As New BackgroundWorker
    Private gridData As List(Of FullPackageInfo) = New List(Of FullPackageInfo)
    Private splash As SplashWindow
    Private Sub Button_reset_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button_reset.Click
        Refresh()
    End Sub

    Private Sub Refresh()
        Me.tb_packId.Text = ""
        Me.tb_partnr.Text = ""
        Me.tb_wrkst.Text = ""
    End Sub
    Private Sub Button_search_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button_search.Click
        search()
    End Sub

    Private Sub search()
        Dim args As String() = New String() { _
                                   Me.tb_packId.Text, _
                                   Me.tb_wrkst.Text, _
                                   Me.tb_partnr.Text}
        bw_search.RunWorkerAsync(args)
        Try
            splash = Nothing
        Catch ex As Exception

        End Try
        splash = New SplashWindow("正在搜索，请稍候")
        splash.ShowDialog()
    End Sub

    Private Sub bw_search_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bw_search.DoWork
        Dim client As PackProcessClient = New PackProcessClient
        Dim packInfos As List(Of FullPackageInfo) = client.GetPackageInfos(e.Argument(0), e.Argument(1), Nothing, e.Argument(2), PackageStatus.Template, New Date(1960, 1, 1), New Date(2100, 1, 1)).ToList
        e.Result = packInfos
        Try
            client.Close()

        Catch ex As Exception
            client.Abort()
        End Try
    End Sub




    Private Sub bw_search_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bw_search.RunWorkerCompleted
        gridData = e.Result
        DataGrid_orderDetails.DataContext = gridData
        Me.label_resultCount.Content = Me.gridData.Count
        Try
            Me.splash.Close()
            Me.splash = Nothing
        Catch ex As Exception

        End Try
    End Sub


    Private Sub Button_print_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button_print.Click
        If Me.DataGrid_orderDetails.SelectedItems.Count = 0 Then
            MsgBox("请先选择需要打印的启动标签", MsgBoxStyle.Critical)
        Else
            Dim config As ReportGenConfig = New ReportGenConfig
            config.NumberOfCopies = 1
            config.Template = Path.Combine(My.Application.Info.DirectoryPath, "Templates\templateLabel.tff")
            config.Printer = My.Settings.printerName

            Dim data As RecordSet = New RecordSet
            For Each packInfo In Me.gridData
                Dim piece As RecordData = New RecordData
                piece.Add("pid", packInfo.PId)
                piece.Add("container", packInfo.ContainerType)
                piece.Add("CustomerPartNr", packInfo.CustomerPartNr)
                piece.Add("capa", packInfo.Capa)
                piece.Add("partNr", packInfo.PartNr)
                piece.Add("Wrkst", packInfo.Wrkst)
                data.Add(piece)
            Next



            Try
                Dim printTask As TecITGener = New TecITGener
                printTask.Print(data, config)
                MsgBox("打印已经成功完成")
            Catch ex As Exception
                MsgBox("打印失败，请首先在设置中设置打印机：" & ex.ToString)
            End Try
        End If
    End Sub

    Private Sub Button_setPrinter_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button_setPrinter.Click
        Dim setter As Window = New ChangePrinter
        setter.ShowDialog()
    End Sub

    Private Sub Button_edit_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button_edit.Click
        If Me.DataGrid_orderDetails.SelectedItems.Count = 0 Then
            MsgBox("请先选择需要打印的启动标签", MsgBoxStyle.Critical)
        Else
            Dim changeCapa As ChangeCapa = New ChangeCapa(DataGrid_orderDetails.SelectedItem.pid)
            changeCapa.ShowDialog()
            search()
        End If

    End Sub
End Class

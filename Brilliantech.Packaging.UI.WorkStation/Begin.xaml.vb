Imports Brilliantech.Packaging.UI.WorkStation.PackService
Imports Brilliantech.Framework.WCF.Data
Imports Brilliantech.ReportGenConnector
Imports Brilliantech.Packaging.UI.WorkStation.LabelDataService
Imports Brilliantech.Packaging.Data
Imports System.ServiceModel
Imports Brilliantech.Packaging.WS
Imports System.ComponentModel



Public Class Begin
    Dim service_main As ServiceHost
    Dim service_label As ServiceHost
    Private client As PackProcessClient
    Private WithEvents timer As System.Timers.Timer
    Private mode As PackagingType = My.Settings.PackagingType
    Private holdon As SplashWindow
    Protected WithEvents worker As BackgroundWorker = New BackgroundWorker
    Protected WithEvents syncTimer As System.Timers.Timer
    Private isStarted As Boolean = False
    Private updater As Forms.Form




    Private Sub Button_Verify_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button_Verify.Click
        client = New PackProcessClient
        Dim result As ServiceMessage = client.Recover(Me.Textbox_PackageID.Text.Trim)
        Try
            client.Close()
        Catch ex As Exception
            client.Abort()
        End Try
        If result Is Nothing Then
            Dim infoBoard As InfoBoard = New InfoBoard(MsgLevel.Mistake, "与服务器通讯发生错误，请重试或与管理员联系")
            infoBoard.ShowDialog()
        Else
            If result.ReturnedResult = False Then
                Dim info As InfoBoard = New InfoBoard(MsgLevel.Mistake, result.ReturnedMessage.ToArray)
                info.ShowDialog()
            Else
                'Dim main As MainWindow = New MainWindow(Me.Textbox_PackageID.Text.Trim)
                'If main.ShowDialog() = True Then
                '    RestartSamePart()
                'Else
                '    Me.InitiateObject()
                'End If
                StartPackWindow(Me.Textbox_PackageID.Text.Trim)

            End If
        End If
    End Sub

    Private Sub timer_elasped() Handles timer.Elapsed
        timer.Stop()
        Dispatcher.Invoke(New autoStart(AddressOf Restart))
    End Sub

    Private Sub syncTimer_elasped() Handles syncTimer.Elapsed
        timer.Stop()
        Try
            ReplicationUtils.ReplicateMasterData()
        Catch ex As Exception
            Try
                Logger.Write("在自动同步时出现问题" & ex.ToString)
            Catch ex1 As Exception

            End Try

        End Try
        timer.Start()
    End Sub

    Private Delegate Sub autoStart()


    Private Sub Restart()
        If My.Settings.autoStart = True Then
            client = New PackProcessClient
            Dim packageMsg As Brilliantech.Packaging.UI.WorkStation.PackService.PackageMessage = client.FindByID(Me.Textbox_PackageID.Text)
            Try
                client.Close()
            Catch ex As Exception
                client.Abort()
            End Try
            If packageMsg.Package Is Nothing Then
                Me.InitiateObject()
            Else
                If packageMsg.Package.Status = 99 And My.Settings.autoStart = True And isStarted = False Then
                    StartProcess()
                Else
                    InitiateObject()
                End If
            End If
        End If
    End Sub


    Private Sub RestartSamePart()
        If My.Settings.autoStart = True Then
            client = New PackProcessClient
            Dim packageMsg As Brilliantech.Packaging.UI.WorkStation.PackService.PackageMessage = client.FindByID(Me.Textbox_PackageID.Text)
            Try
                client.Close()
            Catch ex As Exception
                client.Abort()
            End Try
            If packageMsg.Package Is Nothing Then
                Me.InitiateObject()
            Else
                If Not (packageMsg.Package.Status = 99 And My.Settings.autoStart = True) Then
                    InitiateObject()
                Else
                    timer.Start()
                End If
            End If
        End If
    End Sub

    Private Sub Begin_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
        Try
            Me.service_main.Close()
            Me.service_label.Close()
            Try
                ReplicationUtils.ReplicateMasterData()
            Catch ex As Exception

            End Try
        Catch ex As Exception

        End Try
    End Sub
    Private Sub Window_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        Try
            service_main = New ServiceHost(GetType(PackProcess))
            service_main.Open()
            service_label = New ServiceHost(GetType(PrintService))
            service_label.Open()
            Me.Label_versionNr.Content = "当前版本: " & My.Application.Info.Version.ToString

        Catch ex As Exception
            Dim msg As InfoBoard = New InfoBoard(MsgLevel.Mistake, "启动程序失败，这常是由于上次关闭异常或者端口占用引起的，可以通过重启计算机来解决该问题")
            msg.ShowDialog()
            Me.Close()
        End Try

        Try
            updater = New updater
            updater.Show()


        Catch ex As Exception

        End Try

        syncTimer = New Timers.Timer
        syncTimer.Interval = My.Settings.syncInterval

        Try
            holdon.Close()
            holdon = Nothing
        Catch ex As Exception

        End Try
        holdon = New SplashWindow("正在与服务器同步数据，请稍候")

        worker.RunWorkerAsync()
        holdon.ShowDialog()
        '开始同步数据库，下载主数据



        InitiateObject()
        timer = New Timers.Timer
        timer.Interval = My.Settings.restartInterval


    End Sub

    Private Sub InitiateObject()
        Try
            'client = New PackProcessClient()
            Clear()

            If My.Settings.autoStart = True Then
                Button1.Content = "当前模式：自动开始模式"
            Else
                Button1.Content = "当前模式：手动开始模式"
            End If

            ChangeReworkStatus()
            ChangeReworkStatus() 'run twice to set the rework status

        Catch ex As Exception
            Dim msg As InfoBoard = New InfoBoard(MsgLevel.Mistake, "连接服务器失败，程序将退出，请联系管理员")
            msg.ShowDialog()
            Me.Close()
        End Try
    End Sub

    Private Sub Clear()
        Me.Textbox_PackageID.Text = ""
        Me.Textbox_workStNr.Text = ""
        Me.Textbox_PackageID.Focus()
    End Sub

    Private Sub Image1_ImageFailed(ByVal sender As System.Object, ByVal e As System.Windows.ExceptionRoutedEventArgs) Handles Image1.ImageFailed

    End Sub

    Private Sub Button_Exist_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button_Exist.Click
        Try
            Me.updater.Close()
        Catch ex As Exception

        End Try
        Me.Close()
        Environment.Exit(0)
    End Sub

    Private Sub Textbox_workStNr_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles Textbox_workStNr.KeyUp
        If e.Key = Key.Return Then
            StartProcess()
        End If
    End Sub

    Private Sub StartProcess()
        client = New PackProcessClient
        Dim unfullId As String
        Try
            unfullId = client.GetUnfull(Me.Textbox_PackageID.Text.Trim, Me.Textbox_workStNr.Text.Trim)
        Catch ex As Exception

        End Try

        If mode = PackagingType.rework Then
            Dim info As InfoBoard = New InfoBoard(MsgLevel.Warning, "您正在开始一个返工包装，确定吗？")

            If info.ShowDialog = False Then
                Exit Sub
            End If
        End If

        If Not String.IsNullOrEmpty(unfullId) Then
            Dim infoBoard As InfoBoard = New InfoBoard(MsgLevel.Mistake, "此型号在操作台上已经有一个未满箱，要打印未满箱标识吗？")
            If infoBoard.ShowDialog = True Then
                Dim printClient As PrintServiceClient = New PrintServiceClient
                Dim tasks As List(Of PrintTask) = New List(Of PrintTask)

                tasks = printClient.PrintUnfullLabel(unfullId).PrintTask.ToList


                For Each task As PrintTask In tasks
                    Dim tecITprinter As TecITGener = New TecITGener
                    task.Config.Printer = My.Settings.PrinterName
                    task.Config.Template = System.IO.Path.Combine(New String() {My.Application.Info.DirectoryPath, My.Settings.LabelFolder, task.Config.Template})
                    tecITprinter.Print(task.DataSet, task.Config)
                Next
            End If
        Else
            Dim result As Brilliantech.Packaging.UI.WorkStation.PackService.PackageMessage = client.BeginProcess(Me.Textbox_PackageID.Text.Trim, Me.Textbox_workStNr.Text.Trim, mode)
            Try
                client.Close()
            Catch ex As Exception
                client.Abort()
            End Try
            If result Is Nothing Then
                Dim infoBoard As InfoBoard = New InfoBoard(MsgLevel.Mistake, "与服务器通讯发生错误，请重试或与管理员联系")
                infoBoard.ShowDialog()
            Else

                If result.ReturnedResult = False Then
                    Dim info As InfoBoard = New InfoBoard(MsgLevel.Mistake, result.ReturnedMessage.ToArray)
                    info.ShowDialog()
                Else
                    'Dim main As MainWindow = New MainWindow(result.Package.PackageID)
                    'If main.ShowDialog() = True And My.Settings.autoStart = True Then
                    '    RestartSamePart()
                    'Else
                    '    Me.InitiateObject()
                    'End If
                    StartPackWindow(result.Package.PackageID)
                End If
            End If
        End If

    End Sub

    Private Sub StartPackWindow(ByVal packId As String)
        Dim main As MainWindow = New MainWindow(packId)
        isStarted = True
        If main.ShowDialog() = True And My.Settings.autoStart = True Then
            RestartSamePart()
        Else
            Me.InitiateObject()
        End If
        isStarted = False
    End Sub
    Private Sub Textbox_workStNr_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles Textbox_workStNr.TextChanged

    End Sub

    Private Sub Textbox_PackageID_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles Textbox_PackageID.KeyUp
        If e.Key = Key.Return Then
            Me.Textbox_workStNr.Focus()
        End If
    End Sub

    Private Sub Textbox_PackageID_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles Textbox_PackageID.TextChanged

    End Sub

    Private Sub Button_clear_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button_clear.Click
        Clear()
    End Sub

    Private Sub Button_setPrinter_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button_setPrinter.Click
        Dim PrintWindow As Window = New ChangePrinter
        PrintWindow.ShowDialog()

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button1.Click
        ChangeMode()
    End Sub

    Private Sub ChangeMode()
        If My.Settings.autoStart = True Then
            My.Settings.autoStart = False
            Button1.Content = "当前模式：手动开始模式"
        Else
            My.Settings.autoStart = True
            My.Settings.Save()
            Button1.Content = "当前模式：自动开始模式"
        End If
    End Sub

    Private Sub Button_softKB_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button_softKB.Click
        Try
            System.Diagnostics.Process.Start("C:\Windows\system32\Osk.exe")
        Catch ex As Exception

        End Try

    End Sub

    Private Sub Button_printFull_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button_printFull.Click
        Dim psw As List(Of String) = New List(Of String)
        Dim getPsw As Window = New enterPsw(psw)
        getPsw.ShowDialog()
        If CheckEncrypt(psw(0)) Then
            Dim window As Window = New printLabel
            window.Show()
        Else
            Dim infoBoard As New InfoBoard(MsgLevel.Mistake, "验证码错误")
            infoBoard.ShowDialog()
        End If
     
    End Sub

    Private Sub Button_rework_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button_rework.Click

        Dim psw As List(Of String) = New List(Of String)
        Dim getPsw As Window = New enterPsw(psw)
        getPsw.ShowDialog()
        If CheckEncrypt(psw(0)) Then

            ChangeReworkStatus()
        Else
            Dim infoBoard As New InfoBoard(MsgLevel.Mistake, "验证码错误")
            infoBoard.ShowDialog()
        End If

    End Sub

    Private Sub ChangeReworkStatus()
        Select Case mode
            Case CType(PackagingType.normal, Integer)
                mode = PackagingType.rework

                Label_isReowork.Content = "当前处于返工模式"
                My.Settings.autoStart = False
                Button1.Content = "当前模式：手动开始模式"

            Case CType(PackagingType.rework, Integer)
                mode = PackagingType.normal
                Label_isReowork.Content = ""
        End Select
        My.Settings.PackagingType = mode
        My.Settings.Save()
    End Sub
    Private Sub Button_cancel_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button_cancel.Click
        Dim psw As List(Of String) = New List(Of String)
        Dim getPsw As Window = New enterPsw(psw)
        getPsw.ShowDialog()
        If CheckEncrypt(psw(0)) Then
            Dim window As cancelPackage = New cancelPackage
            window.Show()
        Else
            Dim infoBoard As New InfoBoard(MsgLevel.Mistake, "验证码错误")
            infoBoard.ShowDialog()
        End If

    End Sub

    Private Function CheckEncrypt(ByVal passStr As String) As Boolean
        Dim uid As String = "{62C362DC-1EC9-4BFF-B6AD-3EAF8B2076E7}"
        If String.Compare(passStr, uid) = 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub worker_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles worker.DoWork
        Try
            ReplicationUtils.ReplicateMasterData()
            e.Result = True
        Catch ex As Exception
            e.Result = False
        End Try

    End Sub

    Private Sub worker_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles worker.RunWorkerCompleted
        Try
            holdon.Close()
            holdon = Nothing
            syncTimer.Start()
        Catch ex As Exception

        End Try

        If e.Result = False Then
            Dim info As InfoBoard = New InfoBoard(MsgLevel.Mistake, "从服务器同步数据失败，您可以继续使用该系统，但请通报管理员该错误")
            info.ShowDialog()
        End If
    End Sub

    Private Sub Button_view_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button_view.Click
        Dim viewer As Window = New PackView
        viewer.Show()
    End Sub
End Class

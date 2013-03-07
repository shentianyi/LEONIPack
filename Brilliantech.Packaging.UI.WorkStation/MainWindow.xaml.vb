Imports Brilliantech.Packaging.UI.WorkStation.PackService
Imports Brilliantech.Packaging.UI.WorkStation.LabelDataService
Imports System.Media
Imports Brilliantech.Framework.WCF.Data
Imports Brilliantech.ReportGenConnector
Imports Brilliantech.Packaging.Data

Class MainWindow
    Private processClient As PackProcessClient
    ' Private printClient1 As PrintServiceClient
    Private currentPackage As SinglePackage
    Private currentProcess As UnderProcessing
    Private currentPackageId As String
    Private currentValidate As List(Of CustomValidateViewModel) = New List(Of CustomValidateViewModel)

    Public Sub New(packageID As String)

        InitializeComponent()
        currentPackageId = packageID
        SetValidateItems()
    End Sub

    Private Sub SetValidateItems()
        Dim client As PackProcessClient = New PackProcessClient
        Dim _currentValidate() As Data.CustomValidate = client.GetValidateItemsByPackageId(currentPackageId)
        If _currentValidate IsNot Nothing Then
            For Each cv As Data.CustomValidate In _currentValidate
                currentValidate.Add(New CustomValidateViewModel With {.BarcodeItemName = cv.ValidateName, .PatternString = cv.ValidatePattern})
            Next
        End If
       
    End Sub

    Private Sub InitiateObject(packageID As String)
        processClient = New PackProcessClient()
        'printClient = New PrintServiceClient()
        'processClient = New PackProcessClient(My.Settings.endPointConfige, My.Settings.remoteAddress)
        currentPackage = processClient.FindByID(packageID).Package
        Dim countItem As Integer = processClient.CountItem(packageID)
        If countItem < 0 Then
            Throw New Exception("初始化失败，获取已装箱记录时出错")
        End If
        currentProcess = New UnderProcessing(currentPackage.Capa, countItem)
        Me.basicInfo.DataContext = currentPackage
        Me.Progress.DataContext = currentProcess
        Me.Main_Added.DataContext = currentProcess
        Me.ProgressTxt.DataContext = currentProcess
        If Me.currentPackage.Status = PackageStatus.ReworkBegin Or
            Me.currentPackage.Status = PackageStatus.ReworkBeginUnexpect Or
            Me.currentPackage.Status = PackageStatus.ReworkRebegin Then
            Me.isRework.Content = "返工模式"
        End If
        SetWarningColor()
        If currentPackage Is Nothing Then
            Throw New Exception("初始化失败")
        End If
    End Sub

    Private Sub AutoFocus()
        Me.Textbox_barcode.Text = ""
        Me.Textbox_barcode.Focus()
    End Sub
    Private Sub Window_Loaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        Try
            InitiateObject(currentPackageId)
        Catch ex As Exception

        End Try
        AutoFocus()
        Me.Activate()
    End Sub

    Private Sub ResetValidator()
        If Me.currentValidate IsNot Nothing Then
            For Each cv As CustomValidateViewModel In currentValidate
                cv.IsValidated = False
            Next
        End If
    End Sub
    Private Sub Textbox_barcode_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles Textbox_barcode.PreviewKeyDown
        If e.Key = Key.Enter Then
            If Me.currentValidate.Count > 0 Then
                Dim validateWin As CustomValidate = New CustomValidate(Me.currentValidate)
                If validateWin.ShowDialog = False Then
                    Dim info As InfoBoard = New InfoBoard(MsgLevel.Mistake, "没有通过自定义的扫描验证")
                    info.ShowDialog()
                    AutoFocus()
                Else
                    AddItem(Me.Textbox_barcode.Text)

                End If
                resetValidator()
            Else
                AddItem(Me.Textbox_barcode.Text)
            End If


        End If
    End Sub

    Private Sub SetWarningColor()
        If currentProcess.Remained = 0 Then
            Me.ProgressBar_progress.Foreground = New SolidColorBrush(Color.FromRgb(255, 0, 0))
        Else
            Me.ProgressBar_progress.Foreground = New SolidColorBrush(Color.FromArgb(255, 1, 211, 40))
        End If
    End Sub
    Private Sub AddItem(barcodeStr As String)
        Dim result As ServiceMessage = processClient.AddItem(currentPackage.PackageID, barcodeStr)
        If result IsNot Nothing Then

            If result.ReturnedResult = True Then
                Me.currentProcess.Add()
                SetWarningColor()
                Dim sound As SoundPlayer = New SoundPlayer
                sound.Stream = My.Resources.Beep_Sucess
                sound.Play()
                AutoFinish()
            Else
                Dim sound As SoundPlayer = New SoundPlayer
                sound.Stream = My.Resources.BEEP_error
                sound.Play()
                Dim msgbox As InfoBoard = New InfoBoard(MsgLevel.Mistake, result.ReturnedMessage.ToArray)
                msgbox.ShowDialog()
            End If
        End If
        Me.AutoFocus()
    End Sub


    Private Sub AutoFinish()
        If currentProcess.Remained = 0 Then
            finish()
        End If
    End Sub


    Private Sub Textbox_barcode_TextChanged(sender As System.Object, e As System.Windows.Controls.TextChangedEventArgs) Handles Textbox_barcode.TextChanged

    End Sub
    Private Sub finish()
        Dim msgResult As ServiceMessage = processClient.EndProcess(Me.currentPackage.PackageID)
        If msgResult Is Nothing Then
            Dim info As InfoBoard = New InfoBoard(MsgLevel.Mistake, "与服务器失去联系，请重试或联系管理员")
            info.ShowDialog()
        Else
            If msgResult.ReturnedResult = False Then
                Dim info As InfoBoard = New InfoBoard(MsgLevel.Mistake, msgResult.ReturnedMessage.ToArray)
                info.ShowDialog()
            Else
                Try
                    PrintFinishLabel()
                    Dim info As InfoBoard = New InfoBoard(MsgLevel.Successful, "关闭包装箱结束,标签已打印,窗口将在" & My.Settings.autoCloseInterval / 1000 & "秒后自动关闭，或按否停止自动关闭", My.Settings.autoCloseInterval)
                    If info.ShowDialog() = True Then
                        Me.DialogResult = True
                        Me.Close()
                    Else

                    End If
                Catch ex As Exception
                    Dim msg As InfoBoard = New InfoBoard(MsgLevel.Warning, "关闭包装结束，但打印标签时出错，请重打印或联系管理员")
                    msg.ShowDialog()
                End Try
                Me.currentProcess.IsFulled = True

            End If
        End If
    End Sub

    Private Sub Button_Finish_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button_Finish.Click
        finish()
    End Sub

    Private Sub Button_Exit_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button_Exit.Click
        If currentProcess.IsFinished = False Then
            Dim info As InfoBoard = New InfoBoard(MsgLevel.Mistake, "请先结束或暂停包装后再退出")
            info.ShowDialog()
        Else
            Me.DialogResult = Me.currentProcess.IsFulled
            Me.Close()
        End If
    End Sub

    Private Sub Button_pause_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button_pause.Click
        Dim msgResult As ServiceMessage = processClient.SuspendProcess(Me.currentPackage.PackageID)
        If msgResult Is Nothing Then
            Dim info As InfoBoard = New InfoBoard(MsgLevel.Mistake, "与服务器失去联系，请重试或联系管理员")
            info.ShowDialog()
        Else
            If msgResult.ReturnedResult = False Then
                Dim info As InfoBoard = New InfoBoard(MsgLevel.Mistake, msgResult.ReturnedMessage.ToArray)
                info.ShowDialog()
            Else
                Dim info As InfoBoard = New InfoBoard(MsgLevel.Successful, "暂停包装箱结束，需要打印未满箱标识吗？")

                Dim ifPrint As Boolean = info.ShowDialog()
                If ifPrint Then
                    Try
                        PrintUnfullLabel()
                    Catch ex As Exception
                        Dim msg As InfoBoard = New InfoBoard(MsgLevel.Mistake, "打印未满箱标签失败，请重试或联系管理员")
                        msg.ShowDialog()
                    End Try
                End If
                Me.currentProcess.IsPaused = True
                Dim msg1 As InfoBoard = New InfoBoard(MsgLevel.Successful, "暂停包装成功")
                msg1.ShowDialog()
            End If
        End If
    End Sub

    Private Sub PrintUnfullLabel()
        Dim tasks As List(Of PrintTask) = New List(Of PrintTask)
        Dim f_printClient As PrintServiceClient = New PrintServiceClient
        tasks = f_printClient.PrintUnfullLabel(Me.currentPackage.PackageID).PrintTask.ToList


        For Each task As PrintTask In tasks
            Dim tecITprinter As TecITGener = New TecITGener
            task.Config.Printer = My.Settings.PrinterName
            task.Config.Template = System.IO.Path.Combine(New String() {My.Application.Info.DirectoryPath, My.Settings.LabelFolder, task.Config.Template})
            tecITprinter.Print(task.DataSet, task.Config)
        Next

    End Sub

    Private Sub PrintFinishLabel()
        PrintFinishLabels(Me.currentPackage.PackageID)
    End Sub

    Public Shared Sub PrintFinishLabels(packId As String)
        Dim tasks As List(Of PrintTask) = New List(Of PrintTask)
        Dim f_printClient As PrintServiceClient = New PrintServiceClient
        Dim msg As PrintDataMessage = f_printClient.PrintCloseLabel(packId)
        tasks = msg.PrintTask.ToList
        For Each task As PrintTask In tasks
            Dim tecITprinter As TecITGener = New TecITGener
            task.Config.Printer = My.Settings.PrinterName
            task.Config.Template = System.IO.Path.Combine(New String() {My.Application.Info.DirectoryPath, My.Settings.LabelFolder, task.Config.Template})
            tecITprinter.Print(task.DataSet, task.Config)
        Next
    End Sub

    Private Sub Button_ReprintFull_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button_ReprintFull.Click
        If Me.currentProcess.IsFulled = False Then
            Dim info As InfoBoard = New InfoBoard(MsgLevel.Mistake, "包装还没结束，不能重打闭箱标签")
            info.ShowDialog()
        Else
            Try
                PrintFinishLabel()
                Dim successInfo As InfoBoard = New InfoBoard(MsgLevel.Successful, "标签打印成功")
                successInfo.ShowDialog()
            Catch ex As Exception
                Dim msg As InfoBoard = New InfoBoard(MsgLevel.Mistake, "标签打印出错，请重试或联系管理员" & Chr(10) & ex.Message)
                msg.ShowDialog()
            End Try
        End If
    End Sub

    Private Sub Button_ReprintUnfull_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button_ReprintUnfull.Click
        If Me.currentProcess.IsPaused = False Then
            Dim info As InfoBoard = New InfoBoard(MsgLevel.Mistake, "包装不是已暂停状态，不能重打未满箱标识")
            info.ShowDialog()
        Else
            Try
                PrintUnfullLabel()
                Dim successInfo As InfoBoard = New InfoBoard(MsgLevel.Successful, "标签打印成功")
                successInfo.ShowDialog()
            Catch ex As Exception
                Dim msg As InfoBoard = New InfoBoard(MsgLevel.Mistake, "标签打印出错，请重试或联系管理员" & Chr(10) & ex.Message)
                msg.ShowDialog()
            End Try

        End If
    End Sub

    Private Sub Image1_ImageFailed(sender As System.Object, e As System.Windows.ExceptionRoutedEventArgs) Handles Image1.ImageFailed

    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button1.Click
        Dim PrintWindow As Window = New ChangePrinter
        PrintWindow.ShowDialog()
    End Sub
End Class

Imports System.Media
Imports Brilliantech.ReportGenConnector
Imports SVWControlSystemWinform.DispatchProcess
Imports System.Windows.Media
Imports SVWControlSystemWinform.WPFInfoBoard
Imports System.Text.RegularExpressions

Class PackagingControl
    'Private processClient As PackProcessClient
    'Private printClient As PrintServiceClient
    'Private currentPackage As SinglePackage
    Private currentProcess As UnderProcessing
    Private bindList As List(Of DispatchValidateModelView) = New List(Of DispatchValidateModelView)
    Private dnId As String


    Public Sub New(dnid As String)

        InitializeComponent()
        Me.dnId = dnid
    End Sub

    Private Sub InitiateObject(DnId As String)
        Dim dispatchClient As DispatchProcessClient = New DispatchProcessClient
        Dim result As DispatchProcess.DeliveryNoteMsg = dispatchClient.LoadDNItems(DnId)
        If result.ReturnedResult = False Then
            WPFInfoBoard.CMsgDialog(MsgLevel.Mistake, "不能进行装车检查,错误如下:" & _
                                    Util.StringListFormat(result.ReturnedMessage.ToList)).ShowDialog()
            Me.Close()
        Else
            Me.dnId = DnId
            currentProcess = New UnderProcessing(result.DNItems.Count, 0)
            Me.Progress.DataContext = currentProcess
            Me.Main_Added.DataContext = currentProcess
            Me.ProgressTxt.DataContext = currentProcess
            Me.lb_DNName.Content = DnId

            For Each tppair As TnrPnrPair In result.DNItems
                bindList.Add(New DispatchValidateModelView() With {.Pnr = Trim(tppair.Pnr), .Tnr = Trim(tppair.Tnr)})
            Next
            Me.ls_items.ItemsSource = bindList
        End If
        'processClient = New PackProcessClient()
        'printClient = New PrintServiceClient()
        ''processClient = New PackProcessClient(My.Settings.endPointConfige, My.Settings.remoteAddress)
        'currentPackage = processClient.FindByID(packageID).Package
        'Dim countItem As Integer = processClient.CountItem(packageID)
        'If countItem < 0 Then
        '    Throw New Exception("初始化失败，获取已装箱记录时出错")
        'End If
        'currentProcess = New UnderProcessing(currentPackage.capa, countItem)
        'Me.basicInfo.DataContext = currentPackage
        'Me.Progress.DataContext = currentProcess
        'Me.Main_Added.DataContext = currentProcess
        'Me.ProgressTxt.DataContext = currentProcess
        'SetWarningColor()
        'If currentPackage Is Nothing Then
        'Throw New Exception("初始化失败")
        'End If
    End Sub

    Private Sub Reorganize()
        bindList = (From a In bindList Order By a.ValidateColor Descending)
        ls_items.DataContext = bindList
    End Sub

    Private Sub AutoFocus()
        Me.tb_barcode.Text = ""
        Me.tb_barcode.Focus()
    End Sub
    Private Sub Window_Loaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        Try
            InitiateObject(dnId)
        Catch ex As Exception

        End Try
        AutoFocus()
    End Sub

    Private Sub Textbox_barcode_KeyUp(sender As Object, e As System.Windows.Input.KeyEventArgs) Handles tb_barcode.KeyUp
        If Convert.ToInt16(e.Key) = Windows.Input.Key.Return Then
            AddItem(Me.tb_barcode.Text)
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


        Dim _10PNRegex As Regex = New Regex(My.Settings.regex_10Pnr)
        Dim _7PNrRegex As Regex = New Regex(My.Settings.regex_7Pnr)

        If _10PNRegex.IsMatch(barcodeStr) Then
            Process10PN(barcodeStr)
        ElseIf _7PNrRegex.IsMatch(barcodeStr) Then
            Process7Pn(barcodeStr)
        Else
            ProcessBase64(barcodeStr)
        End If






        'Dim result As ServiceMessage = processClient.AddItem(currentPackage.packageID, barcodeStr)
        'If result IsNot Nothing Then

        '    If result.ReturnedResult = True Then
        '        Me.currentProcess.Add()
        '        SetWarningColor()
        '        Dim sound As SoundPlayer = New SoundPlayer
        '        sound.Stream = My.Resources.Beep_Sucess
        '        sound.Play()
        '    Else
        '        Dim sound As SoundPlayer = New SoundPlayer
        '        sound.Stream = My.Resources.BEEP_error
        '        sound.Play()
        '        Dim msgbox As InfoBoard = New InfoBoard(MsgLevel.Mistake, result.ReturnedMessage.ToArray)
        '        msgbox.ShowDialog()
        '    End If
        'End If
        Me.AutoFocus()
    End Sub

    Private Sub Process10PN(barcode As String)
        VerifyWithPn(barcode.Substring(3))
    End Sub

    Private Sub Process7Pn(barcode As String)
        VerifyWithPn(barcode)
    End Sub

    Private Sub ProcessBase64(barcode As String)
        Dim tnr As String = "unknown"
        Try
            Dim encoder As IHashEncoder = New Base64HashEncoder

            tnr = encoder.Decode(barcode)
        Catch ex As Exception

        End Try

        If String.IsNullOrEmpty(tnr) Then
            ThrowError("解析验证码错误，请确认正确扫描了验证码或7位生产号或10位生产号")
        Else
            VerifyWithTnr(tnr)
        End If

    End Sub

    Private Sub NotifySuccess()
        Me.currentProcess.Add()
        SetWarningColor()
        Dim sound As SoundPlayer = New SoundPlayer
        sound.Stream = My.Resources.Beep_Sucess
        sound.Play()
    End Sub

    Private Sub ThrowError(msg As String)
        Dim sound As SoundPlayer = New SoundPlayer
        sound.Stream = My.Resources.BEEP_error
        sound.Play()
        CMsgDialog(MsgLevel.Mistake, msg).ShowDialog()
    End Sub
    Private Sub VerifyWithPn(pnr As String)
        Dim item As DispatchValidateModelView = bindList.Find(Function(c) c.Pnr = pnr)
        If item IsNot Nothing Then
            If item.HasValidated Then
                ThrowError("生产号 " & pnr & "已经扫描过")
            Else
                item.SetValidate()
                NotifySuccess()
            End If
          
        Else
            ThrowError("运单中没有生产号为 " & pnr & " 的项目")
        End If
    End Sub

    Private Sub VerifyWithTnr(tnr As String)
        Dim item As DispatchValidateModelView = bindList.Find(Function(c) c.Tnr = tnr)
        If item IsNot Nothing Then
            If item.HasValidated Then
                ThrowError("T号 " & tnr & " 已经扫描过")
            Else
                item.SetValidate()
                NotifySuccess()
            End If
        
        Else
            ThrowError("运单中没有T号为 " & tnr & " 的项目")
        End If
    End Sub
    Private Sub Textbox_barcode_TextChanged(sender As System.Object, e As System.Windows.Controls.TextChangedEventArgs) Handles tb_barcode.TextChanged

    End Sub

    Private Sub Button_Finish_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button_Finish.Click
        Dim client As DispatchProcess.DispatchProcessClient = New DispatchProcessClient
        Dim result As ServiceResult
        If Not currentProcess.IsFulled Then
            CMsgDialog(MsgLevel.Mistake, "此运单还有项目没有装入").ShowDialog()
            Exit Sub
        Else
            result = client.CloseDN(dnId)
            If result.ReturnedResult = False Then
                CMsgDialog(MsgLevel.Mistake, "结束运单时出错,错误信息如下: " & _
                           Util.StringListFormat(result.ReturnedMessage.ToList)).ShowDialog()
            Else
                CMsgDialog(MsgLevel.Info, "发运检查结束，将打印验证运单").ShowDialog()
                PrintDL()
            End If
        End If
        'Dim msgResult As ServiceMessage = processClient.EndProcess(Me.currentPackage.packageID)
        'If msgResult Is Nothing Then
        '    Dim info As InfoBoard = New InfoBoard(MsgLevel.Mistake, "与服务器失去联系，请重试或联系管理员")
        '    info.ShowDialog()
        'Else
        '    If msgResult.ReturnedResult = False Then
        '        Dim info As InfoBoard = New InfoBoard(MsgLevel.Mistake, msgResult.ReturnedMessage.ToArray)
        '        info.ShowDialog()
        '    Else
        '        Try
        '            PrintFinishLabel()
        '        Catch ex As Exception
        '            Dim msg As InfoBoard = New InfoBoard(MsgLevel.Successful, "打印标签时出错，请重打印或联系管理员")
        '            msg.ShowDialog()
        '        End Try
        '        Me.currentProcess.IsFulled = True
        '        Dim info As InfoBoard = New InfoBoard(MsgLevel.Successful, "关闭包装箱结束,标签已打印")
        '        info.ShowDialog()
        '    End If
        'End If
    End Sub

    Private Sub Button_Exit_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button_Exit.Click
        'If currentProcess.IsFinished = False Then
        '    Dim info As InfoBoard = New InfoBoard(MsgLevel.Mistake, "请先结束或暂停包装后再退出")
        '    info.ShowDialog()
        'Else
        '    Me.Close()
        'End If
        Dim result As Boolean = CMsgDialog(MsgLevel.Warning, "退出将放弃之前所有的扫描检查，确定吗？").ShowDialog
        If result = True Then
            Me.Close()
        End If
    End Sub










    Private Sub Image1_ImageFailed(sender As System.Object, e As System.Windows.ExceptionRoutedEventArgs) Handles Image1.ImageFailed

    End Sub

    Private Sub ProgressBar_progress_ValueChanged(sender As System.Object, e As System.Windows.RoutedPropertyChangedEventArgs(Of System.Double))

    End Sub

    Private Sub Button_ReprintFull_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button_ReprintFull.Click
        Dim labelClient As LabelDatSvc.LabelDataSvcClient = New LabelDatSvc.LabelDataSvcClient
        Dim labelData As LabelDatSvc.LabelDataMsg = labelClient.LoadDNData(Me.dnId)


        If labelData.ReturnedResult = False Then
            CMsgDialog(MsgLevel.Mistake, "重打送货单时发生如下错误:" & Util.StringListFormat(labelData.ReturnedMessage.ToList)).ShowDialog()
        Else
            Dim records As RecordSet = Offlinereg.ConvertToRecordSet(labelData.PrintData.ToList)
            Dim printTask As TecITGener = New TecITGener
            Try
                printTask.Print(records, _
                                New ReportGenConfig With {.NumberOfCopies = My.Settings.DN_nr, _
                                                          .Printer = _
                                                          My.Settings.productionLabel_printer, _
                                                          .Template = _
                                                             IO.Path.Combine( _
                                                                  My.Application.Info.DirectoryPath, _
                                                                  My.Settings.DNLabel)})

                CMsgDialog(MsgLevel.Successful, "打印成功").ShowDialog()

            Catch ex As Exception
                CMsgDialog(MsgLevel.Mistake, "打印至打印机时出现错误：" & _
                                                               " " & ex.InnerException.Message).ShowDialog()

            End Try
        End If
    End Sub

    Private Sub PrintDL()
        Dim labelClient As LabelDatSvc.LabelDataSvcClient = New LabelDatSvc.LabelDataSvcClient
        Dim labelData As LabelDatSvc.LabelDataMsg = labelClient.LoadDNData(Me.dnId)


        If labelData.ReturnedResult = False Then
            CMsgDialog(MsgLevel.Mistake, "重打送货单时发生如下错误:" & Util.StringListFormat(labelData.ReturnedMessage.ToList)).ShowDialog()
        Else
            Dim records As RecordSet = Offlinereg.ConvertToRecordSet(labelData.PrintData.ToList)
            Dim printTask As TecITGener = New TecITGener
            Try
                printTask.Print(records, _
                                New ReportGenConfig With {.NumberOfCopies = My.Settings.DN_nr, _
                                                          .Printer = _
                                                          My.Settings.productionLabel_printer, _
                                                          .Template = _
                                                             IO.Path.Combine( _
                                                                  My.Application.Info.DirectoryPath, _
                                                                  My.Settings.DNLabel)})

                CMsgDialog(MsgLevel.Successful, "打印成功").ShowDialog()

            Catch ex As Exception
                CMsgDialog(MsgLevel.Mistake, "打印至打印机时出现错误：" & _
                                                               " " & ex.InnerException.Message).ShowDialog()

            End Try
        End If
    End Sub

    Private Sub Button_printer_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button_printer.Click
        Dim settings As ProdLabelPrinterSettings = New ProdLabelPrinterSettings
        settings.ShowDialog()
    End Sub
End Class

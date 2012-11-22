Imports Brilliantech.Packaging.Data
Class MainWindow
    Private currentLabel As PartContainerLabel
    Private currentSourceBarcode As SourceBarcode
    Private currentPartNr As String = ""
    Dim db As PackagingDataDataContext = New PackagingDataDataContext(My.Settings.dbConn)

    Private Sub Window_Loaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        Me.TextBox_partNr.Focus()
    End Sub


    Private Sub init()
        currentLabel = Nothing
        currentSourceBarcode = Nothing
        currentPartNr = ""
        Me.TextBox_partNr.Text = ""
        Me.TextBox_content.Text = ""
        Me.TextBox_fromPos.Text = ""
        Me.TextBox_labelNr.Text = ""
        Me.TextBox_length.Text = ""
        Me.CheckBox_isUniq.IsChecked = False
        Me.TextBox_partNr.Focus()
    End Sub

    Private Sub TextBox_partNr_KeyUp(sender As Object, e As System.Windows.Input.KeyEventArgs) Handles TextBox_partNr.KeyUp
        If e.Key = Key.Enter Then
            If String.IsNullOrEmpty(Me.TextBox_partNr.Text) Then
                MsgBox("请填写零件号！", MsgBoxStyle.Critical)
                init()
            Else


                Try
                    currentLabel = db.PartContainerLabels.FirstOrDefault(Function(c) c.PartNr.ToLower = Me.TextBox_partNr.Text.ToLower)
                    currentSourceBarcode = db.SourceBarcodes.FirstOrDefault(Function(c) c.PartNr.ToLower = Me.TextBox_partNr.Text.ToLower)

                Catch ex As Exception
                    MsgBox("连接数据库时出错了", MsgBoxStyle.Critical)
                    init()
                    Exit Sub
                End Try

                If currentLabel Is Nothing Or currentSourceBarcode Is Nothing Then
                    MsgBox("没有找到该零件号的信息", MsgBoxStyle.Critical)
                    init()
                Else
                    Me.currentPartNr = Me.TextBox_partNr.Text
                    Me.TextBox_content.Text = currentSourceBarcode.DefaultFixedText
                    Me.TextBox_fromPos.Text = currentSourceBarcode.FromPosition
                    Me.TextBox_labelNr.Text = currentLabel.Piece
                    Me.TextBox_length.Text = currentSourceBarcode.Length
                    Me.CheckBox_isUniq.IsChecked = currentSourceBarcode.IsUniq
                End If

            End If

        End If
    End Sub

    Private Sub TextBox_partNr_TextChanged(sender As System.Object, e As System.Windows.Controls.TextChangedEventArgs) Handles TextBox_partNr.TextChanged

    End Sub

    Private Sub Button_setDB_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button_setDB.Click
        Dim window As Window = New dbSetting
        window.ShowDialog()
    End Sub

    Private Function Validate()
        If Not ((Not String.IsNullOrEmpty(Me.TextBox_content.Text)) And IsNumeric(Me.TextBox_fromPos.Text) And IsNumeric(Me.TextBox_labelNr.Text) And IsNumeric(Me.TextBox_length.Text)) Then
            Return False
        Else
            Return True
        End If

    End Function


    Private Sub Button_save_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button_save.Click
        If Me.currentSourceBarcode Is Nothing Or Me.currentLabel Is Nothing Or String.IsNullOrEmpty(Me.currentPartNr) Then
            MsgBox("请重新搜索要修改的零件号", MsgBoxStyle.Critical)
            init()
        Else
            If Me.TextBox_partNr.Text.ToLower <> Me.currentPartNr.ToLower Then
                MsgBox("搜索已经过期，请重新搜索后修改", MsgBoxStyle.Critical)
                init()
            Else
                If Validate() = False Then
                    MsgBox("请填写完整设置", MsgBoxStyle.Critical)
                    Exit Sub
                Else
                    Me.currentLabel.Piece = Me.TextBox_labelNr.Text
                    Me.currentSourceBarcode.IsUniq = Me.CheckBox_isUniq.IsChecked
                    Me.currentSourceBarcode.Length = Me.TextBox_length.Text
                    Me.currentSourceBarcode.FromPosition = Me.TextBox_fromPos.Text
                    Me.currentSourceBarcode.DefaultFixedText = Me.TextBox_content.Text
                    db.SubmitChanges()
                    MsgBox("设置成功！",MsgBoxStyle.Information)
                End If
            End If

        End If
    End Sub
End Class

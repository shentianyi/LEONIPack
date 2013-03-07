Imports Brilliantech.Packaging.UI.WorkStation.PackService
Imports Brilliantech.Packaging.Data
Imports Brilliantech.Packaging.WS

Public Class printLabel

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button_print.Click
        Dim client As PackService.PackProcessClient = New PackProcessClient
        Dim result As PackageMessage
        If Me.ComboBox_mode.SelectedIndex = 0 Then 'tnr
            result = client.FindByItem(Me.TextBox_name.Text)
        Else
            result = client.FindByID(Me.TextBox_name.Text)
        End If
        Try
            client.Close()

        Catch ex As Exception
            client.Abort()

        End Try
        If result Is Nothing Then
            MsgBox("没有获取到数据")
            Exit Sub
        Else
            If result.Package Is Nothing Then
                MsgBox("没有找到相应的包装箱！")
            Else
                If (result.Package.Status <> PackageStatus.Close And _
                    result.Package.Status <> PackageStatus.CloseUnfull And _
                    result.Package.Status <> PackageStatus.CloseWithException) Then
                    MsgBox("只有满箱才能补打满箱标签")
                Else
                    Try
                        MainWindow.PrintFinishLabels(result.Package.PackageID)
                        Dim successInfo As InfoBoard = New InfoBoard(MsgLevel.Successful, "标签打印成功")
                        successInfo.ShowDialog()
                    Catch ex As Exception
                        Dim msg As InfoBoard = New InfoBoard(MsgLevel.Mistake, "标签打印出错，请重试或联系管理员" & Chr(10) & ex.ToString)
                        msg.ShowDialog()
                    End Try
                End If
            End If
        End If
    End Sub

    Private Sub TextBox_name_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles TextBox_name.KeyUp
        If e.Key = Key.Enter Then
            Dispatcher.Invoke(New ShowDelegate(AddressOf ShowStatus))
        End If
    End Sub
    Private Delegate Sub ShowDelegate()

    Private Sub ShowStatus()
        Me.Label_packid.Content = ""
        Me.Label_partnr.Content = ""
        Me.Label_capa.Content = ""
        Me.Label_status.Content = ""
        Dim client As PackService.PackProcessClient = New PackProcessClient
        Dim result As PackageMessage
        If Me.ComboBox_mode.SelectedIndex = 0 Then 'tnr
            result = client.FindByItem(Me.TextBox_name.Text)
        Else
            result = client.FindByID(Me.TextBox_name.Text)
        End If
        Try
            client.Close()

        Catch ex As Exception
            client.Abort()

        End Try

        If result.Package IsNot Nothing Then
            Me.Label_packid.Content = result.Package.PackageID
            Me.Label_partnr.Content = result.Package.PartNr
            Me.Label_capa.Content = result.Package.Capa
            Me.Label_status.Content = Brilliantech.Framework.Utilities.EnumUtil. _
            EnumContainer.GetEnumContent( _
                GetType(PackageStatus), _
                result.Package.Status).Description()
        End If
    End Sub

    Private Sub TextBox_name_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles TextBox_name.TextChanged

    End Sub

    Private Sub Window_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        Me.TextBox_name.Focus()
    End Sub
End Class

Imports System.Drawing.Printing

Public Class ChangePrinter

    Private Sub ok_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles ok.Click
        My.Settings.PrinterName = Me.ComboBox_printers.SelectedItem.ToString
        My.Settings.Save()
        MsgBox("打印机已设置为 " & Me.ComboBox_printers.SelectedItem.ToString)

        If Not IsNumeric(Me.TextBox_sec.Text) Then
            MsgBox("自动关闭时间设置没有成功，请输入一个大于零的数字代表秒数。", MsgBoxStyle.Critical)
        Else
            If CType(Me.TextBox_sec.Text, Integer) <= 0 Then
                MsgBox("自动关闭时间设置没有成功，请输入一个大于零的数字代表秒数。", MsgBoxStyle.Critical)
            Else
                My.Settings.autoCloseInterval = CType(Me.TextBox_sec.Text, Integer) * 1000
                MsgBox("自动关闭时间设置成功")
            End If
        End If
        Me.Close()
    End Sub

    Private Sub exit_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles [exit].Click
        Me.Close()

    End Sub

    Private Sub Window_Loaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        Dim isDefaultIn As Boolean = False

        For Each printer As String In PrinterSettings.InstalledPrinters
            Me.ComboBox_printers.Items.Add(printer)
        Next

        For i As Integer = 0 To Me.ComboBox_printers.Items.Count - 1
            If String.Compare(Me.ComboBox_printers.Items(i), My.Settings.PrinterName, True) = 0 Then
                Me.ComboBox_printers.SelectedIndex = i
                isDefaultIn = True
            End If
        Next

        If isDefaultIn = False Then
            Me.ComboBox_printers.SelectedIndex = 0
        End If

        Me.TextBox_sec.Text = CType(My.Settings.autoCloseInterval, Integer) / 1000

    End Sub

    Private Sub TextBox_sec_GotFocus(sender As Object, e As System.Windows.RoutedEventArgs) Handles TextBox_sec.GotFocus
        Try
            System.Diagnostics.Process.Start("C:\Windows\system32\Osk.exe ")
        Catch ex As Exception

        End Try

    End Sub

    Private Sub TextBox_sec_TextChanged(sender As System.Object, e As System.Windows.Controls.TextChangedEventArgs) Handles TextBox_sec.TextChanged

    End Sub
End Class

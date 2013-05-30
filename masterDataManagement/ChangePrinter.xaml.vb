Imports System.Drawing.Printing

Public Class ChangePrinter

    Private Sub ok_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles ok.Click
        My.Settings.PrinterName = Me.ComboBox_printers.SelectedItem.ToString
        My.Settings.Save()
        MsgBox("打印机已设置为 " & Me.ComboBox_printers.SelectedItem.ToString)

       
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



    End Sub



End Class

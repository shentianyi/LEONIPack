Imports Brilliantech.Framework.WCF.Data
Imports Brilliantech.Packaging.UI.WorkStation.PackService
Public Class cancelPackage

    Private Sub Button_cancel_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button_cancel.Click
        Me.Close()
    End Sub

    Private Sub Start()
        Dim info As InfoBoard = New InfoBoard(MsgLevel.Warning, "您正在开始删除一个包装箱，所有信息将被删除且不可恢复，您确定吗？")

        If info.ShowDialog = False Then
            Exit Sub
        End If

        Dim client As PackProcessClient = New PackProcessClient
        Try
            Dim result As ServiceMessage = _
            client.CancelPackaging(Me.TextBox1.Text)
            Try
                client.Close()
            Catch ex As Exception
                client.Abort()
            End Try
            If result.ReturnedResult = True Then
                Dim infoboard As New InfoBoard(MsgLevel.Successful, "取消包装成功")
                infoboard.ShowDialog()

            Else
                Dim InfoBoard As New InfoBoard(MsgLevel.Mistake, result.ReturnedMessage.ToArray)
                InfoBoard.ShowDialog()

            End If
        Catch ex As Exception
            Dim errorInfo As InfoBoard = New InfoBoard(MsgLevel.Mistake, "取消包装时发生未知错误！")
            errorInfo.ShowDialog()
        End Try
        init()
    End Sub
    Private Sub Button_ok_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button_ok.Click
        Start()
      
    End Sub
    Private Sub init()
        Me.TextBox1.Text = ""
        Me.TextBox1.Focus()
    End Sub
    Private Sub Window_Loaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        init()
    End Sub

    Private Sub TextBox1_IsStylusDirectlyOverChanged(sender As Object, e As System.Windows.DependencyPropertyChangedEventArgs) Handles TextBox1.IsStylusDirectlyOverChanged

    End Sub

    Private Sub TextBox1_KeyUp(sender As Object, e As System.Windows.Input.KeyEventArgs) Handles TextBox1.KeyUp
        If e.Key = Key.Enter Then
            Start()
        End If
    End Sub

    Private Sub TextBox1_TextChanged(sender As System.Object, e As System.Windows.Controls.TextChangedEventArgs) Handles TextBox1.TextChanged

    End Sub

    Private Sub Image1_ImageFailed(sender As System.Object, e As System.Windows.ExceptionRoutedEventArgs) Handles Image1.ImageFailed
      

    End Sub

    Private Sub Image1_MouseDown(sender As Object, e As System.Windows.Input.MouseButtonEventArgs) Handles Image1.MouseDown
        Try
            System.Diagnostics.Process.Start("C:\Windows\system32\Osk.exe")
        Catch ex As Exception

        End Try
    End Sub
End Class

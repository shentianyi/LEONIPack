Public Class dbSetting

    Private Sub Button_save_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button_save.Click
        My.Settings.dbConn = Me.TextBox1.Text
        My.Settings.Save()
        MsgBox("保存成功")
        Me.Close()
    End Sub

    Private Sub TextBox1_TextChanged(sender As System.Object, e As System.Windows.Controls.TextChangedEventArgs) Handles TextBox1.TextChanged

    End Sub

    Private Sub Window_Loaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        Me.TextBox1.Text = My.Settings.dbConn

    End Sub
End Class

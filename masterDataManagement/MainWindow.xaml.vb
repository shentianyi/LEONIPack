Class MainWindow 

    Private Sub Window_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        Me.Label_version.Content = My.Application.Info.Version

    End Sub

    Private Sub template_maintain_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles template_maintain.Click
        Dim tempMain As Window = New templateMaintainance
        tempMain.Show()
    End Sub
End Class

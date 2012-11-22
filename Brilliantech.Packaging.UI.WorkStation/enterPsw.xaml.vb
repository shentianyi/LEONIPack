Public Class enterPsw

    Public psw As List(Of String) = New List(Of String)


    Private Sub PasswordBox_psw_KeyUp(sender As Object, e As System.Windows.Input.KeyEventArgs) Handles PasswordBox_psw.KeyUp
        If e.Key = Key.Enter Then
            psw.Add(PasswordBox_psw.Password)
            Me.Close()
        End If
    End Sub

    Private Sub PasswordBox_psw_PasswordChanged(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles PasswordBox_psw.PasswordChanged

    End Sub

    Private Sub Button_ok_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button_ok.Click
        psw.Add(PasswordBox_psw.Password)
        Me.Close()
    End Sub


    Public Sub New(ByRef refPsw As List(Of String))
        Me.psw = refPsw

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub Window_Loaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        Me.PasswordBox_psw.Focus()
    End Sub
End Class

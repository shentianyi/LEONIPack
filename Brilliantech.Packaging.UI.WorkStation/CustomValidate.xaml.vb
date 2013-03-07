Imports System.Text.RegularExpressions
Public Class CustomValidate
    Private _validateItem As List(Of CustomValidateViewModel)
    Public Sub New(ByVal validates As List(Of CustomValidateViewModel))

        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        If validates Is Nothing Then
            Dim infoBoard As New InfoBoard(MsgLevel.Mistake, "没有需要检验的自定义项")
            infoBoard.ShowDialog()
            Me.Close()
        Else
            If validates.Count = 0 Then
                Dim infoBoard As New InfoBoard(MsgLevel.Mistake, "没有需要检验的自定义项")
                infoBoard.ShowDialog()
                Me.Close()
            Else
                _validateItem = validates


            End If
        End If

    End Sub

    Private Sub TextBox_scan_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles TextBox_scan.PreviewKeyDown
        If e.Key = Key.Enter Then
            Dim selected As CustomValidateViewModel = _
                _validateItem.FirstOrDefault(Function(a) _
                                            Regex.IsMatch( _
                                                Me.TextBox_scan.Text, a.PatternString) = True _
                                            And a.IsValidated = False)

            If selected Is Nothing Then
                Dim infoBoard As New InfoBoard(MsgLevel.Mistake, "您扫描的条码不符合任何检查条件")
                infoBoard.ShowDialog()

            Else
                selected.IsValidated = True
                If _validateItem.LongCount(Function(c) c.IsValidated = False) = 0 Then
                    Me.DialogResult = True
                    Me.Close()
                Else

                End If
            End If
            InitScaner()
        End If
    End Sub

    Private Sub TextBox_scan_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles TextBox_scan.TextChanged

    End Sub

    Private Sub Window_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        init()

    End Sub

    Private Sub init()
        Me.ListBox_conditions.ItemsSource = _validateItem
        InitScaner()
    End Sub

    Private Sub InitScaner()
        Me.TextBox_scan.Text = ""
        Me.TextBox_scan.Focus()
    End Sub

    Private Sub Button_exit_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button_exit.Click
        Me.DialogResult = False
        Me.Close()
    End Sub


End Class

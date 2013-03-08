Imports Brilliantech.Packaging.UI.WorkStation.PackService
Imports System.ServiceModel

Public Class ScanWorkStation

    Private Sub bt_reset_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles bt_reset.Click
        init()
    End Sub

    Private Sub init()
        Me.TextBox_firstScan.Text = ""
        Me.TextBox_secondScan.Text = ""
        Me.TextBox_firstScan.Focus()
    End Sub

    Private Sub ScanWorkStation_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
        Try
 
            'Try
            '    ReplicationUtils.ReplicateMasterData()
            'Catch ex As Exception

            'End Try
        Catch ex As Exception

        End Try
    End Sub



    Private Sub Window_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        init()
        Try

        Catch ex As Exception

        End Try


    End Sub

    Private Sub TextBox_secondScan_PreviewKeyUp(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles TextBox_secondScan.PreviewKeyUp
        If e.Key = Key.Enter Then
            e.Handled = True
            Process()

        End If
    End Sub

    Private Sub Process()
        If String.IsNullOrEmpty(Me.TextBox_firstScan.Text) Or _
            String.IsNullOrEmpty(Me.TextBox_secondScan.Text) Then
            Dim info As InfoBoard = (New InfoBoard(MsgLevel.Mistake, "请扫描两次操作台号"))
            info.ShowDialog()
            Exit Sub
        End If

        If String.Compare(Me.TextBox_firstScan.Text, Me.TextBox_secondScan.Text, True) <> 0 Then
            Dim info As InfoBoard = (New InfoBoard(MsgLevel.Mistake, "两次扫描的操作台号不一致"))
            info.ShowDialog()
            Exit Sub
        End If

        Dim client As PackProcessClient = New PackProcessClient
        Dim hasWrkstn As Boolean = False
        Try
            hasWrkstn = client.WorkstationExists(Me.TextBox_firstScan.Text)
            client.Close()
        Catch ex As Exception
            client.Abort()
        End Try

        If hasWrkstn = False Then
            Dim info As InfoBoard = (New InfoBoard(MsgLevel.Mistake, "扫描入的操作台号不存在"))
            info.ShowDialog()
            Exit Sub
        End If

        Usersession.WorkStationNr = TextBox_firstScan.Text
        Dim beginWindow As Window = New Begin
        beginWindow.Owner = Me

        beginWindow.ShowDialog()
        Usersession.Clear()
        init()
    End Sub

    Private Sub TextBox_secondScan_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles TextBox_secondScan.TextChanged

    End Sub

    Private Sub Button_exit_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button_exit.Click
        Me.Close()
        Environment.Exit(0)
    End Sub

    Private Sub TextBox_firstScan_PreviewKeyUp(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles TextBox_firstScan.PreviewKeyUp
        If e.Key = Key.Enter Then
            Me.TextBox_secondScan.Focus()
        End If
    End Sub

    Private Sub TextBox_firstScan_TextChanged(ByVal sender As System.Object, ByVal e As System.Windows.Controls.TextChangedEventArgs) Handles TextBox_firstScan.TextChanged

    End Sub
End Class

Imports System.Windows.Forms
Class MainWindow
    Protected WithEvents syncTimer As System.Timers.Timer
    Protected ws As WindowState
    Protected wsl As WindowState
    Protected WithEvents notifyicon As NotifyIcon
    Public Sub New()

        ' 此调用是设计器所必需的。
        InitializeComponent()
        Me.WindowState = Windows.WindowState.Minimized
        ' 在 InitializeComponent() 调用之后添加任何初始化。
        ShowInIcon()

        wsl = Me.WindowState
    End Sub

    Private Sub ShowInIcon()
        Me.notifyIcon = New NotifyIcon()
        Me.notifyicon.BalloonTipText = "包装系统数据同步器已经开始"
        Me.notifyicon.Text = "包装系统数据同步器"
        Me.notifyicon.Icon = New System.Drawing.Icon("picon.ico")
        Me.notifyicon.Visible = True

        Me.notifyicon.ShowBalloonTip(1000)
    End Sub

    Private Sub Window_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        syncTimer = New Timers.Timer
        syncTimer.Interval = My.Settings.interval
        syncTimer.Start()
    End Sub

    Private Sub syncTimer_elasped() Handles syncTimer.Elapsed
        syncTimer.Stop()
        Try
            ReplicationUtils.ReplicateMasterData()
        Catch ex As Exception
            Try

            Catch ex1 As Exception

            End Try

        End Try
        Dim rd As System.Random = New System.Random


        syncTimer.Start()
    End Sub

    Private Sub notifyicon_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles notifyicon.MouseDoubleClick
        Me.Show()
        WindowState = wsl
    End Sub

    Private Sub MainWindow_StateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.StateChanged
        ws = Me.WindowState
        If (ws = WindowState.Minimized) Then
            Me.Hide()
        End If
    End Sub
End Class

Imports System
Imports System.IO
Imports System.Net
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Data
Imports System.Windows.Media
Imports System.Windows.Media.Animation
Imports System.Windows.Navigation
Imports Brilliantech.Packaging.UI.WorkStation.PackService
Imports System.ComponentModel


Public Class PackView
    Friend WithEvents bw_search As BackgroundWorker
    Private packStatus As List(Of EnumObject) = New List(Of EnumObject)
    Private splash As SplashWindow
    Private gridData As List(Of Data.FullPackageInfo)
    Public Sub New()

        ' 此调用是设计器所必需的。
        InitializeComponent()
        bw_search = New BackgroundWorker
        ' 在 InitializeComponent() 调用之后添加任何初始化。

    End Sub



    Private Sub PackView_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        BindComb()
        SetDateTime()
    End Sub

    Private Sub SetDateTime()
        Me.DP_WindowsTimeFrom.SelectedDate = New DateTime(Now.Year, Now.Month - 2, 1)
        Me.DP_WindowsTimeTo.SelectedDate = New DateTime(Now.Year, Now.Month + 1, 1)
        Me.TP_WindowsTimeFrom.txtHours.Text = 0
        Me.TP_WindowsTimeFrom.txtHours.Text = 0
        Me.TP_WindowsTimeTo.txtHours.Text = 23
        Me.TP_WindowsTimeTo.txtMinutes.Text = 59

    End Sub

    Private Function GetPackageStatus() As List(Of EnumObject)
        Dim client As PackService.PackProcessClient = New PackProcessClient()
        Dim result As List(Of EnumObject) = client.GetPackageStatus.ToList
        Try
            client.Close()
        Catch ex As Exception
            client.Abort()
        End Try
        Return result
    End Function

    Private Sub BindComb()
        packStatus = GetPackageStatus()
        Me.comb_packStatus.ItemsSource = packStatus
        Try
            Me.comb_packStatus.SelectedIndex = 0
        Catch ex As Exception

        End Try
    End Sub

  
    Private Sub Button_reset_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button_reset.Click
        Me.tb_packId.Text = ""
        Me.tb_partnr.Text = ""
        Me.tb_tnr.Text = ""
        Me.tb_wrkst.Text = ""
    End Sub

    Private Sub Button_search_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button_search.Click
        Dim args As String() = New String() { _
                                            Me.tb_packId.Text, _
                                            Me.tb_wrkst.Text, _
                                            Me.tb_tnr.Text, _
                                            Me.tb_partnr.Text, _
                                            Me.comb_packStatus.SelectedItem.EnumValue, _
                                            New DateTime(Me.DP_WindowsTimeFrom.SelectedDate.Value.Year, _
                                                 Me.DP_WindowsTimeFrom.SelectedDate.Value.Month, _
                                                 Me.DP_WindowsTimeFrom.SelectedDate.Value.Day, _
                                                 Me.TP_WindowsTimeFrom.DateTimeValue.Value.Hour, _
                                                 Me.TP_WindowsTimeFrom.DateTimeValue.Value.Minute, _
                                                 Me.TP_WindowsTimeFrom.DateTimeValue.Value.Second, _
                                                 Me.TP_WindowsTimeFrom.DateTimeValue.Value.Millisecond), _
                                            New DateTime(Me.DP_WindowsTimeTo.SelectedDate.Value.Year, _
                                                 Me.DP_WindowsTimeTo.SelectedDate.Value.Month, _
                                                 Me.DP_WindowsTimeTo.SelectedDate.Value.Day, _
                                                 Me.TP_WindowsTimeTo.DateTimeValue.Value.Hour, _
                                                 Me.TP_WindowsTimeTo.DateTimeValue.Value.Minute, _
                                                 Me.TP_WindowsTimeTo.DateTimeValue.Value.Second, _
                                                 Me.TP_WindowsTimeTo.DateTimeValue.Value.Millisecond)}
        bw_search.RunWorkerAsync(args)
        Try
            splash = Nothing
        Catch ex As Exception

        End Try
        splash = New SplashWindow("正在搜索，请稍候")
        splash.ShowDialog()
    End Sub

    Private Sub bw_search_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bw_search.DoWork
        Dim client As PackProcessClient = New PackProcessClient
        Dim packInfos As List(Of Data.FullPackageInfo) = client.GetPackageInfos(e.Argument(0), e.Argument(1), e.Argument(2), e.Argument(3), e.Argument(4), e.Argument(5), e.Argument(6)).ToList
        e.Result = packInfos
        Try
            client.Close()

        Catch ex As Exception
            client.Abort()
        End Try
    End Sub

    Private Sub bw_search_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bw_search.RunWorkerCompleted
        gridData = e.Result
        DataGrid_orderDetails.DataContext = gridData
        Me.label_resultCount.Content = Me.gridData.Count
        Try
            Me.splash.Close()
            Me.splash = Nothing
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button_viewItems_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button_viewItems.Click
        If Me.DataGrid_orderDetails.SelectedItems.Count < 1 Then
            Dim infoboard As InfoBoard = New InfoBoard(MsgLevel.Mistake, "请选择一个包装箱")
            infoboard.ShowDialog()
        Else
            Dim itemview As Window = New ViewPackageItem(Me.DataGrid_orderDetails.SelectedItem.Pid)
            itemview.ShowDialog()
        End If
    End Sub
End Class

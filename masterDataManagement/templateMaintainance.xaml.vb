Imports masterDataManagement.packSvc
Imports System.ComponentModel
Imports Brilliantech.Packaging.Data

Public Class templateMaintainance
    Friend WithEvents bw_search As New BackgroundWorker
    Private gridData As List(Of FullPackageInfo) = New List(Of FullPackageInfo)
    Private splash As SplashWindow
    Private Sub Button_reset_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button_reset.Click
        Refresh()
    End Sub

    Private Sub Refresh()
        Me.tb_packId.Text = ""
        Me.tb_partnr.Text = ""
        Me.tb_wrkst.Text = ""
    End Sub
    Private Sub Button_search_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button_search.Click
        Dim args As String() = New String() { _
                                      Me.tb_packId.Text, _
                                      Me.tb_wrkst.Text, _
                                      Me.tb_partnr.Text}
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
        Dim packInfos As List(Of FullPackageInfo) = client.GetPackageInfos(e.Argument(0), e.Argument(1), Nothing, e.Argument(2), PackageStatus.Template, New Date(1960, 1, 1), New Date(2100, 1, 1)).ToList
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


End Class

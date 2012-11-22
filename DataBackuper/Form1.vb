Imports System.Data.SqlClient
Imports System.Transactions
Imports System.IO
Imports System.Threading

Public Class Form1
    Protected Friend WithEvents timer As System.Timers.Timer

    Public Sub New()


        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        timer = New System.Timers.Timer
        timer.Interval = My.Settings.interval
        timer.Enabled = True
        timer.Stop()
    End Sub
    Private Sub Button_start_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_start.Click
        DoTransfer(Me.DateTimePicker_start.Value, Me.DateTimePicker_end.Value)
        MsgBox(DoTransfer(Me.DateTimePicker_start.Value, Me.DateTimePicker_end.Value))
    End Sub

    Private Function DoTransfer(ByVal FromDate As Date, ByVal endDate As Date) As String
        Dim returned As String = ""

        Dim fromDateStr As String = FromDate.ToString("yyyy-MM-dd")
        Dim endDateStr As String = endDate.ToString("yyyy-MM-dd")


        Dim cnn As SqlConnection = New SqlConnection(My.Settings.connStr)

        Dim cmd_spInsert As SqlCommand = New SqlCommand(My.Settings.sqlStSinglePackInsert.Replace("@dte", "<'" + endDateStr + "' ").Replace("@dtl", ">'" + fromDateStr + "'"), cnn)
        'cmd_spInsert.Parameters.AddWithValue("@dte", endDateStr)
        'cmd_spInsert.Parameters.AddWithValue("@dtl", fromDateStr)

        Dim cmd_piInsert As SqlCommand = New SqlCommand(My.Settings.sqlPackItemInsert.Replace("@dte", "<'" + endDateStr + "' ").Replace("@dtl", ">'" + fromDateStr + "'"), cnn)
        'cmd_piInsert.Parameters.AddWithValue("@dte", endDateStr)
        'cmd_piInsert.Parameters.AddWithValue("@dtl", fromDateStr)



        Dim cmd_pidel As SqlCommand = New SqlCommand(My.Settings.sqlPackItemDel.Replace("@dte", "<'" + endDateStr + "' ").Replace("@dtl", ">'" + fromDateStr + "'"), cnn)
        'cmd_pidel.Parameters.AddWithValue("@dte", endDateStr)
        'cmd_pidel.Parameters.AddWithValue("@dtl", fromDateStr)

        Dim cmd_padel As SqlCommand = New SqlCommand(My.Settings.sqlSinglePackageDel.Replace("@dte", "<'" + endDateStr + "' ").Replace("@dtl", ">'" + fromDateStr + "'"), cnn)
        'cmd_padel.Parameters.AddWithValue("@dte", endDateStr)
        'cmd_padel.Parameters.AddWithValue("@dtl", fromDateStr)

        Dim spInsert As Integer
        Dim piInsert As Integer
        Dim pidel As Integer
        Dim padel As Integer

        Try
            Using scope As New TransactionScope
                cnn.Open()
                spInsert = cmd_spInsert.ExecuteNonQuery()
                piInsert = cmd_piInsert.ExecuteNonQuery()
                pidel = cmd_pidel.ExecuteNonQuery()
                padel = cmd_padel.ExecuteNonQuery()
                scope.Complete()
            End Using
            WriteLog(Now.ToString + "    " + "转移成功:" + spInsert.ToString + "条包装；" + piInsert.ToString + "条包装内容被转移；" + pidel.ToString + "条包装内容被移除；" + padel.ToString + "条包装信息被移除")
            returned = "转移成功:" + spInsert.ToString + "条包装；" + piInsert.ToString + "条包装内容被转移；" + pidel.ToString + "条包装内容被移除；" + padel.ToString + "条包装信息被移除"
        Catch ex As Exception
            WriteLog(Now.ToString + "    " + "失败：" + ex.ToString)
            returned = Now.ToString + "    " + "失败：" + ex.ToString
        Finally
            Try
                cnn.Close()

            Catch ex As Exception

            End Try

        End Try
        Return returned
    End Function


  
    Private Shared locker As New Object
    Public Shared Sub WriteLog(ByVal msg As String)
        SyncLock locker
            Dim filename As String = "logs\" + Now.ToString("yyyyMMdd") + ".txt"
            Dim file As String = System.IO.Path.Combine(My.Application.Info.DirectoryPath, filename)
            Dim sw As StreamWriter = New StreamWriter(file, True, System.Text.Encoding.UTF8)
            sw.WriteLine(Now.ToString & vbTab & msg)
            sw.Close()
        End SyncLock
    End Sub

    Private Sub Button_auto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_auto.Click
        If My.Settings.autoTransfer = True Then
            Me.timer.Stop()
            My.Settings.autoTransfer = False
            Me.Button_auto.Text = "点击开始自动转移"

        Else
            Me.Button_auto.Text = "点击停止自动转移"
            My.Settings.autoTransfer = True
            Me.timer.Start()
        End If
    End Sub

    Private Sub AutoDoTransfer()

    End Sub

    Private Sub timer_elapsed() Handles timer.Elapsed
        timer.Stop()
        Try
            DoTransfer(SqlTypes.SqlDateTime.MinValue.Value, Now.Subtract(New TimeSpan(My.Settings.transferDay, 0, 0, 0)))

        Catch ex As Exception

        End Try

        timer.Start()
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If My.Settings.autoTransfer = True Then
            Me.Button_auto.Text = "点击停止自动转移"
            Me.timer.Enabled = True
            Me.timer.Start()
        Else
            Me.Button_auto.Text = "点击开始自动转移"
        End If
    End Sub
End Class

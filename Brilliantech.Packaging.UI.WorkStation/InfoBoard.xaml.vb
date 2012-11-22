Imports System.IO

Public Class InfoBoard
    Private Shared ReadOnly infoImage As String = Path.Combine(My.Application.Info.DirectoryPath, "ImageMedia\Info.png")
    Private Shared ReadOnly warningImage As String = Path.Combine(My.Application.Info.DirectoryPath, "ImageMedia\Warning.png")
    Private Shared ReadOnly successImage As String = Path.Combine(My.Application.Info.DirectoryPath, "ImageMedia\ok.png")
    Private Shared ReadOnly errorImage As String = Path.Combine(My.Application.Info.DirectoryPath, "ImageMedia\Error.png")
    Private WithEvents timer As System.Timers.Timer
    Public Sub New(level As MsgLevel, msg As String)
        InitializeComponent()
        Me.AssignPicture(level)
        Me.AssignText(msg)

    End Sub

    Public Sub New(level As MsgLevel, msg As String, autoCloseInterval As Integer)
        Me.New(level, msg)
        timer = New Timers.Timer
        timer.Interval = autoCloseInterval
        timer.Start()
    End Sub
    Public Sub New(level As MsgLevel, msg() As String)
        InitializeComponent()
        Me.AssignPicture(level)
        Me.AssignText(ArrayToString(msg))


    End Sub

    Public Sub New(level As MsgLevel, msg() As String, autoCloseInterval As Integer)
        Me.New(level, msg)
        timer = New Timers.Timer
        timer.Interval = autoCloseInterval
        timer.Start()
    End Sub
    Public Sub timer_elsped() Handles timer.Elapsed
        timer.Stop()
        Dispatcher.Invoke(New CloseMyself(AddressOf CloseSub))
    End Sub

    Private Delegate Sub CloseMyself()

    Private Sub CloseSub()
        Me.DialogResult = True
        Me.Close()
    End Sub

    Public Sub AssignPicture(level As MsgLevel)
        Dim imagePath As String = ""
        Try
            Select Case level
                Case MsgLevel.Info
                    imagePath = infoImage
                    Exit Select
                Case MsgLevel.Mistake
                    imagePath = errorImage
                    Exit Select
                Case MsgLevel.Successful
                    imagePath = successImage
                    Exit Select
                Case MsgLevel.Warning
                    imagePath = warningImage
                    Exit Select
            End Select
            Me.Indicator.Source = New BitmapImage(New Uri(imagePath, UriKind.Absolute))
        Catch ex As Exception

        End Try


    End Sub


    Private Sub AssignText(msg As String)
        Me.TextBox_Message.Text = msg

    End Sub

    Public Shared Function ArrayToString(str() As String)
        Dim combined As String = ""
        If str IsNot Nothing Then
            If str.Length > 0 Then
                For Each st As String In str
                    combined = combined & vbCrLf & st
                Next
            End If

        End If
        Return combined.TrimStart(New Char() {vbCrLf})
    End Function

    Private Sub Button_yes_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button_yes.Click
        Try
            Me.timer.Stop()
        Catch ex As Exception

        End Try
        Me.DialogResult = True
        Me.Close()
    End Sub

    Private Sub Button_no_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Button_no.Click
        Try
            Me.timer.Stop()
        Catch ex As Exception

        End Try

        Me.DialogResult = False
        Me.Close()
    End Sub

    Private Sub Window_Loaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded

    End Sub
End Class

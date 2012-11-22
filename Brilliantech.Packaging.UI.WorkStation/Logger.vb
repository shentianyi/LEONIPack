Imports System.IO
Public Class Logger
    Private Shared locker As New Object
    Public Shared Sub Write(ByVal msg As String)
        SyncLock locker
            Dim filename As String = "logs\" + Now.ToString("yyyyMMdd") + ".txt"
            Dim file As String = System.IO.Path.Combine(My.Application.Info.DirectoryPath, filename)
            Dim sw As StreamWriter = New StreamWriter(file, True, System.Text.Encoding.Default)
            sw.WriteLine(Now.ToString & vbTab & msg)
            sw.Close()
        End SyncLock
    End Sub
End Class

Imports Nini.Config

Public Class ConfigUtil
    Private config As IConfig
    Private source As IConfigSource

    Public Sub New()
    End Sub

    Public Sub New(ByVal node As String)
        config = source.Configs(node)
    End Sub

    Public Sub New(ByVal node As String, ByVal filename As String)
        source = New IniConfigSource(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename))
        config = source.Configs(node)
    End Sub

    ''' <summary>
    ''' update the node
    ''' </summary>
    ''' <param name="node">name of node</param>
    Public Sub Update(ByVal node As String, ByVal filename As String)
        config = source.Configs(node)
        If config Is Nothing Then
            source.AddConfig(node)
            config = source.Configs(node)
        End If
        RemoveKeys()
    End Sub

    ''' <summary>
    ''' get value by key
    ''' </summary>
    ''' <param name="key"></param>
    ''' <returns></returns>
    Public Function [Get](ByVal key As String) As String
        Return config.[Get](key)
    End Function

    ''' <summary>
    ''' get all values of node
    ''' </summary>
    ''' <returns></returns>
    Public Function GetAllNodeValue() As String()
        Return config.GetValues()
    End Function

    Public Function GetAllNodeKey() As String()
        Return config.GetKeys()
    End Function


    ''' <summary>
    ''' set key and value
    ''' </summary>
    ''' <param name="key"></param>
    ''' <param name="value"></param>
    Public Sub [Set](ByVal key As String, ByVal value As Object)
        config.[Set](key, value)
    End Sub

    ''' <summary>
    ''' save the changes
    ''' </summary>
    Public Sub Save()
        source.Save()
    End Sub

    ''' <summary>
    ''' remove all keys
    ''' </summary>
    Private Sub RemoveKeys()
        Dim keys As String() = config.GetKeys()
        For Each key As String In keys
            config.Remove(key)
        Next
    End Sub
End Class


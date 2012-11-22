Public Class IDGenerationException
    Inherits Exception

    Private Shared ReadOnly msg As String = "生成ID号时出错"
    Public Sub New(innex As Exception)
        MyBase.New(msg)
    End Sub

    Public Sub New()
        MyBase.New(msg)
    End Sub
End Class

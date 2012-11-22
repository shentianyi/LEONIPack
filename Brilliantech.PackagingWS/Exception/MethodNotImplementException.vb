Public Class MethodNotImplementException
    Inherits Exception
    Public Sub New(functionName As String)
        MyBase.New("函数" & functionName & "未实现")
    End Sub

    Public Sub New()
        MyBase.New("函数未实现")
    End Sub
End Class

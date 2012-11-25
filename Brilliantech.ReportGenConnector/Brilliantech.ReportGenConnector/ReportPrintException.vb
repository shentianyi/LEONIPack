Public Class ReportPrintException
    Inherits Exception
    Public Sub New(innEx As Exception)
        MyBase.New("生成打印标签或报表时出错", innEx)
    End Sub

End Class

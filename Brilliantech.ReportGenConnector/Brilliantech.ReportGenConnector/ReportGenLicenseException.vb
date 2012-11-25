Public Class ReportGenLicenseException
    Inherits Exception
    Public Sub New(innEx As Exception)
        MyBase.New("打印服务授权异常", innEx)
    End Sub
End Class

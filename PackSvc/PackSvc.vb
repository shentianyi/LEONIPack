Imports System.ServiceModel
Imports Brilliantech.Packaging.WS
Imports log4net
Imports System.IO


Public Class PackSvc
    Private label_Host As ServiceHost
    Private process_host As ServiceHost
    Private Shared serviceLogger As ILog = LogManager.GetLogger("PackageLog")


    Protected Overrides Sub OnStart(ByVal args() As String)
        log4net.Config.XmlConfigurator.ConfigureAndWatch(New FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logConfig.xml")))
        Try
            label_Host = New ServiceHost(GetType(PrintService))
            process_host = New ServiceHost(GetType(PackProcess))
            label_Host.Open()
            process_host.Open()
            serviceLogger.Info("服务已启动")
        Catch ex As Exception
            serviceLogger.Fatal("服务启动出错:" & ex.ToString)
            Me.Stop()
        End Try
    End Sub

    Protected Overrides Sub OnStop()
        ' 在此处添加代码以执行任何必要的拆解操作，从而停止您的服务。
        serviceLogger.Warn("服务已经关闭")
        process_host.Close()
        label_Host.Close()


    End Sub

End Class

Imports System.ServiceModel
Imports Brilliantech.Packaging.WS
Module Module1
    Private packageService As ServiceHost
    Private labelService As ServiceHost
    Sub Main()
        packageService = New ServiceHost(GetType(PackProcess))
        labelService = New ServiceHost(GetType(PrintService))
        packageService.Open()
        labelService.Open()
        Console.WriteLine("包装服务已开始")
        Console.ReadLine()
    End Sub
End Module

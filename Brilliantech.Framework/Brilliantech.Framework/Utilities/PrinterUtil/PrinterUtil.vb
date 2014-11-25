Imports Nini
Public Class PrinterUtil
    Private Shared config As New ConfigUtil("PRINTER", "printer.ini")
    ''' <summary>
    ''' get printer settings
    ''' </summary>
    ''' <returns></returns>
    Public Shared Function GetPrinterConfig() As Hashtable
        Dim ht As Hashtable = New Hashtable
        ht.Add("Copy", config.[Get]("COPY"))
        ht.Add("Template", config.[Get]("TEMPLATE"))
        ht.Add("AutoClose", config.[Get]("AUTOCLOSE_INTERVAL"))
        ht.Add("PrinterName", config.[Get]("PRINTER_NAME"))
        Return ht
    End Function

    ''' <summary>
    ''' set printer name
    ''' </summary>
    ''' <param name="name"></param>
    Public Shared Sub SetPrinterName(ByVal name As String)
        config.[Set]("PRINTER_NAME", name)
        config.Save()
    End Sub

    Public Shared Sub SetPrintCopy(ByVal copy As Integer)
        config.[Set]("COPY", copy)
        config.Save()
    End Sub

    ''' <summary>
    ''' set printer auto close interval
    ''' </summary>
    ''' <param name="misecond"></param>
    Public Shared Sub SetPrinterAutoInterVal(ByVal misecond As Integer)
        config.[Set]("AUTOCLOSE_INTERVAL", misecond)
        config.Save()
    End Sub

    ''' <summary>
    ''' set current dir for web cmd
    ''' </summary>
    ''' <param name="dir"></param>
    Public Shared Sub SetCurrentDir(ByVal dir As String)
        config.[Set]("CURRENTDIR", dir)
        config.Save()
    End Sub

    ''' <summary>
    ''' get current dir for web cmd
    ''' </summary>
    ''' <returns></returns>
    Public Shared Function GetCurrentDir() As String
        Return config.[Get]("CURRENTDIR")
    End Function

    Public Shared Sub Save()
        config.Save()
    End Sub

End Class

Public Class DateUtil

    Public Shared Function DateFormat(ByVal dt As DateTime, ByVal format As String) As String
        Return dt.ToString(format)
    End Function
End Class
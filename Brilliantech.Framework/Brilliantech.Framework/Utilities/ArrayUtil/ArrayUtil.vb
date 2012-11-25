Namespace Utilities


    Public Class ArrayUtil
        Public Shared Function IsArrayNullorEmpty(arr As Array) As Boolean
            If arr Is Nothing Then
                Return True
            Else
                If arr.Length < 1 Then
                    Return True
                Else
                    Return False
                End If
            End If
        End Function
    End Class
End Namespace
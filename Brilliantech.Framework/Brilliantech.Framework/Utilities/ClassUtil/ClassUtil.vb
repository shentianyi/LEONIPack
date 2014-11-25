Public Class ClassUtil
    Public Shared Function GetModelValue(ByVal FieldName As String, ByVal obj As Object) As String
        Try
            Dim Ts As Type = obj.GetType()
            Dim o As Object = Ts.GetProperty(FieldName).GetValue(obj, Nothing)
            Dim Value As String = Convert.ToString(o)
            If String.IsNullOrEmpty(Value) Then
                Return Nothing
            End If
            Return Value
        Catch
            Return Nothing
        End Try
    End Function



    Public Shared Function GetModelValues(ByVal FieldNames As List(Of String), ByVal obj As Object) As List(Of String)
        Try
            Dim Ts As Type = obj.[GetType]()
            Dim values As New List(Of String)()
            For Each FieldName As String In FieldNames
                Dim o As Object = Ts.GetProperty(FieldName).GetValue(obj, Nothing)
                Dim value As String = Convert.ToString(o)
                values.Add(If(String.IsNullOrEmpty(value), Nothing, value))
            Next
            Return values
        Catch
            Return Nothing
        End Try
    End Function

End Class


Namespace Utilities.DBUtil.MongoDB


    Public MustInherit Class MongoTableMapper

        Protected MustOverride Sub LoadFrom()

        Protected mappingDict As Dictionary(Of String, String) = New Dictionary(Of String, String)


        Public Function GetTableName(type As Type) As String
            If Not mappingDict.ContainsKey(type.FullName.ToString) Then
                Throw New TypeTableMappingException
            Else
                Return mappingDict(type.FullName.ToString)
            End If
        End Function
    End Class
End Namespace
Imports Brilliantech.Framework.Utilities.EnumUtil
<DataContract()>
Public Class EnumObject

    Private _key As String
    Private _value As String
    Private _description As String

    Public Sub New(enumkey As String, val As String, desc As String)
        _key = enumkey
        _value = val
        _description = desc
    End Sub
    <DataMember()>
    Public Property EnumKey
        Get
            Return _key
        End Get
        Set(value)
            _key = value
        End Set
    End Property


    <DataMember()>
    Public Property EnumValue
        Get
            Return _value
        End Get
        Set(value)
            _value = value
        End Set
    End Property

    <DataMember()>
    Public Property Description
        Get
            Return _description
        End Get
        Set(value)
            _description = value
        End Set
    End Property

    Public Shared Function TryParse(enumType As Type) As List(Of EnumObject)
        Dim returned As List(Of EnumObject) = New List(Of EnumObject)
        Try
            Dim enums As EnumContainer = EnumContainer.GetEnumContents(enumType)
            For Each enumIt As EnumItem In enums.Items
                returned.Add(New EnumObject(enumIt.Name, enumIt.Value, enumIt.Description))
            Next
        Catch ex As Exception

        End Try
        Return returned
    End Function

End Class

Imports System.Reflection
Imports System.ComponentModel

Namespace Utilities.EnumUtil
    Public Class EnumUitl
        ''' <summary>
        ''' get the enum Desription by the enum value
        ''' </summary>
        ''' <param name="enumValue"></param>
        ''' <returns></returns>
        Public Shared Function GetEnumDescriptionByEnumValue(ByVal enumValue As [Enum]) As String
            Dim description As String = String.Empty
            Dim info As FieldInfo = enumValue.[GetType]().GetField(enumValue.ToString())
            Dim attributes As DescriptionAttribute() = Nothing
            Try
                attributes = TryCast(info.GetCustomAttributes(GetType(DescriptionAttribute), False), DescriptionAttribute())
            Catch
            End Try
            If attributes IsNot Nothing AndAlso attributes.Length > 0 Then
                Return attributes(0).Description
            Else
                Return enumValue.ToString()
            End If
        End Function


        ''' <summary>
        ''' get the enum list
        ''' </summary>
        ''' <param name="enumType">the type of enum</param>
        ''' <returns>the list of enum</returns>
        Public Shared Function GetEnumItemList(ByVal enumType As Type) As List(Of EnumItem)
            If Not enumType.IsEnum Then
                Return Nothing
            End If
            Dim descriptionType As Type = GetType(DescriptionAttribute)
            Dim infos As FieldInfo() = enumType.GetFields()
            Dim items As New List(Of EnumItem)()

            Dim _value As String = String.Empty
            Dim _description As String = String.Empty
            For Each info As FieldInfo In infos
                If info.IsSpecialName Then
                    Continue For
                End If
                _value = info.GetRawConstantValue().ToString()
                Dim attributes As DescriptionAttribute() = TryCast(info.GetCustomAttributes(descriptionType, True), DescriptionAttribute())
                If attributes IsNot Nothing AndAlso attributes.Length > 0 Then
                    _description = attributes(0).Description
                    items.Add(New EnumItem() With {.Description = _description, .Value = _value})
                End If
            Next
            Return items

        End Function
    End Class
End Namespace

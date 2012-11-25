Imports System.Reflection
Imports System.ComponentModel
Imports System.Runtime.Serialization
Namespace Utilities.EnumUtil
    ''' <summary>
    ''' Provide a container and method to reflect the details of an enum
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()>
    <DataContract()>
    Public Class EnumContainer
        Private pf_typeName As String
        Private pf_items As List(Of EnumItem)


        <DataMember()>
        Public Property TypeName As String
            Get
                Return pf_typeName
            End Get
            Set(value As String)
                pf_typeName = value
            End Set
        End Property

        ''' <summary>
        ''' Enum items
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DataMember()>
        Public Property Items As List(Of EnumItem)
            Get
                If pf_items Is Nothing Then
                    pf_items = New List(Of EnumItem)
                End If
                Return pf_items
            End Get
            Set(value As List(Of EnumItem))
                pf_items = value
            End Set
        End Property



        Public Sub Add(Item As EnumItem)
            If Not Item Is Nothing Then
                Me.Items.Add(Item)
            End If
        End Sub

        ''' <summary>
        ''' Get the enum items
        ''' </summary>
        ''' <param name="enumType">enum type</param>
        ''' <returns>Array of enum types</returns>
        ''' <remarks></remarks>
        Public Shared Function GetEnumTypes(ByVal enumType As Type) As Array

            Return [Enum].GetValues(enumType)

        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="enumType"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetEnumContents(ByVal enumType As Type) As EnumContainer
            Dim fEnuminfoColl As EnumContainer = New EnumContainer

            If Not enumType.GetEnumValues Is Nothing Then

                For Each i As Integer In enumType.GetEnumValues
                    fEnuminfoColl.Add(GetEnumContent(enumType, i))
                Next

            End If
            Return fEnuminfoColl
        End Function


        ''' <summary>
        ''' Get an enum item info according to the input item int value
        ''' </summary>
        ''' <param name="enumType"></param>
        ''' <param name="itemValue"></param>
        ''' <returns>An enumitem instance containing enum details</returns>
        ''' <remarks></remarks>
        Public Shared Function GetEnumContent(ByVal enumType As Type, _
                                                 ByVal itemValue As Integer) As EnumItem
            Dim fEnumInfo As EnumItem = New EnumItem

            Dim fField As FieldInfo = _
                enumType.GetField([Enum].GetName(enumType, itemValue))

            Dim fAttribute As DescriptionAttribute = New DescriptionAttribute

            Try
                fAttribute = _
               DirectCast(fField.GetCustomAttributes(GetType(DescriptionAttribute), False)(0),  _
                   DescriptionAttribute)
            Catch ex As Exception

            End Try


            If Not fAttribute Is Nothing Then
                fEnumInfo.Description = fAttribute.Description
            End If

            fEnumInfo.Name = [Enum].GetName(enumType, itemValue)
            fEnumInfo.Value = itemValue
            Return fEnumInfo
        End Function

    End Class
End Namespace


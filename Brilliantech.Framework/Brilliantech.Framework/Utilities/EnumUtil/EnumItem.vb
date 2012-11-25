Imports System.Runtime.Serialization

Namespace Utilities.EnumUtil
    ''' <summary>
    ''' Represent an Enum
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()>
    <DataContract()> _
    Public Class EnumItem
        Private pf_description As String
        Private pf_name As String
        Private pf_value As String

        ''' <summary>
        ''' The description name of the attribute description of a enum item
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DataMember()>
        Public Property Description As String
            Get
                Return pf_description
            End Get
            Set(value As String)
                pf_description = value
            End Set
        End Property


        ''' <summary>
        ''' Name of an enum item
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DataMember()>
        Public Property Name As String
            Get
                Return pf_name
            End Get
            Set(value As String)
                pf_name = value
            End Set
        End Property


        ''' <summary>
        ''' Int value of an enum item
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <DataMember()>
        Public Property Value As Integer
            Get
                Return pf_value
            End Get
            Set(value As Integer)
                pf_value = value
            End Set
        End Property

    End Class
End Namespace
Imports Brilliantech.Framework.Utilities.EnumUtil

''' <summary>
''' 用于将枚举类型的Key,value和描述属性通过属性形式方便调用
''' </summary>
''' <remarks></remarks>
<DataContract()>
Public Class EnumObject 
    Private _key As String
    Private _value As String
    Private _description As String

    ''' <summary>
    ''' 初始化函数
    ''' </summary>
    ''' <param name="enumkey">枚举类型的键</param>
    ''' <param name="val">枚举类型的值</param>
    ''' <param name="desc">枚举类型的描述</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal enumkey As String, ByVal val As String, ByVal desc As String)
        _key = enumkey
        _value = val
        _description = desc
    End Sub

    ''' <summary>
    ''' 代表枚举类型的键
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property EnumKey
        Get
            Return _key
        End Get
        Set(ByVal value)
            _key = value
        End Set
    End Property

    ''' <summary>
    ''' 代表枚举类型的值
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property EnumValue
        Get
            Return _value
        End Get
        Set(ByVal value)
            _value = value
        End Set
    End Property

    ''' <summary>
    ''' 代表枚举类型Description属性的值
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property Description
        Get
            Return _description
        End Get
        Set(ByVal value)
            _description = value
        End Set
    End Property


    ''' <summary>
    ''' 将一个枚举类型转化为EnumObject类型的列表，每个EnumObject对象代表输入枚举类型的一个值项
    ''' </summary>
    ''' <param name="enumType">枚举类型</param>
    ''' <returns>EnumObject实例的列表</returns>
    ''' <remarks>如果转换失败则返回空列表</remarks>
    Public Shared Function TryParse(ByVal enumType As Type) As List(Of EnumObject)
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

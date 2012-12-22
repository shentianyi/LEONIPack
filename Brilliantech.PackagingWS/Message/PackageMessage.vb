Imports Brilliantech.Framework.WCF.Data
Imports Brilliantech.Packaging.Data
''' <summary>
''' 包装对象信息
''' </summary>
''' <remarks></remarks>
<DataContract()>
Public Class PackageMessage
    Inherits ServiceMessage
    Private pf_package As SinglePackage
    ''' <summary>
    ''' 包装箱对象
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DataMember()>
Public Property Package As SinglePackage
        Get
            Return pf_package
        End Get
        Set(ByVal value As SinglePackage)
            pf_package = value
        End Set
    End Property
End Class

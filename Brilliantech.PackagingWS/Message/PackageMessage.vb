Imports Brilliantech.Framework.WCF.Data
Imports Brilliantech.Packaging.Data
<DataContract()>
Public Class PackageMessage
    Inherits ServiceMessage
    Private pf_package As SinglePackage
    <DataMember()>
    Public Property Package As SinglePackage
        Get
            Return pf_package
        End Get
        Set(value As SinglePackage)
            pf_package = value
        End Set
    End Property
End Class

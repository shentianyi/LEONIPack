Imports Brilliantech.Packaging.Data
Public Interface ISinglePackageRepo
    Inherits IBaseRepo(Of SinglePackage)

    Sub AddItem(packageID As String, newItem As PackageItem)


End Interface

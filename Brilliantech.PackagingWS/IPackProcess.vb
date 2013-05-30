Imports Brilliantech.Packaging.Data
Imports Brilliantech.Framework.WCF.Data

''' <summary>
''' 处理包装流程的主服务的接口文件
''' </summary>
''' <remarks></remarks>
<ServiceContract()>
Public Interface IPackProcess
    <OperationContract()>
    Function FindByID(id As String) As PackageMessage
    <OperationContract()>
    Function FindByItem(id As String) As PackageMessage

    <OperationContract()>
    Function CountItem(id As String) As Integer

    <OperationContract()>
    Function Create(package As SinglePackage, mode As PackagingType) As PackageMessage

    <OperationContract()>
    Function Scrap(packageID As String) As ServiceMessage

    <OperationContract()>
    Function AddItem(packageID As String, barcodeStr As String) As ServiceMessage

    <OperationContract()>
    Function Recover(packageID As String) As ServiceMessage


    <OperationContract()>
    Function BeginProcess(packageID As String, wrkStnID As String, mode As PackagingType) As PackageMessage

    <OperationContract()>
    Function EndProcess(packageID As String) As ServiceMessage

    <OperationContract()>
    Function SuspendProcess(packageID As String) As ServiceMessage

    <OperationContract()>
    Function GetUnfull(packageId As String, wrkStnId As String) As String

    <OperationContract()>
    Function CancelPackaging(packageId As String) As ServiceMessage

    <OperationContract()>
    Function GetPackageInfos(ByVal pid As String, _
                                      ByVal wrkstnr As String, _
                                      ByVal tnr As String, _
                                      ByVal partnr As String, _
                                      ByVal status As Integer, _
                                      ByVal fromDate As DateTime, _
                                      ByVal toDate As DateTime) As List(Of FullPackageInfo)

    <OperationContract()>
    Function GetItemsByPackageId(ByVal pId As String) As List(Of PackageItem)

    <OperationContract()>
    Function GetPackageStatus() As List(Of EnumObject)

    <OperationContract()>
    Function GetValidateItemsByPackageId(ByVal packageId As String) As List(Of CustomValidate)

    <OperationContract()>
    Function GetValidateItemsByPartAndWrkst(ByVal partNr As String, ByVal wrkstnr As String) As List(Of CustomValidate)

    <OperationContract()>
    Function WorkstationExists(ByVal wrkstnr As String) As Boolean

    <OperationContract()>
    Function ModifyPackage(ByVal pack As SinglePackage) As ServiceMessage

End Interface

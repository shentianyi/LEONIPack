Imports Brilliantech.Packaging.Data

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports Brilliantech.Packaging.WS



'''<summary>
'''This is a test class for SinglePackageRepoTest and is intended
'''to contain all SinglePackageRepoTest Unit Tests
'''</summary>
<TestClass()> _
Public Class SinglePackageRepoTest


    Private testContextInstance As TestContext

    '''<summary>
    '''Gets or sets the test context which provides
    '''information about and functionality for the current test run.
    '''</summary>
    Public Property TestContext() As TestContext
        Get
            Return testContextInstance
        End Get
        Set(value As TestContext)
            testContextInstance = Value
        End Set
    End Property

#Region "Additional test attributes"
    '
    'You can use the following additional attributes as you write your tests:
    '
    'Use ClassInitialize to run code before running the first test in the class
    '<ClassInitialize()>  _
    'Public Shared Sub MyClassInitialize(ByVal testContext As TestContext)
    'End Sub
    '
    'Use ClassCleanup to run code after all tests in a class have run
    '<ClassCleanup()>  _
    'Public Shared Sub MyClassCleanup()
    'End Sub
    '
    'Use TestInitialize to run code before running each test
    '<TestInitialize()>  _
    'Public Sub MyTestInitialize()
    'End Sub
    '
    'Use TestCleanup to run code after each test has run
    '<TestCleanup()>  _
    'Public Sub MyTestCleanup()
    'End Sub
    '
#End Region


    '''<summary>
    '''A test for GetByItemId
    '''</summary>
    <TestMethod()> _
    Public Sub GetByItemIdTest()
        Dim context As PackagingDataDataContext = New PackagingDataDataContext("data source=E:\Project\莱尼包装\Package代码\Brilliantech.PackagingWS\Brilliantech.PackagingWS\data\package.sdf") ' TODO: Initialize to an appropriate value
        Dim target As SinglePackageRepo = New SinglePackageRepo(context) ' TODO: Initialize to an appropriate value
        Dim itemId As String = "91G207402" ' TODO: Initialize to an appropriate value
        Dim expected As SinglePackage = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As SinglePackage
        actual = target.GetByItemId(itemId)
        Assert.IsNotNull(actual)

    End Sub


    <TestMethod()> _
    Public Sub TestModify()

    End Sub

    '''<summary>
    '''Modify 的测试
    '''</summary>
    <TestMethod()> _
    Public Sub ModifyTest()
        Dim context As PackagingDataDataContext = New PackagingDataDataContext("Data Source=192.168.0.253\dev001;Initial Catalog=Leoni_Packaging_prod;User ID=sa;Password=123456@") ' TODO: 初始化为适当的值
        Dim target As IBaseRepo(Of SinglePackage) = New SinglePackageRepo(context) ' TODO: 初始化为适当的值
        Dim entity As SinglePackage = New SinglePackage With {.capa = 46, .containerID = "MBSTP", .packageID = "TMB001", .partNr = "91G004702", .planedDate = Now, .rowguid = New Guid("{81f9b732-2e87-e211-93f2-000c2901ecf9}"), .status = 99, .wrkstnID = "Minor-TSK-041"}
        Try
            target.Modify(entity)
            Assert.Fail()
        Catch ex As Exception

        End Try


    End Sub
End Class

Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports TestProject
Imports Brilliantech.Packaging.WS



'''<summary>
'''This is a test class for PackageUtilTest and is intended
'''to contain all PackageUtilTest Unit Tests
'''</summary>
<TestClass()> _
Public Class PackageUtilTest


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
    '''A test for FindByItem
    '''</summary>
    <TestMethod()> _
    Public Sub FindByItemTest()
        Dim target As PackageUtil = New PackageUtil() ' TODO: Initialize to an appropriate value
        Dim itemTnr As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As PackageMessage = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As PackageMessage
        actual = target.FindByItem(itemTnr)
        Assert.IsNotNull(actual)
    End Sub
End Class

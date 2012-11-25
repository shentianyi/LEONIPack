Imports TECIT.TFORMer

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports ReportGenConnector



'''<summary>
'''This is a test class for TECITLicenseTest and is intended
'''to contain all TECITLicenseTest Unit Tests
'''</summary>
<TestClass()> _
Public Class TECITLicenseTest


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
    '''A test for TECITLicense Constructor
    '''</summary>
    <TestMethod()> _
    Public Sub TECITLicenseConstructorTest()
        Dim licenseeName As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim key As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim number As Integer = 0 ' TODO: Initialize to an appropriate value
        Dim licenseKind As LicenseKind = New LicenseKind() ' TODO: Initialize to an appropriate value
        Dim target As TECITLicense = New TECITLicense(licenseeName, key, number, licenseKind)
        Assert.Inconclusive("TODO: Implement code to verify target")
    End Sub

    '''<summary>
    '''A test for Register
    '''</summary>
    <TestMethod()> _
    Public Sub RegisterTest()
        Dim licenseeName As String = "1" ' TODO: Initialize to an appropriate value
        Dim key As String = "2" ' TODO: Initialize to an appropriate value
        Dim number As Integer = 10000 ' TODO: Initialize to an appropriate value
        Dim licenseKind As LicenseKind = licenseKind.Developer  ' TODO: Initialize to an appropriate value
        Dim target As TECITLicense = New TECITLicense(licenseeName, key, number, licenseKind) ' TODO: Initialize to an appropriate value
        Try
            target.Register()

        Catch ex As Exception
            Assert.IsInstanceOfType(ex, GetType(ReportGenLicenseException))
        End Try
    End Sub
End Class

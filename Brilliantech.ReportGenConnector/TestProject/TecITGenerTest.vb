Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports ReportGenConnector



'''<summary>
'''This is a test class for TecITGenerTest and is intended
'''to contain all TecITGenerTest Unit Tests
'''</summary>
<TestClass()> _
Public Class TecITGenerTest


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
    '''A test for TecITGener Constructor
    '''</summary>
    <TestMethod()> _
    Public Sub TecITGenerConstructorTest()
        Dim license As TECITLicense = Nothing ' TODO: Initialize to an appropriate value
        Dim target As TecITGener = New TecITGener(license)
        Assert.Inconclusive("TODO: Implement code to verify target")
    End Sub

    '''<summary>
    '''A test for Print
    '''</summary>
    <TestMethod()> _
    Public Sub PrintTest()
        Dim license As TECITLicense = New TECITLicense("1", "1", 1, TECIT.TFORMer.LicenseKind.Developer) ' TODO: Initialize to an appropriate value
        Dim target As IReportGen = New TecITGener(license) ' TODO: Initialize to an appropriate value
        Dim records As RecordSet = New RecordSet  ' TODO: Initialize to an appropriate value
        Dim config As ReportGenConfig = New ReportGenConfig With {.Printer = "Adobe PDF", _
                                                                  .Template = _
                                                                  "C:\Users\lenovo\Desktop\TecITTest\TecITTest\Label\1.tff"}
        'HP Officejet 6500 E709n Series-WLAN
        Dim rec As RecordData = New RecordData
        rec.Add("HEADER_TNR", "T-000001")
        rec.Add("header_knr", "42001123")
        records.Add(rec)

        Dim _record As RecordData
        _record = New RecordData

        _record.Add("nr1", "This is the value of the datafield1")
        _record.Add("nr2", "set1")
        _record.Add("nr3", "set2")

        Dim _record1 As RecordData = New RecordData
        _record1.Add("nr1", "This is the value of the datafield2")
        _record1.Add("nr2", "set5")
        _record1.Add("nr3", "set6")

        records.Add(_record)
        records.Add(_record1)
        target.Print(records, config)

    End Sub
End Class

Imports System

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports Brilliantech.Framework.Utilities.EnumUtil

Imports DataCollection



'''<summary>
'''This is a test class for EnumContainerTest and is intended
'''to contain all EnumContainerTest Unit Tests
'''</summary>
<TestClass()> _
Public Class EnumContainerTest


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
    '''A test for GetEnumContent
    '''</summary>
    <TestMethod()> _
    Public Sub GetEnumContentTest()
        Dim enumType As Type = GetType(TransmissionStatusEnum) ' TODO: Initialize to an appropriate value
        Dim itemValue As Integer = CType(TransmissionStatusEnum.normal, Integer) ' TODO: Initialize to an appropriate value
        Dim expected As String = "正常" ' TODO: Initialize to an appropriate value
        Dim actual As String
        actual = EnumContainer.GetEnumContent(enumType, itemValue).Description
        Assert.AreEqual(expected, actual)

    End Sub
End Class

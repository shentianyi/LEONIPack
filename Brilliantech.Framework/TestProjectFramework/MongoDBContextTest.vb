'Imports System.Collections.Generic

'Imports MongoDB.Driver

'Imports Microsoft.VisualStudio.TestTools.UnitTesting

'Imports Brilliantech.Framework.Utilities.DBUtil.MongoDB



' '''<summary>
' '''This is a test class for MongoDBContextTest and is intended
' '''to contain all MongoDBContextTest Unit Tests
' '''</summary>
'<TestClass()> _
'Public Class MongoDBContextTest


'    Private testContextInstance As TestContext

'    '''<summary>
'    '''Gets or sets the test context which provides
'    '''information about and functionality for the current test run.
'    '''</summary>
'    Public Property TestContext() As TestContext
'        Get
'            Return testContextInstance
'        End Get
'        Set(value As TestContext)
'            testContextInstance = Value
'        End Set
'    End Property

'#Region "Additional test attributes"
'    '
'    'You can use the following additional attributes as you write your tests:
'    '
'    'Use ClassInitialize to run code before running the first test in the class
'    '<ClassInitialize()>  _
'    'Public Shared Sub MyClassInitialize(ByVal testContext As TestContext)
'    'End Sub
'    '
'    'Use ClassCleanup to run code after all tests in a class have run
'    '<ClassCleanup()>  _
'    'Public Shared Sub MyClassCleanup()
'    'End Sub
'    '
'    'Use TestInitialize to run code before running each test
'    '<TestInitialize()>  _
'    'Public Sub MyTestInitialize()
'    'End Sub
'    '
'    'Use TestCleanup to run code after each test has run
'    '<TestCleanup()>  _
'    'Public Sub MyTestCleanup()
'    'End Sub
'    '
'#End Region


'    ' '''<summary>
'    ' '''A test for MongoDBContext Constructor
'    ' '''</summary>
'    '<TestMethod()> _
'    'Public Sub MongoDBContextConstructorTest()
'    '    Dim connStr As String = String.Empty ' TODO: Initialize to an appropriate value
'    '    Dim db As String = String.Empty ' TODO: Initialize to an appropriate value
'    '    Dim target As MongoDBContext = New MongoDBContext(connStr, db)
'    '    Assert.IsTrue(target.Collection.Count > 0)
'    'End Sub

'    '''<summary>
'    '''A test for MongoDBContext Constructor
'    '''</summary>
'    <TestMethod()> _
'    Public Sub MongoDBContextConstructorTest1()
'        Dim target As MongoDBContext = New MongoDBContext()
'        Assert.Inconclusive("TODO: Implement code to verify target")
'    End Sub

'    ' '''<summary>
'    ' '''A test for Collection
'    ' '''</summary>
'    '<TestMethod()> _
'    'Public Sub CollectionTest()
'    '    Dim target As MongoDBContext = New MongoDBContext() ' TODO: Initialize to an appropriate value
'    '    Dim actual As Dictionary(Of String, MongoCollection)
'    '    actual = target.Collection
'    '    Assert.Inconclusive("Verify the correctness of this test method.")
'    'End Sub

'    '''<summary>
'    '''A test for ConnStr
'    '''</summary>
'    <TestMethod()> _
'    Public Sub ConnStrTest()
'        Dim target As MongoDBContext = New MongoDBContext() ' TODO: Initialize to an appropriate value
'        Dim actual As String
'        actual = target.ConnStr
'        Assert.Inconclusive("Verify the correctness of this test method.")
'    End Sub

'    '''<summary>
'    '''A test for DB
'    '''</summary>
'    <TestMethod()> _
'    Public Sub DBTest()
'        Dim target As MongoDBContext = New MongoDBContext() ' TODO: Initialize to an appropriate value
'        Dim actual As MongoDatabase
'        actual = target.DB
'        Assert.Inconclusive("Verify the correctness of this test method.")
'    End Sub

'    '''<summary>
'    '''A test for MongoInstance
'    '''</summary>
'    <TestMethod()> _
'    Public Sub MongoInstanceTest()
'        Dim target As MongoDBContext = New MongoDBContext() ' TODO: Initialize to an appropriate value
'        Dim actual As MongoServer
'        actual = target.MongoInstance
'        Assert.Inconclusive("Verify the correctness of this test method.")
'    End Sub
'End Class

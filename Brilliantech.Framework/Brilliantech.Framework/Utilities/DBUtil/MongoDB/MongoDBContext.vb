Imports MongoDB.Bson
Imports MongoDB.Driver
Namespace Utilities.DBUtil.MongoDB
    Public Class MongoDBContext
        Protected _mongo As MongoServer

        Protected _db As MongoDatabase

        Protected _dbName As String

        Protected ReadOnly _connStr As String

        Protected ReadOnly _mapper As MongoTableMapper

        'Protected _collectionName As String
        'Protected _coll As MongoCollection
        'Protected Shared ReadOnly defaultConnStr As String = "mongodb://localhost"
        'Protected Shared ReadOnly defaultDBName As String = "mainDB"
        ''Protected Shared ReadOnly defaultCollection As String = "mainCollection"

        'Public Sub New(ByVal connStr As String, ByVal dbName As String, ByVal collectionName As String)
        '    If String.IsNullOrEmpty(connStr) Or String.IsNullOrEmpty(dbName) Then
        '        Throw New ArgumentNullException
        '    End If
        '    _connStr = connStr
        '    _dbName = dbName
        '    _collectionName = collectionName
        'End Sub


        Public Sub New(ByVal connStr As String, ByVal dbName As String, ByVal mapper As MongoTableMapper)
            If String.IsNullOrEmpty(connStr) Or String.IsNullOrEmpty(dbName) Then
                Throw New ArgumentNullException
            End If
            _connStr = connStr
            _dbName = dbName
            _mapper = mapper
            '_collectionName = mapper.GetTableName(GetType(T))
        End Sub


        'Public Sub New()
        '    Me.New(defaultConnStr, defaultDBName, defaultCollection)
        'End Sub


        'Public Shared Function GetAllCollections(ByVal connStr As String, ByVal db As String) As Dictionary(Of String, MongoCollection)
        '    If String.IsNullOrEmpty(connStr) Or String.IsNullOrEmpty(db) Then
        '        Throw New ArgumentNullException
        '    End If
        '    Dim server As MongoServer = MongoServer.Create(connStr)
        '    Dim myDB As MongoDatabase = server.GetDatabase(db)
        '    Dim collectionsHash As Dictionary(Of String, MongoCollection) = New Dictionary(Of String, MongoCollection)
        '    Dim collections() As String = myDB.GetCollectionNames.ToArray
        '    If collections IsNot Nothing Then
        '        If collections.Length > 0 Then
        '            For Each collName As String In collections
        '                collectionsHash.Add(collName, myDB.GetCollection(collName))
        '            Next
        '        End If
        '    End If
        '    Return collectionsHash
        'End Function


        Public ReadOnly Property ConnStr As String
            Get
                Return _connStr
            End Get
        End Property

        Public ReadOnly Property MongoInstance As MongoServer
            Get
                If _mongo Is Nothing Then
                    _mongo = MongoServer.Create(ConnStr)
                End If

                Return _mongo
            End Get
        End Property

        Public ReadOnly Property DB As MongoDatabase
            Get
                If _db Is Nothing Then
                    _db = MongoInstance.GetDatabase(_dbName)
                End If
                Return _db
            End Get
        End Property

        Public Function GetCollection(ByVal name As String) As MongoCollection
            If String.IsNullOrEmpty(name) Then
                Throw New ArgumentNullException
            Else
                Return DB.GetCollection(name)
            End If
        End Function

        Public Function GetCollection(ByVal type As Type, ByVal flag As Boolean)
            If type Is Nothing Then
                Throw New ArgumentNullException
            Else
                Return DB.GetCollection(_mapper.GetTableName(type))
            End If
        End Function

        'Public ReadOnly Property Collection As MongoCollection
        '    Get
        '        If _coll Is Nothing Then
        '            _coll = DB.GetCollection(_collectionName)
        '        End If
        '        Return _coll
        '    End Get
        'End Property
    End Class
End Namespace


Imports MongoDB.Driver.Builders
Imports MongoDB.Driver
Imports MongoDB.Bson
Namespace Utilities.DBUtil.MongoDB
    Public MustInherit Class MongoRepository(Of T As Class)
        Implements IMongoRepository(Of T)


        Private dbContext As MongoDBContext
        Private pf_currentCollection As MongoCollection

        Public ReadOnly Property CurrentCollection As MongoCollection
            Get

                If pf_currentCollection Is Nothing Then
                    pf_currentCollection = dbContext.GetCollection(GetType(T), True)
                End If
                Return pf_currentCollection
            End Get
        End Property

        Public Sub New(context As MongoDBContext)
            dbContext = context
            Mapping()
        End Sub

        Public Overridable Sub Add(entity As T) Implements IMongoRepository(Of T).Add
            CurrentCollection.Insert(Of T)(entity)
        End Sub

        Public Overridable Function GetAll() As System.Collections.Generic.IEnumerable(Of T) Implements IMongoRepository(Of T).GetAll
            Return CurrentCollection.FindAllAs(Of T)()
        End Function

        Public Overridable Function GetByID(ID As String) As T Implements IMongoRepository(Of T).GetByID
            Return CurrentCollection.FindOneByIdAs(Of T)(BsonString.Create(ID))
        End Function

        Public Overridable Sub Modify(entity As T) Implements IMongoRepository(Of T).Modify
            CurrentCollection.Save(entity)
        End Sub

        Public Overridable Function Query(queryObj As Object) As System.Collections.Generic.IEnumerable(Of T) Implements IMongoRepository(Of T).Query
            Return CurrentCollection.FindAs(Of T)(queryObj)
        End Function

        Public Overridable Sub Remove(id As String) Implements IMongoRepository(Of T).Remove
            CurrentCollection.Remove(Builders.Query.EQ("_id", id))
        End Sub

        Public MustOverride Sub Mapping() Implements IMongoRepository(Of T).Mapping

        Public Function Exist(id As String) As Boolean Implements IGenericRepository(Of T).Exist
            If CurrentCollection.Count(Builders.Query.EQ("_id", id)) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function
    End Class
End Namespace
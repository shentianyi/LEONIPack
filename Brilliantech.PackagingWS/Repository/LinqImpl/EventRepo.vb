Imports Brilliantech.Packaging.Data
Public Class EventRepo
    Inherits RepoBase
    Implements IEventRepo





    Public Sub New(context As PackagingDataDataContext)
        MyBase.New(context)
    End Sub

    Public Sub Delete(id As String) Implements IBaseRepo(Of Data.PackagingEvent).Delete
        Throw New MethodNotImplementException
    End Sub

    Public Function GetAll() As System.Collections.Generic.IEnumerable(Of Data.PackagingEvent) Implements IBaseRepo(Of Data.PackagingEvent).GetAll
        Return _context.PackagingEvents
    End Function

    Public Function GetByID(id As String) As Data.PackagingEvent Implements IBaseRepo(Of Data.PackagingEvent).GetByID
        Return _context.PackagingEvents.Single(Function(c) c.eventID = id)
    End Function

    Public Sub Insert(entity As Data.PackagingEvent) Implements IBaseRepo(Of Data.PackagingEvent).Insert
        Throw New MethodNotImplementException
    End Sub

    Public Sub Modify(entity As Data.PackagingEvent) Implements IBaseRepo(Of Data.PackagingEvent).Modify
        Throw New MethodNotImplementException
    End Sub

    Public Function Exists(id As String) As Boolean Implements IBaseRepo(Of Data.PackagingEvent).Exists
        Throw New MethodNotImplementException
    End Function
End Class

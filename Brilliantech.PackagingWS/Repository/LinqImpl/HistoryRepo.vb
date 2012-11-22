Imports Brilliantech.Packaging.Data
Public Class HistoryRepo
    Inherits RepoBase
    Implements IHistoryRepo




    Public Sub New(context As PackagingDataDataContext)
        MyBase.New(context)
    End Sub


    Public Sub Delete(id As String) Implements IBaseRepo(Of Data.History).Delete
        Throw New MethodNotImplementException
    End Sub

    Public Function GetAll() As System.Collections.Generic.IEnumerable(Of Data.History) Implements IBaseRepo(Of Data.History).GetAll
        Throw New MethodNotImplementException
    End Function

    Public Function GetByID(id As String) As Data.History Implements IBaseRepo(Of Data.History).GetByID
        Throw New MethodNotImplementException
    End Function

    Public Sub Insert(entity As Data.History) Implements IBaseRepo(Of Data.History).Insert
        Throw New MethodNotImplementException
    End Sub

    Public Sub Modify(entity As Data.History) Implements IBaseRepo(Of Data.History).Modify
        Throw New MethodNotImplementException
    End Sub

    Public Function Exists(id As String) As Boolean Implements IBaseRepo(Of Data.History).Exists
        Throw New MethodNotImplementException
    End Function
End Class

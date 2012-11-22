Imports Brilliantech.Packaging.Data
Public Class ContainerTypeRepo
    Inherits RepoBase
    Implements IContainerTypeRepo



    Public Sub New(context As PackagingDataDataContext)
        MyBase.New(context)
    End Sub

    Public Sub Delete(id As String) Implements IBaseRepo(Of ContainerType).Delete
        Throw New MethodNotImplementException
    End Sub


    Public Function GetAll() As System.Collections.Generic.IEnumerable(Of Data.ContainerType) Implements IBaseRepo(Of Data.ContainerType).GetAll
        Return _context.ContainerTypes

    End Function

    Public Function GetByID(id As String) As Data.ContainerType Implements IBaseRepo(Of Data.ContainerType).GetByID
        Return _context.ContainerTypes.Single(Function(c) c.containerID = id)
    End Function

    Public Sub Insert(entity As Data.ContainerType) Implements IBaseRepo(Of Data.ContainerType).Insert
        Throw New MethodNotImplementException
    End Sub

    Public Sub Modify(entity As Data.ContainerType) Implements IBaseRepo(Of Data.ContainerType).Modify
        Throw New MethodNotImplementException
    End Sub

    Public Function Exists(id As String) As Boolean Implements IBaseRepo(Of Data.ContainerType).Exists
        Throw New MethodNotImplementException
    End Function
End Class

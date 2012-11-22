Public Interface IBaseRepo(Of T)
    Function GetByID(id As String) As T
    Function GetAll() As IEnumerable(Of T)
    Sub Insert(entity As T)
    Sub Delete(id As String)
    Sub Modify(entity As T)
    Function Exists(id As String) As Boolean
End Interface

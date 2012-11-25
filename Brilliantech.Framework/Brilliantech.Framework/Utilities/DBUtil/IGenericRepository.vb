Namespace Utilities.DBUtil
    Public Interface IGenericRepository(Of T As Class)
        Function GetByID(ByVal ID As String) As T
        Function GetAll() As IEnumerable(Of T)
        Sub Add(ByVal entity As T)
        Sub Remove(ByVal id As String)
        Sub Modify(ByVal entity As T)
        Function Query(ByVal queryObj As Object) As IEnumerable(Of T)
        Function Exist(ByVal id As String) As Boolean
    End Interface
End Namespace

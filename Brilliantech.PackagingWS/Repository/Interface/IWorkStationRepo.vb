Imports Brilliantech.Packaging.Data
Public Interface IWorkStationRepo
    Inherits IBaseRepo(Of WorkStation)

    Function GetByProdLineID(prodLineID As String) As IEnumerable(Of WorkStation)

    Function GetByProjectId(projectId As String) As IEnumerable(Of WorkStation)

End Interface

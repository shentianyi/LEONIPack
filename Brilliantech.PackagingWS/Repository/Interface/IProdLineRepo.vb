Imports Brilliantech.Packaging.Data
Public Interface IProdLineRepo
    Inherits IBaseRepo(Of ProdLine)

    Function GetByProjectID(projId As String) As IEnumerable(Of ProdLine)


End Interface

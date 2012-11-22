Imports Brilliantech.Packaging.Data
Public Interface IPartRepository
    Inherits IBaseRepo(Of Part)

    Function GetByProjectID(projID As String) As IEnumerable(Of Part)

End Interface

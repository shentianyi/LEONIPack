
Imports Brilliantech.Packaging.Data
Public Interface IEntityPartRelRepo
    Inherits IBaseRepo(Of PartEntityRel)
    Function GetByPartNumber(ByVal partNr As String) As IEnumerable(Of PartEntityRel)
End Interface

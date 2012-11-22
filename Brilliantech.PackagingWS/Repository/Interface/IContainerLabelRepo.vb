Imports Brilliantech.Packaging.Data
Public Interface IContainerLabelRepo
    Inherits IBaseRepo(Of PartContainerLabel)
    Function GetByPartNumber(partNr As String) As IEnumerable(Of PartContainerLabel)
End Interface

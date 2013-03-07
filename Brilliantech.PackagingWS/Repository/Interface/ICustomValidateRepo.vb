Imports Brilliantech.Packaging.Data
Public Interface ICustomValidateRepo
    Function GetByPartAndWrkstnr(ByVal partnr As String, ByVal wrkstnr As String) As List(Of CustomValidate)
End Interface

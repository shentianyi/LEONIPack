Imports Brilliantech.Packaging.Data
Public Class CustomValidateRepo
    Inherits RepoBase
    Implements ICustomValidateRepo

    Public Sub New(ByVal context As PackagingDataDataContext)
        MyBase.New(context)
    End Sub

    Public Function GetByPartAndWrkstnr(ByVal partnr As String, ByVal wrkstnr As String) As System.Collections.Generic.List(Of Data.CustomValidate) Implements ICustomValidateRepo.GetByPartAndWrkstnr
        Dim result As IQueryable(Of CustomValidate) = (From validates _
            In _context.CustomValidates _
            Where String.Compare(partnr, validates.PartNr, True) = 0 _
            And String.Compare(wrkstnr, validates.WrkstNr) = 0
            Select validates)
        If result IsNot Nothing Then
            Return result.ToList
        Else
            Return New List(Of CustomValidate)
        End If
    End Function
End Class

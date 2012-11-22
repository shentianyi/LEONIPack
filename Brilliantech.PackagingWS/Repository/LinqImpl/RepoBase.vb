Imports Brilliantech.Packaging.Data
Public Class RepoBase
    Protected _context As PackagingDataDataContext

    Public Sub New(context As PackagingDataDataContext)
        If context Is Nothing Then
            Throw New ArgumentNullException("context")
        Else
            _context = context
        End If
    End Sub
End Class

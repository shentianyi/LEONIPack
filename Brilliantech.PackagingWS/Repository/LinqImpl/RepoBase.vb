Imports Brilliantech.Packaging.Data
''' <summary>
''' 记录仓库基类
''' </summary>
''' <remarks></remarks>
Public Class RepoBase
    Protected _context As PackagingDataDataContext

    Public Sub New(ByVal context As PackagingDataDataContext)
        If context Is Nothing Then
            Throw New ArgumentNullException("context")
        Else
            _context = context
        End If
    End Sub
End Class

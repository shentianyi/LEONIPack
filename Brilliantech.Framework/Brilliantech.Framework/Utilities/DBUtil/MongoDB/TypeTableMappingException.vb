Namespace Utilities.DBUtil.MongoDB


    Public Class TypeTableMappingException
        Inherits Exception

        Public Sub New()
            MyBase.New("没有找到类型匹配的集合名")
        End Sub

    End Class
End Namespace
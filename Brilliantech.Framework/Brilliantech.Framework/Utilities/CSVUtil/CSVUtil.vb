Imports System.IO
Imports Brilliantech.Framework.Messages
Imports System.Text

Public Class CSVUtil
    Public Shared Sub GenCSVFile(ByVal ds As CSVDataSet, ByVal filename As String)
        Dim sbs As StringBuilder = New StringBuilder
        Dim sb As StringBuilder = New StringBuilder
        For Each record As CSVDataRecord In ds.DataRecord
            sb.Clear()
            For Each str As String In record.DataRecord
                sb.AppendFormat((str + ";"))
            Next
            sbs.AppendLine(sb.ToString.TrimEnd(Microsoft.VisualBasic.ChrW(59)))
        Next
        File.WriteAllText(filename, sbs.ToString)
    End Sub
End Class
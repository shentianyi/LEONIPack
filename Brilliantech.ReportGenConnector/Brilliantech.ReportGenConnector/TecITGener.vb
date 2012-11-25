
Public Class TecITGener
    Implements IReportGen

    Public Sub New()

    End Sub

    Public Sub Print(records As RecordSet, config As ReportGenConfig) Implements IReportGen.Print
        If records Is Nothing Or config Is Nothing Then
            Throw New ArgumentNullException("打印参数或打印数据不能为空")
        End If

        Try
            TECITLicense.Register()
        Catch ex As Exception
            Throw New ReportGenLicenseException(ex)
        End Try

        Dim job As TECIT.TFORMer.Job
        Dim jobdata As TECIT.TFORMer.JobDataRecordSet
        job = New TECIT.TFORMer.Job
        jobdata = New TECIT.TFORMer.JobDataRecordSet
        job.JobData = jobdata
        job.RepositoryName = config.Template
        job.PrinterName = config.Printer
        job.NumberOfCopies = config.NumberOfCopies

        For i = 0 To records.Count - 1
            Dim rec As TECIT.TFORMer.Record = New TECIT.TFORMer.Record
            For Each ent As KeyValuePair(Of String, String) In records(i)
                rec.Data.Add(ent.Key, ent.Value)
            Next
            jobdata.Records.Add(rec)
        Next

        Try

            job.Print()
            job.Dispose()
        Catch ex As Exception
            Throw New ReportPrintException(ex)
        End Try
    End Sub


End Class

Imports System.Runtime.Serialization
''' <summary>
''' 
''' </summary>
''' <remarks></remarks>
<DataContract()>
Public Class PrintTask
    Private pf_dataset As RecordSet
    Private pf_config As ReportGenConfig

    ''' <summary>
    ''' 打印数据集合
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property DataSet As RecordSet
        Get
            If pf_dataset Is Nothing Then
                pf_dataset = New RecordSet
            End If
            Return pf_dataset
        End Get

        Set(value As RecordSet)
            pf_dataset = value
        End Set
    End Property

    ''' <summary>
    ''' 打印机本参数
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property Config As ReportGenConfig
        Get
            If pf_config Is Nothing Then
                pf_config = New ReportGenConfig
            End If
            Return pf_config
        End Get
        Set(value As ReportGenConfig)
            pf_config = value
        End Set
    End Property

End Class

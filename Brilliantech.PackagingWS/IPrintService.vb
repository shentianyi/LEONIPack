Imports Brilliantech.ReportGenConnector
''' <summary>
''' 处理标签输出功能的主服务接口
''' </summary>
''' <remarks></remarks>
<ServiceContract()>
Public Interface IPrintService
    <OperationContract()>
    Function PrintCloseLabel(packageID As String) As PrintDataMessage
    <OperationContract()>
    Function PrintUnfullLabel(packageID As String) As PrintDataMessage
    <OperationContract()>
    Function PrintOpenLabel(packageID As String) As PrintDataMessage


End Interface

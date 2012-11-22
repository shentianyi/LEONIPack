Imports Brilliantech.ReportGenConnector
<ServiceContract()>
Public Interface IPrintService
    <OperationContract()>
    Function PrintCloseLabel(packageID As String) As PrintDataMessage
    <OperationContract()>
    Function PrintUnfullLabel(packageID As String) As PrintDataMessage
    <OperationContract()>
    Function PrintOpenLabel(packageID As String) As PrintDataMessage


End Interface

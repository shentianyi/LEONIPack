Imports System.Runtime.Serialization
Namespace WCF.Data
    <DataContract()> _
    Public Class ServiceMessage
        Private _myreturnedResult As Boolean = False
        Private _myReturnedMessages As List(Of String)


        Public Sub New()
            _myreturnedResult = False
        End Sub


        <DataMember()> _
        Public Property ReturnedResult() As Boolean

            Get
                Return _myreturnedResult
            End Get
            Set(ByVal value As Boolean)
                _myreturnedResult = value

            End Set
        End Property

        <DataMember()> _
        Public Property ReturnedMessage As List(Of String)
            Get
                If _myReturnedMessages Is Nothing Then
                    _myReturnedMessages = New List(Of String)
                End If
                Return _myReturnedMessages
            End Get
            Set(ByVal value As List(Of String))
                _myReturnedMessages = value
            End Set
        End Property

        Public Function GetMsgText() As String
            Dim txt As String = ""
            For Each Str As String In Me.ReturnedMessage
                txt = Str & Chr(10) & txt
            Next
            Return txt
        End Function
    End Class
End Namespace
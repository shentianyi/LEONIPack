Imports System.Text
Imports Brilliantech.Framework.Utilities.EnumUtil
Imports Brilliantech.Framework.Utilities
Imports System.Runtime.Serialization

Namespace Messages
    <DataContract(), KnownType(GetType(ReturnCode)), KnownType((GetType(List(Of String))))>
    Public Class ProcessMsg
        <DataMember()>
        Public result As Boolean = False
        <DataMember()>
        Public ResultCode As ReturnCode = ReturnCode.Unset
        Private _messages As Dictionary(Of ReturnCode, List(Of String))
        Private Shared _returnedCodes() As ReturnCode = EnumContainer.GetEnumTypes(GetType(ReturnCode))

        Public Sub SetResultCode(code As ReturnCode)
            If CType(code, Integer) >= CType(ResultCode, Integer) Then
                ResultCode = code
            End If
        End Sub

        <DataMember()>
        Public ReadOnly Property Message As Dictionary(Of ReturnCode, List(Of String))
            Get
                If _messages Is Nothing Then
                    _messages = New Dictionary(Of ReturnCode, List(Of String))
                    For Each enumType As ReturnCode In _returnedCodes
                        _messages.Add(enumType, New List(Of String))
                    Next
                End If
                Return _messages
            End Get
        End Property

        Public Sub AddMessage(type As ReturnCode, msg As String)
            Message.Item(type).add(msg)
            SetResultCode(type)
        End Sub

        Public Function GetAllLevelMsgs() As String
            Dim builder As StringBuilder = New StringBuilder
            For Each code As ReturnCode In _returnedCodes
                builder.Append(GetMessage(code))
            Next
            Return builder.ToString()
        End Function

        Public Function GetMessage(level As ReturnCode) As String
            Dim builder As StringBuilder = New StringBuilder
            For Each Str As String In Message.Item(level)
                builder.Append(Str)
                builder.Append(Environment.NewLine)
            Next
            Return builder.ToString
        End Function

        Public Shared Function Merge(Msgs() As ProcessMsg) As ProcessMsg
            Dim merged As ProcessMsg = New ProcessMsg
            Dim returnedCode As List(Of ReturnCode) = New List(Of ReturnCode)
            Dim result As List(Of Boolean) = New List(Of Boolean)

            If ArrayUtil.IsArrayNullorEmpty(Msgs) = False Then
                For Each msg As ProcessMsg In Msgs
                    returnedCode.Add(msg.ResultCode)
                    result.Add(msg.result)
                    For Each keyPair As KeyValuePair(Of ReturnCode, List(Of String)) In msg.Message
                        For Each Str As String In keyPair.Value
                            merged.Message(keyPair.Key).Add(Str)
                        Next
                    Next
                Next
            End If
            If result.Count > 0 Then
                If result.Contains(False) Then
                    merged.result = False
                Else
                    merged.result = True
                End If
            End If

            If returnedCode.Count > 0 Then
                For Each rc As ReturnCode In returnedCode
                    merged.SetResultCode(rc)
                Next
                'If returnedCode.Contains(ReturnCode.Fail) Then
                '    merged.ResultCode = ReturnCode.Fail
                'ElseIf returnedCode.Contains(ReturnCode.Error) Then
                '    merged.ResultCode = ReturnCode.Error
                'ElseIf returnedCode.Contains(ReturnCode.Warning) Then
                '    merged.ResultCode = ReturnCode.Warning
                'Else
                '    merged.ResultCode = ReturnCode.OK
                'End If
            End If

            Return merged
        End Function
    End Class
End Namespace
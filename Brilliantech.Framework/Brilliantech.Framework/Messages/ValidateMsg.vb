Imports System.Text
Namespace Messages
    Public Class ValidateMsg(Of T)

        Private m_target As T

        Public Property Target() As T
            Get
                Return m_target
            End Get
            Set(ByVal value As T)
                m_target = value
            End Set
        End Property

        Public Property Valid() As Boolean
            Get
                Return m_Valid
            End Get
            Set(ByVal value As Boolean)
                m_Valid = value
            End Set
        End Property
        Private m_Valid As Boolean



        Public Property Message() As List(Of String)
            Get
                If m_Message Is Nothing Then
                    m_Message = New List(Of String)
                End If
                Return m_Message
            End Get
            Set(ByVal value As List(Of String))
                m_Message = value
            End Set
        End Property
        Private m_Message As List(Of String)

        Public Overrides Function ToString() As String
            Dim s As StringBuilder = New StringBuilder
            For Each str As String In Me.Message
                s.Append(str + ";")
            Next
            Return s.ToString
        End Function
    End Class
End Namespace

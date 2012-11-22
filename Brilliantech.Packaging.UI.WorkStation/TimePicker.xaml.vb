
Imports System.Windows

Imports System.Windows.Controls

Imports System.Windows.Input

Partial Public Class TimePicker
    Inherits UserControl
    Public Sub New()
        InitializeComponent()
    End Sub

#Region "Properties"

    ''' <summary>
    ''' Gets or sets the date time value.
    ''' </summary>
    ''' <value>The date time value.</value>
    Public Property DateTimeValue() As System.Nullable(Of DateTime)
        Get
            Dim hours As String = Me.txtHours.Text
            Dim minutes As String = Me.txtMinutes.Text
            'Dim amPm As String = Me.txtAmPm.Text
            If Not String.IsNullOrWhiteSpace(hours) AndAlso Not String.IsNullOrWhiteSpace(minutes) Then
                Dim value As String = String.Format("{0}:{1}", Me.txtHours.Text, Me.txtMinutes.Text)
                Dim time As DateTime = DateTime.Parse(value)
                Return time
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As System.Nullable(Of DateTime))
            Dim time As System.Nullable(Of DateTime) = value
            If time.HasValue Then
                Dim timeString As String = time.Value.ToShortTimeString()
                '9:54 AM
                Dim values As String() = timeString.Split(":"c, " "c)
                If values.Length = 3 Then
                    Me.txtHours.Text = values(0)
                    Me.txtMinutes.Text = values(1)
                    'Me.txtAmPm.Text = values(2)
                End If
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the time span value.
    ''' </summary>
    ''' <value>The time span value.</value>
    Public Property TimeSpanValue() As System.Nullable(Of TimeSpan)
        Get
            Dim time As System.Nullable(Of DateTime) = Me.DateTimeValue
            If time.HasValue Then
                Return New TimeSpan(time.Value.Ticks)
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As System.Nullable(Of TimeSpan))
            Dim timeSpan As System.Nullable(Of TimeSpan) = value
            If timeSpan.HasValue Then
                Me.DateTimeValue = New DateTime(timeSpan.Value.Ticks)
            End If
        End Set
    End Property

#End Region

#Region "Event Subscriptions"

    ''' <summary>
    ''' Handles the Click event of the btnDown control.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
    Private Sub btnDown_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Dim controlId As String = Me.GetControlWithFocus().Name
        If "txtHours".Equals(controlId) Then
            Me.ChangeHours(False)
        ElseIf "txtMinutes".Equals(controlId) Then
            Me.ChangeMinutes(False)
        ElseIf "txtAmPm".Equals(controlId) Then
            'Me.ToggleAmPm()
        End If
    End Sub

    ''' <summary>
    ''' Handles the Click event of the btnUp control.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
    Private Sub btnUp_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Dim controlId As String = Me.GetControlWithFocus().Name
        If "txtHours".Equals(controlId) Then
            Me.ChangeHours(True)
        ElseIf "txtMinutes".Equals(controlId) Then
            Me.ChangeMinutes(True)
        ElseIf "txtAmPm".Equals(controlId) Then
            'Me.ToggleAmPm()
        End If
    End Sub

    ''' <summary>
    ''' Handles the PreviewTextInput event of the txtAmPm control.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="System.Windows.Input.TextCompositionEventArgs"/> instance containing the event data.</param>
    Private Sub txtAmPm_PreviewTextInput(ByVal sender As Object, ByVal e As TextCompositionEventArgs)
        ' prevent users to type text
        e.Handled = True
    End Sub

    ''' <summary>
    ''' Handles the KeyUp event of the txt control.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="System.Windows.Input.KeyEventArgs"/> instance containing the event data.</param>
    Private Sub txt_KeyUp(ByVal sender As Object, ByVal e As KeyEventArgs)
        ' check for up and down keyboard presses
        If Key.Up.Equals(e.Key) Then
            btnUp_Click(Me, Nothing)
        ElseIf Key.Down.Equals(e.Key) Then
            btnDown_Click(Me, Nothing)
        End If
    End Sub

    ''' <summary>
    ''' Handles the MouseWheel event of the txt control.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="System.Windows.Input.MouseWheelEventArgs"/> instance containing the event data.</param>
    Private Sub txt_MouseWheel(ByVal sender As Object, ByVal e As MouseWheelEventArgs)
        If e.Delta > 0 Then
            btnUp_Click(Me, Nothing)
        Else
            btnDown_Click(Me, Nothing)
        End If
    End Sub

    ''' <summary>
    ''' Handles the PreviewKeyUp event of the txt control.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="System.Windows.Input.KeyEventArgs"/> instance containing the event data.</param>
    Private Sub txt_PreviewKeyUp(ByVal sender As Object, ByVal e As KeyEventArgs)
        Dim textBox As TextBox = DirectCast(sender, TextBox)
        ' make sure all characters are number
        'Dim allNumbers As Boolean = textBox.Text.All([Char].IsNumber)
        Dim allnumbers As Boolean = textBox.Text.All(Function(c) Char.IsNumber(c))
        If Not allnumbers Then
            e.Handled = True
            Return
        End If


        ' make sure user did not enter values out of range
        Dim value As Integer
        Integer.TryParse(textBox.Text, value)
        If "txtHours".Equals(textBox.Name) AndAlso value > 12 Then
            EnforceLimits(e, textBox)
        ElseIf "txtMinutes".Equals(textBox.Name) AndAlso value > 59 Then
            EnforceLimits(e, textBox)
        End If
    End Sub

#End Region

#Region "Methods"

    ''' <summary>
    ''' Changes the hours.
    ''' </summary>
    ''' <param name="isUp">if set to <c>true</c> [is up].</param>
    Private Sub ChangeHours(ByVal isUp As Boolean)
        Dim value As Integer = Convert.ToInt32(Me.txtHours.Text)
        If isUp Then
            value += 1
            If value = 24 Then
                value = 0
            End If
        Else
            value -= 1
            If value = -1 Then
                value = 23
            End If
        End If
        Me.txtHours.Text = Convert.ToString(value)
    End Sub

    ''' <summary>
    ''' Changes the minutes.
    ''' </summary>
    ''' <param name="isUp">if set to <c>true</c> [is up].</param>
    Private Sub ChangeMinutes(ByVal isUp As Boolean)
        Dim value As Integer = Convert.ToInt32(Me.txtMinutes.Text)
        If isUp Then
            value += 1
            If value = 60 Then
                value = 0
            End If
        Else
            value -= 1
            If value = -1 Then
                value = 59
            End If
        End If

        Dim textValue As String = Convert.ToString(value)
        If value < 10 Then
            textValue = "0" & Convert.ToString(value)
        End If
        Me.txtMinutes.Text = textValue
    End Sub

    ''' <summary>
    ''' Enforces the limits.
    ''' </summary>
    ''' <param name="e">The <see cref="System.Windows.Input.KeyEventArgs"/> instance containing the event data.</param>
    ''' <param name="textBox">The text box.</param>
    ''' <param name="enteredValue">The entered value.</param>
    Private Shared Sub EnforceLimits(ByVal e As KeyEventArgs, ByVal textBox As TextBox)
        Dim enteredValue As String = GetEnteredValue(e.Key)
        Dim text As String = textBox.Text.Replace(enteredValue, "")
        If String.IsNullOrEmpty(text) Then
            text = enteredValue
        End If
        textBox.Text = text
        e.Handled = True
    End Sub

    ''' <summary>
    ''' Gets the control with focus.
    ''' </summary>
    ''' <returns></returns>
    Private Function GetControlWithFocus() As TextBox
        Dim txt As New TextBox()
        If Me.txtHours.IsFocused Then
            txt = Me.txtHours
        ElseIf Me.txtMinutes.IsFocused Then
            txt = Me.txtMinutes
            'ElseIf Me.txtAmPm.IsFocused Then
            '    txt = Me.txtAmPm
        End If
        Return txt
    End Function

    ''' <summary>
    ''' Gets the entered value.
    ''' </summary>
    ''' <param name="key">The key.</param>
    ''' <returns></returns>
    Private Shared Function GetEnteredValue(ByVal key__1 As Key) As String
        Dim value As String = String.Empty
        Select Case key__1
            Case Key.D0, Key.NumPad0
                value = "0"
                Exit Select
            Case Key.D1, Key.NumPad1
                value = "1"
                Exit Select
            Case Key.D2, Key.NumPad2
                value = "2"
                Exit Select
            Case Key.D3, Key.NumPad3
                value = "3"
                Exit Select
            Case Key.D4, Key.NumPad4
                value = "4"
                Exit Select
            Case Key.D5, Key.NumPad5
                value = "5"
                Exit Select
            Case Key.D6, Key.NumPad6
                value = "6"
                Exit Select
            Case Key.D7, Key.NumPad7
                value = "7"
                Exit Select
            Case Key.D8, Key.NumPad8
                value = "8"
                Exit Select
            Case Key.D9, Key.NumPad9
                value = "9"
                Exit Select
        End Select
        Return value
    End Function

    ''' <summary>
    ''' Toggles the am pm.
    ''' </summary>
    'Private Sub ToggleAmPm()
    '    If "AM".Equals(Me.txtAmPm.Text) Then
    '        Me.txtAmPm.Text = "PM"
    '    Else
    '        Me.txtAmPm.Text = "AM"
    '    End If
    'End Sub

#End Region
End Class

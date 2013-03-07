Imports System.ComponentModel
Imports System.Text.RegularExpressions
Public Class CustomValidateViewModel
    Implements INotifyPropertyChanged


    Private _pattern As String
    Public Property PatternString As String
        Get
            Return _pattern
        End Get
        Set(ByVal value As String)
            _pattern = value
        End Set
    End Property
    Private _BarcodeItemName As String
    Public Property BarcodeItemName As String
        Get
            Return _BarcodeItemName
        End Get
        Set(ByVal value As String)
            _BarcodeItemName = value
            OnPropertyChanged(New PropertyChangedEventArgs("BarcodeItemName"))
        End Set
    End Property


    Private _BackgroundColor As String = "White"
    Public Property BackgroundColor As String
        Get
            Return _BackgroundColor
        End Get
        Set(ByVal value As String)
            _BackgroundColor = value
            OnPropertyChanged(New PropertyChangedEventArgs("BackgroundColor"))
        End Set
    End Property

    Private _isValidated As Boolean = False
    Public Property IsValidated As Boolean
        Get
            Return _isValidated
        End Get
        Set(ByVal value As Boolean)
            _isValidated = value
            If value = False Then
                BackgroundColor = "White"
            Else
                BackgroundColor = "Green"
            End If
        End Set
    End Property



    Public Sub OnPropertyChanged(ByVal e As PropertyChangedEventArgs)
        RaiseEvent PropertyChanged(Me, e)
    End Sub

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class

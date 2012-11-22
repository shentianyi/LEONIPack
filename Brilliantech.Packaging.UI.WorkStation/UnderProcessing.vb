Imports System.ComponentModel
Public Class UnderProcessing
    Implements INotifyPropertyChanged


    Private _added As Integer
    Private _remained As Integer
    Private _capa As Integer
    Private _isPaused As Boolean
    Private _isFulled As Boolean
    Private _isFinished As Boolean

    Public Sub New(capa As Integer, addedItem As Integer)
        _capa = capa
        Added = addedItem
        Remained = capa - Added
    End Sub

    Public Property IsPaused As Boolean
        Get
            Return _isPaused
        End Get
        Set(value As Boolean)
            _isPaused = value
            IsFinished = value
            OnPropertyChanged(New PropertyChangedEventArgs("IsPaused"))
        End Set
    End Property

    Public Property IsFulled As Boolean
        Get
            Return _isFulled
        End Get
        Set(value As Boolean)
            _isFulled = value
            IsFinished = value
            OnPropertyChanged(New PropertyChangedEventArgs("IsFulled"))
        End Set
    End Property

    Public Property IsFinished As Boolean
        Get
            Return _isFinished
        End Get
        Set(value As Boolean)
            _isFinished = value
            OnPropertyChanged(New PropertyChangedEventArgs("IsFinished"))
        End Set
    End Property

    Public Property Added As Integer
        Get
            Return _added
        End Get
        Set(value As Integer)
            _added = value
            OnPropertyChanged(New PropertyChangedEventArgs("Added"))
        End Set
    End Property

    Public Property Remained As Integer
        Get
            Return _remained
        End Get
        Set(value As Integer)
            _remained = value
            OnPropertyChanged(New PropertyChangedEventArgs("Remained"))
        End Set
    End Property

    Public ReadOnly Property Capacity As Integer
        Get
            Return _capa
        End Get
    End Property

    Public Sub Add()
        Added = Added + 1
        Remained = Remained - 1
    End Sub



    Public Sub OnPropertyChanged(e As PropertyChangedEventArgs)
        RaiseEvent PropertyChanged(Me, e)
    End Sub

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class

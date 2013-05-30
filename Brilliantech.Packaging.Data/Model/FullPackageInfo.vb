Imports System.Runtime.Serialization
<DataContract()>
Public Class FullPackageInfo

    Private _PId As String
    <DataMember()>
    Public Property PId As String
        Get
            Return _PId
        End Get
        Set(ByVal value As String)
            _PId = value
        End Set
    End Property



    Private _Capa As Integer
    <DataMember()>
    Public Property Capa As Integer
        Get
            Return _capa
        End Get
        Set(ByVal value As Integer)
            _Capa = value
        End Set
    End Property


    Private _PartNr As String
    <DataMember()>
    Public Property PartNr As String
        Get
            Return _PartNr
        End Get
        Set(ByVal value As String)
            _PartNr = value
        End Set
    End Property



    Private _CustomerPartNr As String
    <DataMember()>
    Public Property CustomerPartNr As String
        Get
            Return _CustomerPartNr
        End Get
        Set(ByVal value As String)
            _CustomerPartNr = value
        End Set
    End Property


    Private _Wrkst As String
    <DataMember()>
     Public Property Wrkst As String
        Get
            Return _Wrkst
        End Get
        Set(ByVal value As String)
            _Wrkst = value
        End Set
    End Property


    Private _status As PackageStatus
    <DataMember()>
    Public Property status As PackageStatus
        Get
            Return _status
        End Get
        Set(ByVal value As PackageStatus)
            _status = value
        End Set
    End Property

    Private _containerId As String
    <DataMember()>
    Public Property ContainerType As String
        Get
            Return _containerId
        End Get
        Set(ByVal value As String)
            _containerId = value
        End Set
    End Property
End Class

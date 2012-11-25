Imports System.Runtime.Serialization
Namespace Structures
    Public Class NumberRate
        <DataContract()>
        Public Class NumberRate
            <DataMember()>
            Public Denominator As Integer
            <DataMember()>
            Public Molecule As Integer

            Public Sub New(molecule As Integer, denominator As Integer)
                Me.Denominator = denominator
                Me.Molecule = molecule
            End Sub

            Public Overrides Function ToString() As String
                Return Molecule.ToString & ":" & Denominator
            End Function
        End Class
    End Class
End Namespace

Imports System.Xml.Linq

Namespace Utilities.DBUtil.MongoDB
    Public Class MongoXMLTypeMapper
        Inherits MongoTableMapper

        Private XMLFilePath As String
        Private Shared instance As MongoXMLTypeMapper
        Private Shared locker As Object = New Object
        Private Sub New(ByVal xmlFile As String)
            XMLFilePath = xmlFile
            LoadFrom()
        End Sub

        Public Shared Function CreateInstance(ByVal xmlFilePath As String) As MongoXMLTypeMapper
            If instance Is Nothing Then
                SyncLock (locker)
                    If instance Is Nothing Then
                        instance = New MongoXMLTypeMapper(xmlFilePath)
                    End If
                End SyncLock

            End If
            Return instance
        End Function

        Protected Overrides Sub LoadFrom()
            Dim xmlDoc As XElement = XElement.Load(XMLFilePath)
            Dim nodes As IEnumerable(Of XNode) = From types In xmlDoc.Elements("Mapping")
                                 Select types

            If nodes IsNot Nothing Then
                For Each child As XElement In nodes
                    Me.mappingDict.Add(Trim(child.Element("TypeFullName").Value.Trim), _
                                       Trim(child.Element("CollectionName").Value.Trim))
                Next
            End If
        End Sub

    End Class
End Namespace
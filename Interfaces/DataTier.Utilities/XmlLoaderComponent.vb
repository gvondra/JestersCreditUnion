Imports System.Xml
Public Class XmlLoaderComponent
    Implements ILoaderComponent

    Public Function GetValue(reader As IDataReader, ordinal As Int32) As Object Implements ILoaderComponent.GetValue
        Dim text As String = reader.GetString(ordinal)
        Dim document As New XmlDocument()
        If String.IsNullOrEmpty(text) = False Then
            document.LoadXml(text.TrimEnd)
        End If
        Return document
    End Function

    Public Function IsApplicable(mapping As ColumnMapping) As Boolean Implements ILoaderComponent.IsApplicable
        Return mapping.Info.PropertyType.Equals(GetType(XmlNode))
    End Function
End Class

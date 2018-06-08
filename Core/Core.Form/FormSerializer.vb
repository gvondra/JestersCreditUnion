﻿Imports System.IO
Imports System.Text
Imports System.Xml.Serialization

Public Class FormSerializer(Of T)
    Implements IFormSerializer

    Private m_form As T

    Public Sub New(form As T)
        m_form = form
    End Sub

    Public Function Serialize() As XmlNode Implements IFormSerializer.Serialize
        Dim factory As New XmlSerializerFactory()
        Dim serializer As XmlSerializer = factory.CreateSerializer(GetType(T))
        Dim xml As New StringBuilder
        Dim result As XmlDocument

        Using xmlWriter As XmlWriter = XmlWriter.Create(xml)
            serializer.Serialize(xmlWriter, m_form)
            xmlWriter.Close()
        End Using
        result = New XmlDocument()
        result.LoadXml(xml.ToString)
        Return result
    End Function
End Class

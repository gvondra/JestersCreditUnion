Imports System.IO
Imports System.Text
Imports System.Xml
Imports System.Xml.Xsl
Public Class FormContentTransform
    Implements IFormContentTransform

    Private m_form As IForm

    Public Sub New(ByVal form As IForm)
        m_form = form
    End Sub

    Public Function Transform() As Stream Implements IFormContentTransform.Transform
        Dim xslTransform As New XslCompiledTransform
        Dim stream As Stream = Nothing

        If m_form.Content IsNot Nothing Then
            Using transformReader As XmlReader = GetTransform()
                xslTransform.Load(transformReader)
                stream = New MemoryStream()
                xslTransform.Transform(m_form.Content, Nothing, stream)
            End Using

            If stream IsNot Nothing AndAlso stream.Position > 0 Then
                stream.Position = 0
            End If
        End If

        Return stream
    End Function

    Private Function GetTransform() As XmlReader
        Dim stream As Stream = TransformIndex.GetResourceStream(GetResourceName(GetFileName()))

        If stream Is Nothing Then
            Throw New FormTransformNotFoundException("Resource stream not found " & GetResourceName(GetFileName()), m_form.Style, GetResourceName(GetFileName()))
        End If

        Return XmlReader.Create(stream)
    End Function

    Private Function GetResourceName(ByVal fileName As String) As String
        Return fileName
    End Function

    Private Function GetFileName() As String
        Dim fileName As String = Nothing

        Select Case m_form.Style
            Case enumFormStyle.RoleRequest
                fileName = TransformIndex.RoleRequest1
            Case enumFormStyle.NotSet
                Throw New FormTransformNotFoundException("Form style not set", m_form.Style)
            Case Else
                Throw New FormTransformNotFoundException("Unexpected style " & m_form.Style.ToString, m_form.Style)
        End Select
        Return fileName
    End Function
End Class

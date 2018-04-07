Namespace Forms
    <XmlRoot([Namespace]:="abyssaldataprocessor/forms/rolerequest/v1")>
    Public Class RoleRequest
        <XmlElement(Order:=1)> Public Property FullName As String
        <XmlElement(Order:=2)> Public Property Comment As String

        Public Function Serialize() As XmlNode
            Dim serializer As New FormSerializer(Of RoleRequest)(Me)
            Return serializer.Serialize()
        End Function
    End Class
End Namespace
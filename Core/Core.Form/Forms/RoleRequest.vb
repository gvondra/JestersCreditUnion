Namespace Forms
    <XmlRoot([Namespace]:="abyssaldataprocessor/forms/rolerequest/v1")>
    Public Class RoleRequest
        Implements IRoleRequest

        Private m_formFactory As IFormFactory
        Private m_formSerializerFactory As IFormSerializerFactory

        Friend Sub New(ByVal formFactory As IFormFactory, ByVal formSerializerFactory As IFormSerializerFactory)
            m_formFactory = formFactory
            m_formSerializerFactory = formSerializerFactory
        End Sub

        Public Sub New()
            Throw New NotSupportedException()
        End Sub

        <XmlElement(Order:=1)> Public Property FullName As String Implements IRoleRequest.FullName
        <XmlElement(Order:=2)> Public Property Comment As String Implements IRoleRequest.Comment

        Public Function CreateForm(ByVal user As IUser) As IForm Implements IFormSerializable.CreateForm
            Return m_formFactory.CreateForm(user, enumFormType.RoleRequest, enumFormStyle.RoleRequest, Serialize())
        End Function

        Private Function Serialize() As XmlNode Implements IFormSerializable.Serialize
            Dim serializer As IFormSerializer = m_formSerializerFactory.Create(Of RoleRequest)(Me)
            Return serializer.Serialize()
        End Function
    End Class
End Namespace
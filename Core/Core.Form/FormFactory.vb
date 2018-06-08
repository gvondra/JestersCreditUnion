Imports JestersCreditUnion.DataTier.Core
Imports JestersCreditUnion.DataTier.Core.Models
Imports Autofac
Public Class FormFactory
    Implements IFormFactory

    Private m_formSerializerFactory As IFormSerializerFactory
    Private m_userFactory As IUserFactory
    Private m_container As IContainer

    Public Sub New(ByVal formSerializerFactory As IFormSerializerFactory, ByVal userFactory As IUserFactory)
        m_formSerializerFactory = formSerializerFactory
        m_userFactory = userFactory
        m_container = ObjectContainer.GetContainer
    End Sub

    Public Function CreateForm(user As IUser, formType As enumFormType, style As enumFormStyle, content As XmlNode) As IForm Implements IFormFactory.CreateForm
        Return New Form(m_userFactory, user, formType, style, content)
    End Function

    Public Function CreateRoleRequest() As IRoleRequest Implements IFormFactory.CreateRoleRequest
        Return New Forms.RoleRequest(Me, m_formSerializerFactory)
    End Function

    Public Function [Get](settings As ISettings, formId As Guid) As IForm Implements IFormFactory.Get
        Dim factory As IFormDataFactory
        Dim data As FormData
        Dim result As IForm = Nothing
        Using scope As ILifetimeScope = m_container.BeginLifetimeScope
            factory = scope.Resolve(Of IFormDataFactory)()
            data = factory.Get(New Settings(settings), formId)
            If data IsNot Nothing Then
                result = New Form(m_userFactory, data)
            End If
        End Using
        Return result
    End Function
End Class

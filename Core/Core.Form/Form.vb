Imports JestersCreditUnion.DataTier.Core
Imports JestersCreditUnion.DataTier.Core.Models
Imports autofac
Public Class Form
    Implements IForm

    Private m_formData As FormData
    Private m_user As IUser
    Private m_userFactory As IUserFactory
    Private m_container As IContainer

    Friend Sub New(ByVal userFactory As IUserFactory,
                   ByVal user As IUser,
                   ByVal formType As enumFormType,
                   ByVal formstyle As enumFormStyle,
                   ByVal content As XmlNode)

        m_container = ObjectContainer.GetContainer
        m_userFactory = userFactory
        m_user = user
        m_formData = New FormData() With {
            .FormTypeId = CType(formType, Int16),
            .Style = CType(formstyle, Int16),
            .Content = content
        }
    End Sub

    Friend Sub New(ByVal userFactory As IUserFactory,
                   ByVal container As IContainer,
                   ByVal user As IUser,
                   ByVal formType As enumFormType,
                   ByVal formstyle As enumFormStyle,
                   ByVal content As XmlNode)

        m_container = container
        m_userFactory = userFactory
        m_user = user
        m_formData = New FormData() With {
            .FormTypeId = CType(formType, Int16),
            .Style = CType(formstyle, Int16),
            .Content = content
        }
    End Sub

    Friend Sub New(ByVal userFactory As IUserFactory, ByVal formData As FormData)
        m_formData = formData
        m_userFactory = userFactory
        m_container = ObjectContainer.GetContainer()
    End Sub

    Public ReadOnly Property Content As XmlNode Implements IForm.Content
        Get
            Return m_formData.Content
        End Get
    End Property

    Public ReadOnly Property CreateTimestamp As Date Implements IForm.CreateTimestamp
        Get
            Return m_formData.CreateTimestamp
        End Get
    End Property

    Public ReadOnly Property FormId As Guid Implements IForm.FormId
        Get
            Return m_formData.FormId
        End Get
    End Property

    Public ReadOnly Property [Type] As enumFormType Implements IForm.Type
        Get
            Return CType(m_formData.FormTypeId, enumFormType)
        End Get
    End Property

    Public Property Style As enumFormStyle Implements IForm.Style
        Get
            Return CType(m_formData.Style, enumFormStyle)
        End Get
        Set(value As enumFormStyle)
            m_formData.Style = CType(value, Int16)
        End Set
    End Property

    Private Property UserId As Guid
        Get
            Return m_formData.UserId
        End Get
        Set(value As Guid)
            m_formData.UserId = value
        End Set
    End Property

    Public Sub Create(transactionHandler As ITransactionHandler) Implements ISavable.Create
        Dim creator As DataTier.Utilities.IDataCreator

        Using scope As ILifetimeScope = m_container.BeginLifetimeScope
            UserId = m_user.UserId
            creator = scope.Resolve(Of FormDataSaver)(
                New TypedParameter(GetType(JestersCreditUnion.DataTier.Utilities.ITransactionHandler), New TransactionHandler(transactionHandler)),
                New TypedParameter(GetType(FormData), m_formData)
            )
            creator.Create()
        End Using
    End Sub

    Public Sub Update(transactionHandler As ITransactionHandler) Implements ISavable.Update
        Throw New NotImplementedException()
    End Sub

    Public Function GetUser(settings As ISettings) As IUser Implements IForm.GetUser
        If m_user Is Nothing Then
            m_user = m_userFactory.Get(settings, UserId)
        End If
        Return m_user
    End Function
End Class

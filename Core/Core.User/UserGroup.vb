Imports JestersCreditUnion.DataTier.Core
Imports JestersCreditUnion.DataTier.Core.Models
Imports Autofac
Public Class UserGroup
    Implements IUserGroup

    Private m_userGroupData As UserGroupData
    Private m_innerGroup As IGroup
    Private m_user As IUser
    Private m_container As IContainer

    Public Sub New(ByVal user As IUser, ByVal group As IGroup, ByVal userGroupData As UserGroupData)
        m_userGroupData = userGroupData
        m_innerGroup = group
        m_user = user
        m_container = ObjectContainer.GetContainer
    End Sub

    Public ReadOnly Property GroupId As Guid Implements IGroup.GroupId
        Get
            Return m_innerGroup.GroupId
        End Get
    End Property

    Public Property IsActive As Boolean Implements IUserGroup.IsActive
        Get
            Return m_userGroupData.IsActive
        End Get
        Set(value As Boolean)
            m_userGroupData.IsActive = value
        End Set
    End Property

    Public Property Name As String Implements IGroup.Name
        Get
            Return m_innerGroup.Name
        End Get
        Set(value As String)
            m_innerGroup.Name = value
        End Set
    End Property

    Public Sub Create(transactionHandler As ITransactionHandler) Implements ISavable.Create
        Dim creator As DataTier.Utilities.IDataCreator
        Using scope As ILifetimeScope = m_container.BeginLifetimeScope
            m_userGroupData.UserId = m_user.UserId
            m_userGroupData.GroupId = m_innerGroup.GroupId
            creator = scope.Resolve(Of UserGroupDataSaver)(
                New TypedParameter(GetType(JestersCreditUnion.DataTier.Utilities.ITransactionHandler), New TransactionHandler(transactionHandler)),
                New TypedParameter(GetType(UserGroupData), m_userGroupData)
            )
            creator.Create()
        End Using
    End Sub

    Public Sub Update(transactionHandler As ITransactionHandler) Implements ISavable.Update
        Dim updater As DataTier.Utilities.IDataUpdater
        Using scope As ILifetimeScope = m_container.BeginLifetimeScope
            updater = scope.Resolve(Of UserGroupDataSaver)(
                New TypedParameter(GetType(JestersCreditUnion.DataTier.Utilities.ITransactionHandler), New TransactionHandler(transactionHandler)),
                New TypedParameter(GetType(UserGroupData), m_userGroupData)
            )
            updater.Update()
        End Using
    End Sub

    Public Sub Save(ByVal transactionHandler As ITransactionHandler) Implements IUserGroup.Save
        If m_userGroupData.DataStateManager.GetState(m_userGroupData) = DataTier.Utilities.IDataStateManager(Of UserGroupData).enumState.New Then
            Create(transactionHandler)
        ElseIf m_userGroupData.DataStateManager.GetState(m_userGroupData) = DataTier.Utilities.IDataStateManager(Of UserGroupData).enumState.Updated Then
            Update(transactionHandler)
        End If
    End Sub
End Class

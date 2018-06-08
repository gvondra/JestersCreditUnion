Imports JestersCreditUnion.DataTier.Core
Imports JestersCreditUnion.DataTier.Core.Models
Imports Autofac
Public Class Organization
    Implements IOrganization

    Private m_organizationData As OrganizationData
    Private m_container As IContainer

    Friend Sub New()
        m_container = ObjectContainer.GetContainer
        m_organizationData = New OrganizationData
    End Sub

    Friend Sub New(ByVal organizationData As OrganizationData)
        m_container = ObjectContainer.GetContainer
        m_organizationData = organizationData
    End Sub

    Public Property Name As String Implements IOrganization.Name
        Get
            Return m_organizationData.Name
        End Get
        Set(value As String)
            m_organizationData.Name = value
        End Set
    End Property

    Public ReadOnly Property OrganizationId As Guid Implements IOrganization.OrganizationId
        Get
            Return m_organizationData.OrganizationId
        End Get
    End Property

    Public Sub Create(transactionHandler As ITransactionHandler) Implements ISavable.Create
        Dim creator As DataTier.Utilities.IDataCreator
        Using scope As ILifetimeScope = m_container.BeginLifetimeScope
            creator = scope.ResolveKeyed(Of DataTier.Utilities.IDataCreator)("OrganizationDataSaver",
                New TypedParameter(GetType(JestersCreditUnion.DataTier.Utilities.ITransactionHandler), New TransactionHandler(transactionHandler)),
                New TypedParameter(GetType(OrganizationData), m_organizationData)
            )
            creator.Create()
        End Using
    End Sub

    Public Sub Update(transactionHandler As ITransactionHandler) Implements ISavable.Update
        Dim updater As DataTier.Utilities.IDataUpdater
        Using scope As ILifetimeScope = m_container.BeginLifetimeScope
            updater = scope.ResolveKeyed(Of DataTier.Utilities.IDataUpdater)("OrganizationDataSaver",
                New TypedParameter(GetType(JestersCreditUnion.DataTier.Utilities.ITransactionHandler), New TransactionHandler(transactionHandler)),
                New TypedParameter(GetType(OrganizationData), m_organizationData)
            )
            updater.Update()
        End Using
    End Sub

    Public Sub Save(transactionHandler As ITransactionHandler) Implements IOrganization.Save
        If m_organizationData.DataStateManager.GetState(m_organizationData) = DataTier.Utilities.IDataStateManager(Of OrganizationData).enumState.New Then
            Create(transactionHandler)
        ElseIf m_organizationData.DataStateManager.GetState(m_organizationData) = DataTier.Utilities.IDataStateManager(Of OrganizationData).enumState.Updated Then
            Update(transactionHandler)
        End If
    End Sub
End Class

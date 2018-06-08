Imports JestersCreditUnion.DataTier.Core
Imports JestersCreditUnion.DataTier.Core.Models
Imports Autofac
Public Class Group
    Implements IGroup

    Private m_groupData As GroupData
    Private m_container As IContainer

    Friend Sub New(ByVal groupData As GroupData)
        m_groupData = groupData
        m_container = ObjectContainer.GetContainer()
    End Sub

    Public ReadOnly Property GroupId As Guid Implements IGroup.GroupId
        Get
            Return m_groupData.GroupId
        End Get
    End Property

    Public Property Name As String Implements IGroup.Name
        Get
            Return m_groupData.Name
        End Get
        Set(value As String)
            m_groupData.Name = value
        End Set
    End Property

    Public Sub Create(transactionHandler As ITransactionHandler) Implements ISavable.Create
        Dim creator As DataTier.Utilities.IDataCreator
        Using scope As ILifetimeScope = m_container.BeginLifetimeScope
            creator = scope.Resolve(Of GroupDataSaver)(
                New TypedParameter(GetType(JestersCreditUnion.DataTier.Utilities.ITransactionHandler), New TransactionHandler(transactionHandler)),
                New TypedParameter(GetType(GroupData), m_groupData)
            )
            creator.Create()
        End Using
    End Sub

    Public Sub Update(transactionHandler As ITransactionHandler) Implements ISavable.Update
        Dim updater As DataTier.Utilities.IDataUpdater
        Using scope As ILifetimeScope = m_container.BeginLifetimeScope
            updater = scope.Resolve(Of GroupDataSaver)(
                New TypedParameter(GetType(JestersCreditUnion.DataTier.Utilities.ITransactionHandler), New TransactionHandler(transactionHandler)),
                New TypedParameter(GetType(GroupData), m_groupData)
            )
            updater.Update()
        End Using
    End Sub
End Class

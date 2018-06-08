Imports JestersCreditUnion.DataTier.Core
Imports JestersCreditUnion.DataTier.Core.Models
Imports Autofac
Public Class TaskTypeGroup
    Implements ITaskTypeGroup

    Private m_taskTypeGroupData As TaskTypeGroupData
    Private m_innerGroup As IGroup
    Private m_taskType As ITaskType
    Private m_container As IContainer

    Friend Sub New(ByVal taskType As ITaskType, ByVal group As IGroup, ByVal data As TaskTypeGroupData)
        m_taskTypeGroupData = data
        m_innerGroup = group
        m_taskType = taskType
        m_container = ObjectContainer.GetContainer
    End Sub

    Public ReadOnly Property GroupId As Guid Implements IGroup.GroupId
        Get
            Return m_innerGroup.GroupId
        End Get
    End Property

    Public Property IsActive As Boolean Implements ITaskTypeGroup.IsActive
        Get
            Return m_taskTypeGroupData.IsActive
        End Get
        Set(value As Boolean)
            m_taskTypeGroupData.IsActive = value
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

    Public Sub Save(transactionHandler As ITransactionHandler) Implements ITaskTypeGroup.Save
        If m_taskTypeGroupData.DataStateManager.GetState(m_taskTypeGroupData) = DataTier.Utilities.IDataStateManager(Of UserGroupData).enumState.New Then
            Create(transactionHandler)
        ElseIf m_taskTypeGroupData.DataStateManager.GetState(m_taskTypeGroupData) = DataTier.Utilities.IDataStateManager(Of UserGroupData).enumState.Updated Then
            Update(transactionHandler)
        End If
    End Sub

    Public Sub Create(transactionHandler As ITransactionHandler) Implements ISavable.Create
        Dim creator As DataTier.Utilities.IDataCreator
        Using scope As ILifetimeScope = m_container.BeginLifetimeScope
            m_taskTypeGroupData.TaskTypeId = m_taskType.TaskTypeId
            m_taskTypeGroupData.GroupId = m_innerGroup.GroupId
            creator = scope.Resolve(Of TaskTypeGroupDataSaver)(
                New TypedParameter(GetType(JestersCreditUnion.DataTier.Utilities.ITransactionHandler), New TransactionHandler(transactionHandler)),
                New TypedParameter(GetType(TaskTypeGroupData), m_taskTypeGroupData)
            )
            creator.Create()
        End Using
    End Sub

    Public Sub Update(transactionHandler As ITransactionHandler) Implements ISavable.Update
        Dim updater As DataTier.Utilities.IDataUpdater
        Using scope As ILifetimeScope = m_container.BeginLifetimeScope
            updater = scope.Resolve(Of TaskTypeGroupDataSaver)(
                New TypedParameter(GetType(JestersCreditUnion.DataTier.Utilities.ITransactionHandler), New TransactionHandler(transactionHandler)),
                New TypedParameter(GetType(TaskTypeGroupData), m_taskTypeGroupData)
            )
            updater.Update()
        End Using
    End Sub
End Class

Imports JestersCreditUnion.DataTier.Core
Imports JestersCreditUnion.DataTier.Core.Models
Imports Autofac
Public Class TaskTypeEventType
    Implements ITaskTypeEventType

    Private m_eventTypeTaskTypeData As EventTypeTaskTypeData
    Private m_innerEventType As IEventType
    Private m_taskType As ITaskType
    Private m_container As IContainer

    Friend Sub New(ByVal taskType As ITaskType, ByVal eventType As IEventType, data As EventTypeTaskTypeData)
        m_eventTypeTaskTypeData = data
        m_innerEventType = eventType
        m_taskType = taskType
        m_container = ObjectContainer.GetContainer()
    End Sub

    Public ReadOnly Property EventTypeId As Int16 Implements IEventType.EventTypeId
        Get
            Return m_innerEventType.EventTypeId
        End Get
    End Property

    Public Property IsActive As Boolean Implements ITaskTypeEventType.IsActive
        Get
            Return m_eventTypeTaskTypeData.IsActive
        End Get
        Set(value As Boolean)
            m_eventTypeTaskTypeData.IsActive = value
        End Set
    End Property

    Public Property Title As String Implements IEventType.Title
        Get
            Return m_innerEventType.Title
        End Get
        Set(value As String)
            m_innerEventType.Title = value
        End Set
    End Property

    Public Sub Save(ByVal transactionHandler As ITransactionHandler) Implements ITaskTypeEventType.Save
        If m_eventTypeTaskTypeData.DataStateManager.GetState(m_eventTypeTaskTypeData) = DataTier.Utilities.IDataStateManager(Of UserGroupData).enumState.New Then
            Create(transactionHandler)
        ElseIf m_eventTypeTaskTypeData.DataStateManager.GetState(m_eventTypeTaskTypeData) = DataTier.Utilities.IDataStateManager(Of UserGroupData).enumState.Updated Then
            Update(transactionHandler)
        End If
    End Sub

    Public Sub Create(transactionHandler As ITransactionHandler) Implements ISavable.Create
        Dim creator As DataTier.Utilities.IDataCreator
        Using scope As ILifetimeScope = m_container.BeginLifetimeScope
            m_eventTypeTaskTypeData.EventTypeId = m_innerEventType.EventTypeId
            m_eventTypeTaskTypeData.TaskTypeId = m_taskType.TaskTypeId
            creator = scope.Resolve(Of EventTypeTaskTypeDataSaver)(
                New TypedParameter(GetType(JestersCreditUnion.DataTier.Utilities.ITransactionHandler), New TransactionHandler(transactionHandler)),
                New TypedParameter(GetType(EventTypeTaskTypeData), m_eventTypeTaskTypeData)
            )
            creator.Create()
        End Using
    End Sub

    Public Sub Update(transactionHandler As ITransactionHandler) Implements ISavable.Update
        Dim updater As DataTier.Utilities.IDataUpdater
        Using scope As ILifetimeScope = m_container.BeginLifetimeScope
            updater = scope.Resolve(Of EventTypeTaskTypeDataSaver)(
                New TypedParameter(GetType(JestersCreditUnion.DataTier.Utilities.ITransactionHandler), New TransactionHandler(transactionHandler)),
                New TypedParameter(GetType(EventTypeTaskTypeData), m_eventTypeTaskTypeData)
            )
            updater.Update()
        End Using
    End Sub

    Public Function GetTaskTypes(settings As ISettings) As IEnumerable(Of IEventTypeTaskType) Implements IEventType.GetTaskTypes
        Return m_innerEventType.GetTaskTypes(settings)
    End Function
End Class

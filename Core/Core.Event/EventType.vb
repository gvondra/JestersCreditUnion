Imports JestersCreditUnion.DataTier.Core
Imports JestersCreditUnion.DataTier.Core.Models
Imports Autofac
Public Class EventType
    Implements IEventType

    Private m_eventTypeData As EventTypeData
    Private m_taskTypeFactory As ITaskTypeFactory
    Private m_container As IContainer

    Friend Sub New(ByVal eventTypeData As EventTypeData, ByVal taskTypeFactory As ITaskTypeFactory)
        m_eventTypeData = eventTypeData
        m_taskTypeFactory = taskTypeFactory
        m_container = ObjectContainer.GetContainer
    End Sub

    Friend Sub New(ByVal container As IContainer, ByVal eventTypeData As EventTypeData)
        m_eventTypeData = eventTypeData
        m_container = container
    End Sub

    Public Property Title As String Implements IEventType.Title
        Get
            Return m_eventTypeData.Title
        End Get
        Set(value As String)
            m_eventTypeData.Title = value
        End Set
    End Property

    Public ReadOnly Property EventTypeId As Int16 Implements IEventType.EventTypeId
        Get
            Return m_eventTypeData.EventTypeId
        End Get
    End Property

    Public Sub Create(transactionHandler As ITransactionHandler) Implements ISavable.Create
        Dim creator As DataTier.Utilities.IDataCreator
        Using scope As ILifetimeScope = m_container.BeginLifetimeScope
            creator = scope.Resolve(Of EventTypeDataSaver)(
                New TypedParameter(GetType(JestersCreditUnion.DataTier.Utilities.ITransactionHandler), New TransactionHandler(transactionHandler)),
                New TypedParameter(GetType(EventTypeData), m_eventTypeData)
            )
            creator.Create()
        End Using
    End Sub

    Public Sub Update(transactionHandler As ITransactionHandler) Implements ISavable.Update
        Dim updater As DataTier.Utilities.IDataUpdater
        Using scope As ILifetimeScope = m_container.BeginLifetimeScope
            updater = scope.Resolve(Of EventTypeDataSaver)(
                New TypedParameter(GetType(JestersCreditUnion.DataTier.Utilities.ITransactionHandler), New TransactionHandler(transactionHandler)),
                New TypedParameter(GetType(EventTypeData), m_eventTypeData)
            )
            updater.Update()
        End Using
    End Sub

    Public Function GetTaskTypes(settings As ISettings) As IEnumerable(Of IEventTypeTaskType) Implements IEventType.GetTaskTypes
        Dim factory As IEventTypeTaskTypeDataFactory
        Dim result As IEnumerable(Of IEventTypeTaskType)
        Using scope As ILifetimeScope = m_container.BeginLifetimeScope
            factory = scope.Resolve(Of IEventTypeTaskTypeDataFactory)()
            result = factory.GetByEventTypeId(New Settings(settings), EventTypeId).AsParallel _
                        .Where(Function(d As EventTypeTaskTypeData) d.IsActive) _
                        .Select(Of IEventTypeTaskType)(Function(d As EventTypeTaskTypeData) LoadTaskType(settings, d))
        End Using
        Return result
    End Function

    Private Function LoadTaskType(settings As ISettings, d As EventTypeTaskTypeData) As IEventTypeTaskType
        Return New EventTypeTaskType(Me, m_taskTypeFactory.Get(settings, d.TaskTypeId), d)
    End Function
End Class

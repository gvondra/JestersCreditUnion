Imports JestersCreditUnion.DataTier.Core
Imports JestersCreditUnion.DataTier.Core.Models
Imports autofac
Public Class TaskType
    Implements ITaskType

    Private m_taskTypeData As TaskTypeData
    Private m_container As IContainer

    Friend Sub New(ByVal taskTypeData As TaskTypeData)
        m_taskTypeData = taskTypeData
        m_container = ObjectContainer.GetContainer()
    End Sub

    Public ReadOnly Property TaskTypeId As Guid Implements ITaskType.TaskTypeId
        Get
            Return m_taskTypeData.TaskTypeId
        End Get
    End Property

    Public Property Title As String Implements ITaskType.Title
        Get
            Return m_taskTypeData.Title
        End Get
        Set(value As String)
            m_taskTypeData.Title = value
        End Set
    End Property

    Public Sub Create(transactionHandler As ITransactionHandler) Implements ISavable.Create
        Dim creator As DataTier.Utilities.IDataCreator
        Using scope As ILifetimeScope = m_container.BeginLifetimeScope
            creator = scope.Resolve(Of TaskTypeDataSaver)(
                New TypedParameter(GetType(JestersCreditUnion.DataTier.Utilities.ITransactionHandler), New TransactionHandler(transactionHandler)),
                New TypedParameter(GetType(TaskTypeData), m_taskTypeData)
            )
            creator.Create()
        End Using
    End Sub

    Public Sub Update(transactionHandler As ITransactionHandler) Implements ISavable.Update
        Dim updater As DataTier.Utilities.IDataUpdater
        Using scope As ILifetimeScope = m_container.BeginLifetimeScope
            updater = scope.Resolve(Of TaskTypeDataSaver)(
                New TypedParameter(GetType(JestersCreditUnion.DataTier.Utilities.ITransactionHandler), New TransactionHandler(transactionHandler)),
                New TypedParameter(GetType(TaskTypeData), m_taskTypeData)
            )
            updater.Update()
        End Using
    End Sub

    Public Function GetEventTypes(settings As ISettings) As IEnumerable(Of ITaskTypeEventType) Implements ITaskType.GetEventTypes
        Dim result As IEnumerable(Of ITaskTypeEventType) = {}
        Dim factory As IEventTypeTaskTypeDataFactory

        If TaskTypeId.Equals(Guid.Empty) = False Then
            Using scope As ILifetimeScope = m_container.BeginLifetimeScope
                factory = scope.Resolve(Of IEventTypeTaskTypeDataFactory)()
                result = From data In factory.GetByTaskTypeId(New Settings(settings), TaskTypeId)
                         Where data.EventType IsNot Nothing
                         Select New TaskTypeEventType(Me, New EventType(data.EventType), data)
            End Using
        End If
        Return result
    End Function

    Public Function CreateTaskTypeEventType(eventType As IEventType) As ITaskTypeEventType Implements ITaskType.CreateTaskTypeEventType
        Return New TaskTypeEventType(Me, eventType, New EventTypeTaskTypeData() With {.IsActive = True})
    End Function

    Public Function GetGroups(settings As ISettings) As IEnumerable(Of ITaskTypeGroup) Implements ITaskType.GetGroups
        Dim result As IEnumerable(Of ITaskTypeGroup) = {}
        Dim factory As ITaskTypeGroupDataFactory

        If TaskTypeId.Equals(Guid.Empty) = False Then
            Using scope As ILifetimeScope = m_container.BeginLifetimeScope
                factory = scope.Resolve(Of ITaskTypeGroupDataFactory)()
                result = From data In factory.GetByTaskTypeId(New Settings(settings), TaskTypeId)
                         Where data.Group IsNot Nothing
                         Select New TaskTypeGroup(Me, New Group(data.Group), data)
            End Using
        End If
        Return result
    End Function

    Public Function CreateTaskTypeGroup(group As IGroup) As ITaskTypeGroup Implements ITaskType.CreateTaskTypeGroup
        Return New TaskTypeGroup(Me, group, New TaskTypeGroupData() With {.IsActive = True})
    End Function
End Class

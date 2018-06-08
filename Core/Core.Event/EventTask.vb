Imports JestersCreditUnion.DataTier.Core
Imports JestersCreditUnion.DataTier.Core.Models
Imports Autofac
Public Class EventTask
    Implements IEventTask

    Private m_eventTaskData As EventTaskData
    Private m_innerTask As ITask
    Private m_event As IEvent
    Private m_container As IContainer

    Friend Sub New(ByVal [event] As IEvent, ByVal task As ITask, ByVal data As EventTaskData)
        m_eventTaskData = data
        m_innerTask = task
        m_event = [event]
        m_container = ObjectContainer.GetContainer
    End Sub

    Public ReadOnly Property TaskId As Guid Implements ITask.TaskId
        Get
            Return m_innerTask.TaskId
        End Get
    End Property

    Public Property Message As String Implements ITask.Message
        Get
            Return m_innerTask.Message
        End Get
        Set(value As String)
            m_innerTask.Message = value
        End Set
    End Property

    Public ReadOnly Property CreateTimestamp As DateTime Implements ITask.CreateTimestamp
        Get
            Return m_innerTask.CreateTimestamp
        End Get
    End Property

    Public Property IsClosed As Boolean Implements ITask.IsClosed
        Get
            Return m_innerTask.IsClosed
        End Get
        Set(value As Boolean)
            m_innerTask.IsClosed = value
        End Set
    End Property

    Public Sub Create(transactionHandler As ITransactionHandler) Implements ISavable.Create
        Dim creator As DataTier.Utilities.IDataCreator
        m_innerTask.Create(transactionHandler)
        Using scope As ILifetimeScope = m_container.BeginLifetimeScope
            m_eventTaskData.EventId = m_event.EventId
            m_eventTaskData.TaskId = m_innerTask.TaskId
            creator = scope.Resolve(Of EventTaskDataSaver)(
                New TypedParameter(GetType(JestersCreditUnion.DataTier.Utilities.ITransactionHandler), New TransactionHandler(transactionHandler)),
                New TypedParameter(GetType(EventTaskData), m_eventTaskData)
            )
            creator.Create()
        End Using
    End Sub

    Public Sub Update(transactionHandler As ITransactionHandler) Implements ISavable.Update
        m_innerTask.Update(transactionHandler)
    End Sub

    Public Function GetUser(settings As ISettings) As IUser Implements ITask.GetUser
        Return m_innerTask.GetUser(settings)
    End Function

    Public Sub SetUser(user As IUser) Implements ITask.SetUser
        m_innerTask.SetUser(user)
    End Sub

    Public Function GetTaskType(settings As ISettings) As ITaskType Implements ITask.GetTaskType
        Return m_innerTask.GetTaskType(settings)
    End Function
End Class

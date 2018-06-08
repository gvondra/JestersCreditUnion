Imports JestersCreditUnion.DataTier.Core
Imports JestersCreditUnion.DataTier.Core.Models
Imports Autofac
Public Class [Event]
    Implements IEvent

    Private m_eventData As EventData
    Private m_eventType As IEventType
    Private m_container As IContainer
    Private m_forms As IList(Of IForm)
    Private m_tasks As IList(Of ITask)

    Friend Sub New(ByVal eventType As IEventType)
        m_eventType = eventType
        m_eventData = New EventData() With {.EventTypeId = eventType.EventTypeId}
        m_container = ObjectContainer.GetContainer
    End Sub

    Public ReadOnly Property EventId As Guid Implements IEvent.EventId
        Get
            Return m_eventData.EventId
        End Get
    End Property

    Public ReadOnly Property Type As enumEventType Implements IEvent.Type
        Get
            Return CType(m_eventData.EventTypeId, enumEventType)
        End Get
    End Property

    Public Property Message As String Implements IEvent.Message
        Get
            Return m_eventData.Message
        End Get
        Set(value As String)
            m_eventData.Message = value
        End Set
    End Property

    Private Sub Create(ByVal transactionHandler As ITransactionHandler) Implements ISavable.Create
        Dim creator As DataTier.Utilities.IDataCreator

        Using scope As ILifetimeScope = m_container.BeginLifetimeScope
            creator = scope.Resolve(Of EventDataSaver)(
                New TypedParameter(GetType(JestersCreditUnion.DataTier.Utilities.ITransactionHandler), New TransactionHandler(transactionHandler)),
                New TypedParameter(GetType(EventData), m_eventData)
            )
            creator.Create()
        End Using

        If m_forms IsNot Nothing Then
            For Each form As IForm In m_forms
                form.Create(transactionHandler)
            Next
        End If
        If m_tasks IsNot Nothing Then
            For Each task As ITask In m_tasks
                task.Create(transactionHandler)
            Next
        End If
    End Sub

    Public Sub Update(ByVal transactinoHandler As ITransactionHandler) Implements ISavable.Update
        Throw New NotImplementedException()
    End Sub

    Public Function AddForm(form As IForm) As IForm Implements IEvent.AddForm
        Dim eventForm As New EventForm(Me, form)
        If m_forms Is Nothing Then
            m_forms = New List(Of IForm)
        End If
        m_forms.Add(eventForm)
        Return eventForm
    End Function

    Public Function GetEventType(settings As ISettings) As IEventType Implements IEvent.GetEventType
        Return m_eventType
    End Function

    Public Function AddTask(task As ITask) As ITask Implements IEvent.AddTask
        Dim eventTask As New EventTask(Me, task, New EventTaskData())
        If m_tasks Is Nothing Then
            m_tasks = New List(Of ITask)
        End If
        m_tasks.Add(eventTask)
        Return eventTask
    End Function
End Class

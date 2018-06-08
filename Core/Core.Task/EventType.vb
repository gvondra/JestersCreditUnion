Imports JestersCreditUnion.DataTier.Core.Models
Public Class EventType
    Implements IEventType

    Private m_eventTypeData As EventTypeData

    Friend Sub New(ByVal eventTypeData As EventTypeData)
        m_eventTypeData = eventTypeData
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

    Public Function GetTaskTypes(settings As ISettings) As IEnumerable(Of IEventTypeTaskType) Implements IEventType.GetTaskTypes
        Throw New NotImplementedException()
    End Function

    Public Sub Create(transactionHandler As ITransactionHandler) Implements ISavable.Create
        Throw New NotImplementedException()
    End Sub

    Public Sub Update(transactionHandler As ITransactionHandler) Implements ISavable.Update
        Throw New NotImplementedException()
    End Sub
End Class

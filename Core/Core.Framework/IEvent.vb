Public Interface IEvent
    Inherits ISavable

    ReadOnly Property EventId As Guid
    ReadOnly Property Type As enumEventType
    Property Message As String

    Function AddForm(ByVal form As IForm) As IForm
    Function AddTask(ByVal task As ITask) As ITask
    Function GetEventType(ByVal settings As ISettings) As IEventType
End Interface

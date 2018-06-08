Public Interface IEventType
    Inherits ISavable

    ReadOnly Property EventTypeId As Short
    Property Title As String

    Function GetTaskTypes(ByVal settings As ISettings) As IEnumerable(Of IEventTypeTaskType)
End Interface

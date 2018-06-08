Public Interface ITaskType
    Inherits ISavable

    ReadOnly Property TaskTypeId As Guid
    Property Title As String

    Function GetEventTypes(ByVal settings As ISettings) As IEnumerable(Of ITaskTypeEventType)
    Function CreateTaskTypeEventType(ByVal eventType As IEventType) As ITaskTypeEventType
    Function GetGroups(ByVal settings As ISettings) As IEnumerable(Of ITaskTypeGroup)
    Function CreateTaskTypeGroup(ByVal group As IGroup) As ITaskTypeGroup
End Interface

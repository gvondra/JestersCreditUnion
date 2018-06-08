Public Interface IEventTypeFactory
    Function [Get](ByVal settings As ISettings, ByVal type As enumEventType) As IEventType
    Function GetAll(ByVal settings As ISettings) As IEnumerable(Of IEventType)
End Interface

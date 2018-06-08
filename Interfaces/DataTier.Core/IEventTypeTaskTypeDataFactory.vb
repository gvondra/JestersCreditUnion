Public Interface IEventTypeTaskTypeDataFactory
    Function GetByTaskTypeId(ByVal settings As ISettings, ByVal taskTypeId As Guid) As IEnumerable(Of EventTypeTaskTypeData)
    Function GetByEventTypeId(ByVal settings As ISettings, ByVal eventTypeId As Int16) As IEnumerable(Of EventTypeTaskTypeData)
End Interface

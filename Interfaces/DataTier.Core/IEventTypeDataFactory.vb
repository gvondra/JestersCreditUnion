Public Interface IEventTypeDataFactory
    Function [Get](ByVal settings As ISettings, ByVal id As Short) As EventTypeData
    Function GetAll(ByVal settings As ISettings) As IEnumerable(Of EventTypeData)
End Interface

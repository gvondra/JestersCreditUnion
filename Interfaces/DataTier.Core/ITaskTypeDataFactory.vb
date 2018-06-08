Public Interface ITaskTypeDataFactory
    Function [Get](ByVal settings As ISettings, ByVal taskTypeId As Guid) As TaskTypeData
    Function GetAll(ByVal settings As ISettings) As IEnumerable(Of TaskTypeData)
End Interface

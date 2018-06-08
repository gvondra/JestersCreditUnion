Public Interface ITaskTypeFactory
    Function Create() As ITaskType
    Function [Get](ByVal settings As ISettings, ByVal taskTypeId As Guid) As ITaskType
    Function GetAll(ByVal settings As ISettings) As IEnumerable(Of ITaskType)
End Interface

Public Interface ITaskTypeGroupDataFactory
    Function GetByTaskTypeId(ByVal settings As ISettings, ByVal taskTypeId As Guid) As IEnumerable(Of TaskTypeGroupData)
End Interface

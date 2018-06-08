Public Interface ITaskDataFactory
    Function [Get](ByVal settings As ISettings, ByVal taskId As Guid) As TaskData
    Function GetByUserId(ByVal settings As ISettings, ByVal userId As Guid) As IEnumerable(Of TaskData)
    Function GetFormIds(ByVal settings As ISettings, ByVal taskId As Guid) As IEnumerable(Of Guid)
End Interface

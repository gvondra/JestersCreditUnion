Public Interface ITaskFactory
    Function Create(ByVal taskType As ITaskType) As ITask
    Function [Get](ByVal settings As ISettings, ByVal taskId As Guid) As ITask
    Function GetByUserId(ByVal settings As ISettings, ByVal userId As Guid) As IEnumerable(Of ITask)
    Function GetFormIds(ByVal settings As ISettings, ByVal taskId As Guid) As IEnumerable(Of Guid)
End Interface

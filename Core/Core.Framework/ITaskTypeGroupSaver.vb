Public Interface ITaskTypeGroupSaver
    Sub Save(ByVal settings As ISettings, ByVal taskTypeGroups As IEnumerable(Of ITaskTypeGroup))
End Interface

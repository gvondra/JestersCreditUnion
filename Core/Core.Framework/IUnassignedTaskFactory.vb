Public Interface IUnassignedTaskFactory
    Function GetByUser(ByVal settings As ISettings, ByVal userId As Guid) As IEnumerable(Of IUnassignedTask)
End Interface

Public Interface IUnassignedTaskDataFactory
    Function GetByUser(ByVal settings As ISettings, ByVal userId As Guid) As IEnumerable(Of UnassignedTaskData)
End Interface

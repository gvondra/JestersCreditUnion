Public Interface IUnassignedTask
    ReadOnly Property TaskId As Guid
    ReadOnly Property TaskTypeId As Guid
    ReadOnly Property Message As String
    ReadOnly Property CreateTimestamp As Date
    ReadOnly Property UpdateTimestamp As Date
    ReadOnly Property TaskTypeTitle As String
    ReadOnly Property GroupId As Guid?
    ReadOnly Property GroupName As String
End Interface

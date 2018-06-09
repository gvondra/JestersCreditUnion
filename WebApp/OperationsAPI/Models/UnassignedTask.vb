Public Class UnassignedTask
    Public Property TaskId As Guid
    Public Property TaskTypeId As Guid
    Public Property Message As String
    Public Property CreateTimestamp As Date
    Public Property UpdateTimestamp As Date
    Public Property TaskTypeTitle As String
    Public Property GroupId As Guid?
    Public Property GroupName As String
End Class

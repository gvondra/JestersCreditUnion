Public Class UnassignedTaskData
    <ColumnMapping("TaskId")> Public Property TaskId As Guid
    <ColumnMapping("TaskTypeId")> Public Property TaskTypeId As Guid
    <ColumnMapping("Message")> Public Property Message As String
    <ColumnMapping("CreateTimestamp")> Public Property CreateTimestamp As Date
    <ColumnMapping("UpdateTimestamp")> Public Property UpdateTimestamp As Date
    <ColumnMapping("TaskTypeTitle")> Public Property TaskTypeTitle As String
    <ColumnMapping("GroupId", IsNullable:=True)> Public Property GroupId As Guid?
    <ColumnMapping("GroupName")> Public Property GroupName As String
End Class

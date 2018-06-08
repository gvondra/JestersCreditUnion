Public Class EventTypeTaskTypeData
    Implements IDataManagedState(Of EventTypeTaskTypeData)

    <ColumnMapping("EventTypeId")> Public Property EventTypeId As Short
    <ColumnMapping("TaskTypeId")> Public Property TaskTypeId As Guid
    <ColumnMapping("IsActive")> Public Property IsActive As Boolean
    <ColumnMapping("CreateTimestamp")> Public Property CreateTimestamp As Date
    <ColumnMapping("UpdateTimestamp")> Public Property UpdateTimestamp As Date

    Public Property EventType As EventTypeData
    Public Property TaskType As TaskTypeData
    Public Property DataStateManager As IDataStateManager(Of EventTypeTaskTypeData) = New DataStateManager(Of EventTypeTaskTypeData) Implements IDataManagedState(Of EventTypeTaskTypeData).DataStateManager

    Public Sub AcceptChanges() Implements IDataManagedState(Of EventTypeTaskTypeData).AcceptChanges, IDbTransactionObserver.AfterCommit
        DataStateManager.Original = CType(Clone(), EventTypeTaskTypeData)
    End Sub

    Public Function Clone() As Object Implements ICloneable.Clone
        Return MemberwiseClone()
    End Function
End Class

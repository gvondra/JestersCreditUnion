Public Class EventTaskData
    Implements IDataManagedState(Of EventTaskData)

    <ColumnMapping("EventId")> Public Property EventId As Guid
    <ColumnMapping("TaskId")> Public Property TaskId As Guid
    <ColumnMapping("CreateTimestamp")> Public Property CreateTimestamp As Date
    <ColumnMapping("UpdateTimestamp")> Public Property UpdateTimestamp As Date

    Public Property DataStateManager As IDataStateManager(Of EventTaskData) = New DataStateManager(Of EventTaskData) Implements IDataManagedState(Of EventTaskData).DataStateManager

    Public Sub AcceptChanges() Implements IDataManagedState(Of EventTaskData).AcceptChanges, IDbTransactionObserver.AfterCommit
        DataStateManager.Original = CType(Clone(), EventTaskData)
    End Sub

    Public Function Clone() As Object Implements ICloneable.Clone
        Return MemberwiseClone()
    End Function
End Class

Public Class EventData
    Implements IDataManagedState(Of EventData)

    <ColumnMapping("EventId")> Public Property EventId As Guid
    <ColumnMapping("EventTypeId")> Public Property EventTypeId As Short
    <ColumnMapping("Message")> Public Property Message As String
    <ColumnMapping("CreateTimestamp")> Public Property CreateTimestamp As Date
    <ColumnMapping("UpdateTimestamp")> Public Property UpdateTimestamp As Date

    Public Property DataStateManager As IDataStateManager(Of EventData) = New DataStateManager(Of EventData) Implements IDataManagedState(Of EventData).DataStateManager

    Public Sub AcceptChanges() Implements IDataManagedState(Of EventData).AcceptChanges, IDbTransactionObserver.AfterCommit
        DataStateManager.Original = CType(Clone(), EventData)
    End Sub

    Public Function Clone() As Object Implements ICloneable.Clone
        Return MemberwiseClone()
    End Function
End Class

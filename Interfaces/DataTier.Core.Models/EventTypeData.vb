Public Class EventTypeData
    Implements IDataManagedState(Of EventTypeData)

    <ColumnMapping("EventTypeId")> Public Property EventTypeId As Short
    <ColumnMapping("Title")> Public Property Title As String
    <ColumnMapping("CreateTimestamp")> Public Property CreateTimestamp As Date
    <ColumnMapping("UpdateTimestamp")> Public Property UpdateTimestamp As Date

    Public Property DataStateManager As IDataStateManager(Of EventTypeData) = New DataStateManager(Of EventTypeData) Implements IDataManagedState(Of EventTypeData).DataStateManager

    Public Sub AcceptChanges() Implements IDataManagedState(Of EventTypeData).AcceptChanges, IDbTransactionObserver.AfterCommit
        DataStateManager.Original = CType(Clone(), EventTypeData)
    End Sub

    Public Function Clone() As Object Implements ICloneable.Clone
        Return MemberwiseClone()
    End Function
End Class

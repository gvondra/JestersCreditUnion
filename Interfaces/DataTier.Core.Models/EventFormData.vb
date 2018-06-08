Public Class EventFormData
    Implements IDataManagedState(Of EventFormData)

    <ColumnMapping("EventId")> Public Property EventId As Guid
    <ColumnMapping("FormId")> Public Property FormId As Guid
    <ColumnMapping("CreateTimestamp")> Public Property CreateTimestamp As Date
    <ColumnMapping("UpdateTimestamp")> Public Property UpdateTimestamp As Date

    Public Property DataStateManager As IDataStateManager(Of EventFormData) = New DataStateManager(Of EventFormData) Implements IDataManagedState(Of EventFormData).DataStateManager

    Public Sub AcceptChanges() Implements IDataManagedState(Of EventFormData).AcceptChanges, IDbTransactionObserver.AfterCommit
        DataStateManager.Original = CType(Clone(), EventFormData)
    End Sub

    Public Function Clone() As Object Implements ICloneable.Clone
        Return MemberwiseClone()
    End Function
End Class

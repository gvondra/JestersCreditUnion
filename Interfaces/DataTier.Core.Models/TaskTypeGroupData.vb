Public Class TaskTypeGroupData
    Implements IDataManagedState(Of TaskTypeGroupData)

    <ColumnMapping("TaskTypeId")>
    Public Property TaskTypeId As Guid
    <ColumnMapping("GroupId")>
    Public Property GroupId As Guid
    <ColumnMapping("IsActive")>
    Public Property IsActive As Boolean
    <ColumnMapping("CreateTimestamp")>
    Public Property CreateTimestamp As Date
    <ColumnMapping("UpdateTimestamp")>
    Public Property UpdateTimestamp As Date

    Public Property Group As GroupData
    Public Property DataStateManager As IDataStateManager(Of TaskTypeGroupData) = New DataStateManager(Of TaskTypeGroupData) Implements IDataManagedState(Of TaskTypeGroupData).DataStateManager

    Public Sub AcceptChanges() Implements IDataManagedState(Of TaskTypeGroupData).AcceptChanges, IDbTransactionObserver.AfterCommit
        DataStateManager.Original = CType(Clone(), TaskTypeGroupData)
    End Sub

    Public Function Clone() As Object Implements ICloneable.Clone
        Return MemberwiseClone()
    End Function
End Class

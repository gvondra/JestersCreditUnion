Public Class UserGroupData
    Implements IDataManagedState(Of UserGroupData)

    <ColumnMapping("UserId")>
    Public Property UserId As Guid
    <ColumnMapping("GroupId")>
    Public Property GroupId As Guid
    <ColumnMapping("IsActive")>
    Public Property IsActive As Boolean
    <ColumnMapping("CreateTimestamp")>
    Public Property CreateTimestamp As Date
    <ColumnMapping("UpdateTimestamp")>
    Public Property UpdateTimestamp As Date

    Public Property Group As GroupData
    Public Property DataStateManager As IDataStateManager(Of UserGroupData) = New DataStateManager(Of UserGroupData) Implements IDataManagedState(Of UserGroupData).DataStateManager

    Public Sub AcceptChanges() Implements IDataManagedState(Of UserGroupData).AcceptChanges, IDbTransactionObserver.AfterCommit
        DataStateManager.Original = CType(Clone(), UserGroupData)
    End Sub

    Public Function Clone() As Object Implements ICloneable.Clone
        Return MemberwiseClone()
    End Function
End Class

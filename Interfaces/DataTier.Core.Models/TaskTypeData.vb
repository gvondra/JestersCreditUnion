Public Class TaskTypeData
    Implements IDataManagedState(Of TaskTypeData)

    <ColumnMapping("TaskTypeId")>
    Public Property TaskTypeId As Guid
    <ColumnMapping("Title")>
    Public Property Title As String
    <ColumnMapping("CreateTimestamp")>
    Public Property CreateTimestamp As Date
    <ColumnMapping("UpdateTimestamp")>
    Public Property UpdateTimestamp As Date

    Public Property DataStateManager As IDataStateManager(Of TaskTypeData) = New DataStateManager(Of TaskTypeData)() Implements IDataManagedState(Of TaskTypeData).DataStateManager

    Public Sub AcceptChanges() Implements IDataManagedState(Of TaskTypeData).AcceptChanges, IDbTransactionObserver.AfterCommit
        DataStateManager.Original = CType(Clone(), TaskTypeData)
    End Sub

    Public Function Clone() As Object Implements ICloneable.Clone
        Return MemberwiseClone()
    End Function
End Class

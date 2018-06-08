Public Class GroupData
    Implements IDataManagedState(Of GroupData)

    <ColumnMapping("GroupId")>
    Public Property GroupId As Guid
    <ColumnMapping("Name")>
    Public Property Name As String
    <ColumnMapping("CreateTimestamp")>
    Public Property CreateTimestamp As Date
    <ColumnMapping("UpdateTimestamp")>
    Public Property UpdateTimestamp As Date

    Public Property DataStateManager As IDataStateManager(Of GroupData) = New DataStateManager(Of GroupData) Implements IDataManagedState(Of GroupData).DataStateManager

    Public Sub AcceptChanges() Implements IDataManagedState(Of GroupData).AcceptChanges, IDbTransactionObserver.AfterCommit
        If DataStateManager IsNot Nothing Then DataStateManager.Original = CType(Clone(), GroupData)
    End Sub

    Public Function Clone() As Object Implements ICloneable.Clone
        Return MemberwiseClone()
    End Function
End Class

Public Class OrganizationData
    Implements IDataManagedState(Of OrganizationData)

    <ColumnMapping("OrganizationId")>
    Public Property OrganizationId As Guid
    <ColumnMapping("Name")>
    Public Property Name As String
    <ColumnMapping("CreateTimestamp")>
    Public Property CreateTimestamp As Date
    <ColumnMapping("UpdateTimestamp")>
    Public Property UpdateTimestamp As Date

    Public Property DataStateManager As IDataStateManager(Of OrganizationData) = New DataStateManager(Of OrganizationData) Implements IDataManagedState(Of OrganizationData).DataStateManager

    Public Sub AcceptChanges() Implements IDataManagedState(Of OrganizationData).AcceptChanges, IDbTransactionObserver.AfterCommit
        DataStateManager.Original = CType(Clone(), OrganizationData)
    End Sub

    Public Function Clone() As Object Implements ICloneable.Clone
        Return MemberwiseClone()
    End Function
End Class

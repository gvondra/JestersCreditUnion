Public Class UserData
    Implements IDataManagedState(Of UserData)

    <ColumnMapping("UserId")>
    Public Property UserId As Guid
    <ColumnMapping("FullName")>
    Public Property FullName As String
    <ColumnMapping("ShortName")>
    Public Property ShortName As String
    <ColumnMapping("BirthDate", IsNullable:=True)>
    Public Property BirthDate As Date?
    <ColumnMapping("EmailAddress")>
    Public Property EmailAddress As String
    <ColumnMapping("PhoneNumber")>
    Public Property PhoneNumber As String
    <ColumnMapping("CreateTimestamp")>
    Public Property CreateTimestamp As Date
    <ColumnMapping("UpdateTimestamp")>
    Public Property UpdateTimestamp As Date

    Public Property DataStateManager As IDataStateManager(Of UserData) = New DataStateManager(Of UserData) Implements IDataManagedState(Of UserData).DataStateManager

    Public Sub AcceptChanges() Implements IDataManagedState(Of UserData).AcceptChanges, IDbTransactionObserver.AfterCommit
        If DataStateManager IsNot Nothing Then DataStateManager.Original = CType(Clone(), UserData)
    End Sub

    Public Function Clone() As Object Implements ICloneable.Clone
        Return Me.MemberwiseClone()
    End Function
End Class

Imports System.Xml
Public Class FormData
    Implements IDataManagedState(Of FormData)

    <ColumnMapping("FormId")> Public Property FormId As Guid
    <ColumnMapping("UserId")> Public Property UserId As Guid
    <ColumnMapping("FormTypeId")> Public Property FormTypeId As Short
    <ColumnMapping("Style")> Public Property Style As Short
    <ColumnMapping("Content", IsNullable:=True)> Public Property Content As XmlNode
    <ColumnMapping("CreateTimestamp")> Public Property CreateTimestamp As Date
    <ColumnMapping("UpdateTimestamp")> Public Property UpdateTimestamp As Date

    Public Property DataStateManager As IDataStateManager(Of FormData) = New DataStateManager(Of FormData) Implements IDataManagedState(Of FormData).DataStateManager

    Public Sub AcceptChanges() Implements IDataManagedState(Of FormData).AcceptChanges, IDbTransactionObserver.AfterCommit
        DataStateManager.Original = CType(Clone(), FormData)
    End Sub

    Public Function Clone() As Object Implements ICloneable.Clone
        Return MemberwiseClone()
    End Function

End Class

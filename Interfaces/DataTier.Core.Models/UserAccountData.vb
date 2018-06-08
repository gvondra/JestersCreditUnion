Public Class UserAccountData
    <ColumnMapping("UserAccountId")>
    Public Property UserAccountId As Guid
    <ColumnMapping("UserId")>
    Public Property UserId As Guid
    <ColumnMapping("SubscriberId")>
    Public Property SubscriberId As String
    <ColumnMapping("CreateTimestamp")>
    Public Property CreateTimestamp As Date
    <ColumnMapping("UpdateTimestamp")>
    Public Property UpdateTimestamp As Date
End Class

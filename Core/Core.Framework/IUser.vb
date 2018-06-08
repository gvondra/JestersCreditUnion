Public Interface IUser
    Inherits ISavable

    ReadOnly Property UserId As Guid
    Property FullName As String
    Property ShortName As String
    Property BirthDate As Date?
    Property EmailAddress As String
    Property PhoneNumber As String
    ReadOnly Property CreateTimestamp As Date
    ReadOnly Property UpdateTimestamp As Date

    Sub CreateAccount(ByVal transactionHandler As ITransactionHandler, ByVal subscriberId As String)
    Function GetGroups(ByVal settings As ISettings) As IEnumerable(Of IUserGroup)
    Function CreateUserGroup(ByVal group As IGroup) As IUserGroup
End Interface
